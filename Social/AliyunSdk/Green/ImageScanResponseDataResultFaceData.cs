using Newtonsoft.Json;

namespace Aliyun.Green
{
    /// <summary>
    ///     识别出来的人脸信息。
    /// </summary>
    [JsonObject]
    public class ImageScanResponseDataResultFaceData
    {
        /// <summary>
        ///     以图片左上角为坐标原点，人脸区域左上角到y轴距离。
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "x", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float X { get; set; }

        /// <summary>
        ///     以图片左上角为坐标原点，人脸区域左上角到x轴距离。
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "y", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float Y { get; set; }

        /// <summary>
        ///     人脸区域宽度。
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "w", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float W { get; set; }

        /// <summary>
        ///     人脸区域高度。
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "h", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Populate)]
        public float H { get; set; }

        /// <summary>
        ///     年龄。
        /// </summary>
        [JsonProperty(Order = 5, PropertyName = "age", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float? Age { get; set; }

        /// <summary>
        ///     微笑的概率。
        /// </summary>
        [JsonProperty(Order = 6, PropertyName = "smileRate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float? SmileRate { get; set; }

        /// <summary>
        ///     性别。
        /// </summary>
        [JsonProperty(Order = 7, PropertyName = "gender", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Gender { get; set; }

        /// <summary>
        ///     是否戴眼镜。
        /// </summary>
        [JsonProperty(Order = 8, PropertyName = "glasses", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Glasses { get; set; }

        /// <summary>
        ///     识别出的人名，每个元素包含名字（name）及相似度（rate）。
        /// </summary>
        [JsonProperty(Order = 9, PropertyName = "faces", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ImageScanResponseDataResultFaceDataInfo[] Faces { get; set; }
    }
}