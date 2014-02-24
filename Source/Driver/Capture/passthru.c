/*++
                  Companion Sample Code for the Article
        "Extending the Microsoft PassThru NDIS Intermediate Driver"

Portions Copyright (c) 1992-2000  Microsoft Corporation; used by permission.
Portions Copyright (c) 2003 Printing Communications Associates, Inc. (PCAUSA)

THIS SOFTWARE IS PROVIDED ``AS IS'' AND WITHOUT ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 
Module Name:
 
    passthru.c

Abstract:

    Ndis Intermediate Miniport driver sample. This is a passthru driver.

Author:

Environment:


Revision History:


--*/


#include "precomp.h"
#pragma hdrstop

#pragma NDIS_INIT_FUNCTION(DriverEntry)

NDIS_HANDLE         ProtHandle = NULL;
NDIS_HANDLE         DriverHandle = NULL;
NDIS_MEDIUM         MediumArray[4] =
                    {
                        NdisMedium802_3,    // Ethernet
                        NdisMedium802_5,    // Token-ring
                        NdisMediumFddi,     // Fddi
                        NdisMediumWan       // NDISWAN
                    };

NDIS_SPIN_LOCK     GlobalLock;

PADAPT             pAdaptList = NULL;
LONG               MiniportCount = 0;



//
// To support ioctls from user-mode:
//

#define LINKNAME_STRING     L"\\DosDevices\\NetGate"
#define NTDEVICE_STRING     L"\\Device\\NetGate"

NDIS_HANDLE     NdisDeviceHandle = NULL;
PDEVICE_OBJECT  ControlDeviceObject = NULL;

enum _DEVICE_STATE
{
    PS_DEVICE_STATE_READY = 0,    // ready for create/delete
    PS_DEVICE_STATE_CREATING,    // create operation in progress
    PS_DEVICE_STATE_DELETING    // delete operation in progress
} ControlDeviceState = PS_DEVICE_STATE_READY;



NTSTATUS DriverEntry(IN PDRIVER_OBJECT DriverObject, IN PUNICODE_STRING RegistryPath) {
/*++
Routine Description:
    First entry point to be called, when this driver is loaded.
    Register with NDIS as an intermediate driver.
Arguments:
    DriverObject - pointer to the system's driver object structure for this driver
    RegistryPath - system's registry path for this driver
Return Value:
    STATUS_SUCCESS if all initialization is successful, STATUS_XXX error code if not.
--*/
    NDIS_STATUS                        Status;
    NDIS_PROTOCOL_CHARACTERISTICS      PChars;
    NDIS_MINIPORT_CHARACTERISTICS      MChars;
    NDIS_STRING                        Name;

    Status = NDIS_STATUS_SUCCESS;
    NdisAllocateSpinLock(&GlobalLock);

    NdisMInitializeWrapper(&NdisWrapperHandle, DriverObject, RegistryPath, NULL);

    do
    {
        //
        // Register the miniport with NDIS. Note that it is the miniport
        // which was started as a driver and not the protocol. Also the miniport
        // must be registered prior to the protocol since the protocol's BindAdapter
        // handler can be initiated anytime and when it is, it must be ready to
        // start driver instances.
        //

        NdisZeroMemory(&MChars, sizeof(NDIS_MINIPORT_CHARACTERISTICS));

        MChars.MajorNdisVersion = PASSTHRU_MAJOR_NDIS_VERSION;
        MChars.MinorNdisVersion = PASSTHRU_MINOR_NDIS_VERSION;

        MChars.InitializeHandler = MPInitialize;
        MChars.QueryInformationHandler = MPQueryInformation;
        MChars.SetInformationHandler = MPSetInformation;
        MChars.ResetHandler = NULL;
        MChars.TransferDataHandler = MPTransferData;
        MChars.HaltHandler = MPHalt;
#ifdef NDIS51_MINIPORT
        MChars.CancelSendPacketsHandler = MPCancelSendPackets;
        MChars.PnPEventNotifyHandler = MPDevicePnPEvent;
        MChars.AdapterShutdownHandler = MPAdapterShutdown;
#endif // NDIS51_MINIPORT

        //
        // We will disable the check for hang timeout so we do not
        // need a check for hang handler!
        //
        MChars.CheckForHangHandler = NULL;
        MChars.ReturnPacketHandler = MPReturnPacket;

        //
        // Either the Send or the SendPackets handler should be specified.
        // If SendPackets handler is specified, SendHandler is ignored
        //
        MChars.SendHandler = NULL;    // MPSend;
        MChars.SendPacketsHandler = MPSendPackets;

        Status = NdisIMRegisterLayeredMiniport(NdisWrapperHandle,
                                                  &MChars,
                                                  sizeof(MChars),
                                                  &DriverHandle);
        if (Status != NDIS_STATUS_SUCCESS)
        {
            break;
        }

#ifndef WIN9X
        NdisMRegisterUnloadHandler(NdisWrapperHandle, PtUnload);
#endif

        //
        // Now register the protocol.
        //
        NdisZeroMemory(&PChars, sizeof(NDIS_PROTOCOL_CHARACTERISTICS));
        PChars.MajorNdisVersion = PASSTHRU_PROT_MAJOR_NDIS_VERSION;
        PChars.MinorNdisVersion = PASSTHRU_PROT_MINOR_NDIS_VERSION;

        //
        // Make sure the protocol-name matches the service-name
        // (from the INF) under which this protocol is installed.
        // This is needed to ensure that NDIS can correctly determine
        // the binding and call us to bind to miniports below.
        //
        NdisInitUnicodeString(&Name, L"NetGate");    // Protocol name
        PChars.Name = Name;
        PChars.OpenAdapterCompleteHandler = PtOpenAdapterComplete;
        PChars.CloseAdapterCompleteHandler = PtCloseAdapterComplete;
        PChars.SendCompleteHandler = PtSendComplete;
        PChars.TransferDataCompleteHandler = PtTransferDataComplete;
    
        PChars.ResetCompleteHandler = PtResetComplete;
        PChars.RequestCompleteHandler = PtRequestComplete;
        PChars.ReceiveHandler = PtReceive;
        PChars.ReceiveCompleteHandler = PtReceiveComplete;
        PChars.StatusHandler = PtStatus;
        PChars.StatusCompleteHandler = PtStatusComplete;
        PChars.BindAdapterHandler = PtBindAdapter;
        PChars.UnbindAdapterHandler = PtUnbindAdapter;
        PChars.UnloadHandler = PtUnloadProtocol;

        PChars.ReceivePacketHandler = PtReceivePacket;
        PChars.PnPEventHandler= PtPNPHandler;

        NdisRegisterProtocol(&Status,
                             &ProtHandle,
                             &PChars,
                             sizeof(NDIS_PROTOCOL_CHARACTERISTICS));

        if (Status != NDIS_STATUS_SUCCESS)
        {
            NdisIMDeregisterLayeredMiniport(DriverHandle);
            break;
        }

        NdisIMAssociateMiniport(DriverHandle, ProtHandle);
    }
    while (FALSE);

    if (Status != NDIS_STATUS_SUCCESS)
    {
        NdisTerminateWrapper(NdisWrapperHandle, NULL);
    }

    return(Status);
}


NDIS_STATUS PtRegisterDevice(void) {
/*++
Routine Description:
    Register an ioctl interface - a device object to be used for this
    purpose is created by NDIS when we call NdisMRegisterDevice.

    This routine is called whenever a new miniport instance is
    initialized. However, we only create one global device object,
    when the first miniport instance is initialized. This routine
    handles potential race conditions with PtDeregisterDevice via
    the ControlDeviceState and MiniportCount variables.

    NOTE: do not call this from DriverEntry; it will prevent the driver
    from being unloaded (e.g. on uninstall).
Arguments:
    None
Return Value:
    NDIS_STATUS_SUCCESS if we successfully register a device object.
--*/

    NDIS_STATUS            Status = NDIS_STATUS_SUCCESS;
    UNICODE_STRING         DeviceName;
    UNICODE_STRING         DeviceLinkUnicodeString;
    PDRIVER_DISPATCH       DispatchTable[IRP_MJ_MAXIMUM_FUNCTION+1];

    DBGPRINT(("==>PtRegisterDevice\n"));

    NdisAcquireSpinLock(&GlobalLock);

    ++MiniportCount;
    
    if (1 == MiniportCount)
    {
        ASSERT(ControlDeviceState != PS_DEVICE_STATE_CREATING);

        //
        // Another thread could be running PtDeregisterDevice on
        // behalf of another miniport instance. If so, wait for
        // it to exit.
        //
        while (ControlDeviceState != PS_DEVICE_STATE_READY)
        {
            NdisReleaseSpinLock(&GlobalLock);
            NdisMSleep(1);
            NdisAcquireSpinLock(&GlobalLock);
        }

        ControlDeviceState = PS_DEVICE_STATE_CREATING;

        NdisReleaseSpinLock(&GlobalLock);

        NdisZeroMemory(DispatchTable, (IRP_MJ_MAXIMUM_FUNCTION+1) * sizeof(PDRIVER_DISPATCH));

// BEGIN_NGUSERIO
        DispatchTable[IRP_MJ_CREATE] = netgIO_Open;
        DispatchTable[IRP_MJ_CLEANUP] = netgIO_Cleanup;
        DispatchTable[IRP_MJ_CLOSE] = netgIO_Close;
		DispatchTable[IRP_MJ_READ] = netgIO_Read;
		DispatchTable[IRP_MJ_WRITE] = netgIO_WriteDirectIO;
        DispatchTable[IRP_MJ_DEVICE_CONTROL] = netgIO_IoControl;
// END_NGUSERIO

        NdisInitUnicodeString(&DeviceName, NTDEVICE_STRING);
        NdisInitUnicodeString(&DeviceLinkUnicodeString, LINKNAME_STRING);

        //
        // Create a device object and register our dispatch handlers
        //
        
        Status = NdisMRegisterDevice(
                    NdisWrapperHandle, 
                    &DeviceName,
                    &DeviceLinkUnicodeString,
                    &DispatchTable[0],
                    &ControlDeviceObject,
                    &NdisDeviceHandle
                    );

        NdisAcquireSpinLock(&GlobalLock);

        ControlDeviceState = PS_DEVICE_STATE_READY;
    }

    NdisReleaseSpinLock(&GlobalLock);

    DBGPRINT(("<==PtRegisterDevice: %x\n", Status));

    return (Status);
}

NDIS_STATUS PtDeregisterDevice(void) {
/*++
Routine Description:
    Deregister the ioctl interface. This is called whenever a miniport
    instance is halted. When the last miniport instance is halted, we
    request NDIS to delete the device object
Arguments:
    NdisDeviceHandle - Handle returned by NdisMRegisterDevice
Return Value:
    NDIS_STATUS_SUCCESS if everything worked ok
--*/

    NDIS_STATUS Status = NDIS_STATUS_SUCCESS;

    DBGPRINT(("==>PassthruDeregisterDevice\n"));

    NdisAcquireSpinLock(&GlobalLock);

    ASSERT(MiniportCount > 0);

    --MiniportCount;
    
    if (0 == MiniportCount)
    {
        //
        // All miniport instances have been halted. Deregister
        // the control device.
        //

        ASSERT(ControlDeviceState == PS_DEVICE_STATE_READY);

        //
        // Block PtRegisterDevice() while we release the control
        // device lock and deregister the device.
        // 
        ControlDeviceState = PS_DEVICE_STATE_DELETING;

        NdisReleaseSpinLock(&GlobalLock);

        if (NdisDeviceHandle != NULL)
        {
            Status = NdisMDeregisterDevice(NdisDeviceHandle);
            NdisDeviceHandle = NULL;
        }

        NdisAcquireSpinLock(&GlobalLock);
        ControlDeviceState = PS_DEVICE_STATE_READY;
    }

    NdisReleaseSpinLock(&GlobalLock);

    DBGPRINT(("<== PassthruDeregisterDevice: %x\n", Status));
    return Status;
    
}

void PtUnload(IN PDRIVER_OBJECT DriverObject) {
//
// PassThru driver unload function
//
    UNREFERENCED_PARAMETER(DriverObject);

    DBGPRINT(("PtUnload: entered\n"));
    PtUnloadProtocol();
    NdisIMDeregisterLayeredMiniport(DriverHandle);
    DBGPRINT(("PtUnload: done!\n"));
}

void PtRefAdapter( PADAPT pAdapt ) {
   NdisInterlockedIncrement( &pAdapt->RefCount );
}

void PtDerefAdapter( PADAPT pAdapt ) {
   if( !pAdapt )
   {
      return;
   }

   if( NdisInterlockedDecrement( &pAdapt->RefCount) == 0 )
   {
      DBGPRINT(( "PtDerefAdapter: Adapter: 0x%8.8X\n", pAdapt ? (ULONG )pAdapt : 0 ));

      //
      //  Free all resources on this adapter structure.
      //
      if (pAdapt->RecvPacketPoolHandle != NULL)
      {
         //
         // Free the packet pool that is used to indicate receives
         //
         NdisFreePacketPool(pAdapt->RecvPacketPoolHandle);

         pAdapt->RecvPacketPoolHandle = NULL;
      }

      if (pAdapt->SendPacketPoolHandle != NULL)
      {

         //
         //  Free the packet pool that is used to send packets below
         //

         NdisFreePacketPool(pAdapt->SendPacketPoolHandle);

         pAdapt->SendPacketPoolHandle = NULL;
      }

      // Deinitialize Filter Resources On This Adapter
      FltOnDeinitAdapter( pAdapt );

      NdisFreeMemory(pAdapt, 0, 0);
   }
}

PADAPT PtLookupAdapterByName(IN PUCHAR   pNameBuffer, IN USHORT   NameBufferLength, IN BOOLEAN  bUseVirtualName) {
   PADAPT *ppCursor, pAdapt = NULL;

   // Sanity Checks
   if( !pNameBuffer || !NameBufferLength )
   {
      return( NULL );
   }

   // Walk The Adapter List
   // ---------------------
   // Hold the global lock while walking. Otherwise, the adapter list could be altered at any point in
   // the list processing sequence.
   NdisAcquireSpinLock( &GlobalLock );

   for( ppCursor = &pAdaptList; *ppCursor != NULL;
      ppCursor = &(*ppCursor)->Next
      )
   {
      __try
      {
         if( bUseVirtualName )
         {
            // Check For Match Against Virtual Adapter Name
            if( ( (*ppCursor)->DeviceName.Length == NameBufferLength) &&
                  NdisEqualMemory( (*ppCursor)->DeviceName.Buffer, pNameBuffer, NameBufferLength ))
            {
               // Return Pointer To Found Adapter
               pAdapt = (*ppCursor);
               break;
            }
         }
         else
         {
            // Check For Match Against Lower Adapter Name
            if( ( (*ppCursor)->LowerDeviceName.Length == NameBufferLength) &&
                  NdisEqualMemory( (*ppCursor)->LowerDeviceName.Buffer, pNameBuffer, NameBufferLength))
            {
               // Return Pointer To Found Adapter
               pAdapt = (*ppCursor);
               break;
            }
         }
      }
      __except( EXCEPTION_EXECUTE_HANDLER )
      {
         pAdapt = NULL;
         break;
      }
   }

   //
   // Add Reference To Adapter Memory
   // -------------------------------
   // As soon as the spinlock is released (below) and before returning to the caller it is possible
   // for NDIS to unbind the selected adapter from the PassThru protocol. The reference counting scheme
   // insures that the memory pointed to by pAdapt will remain valid until the last call to
   // PtDerefAdapter.
   //
   if( pAdapt )
   {
	   PtRefAdapter( pAdapt );
   }

   NdisReleaseSpinLock( &GlobalLock );

   return( pAdapt );
}