// ***********************************************************************
// Project          : dlivetv-unofficial-api-net
// Author           : Nils Kleinert
// Created          : 05/01/2019
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
using System.Collections.Generic;
using System.Threading.Tasks;
using dlivetv_unofficial_api_net.Classes;
using dlivetv_unofficial_api_net.Helper;

namespace dlivetv_unofficial_api_net.API
{
    public class Self
    {
        internal readonly string _accessKey;
        internal readonly string _displayname;
        internal readonly string _blockchainName;

        public Self(string displayname, string accessKey)
        {
            if (string.IsNullOrWhiteSpace(displayname))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(displayname));
            if (string.IsNullOrWhiteSpace(accessKey))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(accessKey));

            _accessKey = accessKey;
            _displayname = displayname;
            _blockchainName = new Util(displayname, accessKey).GetBlockchainName().Result;
        }

        public async Task<Follow> FollowUser(string displayname)
        {
            if (string.IsNullOrWhiteSpace(displayname))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(displayname));

            GraphQL.GraphQLClient.GraphQLQueryResult obj = await DLive.GraphQL.QueryAsync(_accessKey, Querys.FollowUser,
                new Dictionary<object, object>
                {
                    {
                        "input", new Dictionary<object, object>
                        {
                            {"streamer", new Util(_displayname, _accessKey).GetBlockchainNameFromDisplayName(displayname).Result}
                        }
                    }
                });

            if (obj.GetData() != null)
            {
                var data = obj.GetData();
                if (data.err != null)
                    return new Follow
                    {
                        Success = false,
                        Error = data.err.ToString()
                    };
                return new Follow {Success = true};
            }

            throw new InvalidOperationException("Unable to read the result from FollowUser");
        }

        public async Task<Unfollow> UnfollowUser(string displayname)
        {
            if (string.IsNullOrWhiteSpace(displayname))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(displayname));

            GraphQL.GraphQLClient.GraphQLQueryResult obj = await DLive.GraphQL.QueryAsync(_accessKey,
                Querys.UnfollowUser,
                new Dictionary<object, object>
                {
                    {
                        "input", new Dictionary<object, object>
                        {
                            {"streamer", new Util(_displayname, _accessKey).GetBlockchainNameFromDisplayName(displayname).Result}
                        }
                    }
                });

            if (obj.GetData() != null)
            {
                var data = obj.GetData();
                if (data.err != null)
                    return new Unfollow
                    {
                        Success = false,
                        Error = data.err.ToString()
                    };
                return new Unfollow {Success = true};
            }

            throw new InvalidOperationException("Unable to read the result from UnfollowUser");
        }
    }
}