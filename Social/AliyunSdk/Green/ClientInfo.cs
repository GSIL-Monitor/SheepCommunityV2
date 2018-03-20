using System.Runtime.Serialization;

namespace Aliyun.Green
{
    /// <summary>
    ///     客户端信息，由ClientInfo结构体JSON序列化所得。
    /// </summary>
    [DataContract]
    public class ClientInfo
    {
        #region 属性

        /// <summary>
        ///     Sdk版本, 通过SDK调用时，需提供该字段。
        /// </summary>
        [DataMember(Order = 1, Name = "sdkVersion")]
        public string SdkVersion { set; get; }

        /// <summary>
        ///     配置信息版本, 通过SDK调用时，需提供该字段。
        /// </summary>
        [DataMember(Order = 2, Name = "cfgVersion")]
        public string CfgVersion { set; get; }

        /// <summary>
        ///     用户账号类型，取值范围为：[“taobao”, “others”]。
        /// </summary>
        [DataMember(Order = 3, Name = "userType")]
        public string UserType { set; get; }

        /// <summary>
        ///     用户ID，唯一标识一个用户。
        /// </summary>
        [DataMember(Order = 4, Name = "userId")]
        public string UserId { set; get; }

        /// <summary>
        ///     用户昵称。
        /// </summary>
        [DataMember(Order = 5, Name = "userNick")]
        public string UserNick { set; get; }

        /// <summary>
        ///     用户头像。
        /// </summary>
        [DataMember(Order = 6, Name = "avatar")]
        public string Avatar { set; get; }

        /// <summary>
        ///     硬件设备码。
        /// </summary>
        [DataMember(Order = 7, Name = "imei")]
        public string Imei { set; get; }

        /// <summary>
        ///     运营商设备码。
        /// </summary>
        [DataMember(Order = 8, Name = "imsi")]
        public string Imsi { set; get; }

        /// <summary>
        ///     设备指纹。
        /// </summary>
        [DataMember(Order = 9, Name = "umid")]
        public string Umid { set; get; }

        /// <summary>
        ///     该IP应该为公网IP；如果请求中不填写，服务端会尝试从链接或者http头中获取。如果请求是从设备端发起的，该字段通常不填写；如果是从后台发起的，该IP为用户的login IP或者设备的公网IP。
        /// </summary>
        [DataMember(Order = 10, Name = "ip")]
        public string Ip { set; get; }

        /// <summary>
        ///     设备的操作系统，如：’Android 6.0’。
        /// </summary>
        [DataMember(Order = 11, Name = "os")]
        public string Os { set; get; }

        /// <summary>
        ///     渠道号。
        /// </summary>
        [DataMember(Order = 12, Name = "channel")]
        public string Channel { set; get; }

        /// <summary>
        ///     宿主应用名称。
        /// </summary>
        [DataMember(Order = 13, Name = "hostAppName")]
        public string HostAppName { set; get; }

        /// <summary>
        ///     宿主应用包名。
        /// </summary>
        [DataMember(Order = 14, Name = "hostPackage")]
        public string HostPackage { set; get; }

        /// <summary>
        ///     宿主应用版本。
        /// </summary>
        [DataMember(Order = 15, Name = "hostVersion")]
        public string HostVersion { set; get; }

        #endregion
    }
}