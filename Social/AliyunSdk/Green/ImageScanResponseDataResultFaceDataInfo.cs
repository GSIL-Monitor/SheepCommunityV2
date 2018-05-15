using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     识别出来的人脸信息中识别出的信息。
    /// </summary>
    [JsonObject]
    public class ImageScanResponseDataResultFaceDataInfo
    {
        /// <summary>
        ///     以图片左上角为坐标原点，人脸区域左上角到y轴距离。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "id", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Id { get; set; }

        /// <summary>
        ///     以图片左上角为坐标原点，人脸区域左上角到x轴距离。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "name", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Name { get; set; }

        /// <summary>
        ///     相似概率。
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "rate", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float Rate { get; set; }

        /// <summary>
        ///     人脸区域高度。
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "detail", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Detail { get; set; }
    }
}