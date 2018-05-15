using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     客户端信息，由ClientInfo结构体JSON序列化所得。
    /// </summary>
    [JsonObject]
    public class ClientInfo
    {
        /// <summary>
        ///     Sdk版本, 通过SDK调用时，需提供该字段。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "sdkVersion", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SdkVersion { set; get; }

        /// <summary>
        ///     配置信息版本, 通过SDK调用时，需提供该字段。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "cfgVersion", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CfgVersion { set; get; }

        /// <summary>
        ///     用户账号类型，取值范围为：[“taobao”, “others”]。
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "userType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string UserType { set; get; }

        /// <summary>
        ///     用户ID，唯一标识一个用户。
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "userId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string UserId { set; get; }

        /// <summary>
        ///     用户昵称。
        /// </summary>
        [JsonProperty(Order = 5, PropertyName = "userNick", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string UserNick { set; get; }

        /// <summary>
        ///     用户头像。
        /// </summary>
        [JsonProperty(Order = 6, PropertyName = "avatar", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Avatar { set; get; }

        /// <summary>
        ///     硬件设备码。
        /// </summary>
        [JsonProperty(Order = 7, PropertyName = "imei", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Imei { set; get; }

        /// <summary>
        ///     运营商设备码。
        /// </summary>
        [JsonProperty(Order = 8, PropertyName = "imsi", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Imsi { set; get; }

        /// <summary>
        ///     设备指纹。
        /// </summary>
        [JsonProperty(Order = 9, PropertyName = "umid", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Umid { set; get; }

        /// <summary>
        ///     该IP应该为公网IP；如果请求中不填写，服务端会尝试从链接或者http头中获取。如果请求是从设备端发起的，该字段通常不填写；如果是从后台发起的，该IP为用户的login IP或者设备的公网IP。
        /// </summary>
        [JsonProperty(Order = 10, PropertyName = "ip", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string IP { set; get; }

        /// <summary>
        ///     设备的操作系统，如：’Android 6.0’。
        /// </summary>
        [JsonProperty(Order = 11, PropertyName = "os", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OS { set; get; }

        /// <summary>
        ///     渠道号。
        /// </summary>
        [JsonProperty(Order = 12, PropertyName = "channel", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Channel { set; get; }

        /// <summary>
        ///     宿主应用名称。
        /// </summary>
        [JsonProperty(Order = 13, PropertyName = "hostAppPropertyName", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string HostAppPropertyName { set; get; }

        /// <summary>
        ///     宿主应用包名。
        /// </summary>
        [JsonProperty(Order = 14, PropertyName = "hostPackage", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string HostPackage { set; get; }

        /// <summary>
        ///     宿主应用版本。
        /// </summary>
        [JsonProperty(Order = 15, PropertyName = "hostVersion", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string HostVersion { set; get; }
    }
}