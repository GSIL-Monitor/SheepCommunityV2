using System.Runtime.Serialization;

namespace Tencent.Cos
{
    /// <summary>
    ///     使用该 API 对腾讯云对象存储中某个目录进行删除。删除目录前必须先清空目录下的文件。无法删除 Bucket。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://cloud.tencent.com/document/product/436/6064
    /// </remarks>
    [DataContract]
    public class DeleteFolderRequest
    {
        /// <summary>
        ///     操作类型，填"delete"。
        /// </summary>
        [DataMember(Order = 1, Name = "op", IsRequired = true)]
        public string Operation { get; set; }
    }
}