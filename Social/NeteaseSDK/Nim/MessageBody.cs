using System.Runtime.Serialization;

namespace Netease.Nim
{
    /// <summary>
    ///     消息体。
    /// </summary>
    [DataContract]
    public class MessageBody
    {
        #region 属性

        /// <summary>
        ///     文本消息。
        /// </summary>
        [DataMember(Order = 1, Name = "msg")]
        public string Message { get; set; }

        /// <summary>
        ///     地理位置的名称。
        /// </summary>
        [DataMember(Order = 2, Name = "title")]
        public string Title { get; set; }

        /// <summary>
        ///     经度。
        /// </summary>
        [DataMember(Order = 3, Name = "lng")]
        public double? Longitude { get; set; }

        /// <summary>
        ///     纬度。
        /// </summary>
        [DataMember(Order = 4, Name = "lat")]
        public double? Latitude { get; set; }

        /// <summary>
        ///     图片或文件的名称。
        /// </summary>
        [DataMember(Order = 5, Name = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     音频或视频的持续时差（毫秒）。
        /// </summary>
        [DataMember(Order = 6, Name = "dur")]
        public int? Duration { get; set; }

        /// <summary>
        ///     图片或文件或音频或视频的Md5哈希编码。
        /// </summary>
        [DataMember(Order = 7, Name = "md5")]
        public string Md5 { get; set; }

        /// <summary>
        ///     图片或文件或音频或视频的地址。
        /// </summary>
        [DataMember(Order = 8, Name = "url")]
        public string Url { get; set; }

        /// <summary>
        ///     消息格式的扩展名。
        /// </summary>
        [DataMember(Order = 9, Name = "ext")]
        public string Extension { get; set; }

        /// <summary>
        ///     图片或视频的宽度。
        /// </summary>
        [DataMember(Order = 10, Name = "w")]
        public int? Width { get; set; }

        /// <summary>
        ///     图片或视频的高度。
        /// </summary>
        [DataMember(Order = 11, Name = "h")]
        public int? Height { get; set; }

        /// <summary>
        ///     图片或音频或视频的大小。
        /// </summary>
        [DataMember(Order = 12, Name = "size")]
        public int? Size { get; set; }

        #endregion
    }
}