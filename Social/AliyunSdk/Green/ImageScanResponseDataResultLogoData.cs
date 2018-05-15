using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     识别出来的徽标信息。
    /// </summary>
    [JsonObject]
    public class ImageScanResponseDataResultLogoData
    {
        /// <summary>
        ///     以图片左上角为坐标原点，徽标区域左上角到y轴距离。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "x", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float X { get; set; }

        /// <summary>
        ///     以图片左上角为坐标原点，徽标区域左上角到x轴距离。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "y", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float Y { get; set; }

        /// <summary>
        ///     徽标区域宽度。
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "w", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float W { get; set; }

        /// <summary>
        ///     徽标区域高度。
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "h", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float H { get; set; }

        /// <summary>
        ///     识别出的徽标名称。
        /// </summary>
        [JsonProperty(Order = 5, PropertyName = "name", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Name { get; set; }

        /// <summary>
        ///     识别出的徽标类型，目前值可能为TV (台标)。
        /// </summary>
        [JsonProperty(Order = 6, PropertyName = "type", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Type { get; set; }
    }
}