// ***********************************************************************
// Project          : dlivetv-unofficial-api-net
// Author           : Nils Kleinert
// Created          : 05/17/2019
// 
// Last Modified On : 05/18/2019
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
using System.Threading.Tasks;
using dlivetv_unofficial_api_net.Classes;
using dlivetv_unofficial_api_net.Helper;

namespace dlivetv_unofficial_api_net.API
{
    public class Util
    {
        internal readonly string _accessKey;
        internal readonly string _displayname;
        internal readonly string _blockchainName;

        public Util(string displayname, string accessKey)
        {
            if (string.IsNullOrWhiteSpace(displayname))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(displayname));
            if (string.IsNullOrWhiteSpace(accessKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(accessKey));

            _accessKey = accessKey;
            _displayname = displayname;
            _blockchainName = GetBlockchainName().Result;
        }

        public async Task<ChannelInformations> ChannelInformations(string displayname)
        {
            if (string.IsNullOrWhiteSpace(displayname))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(displayname));

            GraphQL.GraphQLClient.GraphQLQueryResult obj = await DLive.GraphQL.QueryAsync(_accessKey,
                Querys.LivestreamPage,
                new Dictionary<object, object>
                {
                    {"displayname", displayname},
                    {"add", false},
                    {"isLoggedIn", true}
                });

            if (obj.GetData() == null)
                throw new InvalidOperationException("Unable to read the result from ChannelInformations");

            string data = obj.GetData().ToString();
            return Classes.ChannelInformations.FromJson(data);
        }

        internal async Task<string> GetChannelName()
        {

            GraphQL.GraphQLClient.GraphQLQueryResult obj = await DLive.GraphQL.QueryAsync(_accessKey,
                Querys.LivestreamPage,
                new Dictionary<object, object>
                {
                    {"displayname", _displayname},
                    {"add", false},
                    {"isLoggedIn", true}
                });

            if (obj.GetData() == null)
                throw new InvalidOperationException("Unable to read the result from ChannelInformations");

            string data = obj.GetData().ToString();
            return Classes.ChannelInformations.FromJson(data).UserByDisplayName.Displayname;
        }

        internal async Task<string> GetBlockchainName()
        {

            GraphQL.GraphQLClient.GraphQLQueryResult obj = await DLive.GraphQL.QueryAsync(_accessKey,
                Querys.LivestreamPage,
                new Dictionary<object, object>
                {
                    {"displayname", _displayname},
                    {"add", false},
                    {"isLoggedIn", true}
                });

            if (obj.GetData() == null)
                throw new InvalidOperationException("Unable to read the result from ChannelInformations");

            string data = obj.GetData().ToString();
            return Classes.ChannelInformations.FromJson(data).UserByDisplayName.Username;
        }

        public async Task<string> GetBlockchainNameFromDisplayName(string displayname)
        {

            GraphQL.GraphQLClient.GraphQLQueryResult obj = await DLive.GraphQL.QueryAsync(_accessKey,
                Querys.LivestreamPage,
                new Dictionary<object, object>
                {
                    {"displayname", displayname},
                    {"add", false},
                    {"isLoggedIn", true}
                });

            if (obj.GetData() == null)
                throw new InvalidOperationException("Unable to read the result from ChannelInformations");

            string data = obj.GetData().ToString();
            return Classes.ChannelInformations.FromJson(data).UserByDisplayName.Username;
        }
    }
}