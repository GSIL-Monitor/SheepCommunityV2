using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tencent.Cos
{
    /// <summary>
    ///     上传简单文件。
    ///     使用该 API 上传 1M 大小的简单文件（超过 20M 的文件，请使用“分片上传文件” API）。成功上传文件的前提条件是 Bucket 中已存在目录。如果该 Bucket 中没有文件目录则请求不成功。
    /// </summary>
    /// <remarks>
    ///     请求说明：
    ///     http请求方式: POST
    ///     https://cloud.tencent.com/document/product/436/6066
    /// </remarks>
    [DataContract]
    public class UploadFileRequest
    {
        /// <summary>
        ///     操作类型，填"upload"。
        /// </summary>
        [DataMember(Order = 1, Name = "op", IsRequired = true)]
        public string Operation { get; set; } = "upload";

        /// <summary>
        ///     文件内容。
        /// </summary>
        [DataMember(Order = 2, Name = "fileContent", IsRequired = true)]
        public byte[] FileContent { get; set; }

        /// <summary>
        ///     文件的 SHA-1 校验码。
        /// </summary>
        [DataMember(Order = 3, Name = "sha")]
        public string Sha { get; set; }

        /// <summary>
        ///     COS 服务调用方自定义属性，可通过 查询文件夹属性 获取该属性值。
        /// </summary>
        [DataMember(Order = 4, Name = "biz_attr")]
        public string BizAttribute { get; set; }

        /// <summary>
        ///     同名文件覆盖选项，有效值：0 覆盖（删除已有的重名文件，存储新上传的文件）1 不覆盖（若已存在重名文件，则不做覆盖，返回“上传失败”）。默认为 1 不覆盖。
        /// </summary>
        [DataMember(Order = 5, Name = "insertOnly")]
        public int InsertOnly { get; set; }

        /// <summary>
        ///     转换成数据字典。
        /// </summary>
        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
                   {
                       {
                           "op", Operation
                       },
                       {
                           "sha", Sha
                       },
                       {
                           "biz_attr", BizAttribute
                       },
                       {
                           "insertOnly", InsertOnly
                       }
                   };
        }

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