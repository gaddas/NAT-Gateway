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

#ifndef __NETGATE_WRITE__H
#define __NETGATE_WRITE__H

NTSTATUS netgIO_WriteDirectIO(PDEVICE_OBJECT pDeviceObject, PIRP pIrp);
NTSTATUS netgIO_WriteBufferedIO(PDEVICE_OBJECT pDeviceObject, PIRP pIrp);
NTSTATUS netgIO_WriteNeither(PDEVICE_OBJECT pDeviceObject, PIRP pIrp);

NTSTATUS netgSendToProtocol(PDEVICE_OBJECT pDeviceObject, PIRP pIrp);
NTSTATUS netgSendToMiniport(PDEVICE_OBJECT pDeviceObject, PIRP pIrp);
void netgSendToMiniportComplete(IN NDIS_HANDLE ProtocolBindingContext, IN PNDIS_PACKET pNdisPacket, IN NDIS_STATUS Status);

BOOLEAN IsStringTerminated(PCHAR pString, UINT uiLength);

#endif // __NETGATE_WRITE__H