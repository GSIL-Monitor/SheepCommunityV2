using System.Runtime.Serialization;

namespace Tencent.Cos
{
    /// <summary>
    ///     在 COS 的 Bucket 中创建一个新文件夹。
    ///     成功创建新文件夹的前提条件是已经在控制台创建了 Bucket 。如果该 COS 中没有 Bucket 或新建文件夹名已存在，则创建新文件夹不成功。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://cloud.tencent.com/document/product/436/6061
    /// </remarks>
    [DataContract]
    public class CreateFolderRequest
    {
        /// <summary>
        ///     操作类型，填"create"。
        /// </summary>
        [DataMember(Order = 1, Name = "op", IsRequired = true)]
        public string Operation { get; set; } = "create";

        /// <summary>
        ///     COS 服务调用方自定义属性，可通过 查询文件夹属性 获取该属性值。
        /// </summary>
        [DataMember(Order = 2, Name = "biz_attr")]
        public string BizAttribute { get; set; }
    }
}