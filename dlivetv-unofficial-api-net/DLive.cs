// ***********************************************************************
// Project          : dlivetv-unofficial-api-net
// Author           : Nils Kleinert
// Created          : 04/19/2019
// 
// Last Modified On : 05/17/2019
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
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using dlivetv_unofficial_api_net.API;
using dlivetv_unofficial_api_net.Classes;
using dlivetv_unofficial_api_net.Helper;
using PureWebSockets;
using static Newtonsoft.Json.Linq.JObject;
using Message = dlivetv_unofficial_api_net.API.Message;

namespace dlivetv_unofficial_api_net
{
    public class DLive
    {
        internal static GraphQL.GraphQLClient GraphQL = new GraphQL.GraphQLClient();

        internal readonly string _accessKey;

        internal readonly string _blockchainName;
        internal readonly string _displayname;

        internal PureWebSocket _ws;

        public Events Events = new Events();

        public Message Message;

        public Moderation Moderation;

        public Self Self;
        public Util Utils;

        public DLive(string displayname, string accessKey)
        {
            if (string.IsNullOrWhiteSpace(displayname))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(displayname));
            if (string.IsNullOrWhiteSpace(accessKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(accessKey));


            Message = new Message(displayname, accessKey);
            Moderation = new Moderation(displayname, accessKey);
            Self = new Self(displayname, accessKey);
            Utils = new Util(displayname, accessKey);

            _displayname = displayname;
            _accessKey = accessKey;
            _blockchainName = new Util(_displayname, _accessKey).GetBlockchainNameFromDisplayName(displayname).Result;
        }

        public void ListenEvents(bool listen)
        {
            if (listen)
            {
                if (_ws != null)
                    if (_ws.State == WebSocketState.Open)
                        return;
                Events.FireOnConnect(InitWs().Result);
            }
            else
            {
                if (_ws.State == WebSocketState.Open) _ws.Disconnect();
                Events.FireOnDisconnect();
            }
        }

        public string GetCurrentBlockchainName()
        {
            return _blockchainName;
        }

        private async Task<bool> InitWs()
        {
            _ws = new PureWebSocket("wss://graphigostream.prd.dlive.tv", new PureWebSocketOptions
            {
                DebugMode = false,
                SubProtocols = new[] {"graphql-ws"}
            });
            _ws.OnOpened += () =>
            {
                _ws.SendAsync("{ \"type\": \"connection_init\", \"payload\": {} }").Wait();
                _ws.SendAsync("{\"id\":\"1\",\"type\":\"start\",\"payload\":{\"variables\":{\"streamer\":\"" +
                              _blockchainName +
                              "\"},\"extensions\":{},\"operationName\":\"StreamMessageSubscription\",\"query\":\"subscription StreamMessageSubscription($streamer: String!) {\\n streamMessageReceived(streamer: $streamer) {\\n type\\n ... on ChatGift {\\n id\\n gift\\n amount\\n recentCount\\n expireDuration\\n ...VStreamChatSenderInfoFrag\\n }\\n ... on ChatHost {\\n id\\n viewer\\n ...VStreamChatSenderInfoFrag\\n }\\n ... on ChatSubscription {\\n id\\n month\\n ...VStreamChatSenderInfoFrag\\n }\\n ... on ChatChangeMode {\\n mode\\n }\\n ... on ChatText {\\n id\\n content\\n ...VStreamChatSenderInfoFrag\\n }\\n ... on ChatFollow {\\n id\\n ...VStreamChatSenderInfoFrag\\n }\\n ... on ChatDelete {\\n ids\\n }\\n ... on ChatBan {\\n id\\n ...VStreamChatSenderInfoFrag\\n }\\n ... on ChatModerator {\\n id\\n ...VStreamChatSenderInfoFrag\\n add\\n }\\n ... on ChatEmoteAdd {\\n id\\n ...VStreamChatSenderInfoFrag\\n emote\\n }\\n }\\n}\\n\\nfragment VStreamChatSenderInfoFrag on SenderInfo {\\n subscribing\\n role\\n roomRole\\n sender {\\n id\\n username\\n displayname\\n avatar\\n partnerStatus\\n }\\n}\\n\"}}\t")
                    .Wait();
            };
            _ws.OnMessage += OnWebsocketMessage;
            _ws.OnClosed += reason => { Events.FireOnDisconnect(); };

            return _ws.ConnectAsync().Result;
        }

        private void OnWebsocketMessage(string message)
        {
            dynamic res = Parse(message);

            if (res.payload == null) return;
            //Console.WriteLine(res.payload);

            if (res.payload.data == null) return;
            if (res.payload.data.streamMessageReceived == null) return;

            foreach (var messageResult in res.payload.data.streamMessageReceived)
            {
                Console.WriteLine(messageResult.__typename);
                if (messageResult.__typename == "ChatText")
                    Events.FireOnMessageReceived(new MessageSent(_blockchainName, _accessKey)
                    {
                        Content = messageResult.content,
                        MessageId = messageResult.id,
                        Role = messageResult.role,
                        RoomRole = messageResult.roomRole,
                        Subscribing = messageResult.subscribing
                    }, new MessageSent.Sender(_blockchainName, _accessKey)
                    {
                        Avatar = messageResult.sender.id,
                        DisplayName = messageResult.sender.displayname,
                        UserId = messageResult.sender.id,
                        PartnerStatus = messageResult.sender.partnerstatus,
                        Username = messageResult.sender.username
                    });

                if (messageResult.__typename == "ChatDelete")
                {
                    var ids = new List<string>();

                    foreach (string id in messageResult.ids) ids.Add(id);

                    Events.FireOnMessageDeleted(new ChatDeleted
                    {
                        Ids = ids
                    });
                }
            }
        }
    }
}