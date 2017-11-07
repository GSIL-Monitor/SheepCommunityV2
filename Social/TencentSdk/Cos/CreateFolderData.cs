using System.Runtime.Serialization;

namespace Tencent.Cos
{
    /// <summary>
    ///     在 COS 的 Bucket 中创建一个新文件夹的响应数据体。
    /// </summary>
    [DataContract]
    public class CreateFolderData
    {
        /// <summary>
        ///     创建时间，10 位 Unix 时间戳（UNIX 时间是从协调世界时 1970 年 1 月 1 日 0 时 0 分 0 秒起的总秒数）。
        /// </summary>
        [DataMember(Order = 1, Name = "ctime")]
        public long CreatedTime { get; set; }
    }
}