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


void netgOnUnbindAdapter( POPEN_CONTEXT pOpenContext ) {
   PADAPT pAdapt = NULL;

   if( !pOpenContext ) return;
   DBGPRINT(("==> netgOnUnbindAdapter: Context %p\n", pOpenContext ));

   // Set Flag That Will Cause Future I/O To Fail
   pOpenContext->bAdapterClosed = TRUE;

   // Wait For Pending NDIS Operations To Complete
   // Cancel Pending User-Mode I/O Operations
   
   DBGPRINT(("<== netgOnUnbindAdapter\n"));
}

NTSTATUS netgOpenAdapter(IN PDEVICE_OBJECT pDeviceObject, IN PIRP pIrp, IN BOOLEAN bUseVirtualName) {
   PIO_STACK_LOCATION  pIrpSp;
   NTSTATUS            NtStatus = STATUS_SUCCESS;
   ULONG               BytesReturned = 0;
   PUCHAR              pNameBuffer = NULL;
   ULONG               NameBufferLength;
   PADAPT              pAdapt;
   POPEN_CONTEXT       pOpenContext;

   UNREFERENCED_PARAMETER(pDeviceObject);

   pIrpSp = IoGetCurrentIrpStackLocation(pIrp);

   DBGPRINT(("==> netgOpenAdapter: FileObject %p\n", pIrpSp->FileObject));
   
   pNameBuffer = pIrp->AssociatedIrp.SystemBuffer;
   NameBufferLength  = pIrpSp->Parameters.DeviceIoControl.InputBufferLength;

   DBGPRINT(( "   Looking For Name : \042%*.*ws\042\n",
      NameBufferLength/sizeof( wchar_t ),
      NameBufferLength/sizeof( wchar_t ),
      pNameBuffer
      ));

   //
   // Lookup Adapter By Name
   // ----------------------
   // If successful the lookup function has added a ref count to the found ADAPT
   // structure.
   //
   pAdapt = PtLookupAdapterByName( pNameBuffer, (USHORT )NameBufferLength, bUseVirtualName );

   if( !pAdapt ) {
      DBGPRINT(( "      Adapter Not Found\n" ));
      NtStatus = STATUS_OBJECT_NAME_NOT_FOUND;
      goto CompleteTheIRP;
   }

   DBGPRINT(( "      Found Adapter\n" ));

   // Fail Open If Unbind Is In Progress
   NdisAcquireSpinLock(&pAdapt->Lock);
   if( pAdapt->UnbindingInProcess ) {
      NdisReleaseSpinLock(&pAdapt->Lock);
      DBGPRINT(( "      Unbind In Process\n" ));
      PtDerefAdapter( pAdapt );

      NtStatus = STATUS_INVALID_DEVICE_STATE;
      goto CompleteTheIRP;
   }
   NdisReleaseSpinLock(&pAdapt->Lock);

   if( pAdapt->pOpenContext ) {
      DBGPRINT(( "      Handle Already Associated(1)\n" ));
      PtDerefAdapter( pAdapt );
      NtStatus = STATUS_DEVICE_BUSY;
      goto CompleteTheIRP;
   }

   pOpenContext = netgAllocateOpenContext( pAdapt );

   if( !pOpenContext ) {
      DBGPRINT(( "      Unable To Allocate Open Context\n" ));
      PtDerefAdapter( pAdapt );
      NtStatus = STATUS_INSUFFICIENT_RESOURCES;
      goto CompleteTheIRP;
   }

   //
   // Sanity Check For Concurrent Open Race Condition
   // -----------------------------------------------
   // At this point we enforce exclusive access on a per-binding basis.
   //
   // This logic deals with the situation where two concurrent adapter
   // opens could be in progress. We want an atomic mechanism that insures
   // that only one of the opens will be successful.
   //
   // This InterlockedXXX function performs an atomic operation: First it
   // compares pAdapt->pOpenContext with NULL, if they are equal, the function
   // puts pOpenContext into pAdapt->pOpenContext, and return NULL. Otherwise,
   // it return existing pAdapt->pOpenContext without changing anything.
   //
   // NOTE: This implementation is borrowed from the NDISPROT sample from
   // the Windows DDK.
   // 
   
   if ( InterlockedCompareExchangePointer (& (pAdapt->pOpenContext), pOpenContext, NULL) != NULL) {
      DBGPRINT(( "      Handle Already Associated(2)\n" ));
      PtDerefAdapter( pAdapt );

      NtStatus = STATUS_DEVICE_BUSY;
      goto CompleteTheIRP;
   }

   // Associate This Handle With The Open Context
   pIrpSp->FileObject->FsContext = pOpenContext;


   // Complete The IRP
CompleteTheIRP:

   pIrp->IoStatus.Information = BytesReturned;
   pIrp->IoStatus.Status = NtStatus;
   IoCompleteRequest(pIrp, IO_NO_INCREMENT);

   DBGPRINT(("<== netgOpenAdapter\n"));
   
   return NtStatus;
}

POPEN_CONTEXT netgAllocateOpenContext(PADAPT pAdapt) {
   POPEN_CONTEXT pOpenContext = NULL;

   NdisAllocateMemoryWithTag( &pOpenContext, sizeof( OPEN_CONTEXT ), TAG );

   if( !pOpenContext ) {
      return( NULL );
   }

   // Initialize The Open Context Structure
   NdisZeroMemory( pOpenContext, sizeof( OPEN_CONTEXT ) );
   NdisAllocateSpinLock( &pOpenContext->Lock );
   NdisInitializeEvent( &pOpenContext->LocalRequest.RequestEvent );

   // Initialize Filter Resources On This Handle
   FltOnInitOpenContext( pOpenContext );

   // Add Initial Reference To Open Context
   // -------------------------------------
   // Note that we already have added an implicit reference to the adapter
   // because of the PtLookupAdapterByName call.
   pOpenContext->RefCount = 1;
   pOpenContext->pAdapt = pAdapt;
   return( pOpenContext );
}

void netgRefOpenContext( POPEN_CONTEXT pOpenContext ) {
   PtRefAdapter( pOpenContext->pAdapt );
   NdisInterlockedIncrement( &pOpenContext->RefCount );
}

void netgDerefOpenContext( POPEN_CONTEXT pOpenContext ) {
   PADAPT pAdapt = NULL;

   if( !pOpenContext ) {
      return;
   }

   pAdapt = pOpenContext->pAdapt;
   if( NdisInterlockedDecrement( &pOpenContext->RefCount) == 0 )    {
      DBGPRINT(( "netgDerefOpenContext: Context: 0x%8.8X\n", pOpenContext ? (ULONG )pOpenContext : 0 ));
      NdisFreeSpinLock( &pOpenContext->Lock );

      // Deinitialize Filter Resources On This Handle
      FltOnDeinitOpenContext( pOpenContext );
      NdisFreeMemory(pOpenContext, 0, 0);
   }

   PtDerefAdapter( pAdapt );
}

NTSTATUS netgEnumerateBindings(IN PDEVICE_OBJECT pDeviceObject, IN PIRP pIrp) {
   PIO_STACK_LOCATION  pIrpSp;
   NTSTATUS            NtStatus = STATUS_SUCCESS;
   ULONG               BytesReturned = 0;
   PUCHAR              ioBuffer = NULL;
   ULONG               inputBufferLength;
   ULONG               outputBufferLength, Remaining;
   PADAPT              *ppCursor;

   UNREFERENCED_PARAMETER(pDeviceObject);

   pIrpSp = IoGetCurrentIrpStackLocation(pIrp);

   ioBuffer = pIrp->AssociatedIrp.SystemBuffer;
   inputBufferLength  = pIrpSp->Parameters.DeviceIoControl.InputBufferLength;
   outputBufferLength = pIrpSp->Parameters.DeviceIoControl.OutputBufferLength;
   Remaining = outputBufferLength;

   DBGPRINT(("==> netgEnumerateBindings: FileObject %p\n", pIrpSp->FileObject ));

   // Sanity Check On Length
   if( sizeof( UNICODE_NULL ) > Remaining ) {
      BytesReturned = 0;
      NtStatus = NDIS_STATUS_BUFFER_OVERFLOW;
      goto CompleteTheIRP;
   }

   // Walk The Adapter List
   NdisAcquireSpinLock( &GlobalLock );

   __try
   {
      // Insert List-Terminating NULL
      *((PWCHAR )ioBuffer) = UNICODE_NULL;

      BytesReturned = sizeof( UNICODE_NULL );
      Remaining -= sizeof( UNICODE_NULL );

      for( ppCursor = &pAdaptList; *ppCursor != NULL; ppCursor = &(*ppCursor)->Next) {
         // Sanity Check On Length
         if( (*ppCursor)->DeviceName.Length + sizeof( UNICODE_NULL) > Remaining ) {
            BytesReturned = 0;
            NtStatus = NDIS_STATUS_BUFFER_OVERFLOW;
            break;
         }

         // Add The Virtual DeviceName To The Buffer
         // ----------------------------------------
         // This name passed to NdisIMInitializeDeviceInstanceEx.
         NdisMoveMemory(
            ioBuffer,
            (*ppCursor)->DeviceName.Buffer,
            (*ppCursor)->DeviceName.Length
            );

         DBGPRINT(( "Adding VA Name : %d, %d, \042%*.*ws\042\n",
            (*ppCursor)->DeviceName.Length,
            (*ppCursor)->DeviceName.MaximumLength,
            (*ppCursor)->DeviceName.Length/sizeof( wchar_t ),
            (*ppCursor)->DeviceName.Length/sizeof( wchar_t ),
            (*ppCursor)->DeviceName.Buffer
            ));

         // Move Past Virtual DeviceName In Buffer
         Remaining -= (*ppCursor)->DeviceName.Length;
         BytesReturned += (*ppCursor)->DeviceName.Length;
         ioBuffer += (*ppCursor)->DeviceName.Length;

         // Add Name-Terminating NULL
         *((PWCHAR )ioBuffer) = UNICODE_NULL;

         Remaining -= sizeof( UNICODE_NULL );
         BytesReturned += sizeof( UNICODE_NULL );
         ioBuffer += sizeof( UNICODE_NULL );

         // Sanity Check On Length
         if( (*ppCursor)->LowerDeviceName.Length + sizeof( UNICODE_NULL ) > Remaining ) {
            BytesReturned = 0;
            NtStatus = NDIS_STATUS_BUFFER_OVERFLOW;
            break;
         }

         // Add The Lower DeviceName To The Buffer
         // --------------------------------------
         // This name passed to NdisOpenAdapter.
         NdisMoveMemory(
            ioBuffer,
            (*ppCursor)->LowerDeviceName.Buffer,
            (*ppCursor)->LowerDeviceName.Length
            );

         DBGPRINT(( "Adding LA Name : %d, %d, \042%*.*ws\042\n",
            (*ppCursor)->LowerDeviceName.Length,
            (*ppCursor)->LowerDeviceName.MaximumLength,
            (*ppCursor)->LowerDeviceName.Length/sizeof( wchar_t ),
            (*ppCursor)->LowerDeviceName.Length/sizeof( wchar_t ),
            (*ppCursor)->LowerDeviceName.Buffer
            ));

         // Move Past Lower DeviceName In Buffer
         Remaining -= (*ppCursor)->LowerDeviceName.Length;
         BytesReturned += (*ppCursor)->LowerDeviceName.Length;
         ioBuffer += (*ppCursor)->LowerDeviceName.Length;

         // Add Name-Terminating NULL
         *((PWCHAR )ioBuffer) = UNICODE_NULL;

         Remaining -= sizeof( UNICODE_NULL );
         BytesReturned += sizeof( UNICODE_NULL );
         ioBuffer += sizeof( UNICODE_NULL );

         // Add List-Terminating NULL
         // -------------------------
         // Space is already accomodated for this.
         *((PWCHAR )ioBuffer) = UNICODE_NULL;
      }
   }
   __except( EXCEPTION_EXECUTE_HANDLER )
   {
      BytesReturned = 0;
      NtStatus = STATUS_INVALID_PARAMETER;
   }

   NdisReleaseSpinLock( &GlobalLock );

CompleteTheIRP:
   if (NtStatus != STATUS_PENDING)
   {
      pIrp->IoStatus.Information = BytesReturned;
      pIrp->IoStatus.Status = NtStatus;
      IoCompleteRequest(pIrp, IO_NO_INCREMENT);
   }

   DBGPRINT(("<== netgEnumerateBindings\n"));
   return NtStatus;
}


NTSTATUS netgIO_IoControl(PDEVICE_OBJECT pDeviceObject, PIRP pIrp) {
    PIO_STACK_LOCATION  pIrpSp;
    NTSTATUS            NtStatus = STATUS_SUCCESS;
    ULONG               BytesReturned = 0;
    ULONG               FunctionCode;
    PUCHAR              ioBuffer = NULL;
    ULONG               inputBufferLength;
    ULONG               outputBufferLength;
    
    UNREFERENCED_PARAMETER(pDeviceObject);
    
    pIrpSp = IoGetCurrentIrpStackLocation(pIrp);
    
    ioBuffer = pIrp->AssociatedIrp.SystemBuffer;
    inputBufferLength  = pIrpSp->Parameters.DeviceIoControl.InputBufferLength;
    outputBufferLength = pIrpSp->Parameters.DeviceIoControl.OutputBufferLength;
    
    FunctionCode = pIrpSp->Parameters.DeviceIoControl.IoControlCode;
    
    DBGPRINT(("==> netgIO_IoControl: Context %p\n", (pIrpSp->FileObject)->FsContext ));
    
    switch (FunctionCode)
    {
		case IOCTL_NETGUSERIO_ENUMERATE:
			return( netgEnumerateBindings(pDeviceObject, pIrp));
			break;
		case IOCTL_NETGUSERIO_OPEN_ADAPTER:
			return( netgOpenAdapter(pDeviceObject, pIrp, FALSE));
			break;
		case IOCTL_NETGUSERIO_RECV_FROM_MINIPORT:
			return( netgRecvFromMiniport(pDeviceObject, pIrp));
			break;
		case IOCTL_NETGUSERIO_SEND_TO_PROTOCOL:
			return( netgSendToProtocol(pDeviceObject, pIrp));
			break;
		case IOCTL_NETGUSERIO_SEND_TO_MINIPORT:
			return( netgSendToMiniport(pDeviceObject, pIrp));
			break;
		case IOCTL_NETGUSERIO_DEBUG:
			return( netgDebug(pDeviceObject, pIrp));
			break;

        default:
			NtStatus = STATUS_NOT_SUPPORTED;
			break;
    }
    
    if (NtStatus != STATUS_PENDING)
    {
        pIrp->IoStatus.Information = BytesReturned;
        pIrp->IoStatus.Status = NtStatus;
        IoCompleteRequest(pIrp, IO_NO_INCREMENT);
    }
    
    DBGPRINT(("<== netgIO_IoControl\n"));
    
    return NtStatus;
}

NTSTATUS netgIO_UnSupportedFunction(PDEVICE_OBJECT pDeviceObject, PIRP pIrp) {
    NTSTATUS NtStatus = STATUS_NOT_SUPPORTED;
	PIO_STACK_LOCATION pIoStackIrp = NULL;

	pIoStackIrp = IoGetCurrentIrpStackLocation(pIrp);

	if(pIoStackIrp) {
		DBGPRINT(("      netgIO_UnSupportedFunction Called, %x\n", pIoStackIrp->MajorFunction));
	} else {
		DBGPRINT(("      netgIO_UnSupportedFunction Called\n"));
	}
    return NtStatus;
}




