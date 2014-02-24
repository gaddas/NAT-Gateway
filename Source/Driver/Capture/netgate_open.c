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


NTSTATUS netgIO_Open(IN PDEVICE_OBJECT pDeviceObject, IN PIRP pIrp) {
    PIO_STACK_LOCATION  pIrpSp;
    NTSTATUS            NtStatus = STATUS_SUCCESS;
	
    UNREFERENCED_PARAMETER(pDeviceObject);
    
    pIrpSp = IoGetCurrentIrpStackLocation(pIrp);
	
    pIrpSp->FileObject->FsContext = NULL;
    pIrpSp->FileObject->FsContext2 = NULL;
	
    DBGPRINT(("==> netgIO_Open: FileObject %p\n", pIrpSp->FileObject));
	
    pIrp->IoStatus.Information = 0;
    pIrp->IoStatus.Status = NtStatus;
	
    IoCompleteRequest(pIrp, IO_NO_INCREMENT);
	
    DBGPRINT(("<== netgIO_Open\n"));
	
    return NtStatus;
}


NTSTATUS netgIO_Cleanup(IN PDEVICE_OBJECT pDeviceObject, IN PIRP pIrp) {
    PIO_STACK_LOCATION  pIrpSp;
    NTSTATUS            NtStatus = STATUS_SUCCESS;
    POPEN_CONTEXT       pOpenContext;
	
    UNREFERENCED_PARAMETER(pDeviceObject);
    
    pIrpSp = IoGetCurrentIrpStackLocation(pIrp);
    pOpenContext = pIrpSp->FileObject->FsContext;
	
    DBGPRINT(("==> netgIO_Cleanup: Context %p\n", (pIrpSp->FileObject)->FsContext ));
	
    if( pOpenContext )
    {
    }

    pIrp->IoStatus.Information = 0;
    pIrp->IoStatus.Status = NtStatus;
    IoCompleteRequest(pIrp, IO_NO_INCREMENT);
	
    DBGPRINT(("<== netgIO_Cleanup\n"));
	
    return NtStatus;
} 


NTSTATUS netgIO_Close(PDEVICE_OBJECT pDeviceObject, PIRP pIrp) {
    PIO_STACK_LOCATION  pIrpSp;
    NTSTATUS            NtStatus = STATUS_SUCCESS;
    POPEN_CONTEXT       pOpenContext;
	
    UNREFERENCED_PARAMETER(pDeviceObject);
    
    pIrpSp = IoGetCurrentIrpStackLocation(pIrp);
    pOpenContext = pIrpSp->FileObject->FsContext;
	
    DBGPRINT(("==> netgIO_Close: Context %p\n", (pIrpSp->FileObject)->FsContext ));

    //
    // Undo IRP_MJ_CREATE Operations
    //
    pIrpSp->FileObject->FsContext = NULL;
    pIrpSp->FileObject->FsContext2 = NULL;
	
    if( pOpenContext )
    {
      if( pOpenContext->pAdapt )
      {
         NdisAcquireSpinLock(&(pOpenContext->pAdapt)->Lock);
         (pOpenContext->pAdapt)->pOpenContext = NULL;
         NdisReleaseSpinLock(&(pOpenContext->pAdapt)->Lock);
      }

      netgDerefOpenContext( pOpenContext );
    }
	
    pIrp->IoStatus.Information = 0;
    pIrp->IoStatus.Status = NtStatus;
    IoCompleteRequest(pIrp, IO_NO_INCREMENT);
	
    DBGPRINT(("<== netgIO_Close\n"));
	
    return NtStatus;
}


NTSTATUS netgDebug(PDEVICE_OBJECT pDeviceObject, PIRP pIrp) {
	PIO_STACK_LOCATION  pIrpSp;
	NTSTATUS            NtStatus = STATUS_SUCCESS;
	ULONG               BytesReturned = 0;
	PUCHAR              ioBuffer = NULL;
	ULONG               inputBufferLength;
	ULONG               outputBufferLength, Remaining;
	POPEN_CONTEXT       pOpenContext;

	DBGPRINT(("      netgDebug Called \n"));

	UNREFERENCED_PARAMETER(pDeviceObject);

	pIrpSp = IoGetCurrentIrpStackLocation(pIrp);

	pOpenContext = pIrpSp->FileObject->FsContext;
	ioBuffer = pIrp->AssociatedIrp.SystemBuffer;
	inputBufferLength  = pIrpSp->Parameters.DeviceIoControl.InputBufferLength;
	outputBufferLength = pIrpSp->Parameters.DeviceIoControl.OutputBufferLength;
	Remaining = outputBufferLength;

	DBGPRINT(("==> netgDebug: FileObject %p\n", pIrpSp->FileObject ));

	if( !pOpenContext ) {
		DBGPRINT(( "      Invalid Handle\n" ));
		NtStatus = STATUS_INVALID_HANDLE;
		goto CompleteTheIRP;
	}

	if(IsStringTerminated(ioBuffer, inputBufferLength)) {
		DBGPRINT((ioBuffer));
	} else {
		DBGPRINT(("      String is not ZeroTerminated!\n"));
	}


CompleteTheIRP:
	if (NtStatus != STATUS_PENDING) {
		pIrp->IoStatus.Information = BytesReturned;
		pIrp->IoStatus.Status = NtStatus;
		IoCompleteRequest(pIrp, IO_NO_INCREMENT);
	}

	DBGPRINT(("<== netgDebug\n"));
	return NtStatus;
}

