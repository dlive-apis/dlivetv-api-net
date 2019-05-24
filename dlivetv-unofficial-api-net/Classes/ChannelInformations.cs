// ***********************************************************************
// Project          : dlivetv-unofficial-api-net
// Author           : Nils Kleinert
// Created          : 05/17/2019
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
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace dlivetv_unofficial_api_net.Classes
{
    public partial class ChannelInformations
    {
        [JsonProperty("userByDisplayName", Required = Required.Always)]
        public UserByDisplayName UserByDisplayName { get; internal set; }
    }

    public class UserByDisplayName
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; internal set; }

        [JsonProperty("avatar", Required = Required.Always)]
        public Uri Avatar { get; internal set; }

        [JsonProperty("__typename", Required = Required.Always)]
        public string Typename { get; internal set; }

        [JsonProperty("displayname", Required = Required.Always)]
        public string Displayname { get; internal set; }

        [JsonProperty("partnerStatus", Required = Required.Always)]
        public string PartnerStatus { get; internal set; }

        [JsonProperty("username", Required = Required.Always)]
        public string Username { get; internal set; }

        [JsonProperty("followers", Required = Required.Always)]
        public Followers Followers { get; internal set; }

        [JsonProperty("canSubscribe", Required = Required.Always)]
        public bool CanSubscribe { get; internal set; }

        [JsonProperty("subSetting", Required = Required.AllowNull)]
        public dynamic SubSetting { get; internal set; }

        [JsonProperty("offlineImage", Required = Required.Always)]
        public Uri OfflineImage { get; internal set; }

        [JsonProperty("banStatus", Required = Required.Always)]
        public string BanStatus { get; internal set; }

        [JsonProperty("deactivated", Required = Required.Always)]
        public bool Deactivated { get; internal set; }

        [JsonProperty("about", Required = Required.Always)]
        public string About { get; internal set; }

        [JsonProperty("livestream", Required = Required.AllowNull)]
        public Livestream Livestream { get; internal set; }


        [JsonProperty("hostingLivestream", Required = Required.AllowNull)]
        public dynamic HostingLivestream { get; internal set; }

        [JsonProperty("videos", Required = Required.Always)]
        public Followers Videos { get; internal set; }

        [JsonProperty("pastBroadcasts", Required = Required.Always)]
        public Followers PastBroadcasts { get; internal set; }

        [JsonProperty("following", Required = Required.Always)]
        public Followers Following { get; internal set; }
    }

    public class Followers
    {
        [JsonProperty("totalCount", Required = Required.Always)]
        public long TotalCount { get; internal set; }

        [JsonProperty("__typename", Required = Required.Always)]
        public string Typename { get; internal set; }
    }

    public class Livestream
    {
        [JsonProperty("category", Required = Required.Always)]
        public Category Category { get; internal set; }

        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; internal set; }

        [JsonProperty("watchingCount", Required = Required.Always)]
        public long WatchingCount { get; internal set; }

        [JsonProperty("totalReward", Required = Required.Always)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long TotalReward { get; internal set; }

        [JsonProperty("permlink", Required = Required.Always)]
        public string Permlink { get; internal set; }

        [JsonProperty("creator", Required = Required.Always)]
        public Creator Creator { get; internal set; }

        [JsonProperty("__typename", Required = Required.Always)]
        public string Typename { get; internal set; }

        [JsonProperty("content", Required = Required.Always)]
        public string Content { get; }

        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; }

        [JsonProperty("watchTime", Required = Required.Always)]
        public bool WatchTime { get; }

        [JsonProperty("disableAlert", Required = Required.Always)]
        public bool DisableAlert { get; }

        [JsonProperty("language", Required = Required.Always)]
        public Language Language { get; }
    }

    public class Category
    {
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; internal set; }

        [JsonProperty("imgUrl", Required = Required.Always)]
        public Uri ImgUrl { get; internal set; }

        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; internal set; }

        [JsonProperty("backendID", Required = Required.Always)]
        public long BackendId { get; internal set; }

        [JsonProperty("__typename", Required = Required.Always)]
        public string Typename { get; internal set; }
    }

    public class Creator
    {
        [JsonProperty("username", Required = Required.Always)]
        public string Username { get; internal set; }

        [JsonProperty("__typename", Required = Required.Always)]
        public string Typename { get; internal set; }
    }

    public class Language
    {
        [JsonProperty("language", Required = Required.Always)]
        public string LanguageLanguage { get; internal set; }

        [JsonProperty("__typename", Required = Required.Always)]
        public string Typename { get; internal set; }
    }

    public class Buff
    {
        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; internal set; }

        [JsonProperty("boost", Required = Required.Always)]
        public long Boost { get; internal set; }

        [JsonProperty("__typename", Required = Required.Always)]
        public string Typename { get; internal set; }
    }

    public partial class ChannelInformations
    {
        public static ChannelInformations FromJson(string json)
        {
            return JsonConvert.DeserializeObject<ChannelInformations>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this ChannelInformations self)
        {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            }
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public static readonly ParseStringConverter Singleton = new ParseStringConverter();

        public override bool CanConvert(Type t)
        {
            return t == typeof(long) || t == typeof(long?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (long.TryParse(value, out l)) return l;
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            var value = (long) untypedValue;
            serializer.Serialize(writer, value.ToString());
        }
    }
}