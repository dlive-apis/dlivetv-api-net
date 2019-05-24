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

using System.Threading.Tasks;
using dlivetv_unofficial_api_net.API;

namespace dlivetv_unofficial_api_net.Classes
{
    public class MessageSent
    {
        internal readonly string _accessKey;

        internal readonly string _blockchainName;

        public MessageSent(string blockchainName, string accessKey)
        {
            _blockchainName = blockchainName;
            _accessKey = accessKey;
        }

        public string MessageId { get; internal set; }

        public string Content { get; internal set; }

        public bool Subscribing { get; internal set; }

        public string Role { get; internal set; }

        public string RoomRole { get; internal set; }

        public async Task DeleteMessage()
        {
            await new Moderation(_blockchainName, _accessKey).DeleteChatMessage(_blockchainName, MessageId)
                .ContinueWith(
                    complete => { complete.Dispose(); });
        }

        public class Sender
        {
            internal readonly string _accessKey;

            internal readonly string _blockchainName;

            public Sender(string blockchainName, string accessKey)
            {
                _blockchainName = blockchainName;
                _accessKey = accessKey;
            }

            public string UserId { get; internal set; }

            public string Username { get; internal set; }
            public string DisplayName { get; internal set; }

            public string Avatar { get; internal set; }

            public string PartnerStatus { get; internal set; }

            public async Task BanUser()
            {
                // Todo
            }

            public async Task MuteUser()
            {
                // Todo
            }
        }
    }
}