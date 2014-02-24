// NetGate
// Copyright (c) 2008-2009, Danail Dimitrov
//
// This file is part of NetGate.
//
// NetGate is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// NetGate is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with NetGate. If not, see <http://www.gnu.org/licenses/>.


#include "precomp.h"
#pragma hdrstop
#include "iocommon.h"


/* 
* Setting the flags on the device driver object determine what type of I/O
* you wish to use.
*   
*  Direct I/O - MdlAddress describes the Virtual Address list.  This is then used to lock the pages in memory.          
*
*					PROS: Fast, Pages are not copied.
*					CONS: Uses resources, needs to lock pages into memory.
*
*  Buffered I/o - SystemBuffer is then used by the driver to access the data.  The I/O manager will copy the 
*                 data given by the user mode driver into this buffer on behalf of the driver.
*
*					PROS: Easier to use, driver simply accesses the buffer
*                         Usermode buffer is not locked in memory
*                   CONS: Slower operation (Use on smaller data sets)
*                         Uses resources, allocates Non-paged memory
*                         Large allocations may not work since it would
*                         require allocating large sequential non-paged memory.
*
*  Neither Buffered or Direct - This is when you simply read the buffer directly using the user-mode address.
*                   Simply omit DO_DIRECT_IO and DO_BUFFERED_IO to perform this action.
*
*					PROS: No copying or locking pages occurs.
*					CONS: You *MUST* be in the context of the user-mode thread that made the request.
*						  being in another process space you the page tables would not point to
*                         the same location.
*                         You have to perform some checking and probeing in order to verify
*                         when you can read/write from the pages.
*                         You cannot access a user mode address unless it's locked into memory
*                         at >= DPC level.
*                         The usermode process could also change the access rights of the
*                         buffer while the driver is trying to read it!
*
*/

NTSTATUS netgIO_WriteDirectIO(PDEVICE_OBJECT pDeviceObject, PIRP pIrp) {
    NTSTATUS			NtStatus = STATUS_SUCCESS;
    PIO_STACK_LOCATION	pIoStackIrp = NULL;
	POPEN_CONTEXT       pOpenContext;
    PCHAR				pWriteDataBuffer;

    DBGPRINT(("      netgIO_WriteDirectIO Called \n"));
    
    /*
     * Each time the IRP is passed down the driver stack a new stack location is added
     * specifying certain parameters for the IRP to the driver.
     */
    pIoStackIrp = IoGetCurrentIrpStackLocation(pIrp);

	pOpenContext = pIoStackIrp->FileObject->FsContext;
	if (pOpenContext == NULL) {
			DBGPRINT(("Write: FileObject %p not yet associated with a device\n", pIoStackIrp->FileObject));
            return STATUS_INVALID_HANDLE;
    }

    if (pIrp->MdlAddress == NULL) {
            DBGPRINT(("Write: NULL MDL address on IRP %p\n", pIrp));
            return STATUS_INVALID_PARAMETER;            
    }

    if(pIoStackIrp) {
		// Try to get a virtual address for the MDL.
        pWriteDataBuffer = MmGetSystemAddressForMdlSafe(pIrp->MdlAddress, NormalPagePriority);
    
		if (pWriteDataBuffer == NULL) {
            DBGPRINT(("Write: MmGetSystemAddr failed for IRP %p, MDL %p\n", pIrp, pIrp->MdlAddress));
            return STATUS_INSUFFICIENT_RESOURCES;
        }
                            
        /*
		* We need to verify that the string is NULL terminated. Bad things can happen
		* if we access memory not valid while in the Kernel.
		*/
		if(IsStringTerminated(pWriteDataBuffer, pIoStackIrp->Parameters.Write.Length)) {
			DBGPRINT((pWriteDataBuffer));
		} else {
			DBGPRINT(("Write: String is not ZeroTerminated!"));
		}
    }

	pIrp->IoStatus.Status = NtStatus;
    pIrp->IoStatus.Information = 0;
    IoCompleteRequest(pIrp, IO_NO_INCREMENT);

    return NtStatus;
}



NTSTATUS netgIO_WriteBufferedIO(PDEVICE_OBJECT pDeviceObject, PIRP pIrp) {
    NTSTATUS NtStatus = STATUS_SUCCESS;
    PIO_STACK_LOCATION pIoStackIrp = NULL;
    PCHAR pWriteDataBuffer;

    DBGPRINT(("      netgIO_WriteBufferedIO Called \n"));
    
    /*
     * Each time the IRP is passed down the driver stack a new stack location is added
     * specifying certain parameters for the IRP to the driver.
     */
    pIoStackIrp = IoGetCurrentIrpStackLocation(pIrp);
    
    if(pIoStackIrp) {
        pWriteDataBuffer = (PCHAR)pIrp->AssociatedIrp.SystemBuffer;
    
        if(pWriteDataBuffer) {                             
            /*
             * We need to verify that the string is NULL terminated. Bad things can happen
             * if we access memory not valid while in the Kernel.
             */
			if(IsStringTerminated(pWriteDataBuffer, pIoStackIrp->Parameters.Write.Length)) {
				DBGPRINT((pWriteDataBuffer));
			} else {
				DBGPRINT(("Write: String is not ZeroTerminated!"));
			}
        }
    }

    return NtStatus;
}



NTSTATUS netgIO_WriteNeither(PDEVICE_OBJECT pDeviceObject, PIRP pIrp) {
    NTSTATUS NtStatus = STATUS_SUCCESS;
    PIO_STACK_LOCATION pIoStackIrp = NULL;
    PCHAR pWriteDataBuffer;

    DBGPRINT(("      netgIO_WriteNeither Called \n"));
    
    /*
     * Each time the IRP is passed down the driver stack a new stack location is added
     * specifying certain parameters for the IRP to the driver.
     */
    pIoStackIrp = IoGetCurrentIrpStackLocation(pIrp);
    
    if(pIoStackIrp) {
        /*
         * We need this in an exception handler or else we could trap.
         */
        __try {
        
                ProbeForRead(pIrp->UserBuffer, pIoStackIrp->Parameters.Write.Length, TYPE_ALIGNMENT(char));
                pWriteDataBuffer = pIrp->UserBuffer;
            
                if(pWriteDataBuffer) {                             
                    /*
                     * We need to verify that the string is NULL terminated. Bad things can happen
                     * if we access memory not valid while in the Kernel.
                     */
                   if(IsStringTerminated(pWriteDataBuffer, pIoStackIrp->Parameters.Write.Length)) {
                        DBGPRINT((pWriteDataBuffer));
				   } else {
						DBGPRINT(("Write: String is not ZeroTerminated!"));
                   }
                }

        } __except( EXCEPTION_EXECUTE_HANDLER ) {
              NtStatus = GetExceptionCode();     
        }

    }

    return NtStatus;
}


BOOLEAN IsStringTerminated(PCHAR pString, UINT uiLength) {
    BOOLEAN bStringIsTerminated = FALSE;
    UINT uiIndex = 0;

    while(uiIndex < uiLength && bStringIsTerminated == FALSE) {
        if(pString[uiIndex] == '\0') {
            bStringIsTerminated = TRUE;
        } else {
           uiIndex++;
        }
    }

    return bStringIsTerminated;
}


NTSTATUS netgSendToProtocol(PDEVICE_OBJECT pDeviceObject, PIRP pIrp) {
	PIO_STACK_LOCATION  pIrpSp;
	NTSTATUS            NtStatus = STATUS_SUCCESS;
	ULONG               BytesReturned = 0;
	PUCHAR              ioBuffer = NULL;
	ULONG               inputBufferLength;
	ULONG               outputBufferLength, Remaining;
	POPEN_CONTEXT       pOpenContext;

	DBGPRINT(("      netgSendToProtocol Called \n"));

	UNREFERENCED_PARAMETER(pDeviceObject);

	pIrpSp = IoGetCurrentIrpStackLocation(pIrp);

	pOpenContext = pIrpSp->FileObject->FsContext;
	ioBuffer = pIrp->AssociatedIrp.SystemBuffer;
	inputBufferLength  = pIrpSp->Parameters.DeviceIoControl.InputBufferLength;
	outputBufferLength = pIrpSp->Parameters.DeviceIoControl.OutputBufferLength;
	Remaining = outputBufferLength;

	DBGPRINT(("==> netgSendToProtocol: FileObject %p\n", pIrpSp->FileObject ));

	if( !pOpenContext ) {
		DBGPRINT(( "      Invalid Handle\n" ));
		NtStatus = STATUS_INVALID_HANDLE;
		goto CompleteTheIRP;
	}

	DBGPRINT(("     Len: %d Data: %x%x...\n", pIrpSp->Parameters.DeviceIoControl.InputBufferLength, ioBuffer[0], ioBuffer[1]));


CompleteTheIRP:
	if (NtStatus != STATUS_PENDING) {
		pIrp->IoStatus.Information = BytesReturned;
		pIrp->IoStatus.Status = NtStatus;
		IoCompleteRequest(pIrp, IO_NO_INCREMENT);
	}

	DBGPRINT(("<== netgSendToProtocol\n"));
	return NtStatus;
}


NTSTATUS netgSendToMiniport(PDEVICE_OBJECT pDeviceObject, PIRP pIrp) {
	PIO_STACK_LOCATION  pIrpSp;
	NTSTATUS            NtStatus = STATUS_SUCCESS;
	NDIS_STATUS         Status;
	ULONG               BytesReturned = 0;
	PUCHAR              ioBuffer = NULL;
	ULONG               inputBufferLength;
	ULONG               outputBufferLength, Remaining;
	POPEN_CONTEXT       pOpenContext;
	PNDIS_PACKET        pNdisPacket;
    PNDIS_BUFFER        pNdisBuffer;

	DBGPRINT(("      netgSendToMiniport Called \n"));

	UNREFERENCED_PARAMETER(pDeviceObject);

	pIrpSp = IoGetCurrentIrpStackLocation(pIrp);

	pOpenContext = pIrpSp->FileObject->FsContext;
	ioBuffer = pIrp->AssociatedIrp.SystemBuffer;
	inputBufferLength  = pIrpSp->Parameters.DeviceIoControl.InputBufferLength;
	outputBufferLength = pIrpSp->Parameters.DeviceIoControl.OutputBufferLength;
	Remaining = outputBufferLength;

	DBGPRINT(("==> netgSendToMiniport: FileObject %p\n", pIrpSp->FileObject ));

	if( !pOpenContext ) {
		DBGPRINT(( "      Invalid Handle\n" ));
		NtStatus = STATUS_INVALID_HANDLE;
		goto CompleteTheIRP;
	}

	DBGPRINT(("     Len: %d Data: %x%x...\n", pIrpSp->Parameters.DeviceIoControl.InputBufferLength, ioBuffer[0], ioBuffer[1]));

	// Allocate a send packet.
	pNdisPacket = NULL;
	NdisAllocatePacket(&Status, &pNdisPacket, pOpenContext->SendPacketPool);
	if (Status != NDIS_STATUS_SUCCESS) {
		DBGPRINT(("     netgSendToMiniport: open %p, failed to alloc send pkt\n", pOpenContext));
        NtStatus = STATUS_INSUFFICIENT_RESOURCES;
        goto CompleteTheIRP;
    }

	// Allocate a send buffer.
	NdisAllocateBuffer(&Status, &pNdisBuffer, pOpenContext->SendBufferPool, ioBuffer, inputBufferLength);
    if (Status != NDIS_STATUS_SUCCESS) {
        NdisFreePacket(pNdisPacket);
        DBGPRINT(("     netgSendToMiniport: open %p, failed to alloc send buf\n", pOpenContext));
        NtStatus = STATUS_INSUFFICIENT_RESOURCES;
		goto CompleteTheIRP;
	}

	IoMarkIrpPending(pIrp);
	((PNPROT_SEND_PACKET_RSVD)&((pNdisPacket)->ProtocolReserved[0]))->RefCount = 1;
	(((PNPROT_SEND_PACKET_RSVD)&((pNdisPacket)->ProtocolReserved[0]))->pIrp) = pIrp;
	NtStatus = STATUS_PENDING;

	pNdisBuffer->Next = NULL;
	NdisChainBufferAtFront(pNdisPacket, pNdisBuffer);
	NdisSendPackets(pOpenContext->pAdapt->BindingHandle, &pNdisPacket, 1);

CompleteTheIRP:
	if (NtStatus != STATUS_PENDING) {
		pIrp->IoStatus.Information = BytesReturned;
		pIrp->IoStatus.Status = NtStatus;
		IoCompleteRequest(pIrp, IO_NO_INCREMENT);
	}

	DBGPRINT(("<== netgSendToMiniport\n"));
	return NtStatus;
}

void netgSendToMiniportComplete(IN NDIS_HANDLE ProtocolBindingContext, IN PNDIS_PACKET pNdisPacket, IN NDIS_STATUS Status) {
	PIRP                        pIrp;
    PIO_STACK_LOCATION          pIrpSp;
	PADAPT						pAdapt;
    POPEN_CONTEXT				pOpenContext;
	PNDIS_BUFFER                pNdisBuffer;
	PVOID                       VirtualAddr;
	UINT                        BufferLength;
	UINT                        TotalLength;

	DBGPRINT(("==> netgSendToMiniportComplete\n"));

	pAdapt = (PADAPT)ProtocolBindingContext;
	pOpenContext = pAdapt->pOpenContext;
	pIrp = (((PNPROT_SEND_PACKET_RSVD)&((pNdisPacket)->ProtocolReserved[0]))->pIrp);

	// Free buffer
	NdisGetFirstBufferFromPacket(pNdisPacket, &pNdisBuffer, &VirtualAddr, &BufferLength, &TotalLength);
	NdisFreeBuffer(pNdisBuffer);

	// Free packet
	if (NdisInterlockedDecrement((PLONG)&((PNPROT_SEND_PACKET_RSVD)&((pNdisPacket)->ProtocolReserved[0]))->RefCount) == 0) {                                                                           \
		NdisFreePacket(pNdisPacket);
	}   

CompleteTheIRP:
	// Complete the Write IRP with the right status.
	pIrpSp = IoGetCurrentIrpStackLocation(pIrp);
    if (Status == NDIS_STATUS_SUCCESS) {
		pIrp->IoStatus.Information = pIrpSp->Parameters.Write.Length;
        pIrp->IoStatus.Status = STATUS_SUCCESS;
    } else {
        pIrp->IoStatus.Information = 0;
        pIrp->IoStatus.Status = STATUS_UNSUCCESSFUL;
    }
	IoCompleteRequest(pIrp, IO_NO_INCREMENT);

	DBGPRINT(("<== netgSendToMiniportComplete\n"));
}
