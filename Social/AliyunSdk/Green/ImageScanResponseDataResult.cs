using System.Collections.Generic;
using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     检测图片的结果。
    /// </summary>
    [JsonObject]
    public class ImageScanResponseDataResult
    {
        /// <summary>
        ///     风险场景，和传递进来的场景对应。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "scene", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Scene { get; set; }

        /// <summary>
        ///     建议用户处理，取值范围：[“pass”, “review”, “block”], pass:图片正常，review：需要人工审核，block：图片违规，可以直接删除或者做限制处理。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "suggestion", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Suggestion { get; set; }

        /// <summary>
        ///     该文本的分类。
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "label", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Label { get; set; }

        /// <summary>
        ///     结果为该分类的概率；值越高，越趋于该分类；取值为[0.00-100.00]。
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "rate", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float Rate { get; set; }

        /// <summary>
        ///     图片中含有二维码时，该字段是图片中所有二维码包含的文本信息。
        /// </summary>
        [JsonProperty(Order = 5, PropertyName = "qrcodeData", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] QRCodeData { get; set; }

        /// <summary>
        ///     静态图（非git图片）有文字时，返回识别出来的文字列表。
        /// </summary>
        [JsonProperty(Order = 6, PropertyName = "ocrData", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] OCRData { get; set; }

        /// <summary>
        ///     静态图（非git图片）有文字时，返回识别出来的文字列表的位置。
        /// </summary>
        [JsonProperty(Order = 7, PropertyName = "ocrLocations", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ImageScanResponseDataResultOCRLocation[] OCRLocations { get; set; }

        /// <summary>
        ///     识别出来的人脸信息列表。
        /// </summary>
        [JsonProperty(Order = 8, PropertyName = "sfaceData", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ImageScanResponseDataResultFaceData[] FaceData { get; set; }

        /// <summary>
        ///     识别出来的徽标信息列表。
        /// </summary>
        [JsonProperty(Order = 9, PropertyName = "logoData", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ImageScanResponseDataResultLogoData[] LogoData { get; set; }

        /// <summary>
        ///     附加信息. 该值将来可能会调整，建议不要在业务上进行依赖。
        /// </summary>
        [JsonProperty(Order = 10, PropertyName = "extras", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, object> Extras { get; set; }
    }
}