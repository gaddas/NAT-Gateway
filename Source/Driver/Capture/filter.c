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


void FltOnInitOpenContext(IN POPEN_CONTEXT pOpenContext) {
	NDIS_STATUS             Status;

	DBGPRINT(("FltOnInitOpenContext: alloc send packet pool\n"));
	NdisAllocatePacketPoolEx(&Status,
							 &pOpenContext->SendPacketPool,
							 MIN_SEND_PACKET_POOL_SIZE,
							 MAX_SEND_PACKET_POOL_SIZE - MIN_SEND_PACKET_POOL_SIZE,
							 sizeof(NPROT_SEND_PACKET_RSVD));
       
	if (Status != NDIS_STATUS_SUCCESS) {
		DBGPRINT(("FltOnInitOpenContext: failed to alloc send packet pool: %x\n", Status));
		return;
    }

	DBGPRINT(("FltOnInitOpenContext: alloc recv packet pool\n"));
	NdisAllocatePacketPoolEx(&Status,
							 &pOpenContext->RecvPacketPool,
							 MIN_RECV_PACKET_POOL_SIZE,
							 MAX_RECV_PACKET_POOL_SIZE - MIN_RECV_PACKET_POOL_SIZE,
							 sizeof(NPROT_RECV_PACKET_RSVD));
       
	if (Status != NDIS_STATUS_SUCCESS) {
		DBGPRINT(("FltOnInitOpenContext: failed to alloc recv packet pool: %x\n", Status));
		return;
    }

	DBGPRINT(("FltOnInitOpenContext: alloc send buffer pool\n"));
	NdisAllocateBufferPool(&Status,
						   &pOpenContext->SendBufferPool,
						   MAX_SEND_PACKET_POOL_SIZE);
            
	if (Status != NDIS_STATUS_SUCCESS) {
		DBGPRINT(("FltOnInitOpenContext: failed to alloc send buffer pool: %x\n", Status));
		return;
	}

	DBGPRINT(("FltOnInitOpenContext: alloc recv buffer pool\n"));
	NdisAllocateBufferPool(&Status,
						   &pOpenContext->RecvBufferPool,
						   MAX_RECV_PACKET_POOL_SIZE);
        
	if (Status != NDIS_STATUS_SUCCESS) {
		DBGPRINT(("FltOnInitOpenContext: failed to alloc recv buffer pool: %x\n", Status));
		return;
    }

}
void FltOnDeinitOpenContext(IN POPEN_CONTEXT pOpenContext) {
	if (pOpenContext->SendPacketPool != NULL) {
        NdisFreePacketPool(pOpenContext->SendPacketPool);
        pOpenContext->SendPacketPool = NULL;
		DBGPRINT(("FltOnDeinitOpenContext: free send packet pool\n"));
    }
    if (pOpenContext->RecvPacketPool != NULL) {
        NdisFreePacketPool(pOpenContext->RecvPacketPool);
        pOpenContext->RecvPacketPool = NULL;
		DBGPRINT(("FltOnDeinitOpenContext: free recv packet pool\n"));
    }
    if (pOpenContext->SendBufferPool != NULL) {
        NdisFreeBufferPool(pOpenContext->SendBufferPool);
        pOpenContext->SendBufferPool = NULL;
		DBGPRINT(("FltOnDeinitOpenContext: free send buffer pool\n"));
    }
	if (pOpenContext->RecvBufferPool != NULL) {
        NdisFreeBufferPool(pOpenContext->RecvBufferPool);
        pOpenContext->RecvBufferPool = NULL;
		DBGPRINT(("FltOnDeinitOpenContext: free recv buffer pool\n"));
    }
}
void FltOnInitAdapter(IN PADAPT  pAdapt) {
   
}
void FltOnDeinitAdapter(IN PADAPT  pAdapt) {
   
}
ULONG FltFilterSendPacket(IN PADAPT pAdapt, IN PNDIS_PACKET   pSendPacket, IN BOOLEAN DispatchLevel) {
   ULONG                Action = ACTION_SIMPLE_PASSTHRU;
   
   // Hold Adapter Spin Lock When Using Filter Data
   if( DispatchLevel )
      NdisDprAcquireSpinLock(&pAdapt->Lock);
   else
      NdisAcquireSpinLock(&pAdapt->Lock);

ExitTheFilter:
   // Release Adapter Spin Lock After Filtering
   if( DispatchLevel )
      NdisDprReleaseSpinLock(&pAdapt->Lock);
   else
      NdisReleaseSpinLock(&pAdapt->Lock);

   return( Action );
}


ULONG FltFilterReceivePacket(IN PADAPT pAdapt,	IN	PNDIS_PACKET pReceivedPacket) {
   ULONG                RcvFltAction = ACTION_SIMPLE_PASSTHRU;

   // Hold Adapter Spin Lock When Using Filter Data
   NdisDprAcquireSpinLock(&pAdapt->Lock);

   
ExitTheFilter:
   // Release Adapter Spin Lock After Filtering
   NdisDprReleaseSpinLock(&pAdapt->Lock);
   return( RcvFltAction );
}


ULONG FltFilterReceive(IN PADAPT pAdapt, IN NDIS_HANDLE MacReceiveContext, IN PVOID HeaderBuffer, IN UINT HeaderBufferSize, IN PVOID LookAheadBuffer, IN UINT LookAheadBufferSize, IN UINT PacketSize) {
   ULONG                RcvFltAction = ACTION_SIMPLE_PASSTHRU;

   // Hold Adapter Spin Lock When Using Filter Data
   NdisDprAcquireSpinLock(&pAdapt->Lock);

ExitTheFilter:
   // Release Adapter Spin Lock After Filtering
   NdisDprReleaseSpinLock(&pAdapt->Lock);
   return( RcvFltAction );
}

////////////////////////////////////////////////////////////////////////////
//                            Utility Functions                           //
////////////////////////////////////////////////////////////////////////////
int IPv4AddrCompare( const void *pKey, const void *pElement )
{
   ULONG a1 = *(PULONG )pKey;
   ULONG a2 = *(PULONG )pElement;

   if( a1 == a2 )
   {
      return( 0 );
   }

   if( a1 < a2 )
   {
      return( -1 );
   }

   return( 1 );
}

void FltReadOnPacket(IN PNDIS_PACKET Packet, IN PVOID lpBufferIn,IN ULONG nNumberOfBytesToRead, IN ULONG nOffset, OUT PULONG lpNumberOfBytesRead) {
   PNDIS_BUFFER    CurrentBuffer;
   UINT            nBufferCount, TotalPacketLength;
   PUCHAR          VirtualAddress = NULL;
   PUCHAR          lpBuffer = (PUCHAR )lpBufferIn;
   UINT            CurrentLength, CurrentOffset;
   UINT            AmountToMove;

   // Sanity Check
   if( !Packet || !lpBuffer || !lpNumberOfBytesRead ) {
      if( lpNumberOfBytesRead ) {
         *lpNumberOfBytesRead = 0;
      }
      return;
   }

   *lpNumberOfBytesRead = 0;
   if (!nNumberOfBytesToRead)
      return;

   // Query Packet
   NdisQueryPacket(
      (PNDIS_PACKET )Packet,
      (PUINT )NULL,           // Physical Buffer Count
      (PUINT )&nBufferCount,  // Buffer Count
      &CurrentBuffer,         // First Buffer
      &TotalPacketLength      // TotalPacketLength
      );

   // Query The First Buffer
#if (defined(NDIS50) || defined(NDIS51))
   NdisQueryBufferSafe(
      CurrentBuffer,
      &VirtualAddress,
      &CurrentLength,
      NormalPagePriority
      );
#else
   NdisQueryBuffer(
      CurrentBuffer,
      &VirtualAddress,
      &CurrentLength
      );
#endif

   // Handle Possible Low-Resource Failure Of NdisQueryBufferSafe
   if( !VirtualAddress )
   {
      return;
   }

   __try
   {
      CurrentOffset = 0;

      while( nOffset || nNumberOfBytesToRead )
      {
         while( !CurrentLength )
         {
            NdisGetNextBuffer(
               CurrentBuffer,
               &CurrentBuffer
               );

            // If we've reached the end of the packet.  We return with what
            // we've done so far (which must be shorter than requested).
            if (!CurrentBuffer)
               __leave; // Leave __try and eventually return...

#if (defined(NDIS50) || defined(NDIS51))
            NdisQueryBufferSafe(
               CurrentBuffer,
               &VirtualAddress,
               &CurrentLength,
               NormalPagePriority
               );
#else
            NdisQueryBuffer(
               CurrentBuffer,
               &VirtualAddress,
               &CurrentLength
               );
#endif

            //
            // Handle Possible Low-Resource Failure Of NdisQueryBufferSafe
            //
            if( !VirtualAddress )
            {
               __leave; // Leave __try and eventually return...
            }

            CurrentOffset = 0;
         }

         if( nOffset )
         {
            // Compute how much data to move from this fragment
            if( CurrentLength > nOffset )
               CurrentOffset = nOffset;
            else
               CurrentOffset = CurrentLength;

            nOffset -= CurrentOffset;
            CurrentLength -= CurrentOffset;
         }

         if( nOffset )
         {
            CurrentLength = 0;
            continue;
         }

         if( !CurrentLength )
         {
            continue;
         }

         // Compute how much data to move from this fragment
         if (CurrentLength > nNumberOfBytesToRead)
            AmountToMove = nNumberOfBytesToRead;
         else
            AmountToMove = CurrentLength;

         // Copy the data.
         NdisMoveMemory(
            lpBuffer,
            &VirtualAddress[ CurrentOffset ],
            AmountToMove
            );

         // Update destination pointer
         lpBuffer += AmountToMove;

         // Update counters
         *lpNumberOfBytesRead +=AmountToMove;
         nNumberOfBytesToRead -=AmountToMove;
         CurrentLength = 0;
      }
   }
   __finally
   {
      //
      // lpNumberOfBytesRead may be less then specified if exception
      // occured...
      //
   }
}


////////////////////////////////////////////////////////////////////////////
//                              Debug Functions                           //
////////////////////////////////////////////////////////////////////////////

#if DBG

#endif // DBG


