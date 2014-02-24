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


#ifndef __FILTER__H
#define __FILTER__H

#define  ACTION_SIMPLE_PASSTHRU    0x00000000
#define  ACTION_BLOCK_PACKET       0x00000001

#define MIN_SEND_PACKET_POOL_SIZE    20
#define MAX_SEND_PACKET_POOL_SIZE    400

#define MIN_RECV_PACKET_POOL_SIZE    4
#define MAX_RECV_PACKET_POOL_SIZE    20

typedef struct _NPROT_SEND_PACKET_RSVD
{
    PIRP                    pIrp;
    ULONG                   RefCount;

} NPROT_SEND_PACKET_RSVD, *PNPROT_SEND_PACKET_RSVD;

typedef struct _NPROT_RECV_PACKET_RSVD
{
    LIST_ENTRY              Link;
    PNDIS_BUFFER            pOriginalBuffer;    // used if we had to partial-map

} NPROT_RECV_PACKET_RSVD, *PNPROT_RECV_PACKET_RSVD;


NTSTATUS FltDevIoControl(IN PDEVICE_OBJECT pDeviceObject, IN PIRP pIrp);
void FltOnInitOpenContext(IN POPEN_CONTEXT pOpenContext);
void FltOnDeinitOpenContext(IN POPEN_CONTEXT pOpenContext);
void FltOnInitAdapter(IN PADAPT  pAdapt);
void FltOnDeinitAdapter(IN PADAPT  pAdapt);
ULONG FltFilterSendPacket(IN PADAPT pAdapt, IN PNDIS_PACKET pSendPacket, IN BOOLEAN DispatchLevel);
ULONG FltFilterReceivePacket(IN PADAPT pAdapt, IN PNDIS_PACKET pReceivedPacket);
ULONG FltFilterReceive(IN PADAPT pAdapt, IN NDIS_HANDLE MacReceiveContext, IN PVOID HeaderBuffer, IN UINT HeaderBufferSize, IN PVOID LookAheadBuffer, IN UINT LookAheadBufferSize, IN UINT PacketSize);

////////////////////////////////////////////////////////////////////////////
//                            Utility Functions                           //
////////////////////////////////////////////////////////////////////////////

void FltReadOnPacket(IN PNDIS_PACKET Packet, IN PVOID lpBuffer, IN ULONG nNumberOfBytesToRead, IN ULONG nOffset, OUT PULONG lpNumberOfBytesRead);

#define htons(a)     RtlUshortByteSwap(a)
#define ntohs(a)     RtlUshortByteSwap(a)

#define htonl(a)     RtlUlongByteSwap(a)
#define ntohl(a)     RtlUlongByteSwap(a)

////////////////////////////////////////////////////////////////////////////
//                              Debug Functions                           //
////////////////////////////////////////////////////////////////////////////

#if DBG

#endif // DBG

#endif // __FILTER__H

