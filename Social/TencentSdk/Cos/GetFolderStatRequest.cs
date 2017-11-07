using System.Runtime.Serialization;

namespace Tencent.Cos
{
    /// <summary>
    ///     查询文件夹的属性信息。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: GET
    ///     https://cloud.tencent.com/document/product/436/6063
    /// </remarks>
    [DataContract]
    public class GetFolderStatRequest
    {
        /// <summary>
        ///     操作类型，填"stat"。
        /// </summary>
        [DataMember(Order = 1, Name = "op", IsRequired = true)]
        public string Operation { get; set; }

        /// <summary>
        ///     转换成查询字符串格式的文本。
        /// </summary>
        /// <returns>查询字符串格式的文本。</returns>
        public string ToQueryString()
        {
            return string.Format("op={0}", Operation);
        }
    }
}