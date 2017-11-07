using System.Runtime.Serialization;

namespace Tencent.Cos
{
    /// <summary>
    ///     查询文件夹的属性信息的响应数据体。
    /// </summary>
    [DataContract]
    public class GetFolderStatData
    {
        /// <summary>
        ///     COS 服务调用方自定义属性。
        /// </summary>
        [DataMember(Order = 1, Name = "biz_attr")]
        public string BizAttribute { get; set; }

        /// <summary>
        ///     创建时间，10 位 Unix 时间戳（UNIX 时间是从协调世界时 1970 年 1 月 1 日 0 时 0 分 0 秒起的总秒数）。
        /// </summary>
        [DataMember(Order = 2, Name = "ctime")]
        public long CreatedTime { get; set; }

        /// <summary>
        ///     修改时间，10 位 Unix 时间戳（UNIX时间是从协调世界时 1970 年 1 月 1 日 0 时 0 分 0 秒起的总秒数）
        /// </summary>
        [DataMember(Order = 3, Name = "mtime")]
        public long ModifiedTime { get; set; }
    }
}