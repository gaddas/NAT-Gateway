/*
  FwHookDrv.c

  Author: Jesús O.
  Last Updated: 12/09/03 
*/


#include <string.h>
#include <stdio.h>
#include <ntddk.h>
#include <ndis.h>
#include <ipFirewall.h>

#include "FwHookDrv.h"
#include "NetHeaders.h"

#if DBG
#define dprintf DbgPrint
#else
#define dprintf(x)
#endif


BOOLEAN loaded = FALSE;


#define NT_DEVICE_NAME L"\\Device\\FwHookDrv"
#define DOS_DEVICE_NAME L"\\DosDevices\\FwHookDrv"

// Structure to define the linked list of rules
struct filterList
{
	IPFilter ipf;

	struct filterList *next;
};

// Function declarations
NTSTATUS DrvDispatch(IN PDEVICE_OBJECT DeviceObject, IN PIRP Irp);
VOID DrvUnload(IN PDRIVER_OBJECT DriverObject);

NTSTATUS SetFilterFunction(IPPacketFirewallPtr filterFunction, BOOLEAN load);

NTSTATUS AddFilterToList(IPFilter *pf);
NTSTATUS RemFilterFromList(int *id);
void ClearFilterList(void);

FORWARD_ACTION cbFilterFunction(VOID			**pData,
								UINT			RecvInterfaceIndex,
								UINT			*pSendInterfaceIndex,
								UCHAR			*pDestinationType,
								VOID			*pContext,
								UINT			ContextLength,
								struct IPRcvBuf **pRcvBuf);

FORWARD_ACTION FilterPacket(unsigned char *PacketHeader,
							unsigned char *Packet, 
							unsigned int PacketLength, 
							DIRECTION_E direction, 
							unsigned int RecvInterfaceIndex, 
							unsigned int SendInterfaceIndex);



struct filterList *first = NULL;
struct filterList *last = NULL;

/*++

Description:

    Entry point of the driver.

Arguments:

    DriverObject - pointer to a driver object

    RegistryPath - pointer to a unicode string that contain registry path.

Return:

    STATUS_SUCCESS if success
    STATUS_UNSUCCESSFUL If the function fails

--*/
NTSTATUS DriverEntry(IN PDRIVER_OBJECT DriverObject, IN PUNICODE_STRING RegistryPath) {

	PDEVICE_OBJECT         deviceObject = NULL;
    NTSTATUS               ntStatus;
    UNICODE_STRING         deviceNameUnicodeString;
    UNICODE_STRING         deviceLinkUnicodeString;
	
	dprintf("FwHookDrv.sys: Loading Driver....\n");

	// Create the device
	RtlInitUnicodeString(&deviceNameUnicodeString, NT_DEVICE_NAME);

	ntStatus = IoCreateDevice(DriverObject, 
								0,
								&deviceNameUnicodeString, 
								FILE_DEVICE_FWHOOKDRV,
								0,
								FALSE,
								&deviceObject);


    if ( NT_SUCCESS(ntStatus) ) {
    
		// Create symbolic link
        RtlInitUnicodeString(&deviceLinkUnicodeString, DOS_DEVICE_NAME);

        ntStatus = IoCreateSymbolicLink(&deviceLinkUnicodeString, &deviceNameUnicodeString);

        if ( !NT_SUCCESS(ntStatus) ) {
            dprintf("FwHookDrv.sys: Error creating symbolic link.\n");
        }

        // Define driver's control functions
        DriverObject->MajorFunction[IRP_MJ_CREATE]         =
        DriverObject->MajorFunction[IRP_MJ_CLOSE]          =
        DriverObject->MajorFunction[IRP_MJ_DEVICE_CONTROL] = DrvDispatch;
        DriverObject->DriverUnload                         = DrvUnload;
    }

    if ( !NT_SUCCESS(ntStatus) ) {
        dprintf("Error Intitializing. Unloading driver...");
		DrvUnload(DriverObject);
    }

    return ntStatus;
}



/*++

Description:

    Process all IRPs the driver receive

Arguments:

    DeviceObject - Pointer to device object

    Irp          - IRP

--*/
NTSTATUS DrvDispatch(IN PDEVICE_OBJECT DeviceObject, IN PIRP Irp) {

    PIO_STACK_LOCATION  irpStack;
    PVOID               ioBuffer;
    ULONG               inputBufferLength;
    ULONG               outputBufferLength;
    ULONG               ioControlCode;
    NTSTATUS            ntStatus;

    Irp->IoStatus.Status      = STATUS_SUCCESS;
    Irp->IoStatus.Information = 0;

     // Get pointer to current stack location
    irpStack = IoGetCurrentIrpStackLocation(Irp);


    // Get pointer to buffer information
    ioBuffer           = Irp->AssociatedIrp.SystemBuffer;
    inputBufferLength  = irpStack->Parameters.DeviceIoControl.InputBufferLength;
    outputBufferLength = irpStack->Parameters.DeviceIoControl.OutputBufferLength;

    switch (irpStack->MajorFunction) {

    case IRP_MJ_CREATE:
        dprintf("FwHookDrv.sys: IRP_MJ_CREATE\n");
        break;

    case IRP_MJ_CLOSE:
        dprintf("FwHookDrv.sys: IRP_MJ_CLOSE\n");
        break;

    case IRP_MJ_DEVICE_CONTROL:
        dprintf("FwHookDrv.sys: IRP_MJ_DEVICE_CONTROL\n");
        ioControlCode = irpStack->Parameters.DeviceIoControl.IoControlCode;

        switch (ioControlCode) {
			// IOCTL to install filter function.
			case START_IP_HOOK:
			{
				if(!loaded) {
					dprintf("FwHookDrv.sys: START_IP_HOOK\n");
					loaded = TRUE;
					SetFilterFunction(cbFilterFunction, TRUE);
				} else {
					dprintf("FwHookDrv.sys: START_IP_HOOK, already started.\n");
				}
				break;
			}

			// IOCTL to delete filter function.
			case STOP_IP_HOOK:
			{
				if(loaded) {
					dprintf("FwHookDrv.sys: STOP_IP_HOOK\n");
					loaded = FALSE;
					SetFilterFunction(cbFilterFunction, FALSE);
				} else {
					dprintf("FwHookDrv.sys: STOP_IP_HOOK, already stopped.\n");
				}           
				break;
			}

            // IOCTL to add filter rule.
			case ADD_FILTER:
			{
				if(inputBufferLength == sizeof(IPFilter)) {
					IPFilter *nf;
					nf = (IPFilter *)ioBuffer;

					dprintf("FwHookDrv.sys: ADD_FILTER drop=%d srcAddr=0x%x scrMask=0x%x srcPort=%d dstAddr=0x%x dstMask=0x%x dstPort=%d proto=%d\n", 
							 nf->drop, nf->sourceIp, nf->sourceMask, nf->sourcePort, nf->destinationIp, nf->destinationMask, nf->destinationPort, nf->protocol);
					
					AddFilterToList(nf);
				} else {
					dprintf("FwHookDrv.sys: ADD_FILTER wrong size: %d must be %d\n", inputBufferLength, sizeof(IPFilter));
				}
				break;
			}

			// IOCTL to remove filter rule.
			case REM_FILTER:
			{
				if(inputBufferLength == sizeof(int)) {
					int *id;
					id = (int *)ioBuffer;

					dprintf("FwHookDrv.sys: REM_FILTER id=%d\n", *id);
					
					RemFilterFromList(id);
				} else {
					dprintf("FwHookDrv.sys: REM_FILTER wrong size: %d must be %d\n", inputBufferLength, sizeof(int));
				}
				break;
			}

			// IOCTL to delete filter rule.
			case CLEAR_FILTER:
			{
				dprintf("FwHookDrv.sys: CLEAR_FILTER\n");
				ClearFilterList();
				break;
			}

			default:
				Irp->IoStatus.Status = STATUS_INVALID_PARAMETER;
				dprintf("FwHookDrv.sys: Unknown IOCTL.\n");
				break;
        }

        break;
    }


    // We can't return Irp-IoStatus directly because after IoCompleteRequest
	// we aren't the owners of the IRP.
    ntStatus = Irp->IoStatus.Status;

    IoCompleteRequest(Irp, IO_NO_INCREMENT);

    return ntStatus;
}


/*++

Description:

    Unload the driver. Free all resources.

Arguments:

    DriverObject - Pointer to driver object.

--*/
VOID DrvUnload(IN PDRIVER_OBJECT DriverObject)
{
    UNICODE_STRING         deviceLinkUnicodeString;

	dprintf("FwHookDrv.sys: Unloading driver...\n");

	if(loaded)
	{
		loaded = FALSE;
		SetFilterFunction(cbFilterFunction, FALSE);
	}

	// Free filter rules
	ClearFilterList();
   
    // Remove symbolic link
    RtlInitUnicodeString(&deviceLinkUnicodeString, DOS_DEVICE_NAME);
    IoDeleteSymbolicLink(&deviceLinkUnicodeString);
   
	// Remove the device
    IoDeleteDevice(DriverObject->DeviceObject);
}



/*++

Description:

    Install/Uninstall the filter function

Arguments:

    filterFunction - Pointer to the filter function.
	load		   - If TRUE, the function is added. Else, the function will be removed.

Return:

    STATUS_SUCCESS If success,

--*/
NTSTATUS SetFilterFunction(IPPacketFirewallPtr filterFunction, BOOLEAN load)
{
	NTSTATUS status = STATUS_SUCCESS, waitStatus=STATUS_SUCCESS;
	UNICODE_STRING filterName;
	PDEVICE_OBJECT ipDeviceObject=NULL;
	PFILE_OBJECT ipFileObject=NULL;

	IP_SET_FIREWALL_HOOK_INFO filterData;

	KEVENT event;
	IO_STATUS_BLOCK ioStatus;
	PIRP irp;

	// Get pointer to Ip device
	RtlInitUnicodeString(&filterName, DD_IP_DEVICE_NAME);
	status = IoGetDeviceObjectPointer(&filterName,STANDARD_RIGHTS_ALL, &ipFileObject, &ipDeviceObject);
	
	if(NT_SUCCESS(status))
	{
		// Init firewall hook structure
		filterData.FirewallPtr	= filterFunction;
		filterData.Priority		= 1;
		filterData.Add			= load;

		KeInitializeEvent(&event, NotificationEvent, FALSE);

		// Build Irp to establish filter function
		irp = IoBuildDeviceIoControlRequest(IOCTL_IP_SET_FIREWALL_HOOK,
			  							    ipDeviceObject,
											(PVOID) &filterData,
											sizeof(IP_SET_FIREWALL_HOOK_INFO),
											NULL,
											0,
											FALSE,
											&event,
											&ioStatus);


		if(irp != NULL)
		{
			// Send the Irp and wait for its completion
			status = IoCallDriver(ipDeviceObject, irp);

			
			if (status == STATUS_PENDING) 
			{
				waitStatus = KeWaitForSingleObject(&event, Executive, KernelMode, FALSE, NULL);

				if (waitStatus 	!= STATUS_SUCCESS ) 
					dprintf("FwHookDrv.sys: Error waiting for Ip Driver.");
			}

			status = ioStatus.Status;

			if(!NT_SUCCESS(status))
				dprintf("FwHookDrv.sys: E/S error with Ip Driver\n");
		}
		
		else
		{
			status = STATUS_INSUFFICIENT_RESOURCES;

			dprintf("FwHookDrv.sys: Error creating the IRP\n");
		}

		// Free resources
		if(ipFileObject != NULL)
			ObDereferenceObject(ipFileObject);
		
		ipFileObject = NULL;
		ipDeviceObject = NULL;
	}
	
	else
		dprintf("FwHookDrv.sys: Error getting pointer to Ip driver.\n");
	
	return status;
}




/*++

Description:

    Add a rule to linked list filter rules.

Arguments:

      pf - Pointer to a filter rule.


Return:

    STATUS_SUCCESS If success
    STATUS_INSUFFICIENT_RESOURCES if the function fails
 
--*/
NTSTATUS AddFilterToList(IPFilter *pf)
{
	struct filterList *aux=NULL;

	// Reserve memory
	aux=(struct filterList *) ExAllocatePool(NonPagedPool, sizeof(struct filterList));
	
	if(aux == NULL)
	{
		dprintf("FwHookDrv.sys: Error al reservar memoria en AddFilterToList.\n");
	
		return STATUS_INSUFFICIENT_RESOURCES;
	}

	// Fill the rule data
	aux->ipf.destinationIp = pf->destinationIp;
	aux->ipf.sourceIp = pf->sourceIp;

	aux->ipf.destinationMask = pf->destinationMask;
	aux->ipf.sourceMask = pf->sourceMask;

	aux->ipf.destinationPort = pf->destinationPort;
	aux->ipf.sourcePort = pf->sourcePort;

	aux->ipf.protocol = pf->protocol;

	aux->ipf.drop=pf->drop;

	// Put the rule in the end of linked list
	if(first == NULL)
	{
		first = last = aux;
		
		first->next = NULL;
	}
	
	else
	{
		last->next = aux;
		last = aux;
		last->next = NULL;
	}

	return STATUS_SUCCESS;
}


/*++

Description:

    Remove a rule from linked list filter rules.

Arguments:

      pf - Pointer to a filter rule.


Return:

    STATUS_SUCCESS If success
    STATUS_INSUFFICIENT_RESOURCES if the function fails
 
--*/
NTSTATUS RemFilterFromList(int *id)
{
	int countRule = 0;
	struct filterList *prevous = first;
	struct filterList *current = first;

	while(current != NULL) {
		if (countRule == *id ) {

			if ((current == first) && (current == last)) {
			// Only 1 element in list
				first = NULL;
				last = NULL;
			} else if (current == first) {
			// Removing of First element
				first = current->next;
			} else if (current == last) {
			// Removing of Last element
				last = prevous;
				last->next = NULL;
			} else 
				prevous->next = current->next;

			ExFreePool(current);
			break;
		}

		countRule++;
		prevous = current;
		current = current->next;
	}

	return STATUS_SUCCESS;
}




/*++

Description:

    Remove linked list of rules.

Arguments:


--*/
void ClearFilterList(void)
{
	struct filterList *aux = NULL;

		
	while(first != NULL)
	{
		aux = first;
		first = first->next;
		ExFreePool(aux);	
	}

	first = last = NULL;

}



/*++

Description:

    Firewall Hook filter function.
	
Arguments:

--*/

FORWARD_ACTION cbFilterFunction(VOID			 **pData,
								UINT			 RecvInterfaceIndex,
								UINT			 *pSendInterfaceIndex,
								UCHAR			 *pDestinationType,
								VOID			 *pContext,
								UINT		 	 ContextLength,
								struct IPRcvBuf  **pRcvBuf)
{

	FORWARD_ACTION result = FORWARD;
	char *packet = NULL;
	int bufferSize;
	struct IPRcvBuf *buffer =(struct IPRcvBuf *) *pData; 
	PFIREWALL_CONTEXT_T fwContext = (PFIREWALL_CONTEXT_T)pContext;



	// First I get the size of the packet
	if(buffer != NULL)
	{
		bufferSize = buffer->ipr_size;
		
		while(buffer->ipr_next != NULL)
		{
			buffer = buffer->ipr_next;

			bufferSize += buffer->ipr_size;
		}

		// Reserve memory for the complete packet.
		packet = (char *) ExAllocatePool(NonPagedPool, bufferSize);
		if(packet != NULL)
		{
			IPHeader *ipp = (IPHeader *)packet;
			unsigned int offset = 0;

			buffer = (struct IPRcvBuf *) *pData;

			memcpy(packet, buffer->ipr_buffer, buffer->ipr_size);
			
			
			while(buffer->ipr_next != NULL)
			{		
				offset += buffer->ipr_size;
				buffer = buffer->ipr_next;
							
				memcpy(packet + offset, buffer->ipr_buffer, buffer->ipr_size);
			}

		

			// Call filter function
			// The header field untis is words (32bits)
			// lenght in bytes = ipp->headerLength * (32 bits/8)
			result =  FilterPacket(packet,
								   packet + (ipp->headerLength * 4),  
								   bufferSize - (ipp->headerLength * 4), 
								   (fwContext != NULL) ? fwContext->Direction: 0, 
								   RecvInterfaceIndex, 
								   (pSendInterfaceIndex != NULL) ? *pSendInterfaceIndex : 0);


		}

		else
			dprintf("FwHookDrv.sys: Insufficient resources.\n");
	}

	if(packet != NULL)
		ExFreePool(packet);

	// Default operation: Accept all.
	return result;
}



/*++

Description:

    Filter function
	
Arguments:
	PacketHeader - Pointer to Ip header.
	Packet - Pointer to Ip Packet payload.
	PacketLength - Size of Packet buffer.
	direction - Indicate incoming or outgoing packet.
	RecvInterfaceIndex - Interface where the packet has been received.
	SendInterfaceIndex - Interface where the packet will be sent.

Returns:
	FORWARD: To pass the packet.
	DROP: To drop the packet.
--*/

FORWARD_ACTION FilterPacket(unsigned char *PacketHeader,
							unsigned char *Packet, 
							unsigned int PacketLength, 
							DIRECTION_E direction, 
							unsigned int RecvInterfaceIndex, 
							unsigned int SendInterfaceIndex)
{
	IPHeader *ipp;
	TCPHeader *tcph;
	UDPHeader *udph;
	ICMPHeader *icmph;

	int countRule = 0;

	struct filterList *aux = first;

	BOOLEAN retTraffic;

	// Extract Ip header.
	ipp=(IPHeader *)PacketHeader;

		
	if(ipp->protocol == IPPROTO_ICMP)
	{
		icmph = (ICMPHeader *) Packet;
	}


	if(ipp->protocol == IPPROTO_TCP)
		tcph=(TCPHeader *)Packet; 

	
	// Compare each packet with filter rules
	while(aux != NULL)
	{
		if(aux->ipf.protocol == 0 || ipp->protocol == aux->ipf.protocol)
		{
			retTraffic = FALSE;

			if(aux->ipf.sourceIp != 0 && (ipp->source & aux->ipf.sourceMask) != aux->ipf.sourceIp)
			{
				// For tcp packets of accepted conexions, pass packets in both directions.
				if(ipp->protocol == IPPROTO_TCP)
				{	
					// TCP rules!
					if(((tcph->flags & TH_SYN) != TH_SYN) || ((tcph->flags & (TH_SYN | TH_ACK)) == (TH_SYN | TH_ACK)))
					{
						if((ipp->destination & aux->ipf.sourceMask) == aux->ipf.sourceIp)
						{
							retTraffic = TRUE;
						}
					}
				}

				if(retTraffic != TRUE)
				{
					aux=aux->next;
				
					countRule++;
					continue;
				}
			}
									
			
			if(!retTraffic)
			{
				if(aux->ipf.destinationIp != 0 && (ipp->destination & aux->ipf.destinationMask) != aux->ipf.destinationIp)
				{
					aux=aux->next;

					countRule++;
					continue;
				}
			}

			else
			{
				if(aux->ipf.destinationIp != 0 && (ipp->source & aux->ipf.destinationMask) != aux->ipf.destinationIp)
				{
					aux=aux->next;

					countRule++;
					continue;
				}
			}	
			
			if(ipp->protocol == IPPROTO_TCP) 
			{
				if(!retTraffic)
				{
					if(aux->ipf.sourcePort == 0 || tcph->sourcePort == aux->ipf.sourcePort)
					{ 
						if(aux->ipf.destinationPort == 0 || tcph->destinationPort == aux->ipf.destinationPort) 
						{
							if(aux->ipf.drop)
									 return  DROP;
								else
									return FORWARD;
						}
					}
				}

				else
				{
					if(aux->ipf.sourcePort == 0 || tcph->destinationPort == aux->ipf.sourcePort)
					{ 
						if(aux->ipf.destinationPort == 0 || tcph->sourcePort == aux->ipf.destinationPort) 
						{
							if(aux->ipf.drop)
									 return  DROP;
								else
									return FORWARD;
						}
					}
				}

			}
				
			//Si es un datagrama UDP, miro los puertos
			else if(ipp->protocol == IPPROTO_UDP) 
			{
				udph=(UDPHeader *)Packet; 

				if(aux->ipf.sourcePort == 0 || udph->sourcePort == aux->ipf.sourcePort) 
				{ 
					if(aux->ipf.destinationPort == 0 || udph->destinationPort == aux->ipf.destinationPort) 
					{
						// Coincidencia!! Decido que hacer con el paquete.
						if(aux->ipf.drop)
							return  DROP;
						
						else
							return FORWARD;
					}
				}
			}	
			
			else
			{
				// return result
				if(aux->ipf.drop)
					return  DROP;
				else
					return FORWARD;
			}	
		}
		
		// Next rule...
		countRule++;
		aux=aux->next;
	}


	return FORWARD;
}