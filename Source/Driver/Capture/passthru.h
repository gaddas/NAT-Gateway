#ifndef __PASSTHRU__H
#define __PASSTHRU__H

/*++
                  Companion Sample Code for the Article
        "Extending the Microsoft PassThru NDIS Intermediate Driver"

Portions Copyright (c) 1992-2000  Microsoft Corporation; used by permission.
Portions Copyright (c) 2003 Printing Communications Associates, Inc. (PCAUSA)

THIS SOFTWARE IS PROVIDED ``AS IS'' AND WITHOUT ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.

Module Name:

    passthru.h

Abstract:

    Ndis Intermediate Miniport driver sample. This is a passthru driver.

Author:

Environment:


Revision History:

 
--*/

#ifdef NDIS51_MINIPORT
#define PASSTHRU_MAJOR_NDIS_VERSION            5
#define PASSTHRU_MINOR_NDIS_VERSION            1
#else
#define PASSTHRU_MAJOR_NDIS_VERSION            4
#define PASSTHRU_MINOR_NDIS_VERSION            0
#endif

#ifdef NDIS51
#define PASSTHRU_PROT_MAJOR_NDIS_VERSION    5
#define PASSTHRU_PROT_MINOR_NDIS_VERSION    0
#else
#define PASSTHRU_PROT_MAJOR_NDIS_VERSION    4
#define PASSTHRU_PROT_MINOR_NDIS_VERSION    0
#endif

#define MAX_BUNDLEID_LENGTH 50

#define TAG 'ImPa'
#define WAIT_INFINITE 0



//advance declaration
typedef struct _ADAPT ADAPT, *PADAPT;
// BEGIN_PTUSERIO
typedef struct _OPEN_CONTEXT OPEN_CONTEXT, *POPEN_CONTEXT;
typedef struct _NDIS_REQUEST_EX NDIS_REQUEST_EX, *PNDIS_REQUEST_EX;

typedef void (*LOCAL_REQUEST_COMPLETE_HANDLER)(
   IN  PADAPT              pAdapt,
   IN  PNDIS_REQUEST_EX    pLocalRequest,
   IN  NDIS_STATUS         Status
   );

typedef struct _NDIS_REQUEST_EX {
   NDIS_REQUEST                     Request;
   LOCAL_REQUEST_COMPLETE_HANDLER   RequestCompleteHandler;
   PVOID                            RequestContext;
   NDIS_STATUS                      RequestStatus;
   NDIS_EVENT                       RequestEvent;
}  NDIS_REQUEST_EX, *PNDIS_REQUEST_EX;

typedef struct _OPEN_CONTEXT {
	ULONG					RefCount;
	NDIS_SPIN_LOCK			Lock;
	BOOLEAN					bAdapterClosed;
	PADAPT					pAdapt;
	NDIS_REQUEST_EX			LocalRequest;

	NDIS_STRING             DeviceName;			// used in NdisOpenAdapter
    NDIS_STRING				DeviceDescr;		// friendly name

    NDIS_HANDLE				SendPacketPool;
    NDIS_HANDLE             SendBufferPool;
    NDIS_HANDLE             RecvPacketPool;
	NDIS_HANDLE             RecvBufferPool;
}  OPEN_CONTEXT, *POPEN_CONTEXT;
// END_PTUSERIO

NDIS_HANDLE        NdisWrapperHandle;

extern NTSTATUS DriverEntry(IN PDRIVER_OBJECT DriverObject, IN PUNICODE_STRING RegistryPath);

NDIS_STATUS PtRegisterDevice(void);
NDIS_STATUS PtDeregisterDevice(void);
void PtUnloadProtocol(void);

PADAPT PtLookupAdapterByName(IN PUCHAR   pNameBuffer, IN USHORT   NameBufferLength, IN BOOLEAN  bUseVirtualName);
void PtRefAdapter(PADAPT pAdapt);
void PtDerefAdapter(PADAPT pAdapt);

//
// Protocol proto-types
//
extern void PtOpenAdapterComplete(IN NDIS_HANDLE ProtocolBindingContext, IN NDIS_STATUS Status, IN NDIS_STATUS OpenErrorStatus);
extern void PtCloseAdapterComplete(IN NDIS_HANDLE ProtocolBindingContext, IN NDIS_STATUS Status);
extern void PtResetComplete(IN NDIS_HANDLE ProtocolBindingContext, IN NDIS_STATUS Status);
extern void PtRequestComplete(IN NDIS_HANDLE ProtocolBindingContext, IN PNDIS_REQUEST NdisRequest, IN NDIS_STATUS Status);
extern void PtStatus(IN NDIS_HANDLE ProtocolBindingContext, IN NDIS_STATUS GeneralStatus, IN PVOID StatusBuffer, IN UINT StatusBufferSize);
extern void PtStatusComplete(IN NDIS_HANDLE ProtocolBindingContext);
extern void PtSendComplete(IN NDIS_HANDLE ProtocolBindingContext, IN PNDIS_PACKET Packet, IN NDIS_STATUS Status);
extern void PtTransferDataComplete(IN NDIS_HANDLE ProtocolBindingContext, IN PNDIS_PACKET Packet, IN NDIS_STATUS Status, IN UINT BytesTransferred);
extern NDIS_STATUS PtReceive(IN NDIS_HANDLE ProtocolBindingContext, IN NDIS_HANDLE MacReceiveContext, IN PVOID HeaderBuffer, IN UINT HeaderBufferSize, IN PVOID LookAheadBuffer, IN UINT LookaheadBufferSize, IN UINT PacketSize);
extern void PtReceiveComplete(IN NDIS_HANDLE ProtocolBindingContext);
extern int PtReceivePacket(IN NDIS_HANDLE ProtocolBindingContext, IN PNDIS_PACKET Packet);
extern void PtBindAdapter(OUT PNDIS_STATUS Status, IN NDIS_HANDLE BindContext, IN PNDIS_STRING DeviceName, IN PVOID SystemSpecific1, IN PVOID SystemSpecific2);
extern void PtUnbindAdapter(OUT PNDIS_STATUS Status, IN NDIS_HANDLE ProtocolBindingContext, IN NDIS_HANDLE UnbindContext); 
void PtUnload(IN PDRIVER_OBJECT DriverObject);

extern NDIS_STATUS PtPNPHandler(IN NDIS_HANDLE ProtocolBindingContext, IN PNET_PNP_EVENT pNetPnPEvent);
NDIS_STATUS PtPnPNetEventReconfigure(IN PADAPT pAdapt, IN PNET_PNP_EVENT pNetPnPEvent);    
NDIS_STATUS PtPnPNetEventSetPower (IN PADAPT pAdapt, IN PNET_PNP_EVENT pNetPnPEvent);
    

//
// Miniport proto-types
//
NDIS_STATUS MPInitialize(OUT PNDIS_STATUS OpenErrorStatus, OUT PUINT SelectedMediumIndex, IN PNDIS_MEDIUM MediumArray, IN UINT MediumArraySize, IN NDIS_HANDLE MiniportAdapterHandle, IN NDIS_HANDLE WrapperConfigurationContext);
void MPSendPackets(IN NDIS_HANDLE MiniportAdapterContext, IN PPNDIS_PACKET PacketArray, IN UINT NumberOfPackets);
NDIS_STATUS MPSend(IN NDIS_HANDLE MiniportAdapterContext, IN PNDIS_PACKET Packet, IN UINT Flags);
NDIS_STATUS MPQueryInformation(IN NDIS_HANDLE MiniportAdapterContext, IN NDIS_OID Oid, IN PVOID InformationBuffer, IN ULONG InformationBufferLength, OUT PULONG BytesWritten, OUT PULONG BytesNeeded);
NDIS_STATUS MPSetInformation(IN NDIS_HANDLE MiniportAdapterContext, IN NDIS_OID Oid, IN PVOID InformationBuffer, IN ULONG InformationBufferLength, OUT PULONG BytesRead, OUT PULONG BytesNeeded);
void MPReturnPacket(IN NDIS_HANDLE MiniportAdapterContext, IN PNDIS_PACKET Packet);
NDIS_STATUS MPTransferData(OUT PNDIS_PACKET Packet, OUT PUINT BytesTransferred, IN NDIS_HANDLE MiniportAdapterContext, IN NDIS_HANDLE MiniportReceiveContext, IN UINT ByteOffset, IN UINT BytesToTransfer);
void MPHalt(IN NDIS_HANDLE MiniportAdapterContext);
void MPQueryPNPCapabilities(OUT PADAPT MiniportProtocolContext, OUT PNDIS_STATUS Status);
NDIS_STATUS MPSetMiniportSecondary (IN PADAPT Secondary, IN PADAPT Primary);

#ifdef NDIS51_MINIPORT

VOID
MPCancelSendPackets(
    IN NDIS_HANDLE            MiniportAdapterContext,
    IN PVOID                  CancelId
    );

VOID
MPAdapterShutdown(
    IN NDIS_HANDLE                MiniportAdapterContext
    );

VOID
MPDevicePnPEvent(
    IN NDIS_HANDLE                MiniportAdapterContext,
    IN NDIS_DEVICE_PNP_EVENT      DevicePnPEvent,
    IN PVOID                      InformationBuffer,
    IN ULONG                      InformationBufferLength
    );

#endif // NDIS51_MINIPORT


NDIS_STATUS MPPromoteSecondary (IN PADAPT pAdapt);
NDIS_STATUS MPBundleSearchAndSetSecondary (IN PADAPT pAdapt);
void MPProcessSetPowerOid(IN OUT PNDIS_STATUS pNdisStatus, IN PADAPT pAdapt, IN PVOID InformationBuffer, IN ULONG InformationBufferLength, OUT PULONG BytesRead, OUT PULONG BytesNeeded);

//
// There should be no DbgPrint's in the Free version of the driver
//
#if DBG

#define DBGPRINT(Fmt)                                        \
    {                                                        \
        DbgPrint("Passthru: ");                                \
        DbgPrint Fmt;                                        \
    }

#else // if DBG

#define DBGPRINT(Fmt)                                            

#endif // if DBG 

#define    NUM_PKTS_IN_POOL    256


//
// Protocol reserved part of a sent packet that is allocated by us.
//
typedef struct _SEND_RSVD
{
    PNDIS_PACKET    OriginalPkt;
} SEND_RSVD, *PSEND_RSVD;

//
// Miniport reserved part of a received packet that is allocated by
// us. Note that this should fit into the MiniportReserved space
// in an NDIS_PACKET.
//
typedef struct _RECV_RSVD
{
    PNDIS_PACKET    OriginalPkt;
} RECV_RSVD, *PRECV_RSVD;

C_ASSERT(sizeof(RECV_RSVD) <= sizeof(((PNDIS_PACKET)0)->MiniportReserved));

//
// Event Codes related to the PassthruEvent Structure
//

typedef enum 
{
    Passthru_Invalid,
    Passthru_SetPower,
    Passthru_Unbind

} PASSSTHRU_EVENT_CODE, *PPASTHRU_EVENT_CODE; 

//
// Passthru Event with  a code to state why they have been state
//

typedef struct _PASSTHRU_EVENT
{
    NDIS_EVENT Event;
    PASSSTHRU_EVENT_CODE Code;

} PASSTHRU_EVENT, *PPASSTHRU_EVENT;


//
// Structure used by both the miniport as well as the protocol part of the intermediate driver
// to represent an adapter and its corres. lower bindings
//
typedef struct _ADAPT
{
    struct _ADAPT *                Next;
    
    NDIS_HANDLE                    BindingHandle;		// To the lower miniport
    NDIS_HANDLE                    MiniportHandle;		// NDIS Handle to for miniport up-calls
    NDIS_HANDLE                    SendPacketPoolHandle;
    NDIS_HANDLE                    RecvPacketPoolHandle;
    NDIS_STATUS                    Status;				// Open Status
    NDIS_EVENT                     Event;				// Used by bind/halt for Open/Close Adapter synch.
    NDIS_MEDIUM                    Medium;
    NDIS_REQUEST                   Request;				// This is used to wrap a request coming down
														// to us. This exploits the fact that requests
														// are serialized down to us.
    PULONG                         BytesNeeded;
    PULONG                         BytesReadOrWritten;
    BOOLEAN                        IndicateRcvComplete;
    
    BOOLEAN                        OutstandingRequests;	// TRUE iff a request is pending
                                                        // at the miniport below
    BOOLEAN                        QueuedRequest;		// TRUE iff a request is queued at
                                                        // this IM miniport

    BOOLEAN                        StandingBy;			// True - When the miniport or protocol is transitioning from a D0 to Standby (>D0) State
    BOOLEAN                        UnbindingInProcess;
    NDIS_SPIN_LOCK                 Lock;
                                                        // False - At all other times, - Flag is cleared after a transition to D0

    NDIS_DEVICE_POWER_STATE        MPDeviceState;		// Miniport's Device State 
    NDIS_DEVICE_POWER_STATE        PTDeviceState;		// Protocol's Device State 
    NDIS_STRING                    DeviceName;			// For initializing the miniport edge
// BEGIN_PTUSERIO
    NDIS_STRING                    LowerDeviceName;     // As passed to NdisOpenAdapter
    
    ULONG                          RefCount;
    POPEN_CONTEXT                  pOpenContext;
// END_PTUSERIO
    NDIS_EVENT                     MiniportInitEvent;	// For blocking UnbindAdapter while
                                                        // an IM Init is in progress.
    BOOLEAN                        MiniportInitPending;    // TRUE iff IMInit in progress
    NDIS_STATUS                    LastIndicatedStatus;    // The last indicated media status
    NDIS_STATUS                    LatestUnIndicateStatus; // The latest suppressed media status
    ULONG                          OutstandingSends;

   //
   // ATTENTION!!! Adapter names strings are appended to ADAPT...
   //
} ADAPT, *PADAPT;

extern    NDIS_HANDLE                        ProtHandle, DriverHandle;
extern    NDIS_MEDIUM                        MediumArray[4];
extern    PADAPT                             pAdaptList;
extern    NDIS_SPIN_LOCK                     GlobalLock;


#define ADAPT_MINIPORT_HANDLE(_pAdapt)    ((_pAdapt)->MiniportHandle)
#define ADAPT_DECR_PENDING_SENDS(_pAdapt)     \
    {                                         \
        NdisAcquireSpinLock(&(_pAdapt)->Lock);   \
        (_pAdapt)->OutstandingSends--;           \
        NdisReleaseSpinLock(&(_pAdapt)->Lock);   \
    }

//
// Custom Macros to be used by the passthru driver 
//
/*
BOOLEAN IsIMDeviceStateOn(PADAPT)
*/
#define IsIMDeviceStateOn(_pP)        ((_pP)->MPDeviceState == NdisDeviceStateD0 && (_pP)->PTDeviceState == NdisDeviceStateD0 ) 

#endif // __PASSTHRU__H