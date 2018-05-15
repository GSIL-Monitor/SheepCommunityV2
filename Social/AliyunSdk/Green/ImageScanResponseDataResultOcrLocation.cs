using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     图文识别的结果位置。
    /// </summary>
    [JsonObject]
    public class ImageScanResponseDataResultOCRLocation
    {
        /// <summary>
        ///     以图片左上角为坐标原点，图文识别区域左上角到y轴距离。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "x", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float X { get; set; }

        /// <summary>
        ///     以图片左上角为坐标原点，图文识别区域左上角到x轴距离。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "y", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float Y { get; set; }

        /// <summary>
        ///     图文识别区域宽度。
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "w", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float W { get; set; }

        /// <summary>
        ///     图文识别区域高度。
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "h", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float H { get; set; }

        /// <summary>
        ///     识别的文本。
        /// </summary>
        [JsonProperty(Order = 5, PropertyName = "text", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Text { get; set; }
    }
}