// ***********************************************************************
// Project          : dlivetv-unofficial-api-net
// Author           : Nils Kleinert
// Created          : 04/19/2019
// 
// Last Modified On : 05/15/2019
// GitHub           : https://github.com/dlive-apis/dlivetv-api-net
// ***********************************************************************
// dlivetv-unofficial-api-net - unofficial DLive.tv API
// Copyright (C) 2019 Nils Kleinert
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// ***********************************************************************

using System;
using dlivetv_unofficial_api_net.Classes;

namespace dlivetv_unofficial_api_net
{
    public class Events
    {
        public event Action<MessageSent, MessageSent.Sender> OnMessageReceived;

        public event Action<ChatDeleted> OnMessageDeleted;

        public event Action<bool, string> OnConnect;

        public event Action OnDisconnect;

        internal void FireOnMessageReceived(MessageSent messageSent, MessageSent.Sender sender)
        {
            if (messageSent == null) throw new ArgumentNullException(nameof(messageSent));
            if (sender == null) throw new ArgumentNullException(nameof(sender));

            OnMessageReceived?.Invoke(messageSent, sender);
        }

        internal void FireOnConnect(bool success, string error = "")
        {
            if (error == null) throw new ArgumentNullException(nameof(error));

            OnConnect?.Invoke(success, error);
        }

        internal void FireOnDisconnect()
        {
            OnDisconnect?.Invoke();
        }

        internal void FireOnMessageDeleted(ChatDeleted chatDelete)
        {
            if (chatDelete == null) throw new ArgumentNullException(nameof(chatDelete));

            OnMessageDeleted?.Invoke(chatDelete);
        }
    }
}