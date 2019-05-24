// ***********************************************************************
// Project          : dlivetv-unofficial-api-net
// Author           : Nils Kleinert
// Created          : 05/03/2019
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
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dlivetv_unofficial_api_net.Helper
{
    internal class GraphQL
    {
        internal class GraphQLClient
        {
            private readonly string url;

            public GraphQLClient()
            {
                url = "https://graphigo.prd.dlive.tv/";
            }

            public async Task<dynamic> QueryAsync(string authKey, string query, object variables)
            {
                var fullQuery = new GraphQLQuery
                {
                    query = query,
                    variables = variables
                };
                var jsonContent = JsonConvert.SerializeObject(fullQuery);
                var request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "POST";
                request.Headers.Add("authorization", authKey);


                var encoding = new UTF8Encoding();
                var byteArray = encoding.GetBytes(jsonContent.Trim());

                request.ContentLength = byteArray.Length;
                request.ContentType = @"application/json";

                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                var response = await request.GetResponseAsync();
                using (response)
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream == null) return new GraphQLQueryResult(null);
                        var reader = new StreamReader(responseStream, Encoding.UTF8);
                        var json = await reader.ReadToEndAsync();
                        return new GraphQLQueryResult(json);
                    }
                }
            }

            private class GraphQLQuery
            {
                public string query { get; internal set; }

                public object variables { get; internal set; }
            }

            public class GraphQLQueryResult
            {
                private readonly JObject data;
                private readonly Exception Exception;
                private readonly string raw;

                public GraphQLQueryResult(string text, Exception ex = null)
                {
                    Exception = ex;
                    raw = text;
                    data = text != null ? JObject.Parse(text) : null;
                }

                public Exception GetException()
                {
                    return Exception;
                }

                public string GetRaw()
                {
                    return raw;
                }

                public T Get<T>(string key)
                {
                    if (data == null) return default;
                    try
                    {
                        return JsonConvert.DeserializeObject<T>(data["data"][key].ToString());
                    }
                    catch
                    {
                        return default;
                    }
                }

                public dynamic Get(string key)
                {
                    if (data == null) return null;
                    try
                    {
                        return JsonConvert.DeserializeObject<dynamic>(data["data"][key].ToString());
                    }
                    catch
                    {
                        return null;
                    }
                }

                public dynamic GetData()
                {
                    if (data == null) return null;
                    try
                    {
                        return JsonConvert.DeserializeObject<dynamic>(data["data"].ToString());
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }
    }
}