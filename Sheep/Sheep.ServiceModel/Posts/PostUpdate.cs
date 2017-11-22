using System.Runtime.Serialization;
using ServiceStack;
using Sheep.ServiceModel.Posts.Entities;

namespace Sheep.ServiceModel.Posts
{
    /// <summary>
    ///     更新一个帖子的请求。
    /// </summary>
    [Route("/posts/{PostId}", HttpMethods.Put, Summary = "更新一个帖子")]
    [DataContract]
    public class PostUpdate : IReturn<PostUpdateResponse>
    {
        /// <summary>
        ///     帖子编号。
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        [ApiMember(Description = "帖子编号")]
        public string PostId { get; set; }

        /// <summary>
        ///     群组编号。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "群组编号（可选）")]
        public string GroupId { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        [ApiMember(Description = "标题")]
        public string Title { get; set; }

        /// <summary>
        ///     概要。
        /// </summary>
        [DataMember(Order = 4)]
        [ApiMember(Description = "概要")]
        public string Summary { get; set; }

        /// <summary>
        ///     来源图片的地址。
        /// </summary>
        [DataMember(Order = 5)]
        [ApiMember(Description = "来源图片的地址")]
        public string SourcePictureUrl { get; set; }

        /// <summary>
        ///     内容的类型。（可选值：图文, 音频, 视频）
        /// </summary>
        [DataMember(Order = 6, IsRequired = true)]
        [ApiMember(Description = "内容的类型（可选值：图文, 音频, 视频）")]
        public string ContentType { get; set; }

        /// <summary>
        ///     正文内容。
        /// </summary>
        [DataMember(Order = 7)]
        [ApiMember(Description = "正文内容")]
        public string Content { get; set; }

        /// <summary>
        ///     内容的地址。（当类型为音频或视频时，填写音频或视频的地址）
        /// </summary>
        [DataMember(Order = 8)]
        [ApiMember(Description = "内容的地址。（当类型为音频或视频时，填写音频或视频的地址）")]
        public string ContentUrl { get; set; }

        /// <summary>
        ///     分类的标签集合。（使用英文分号分隔）
        /// </summary>
        [DataMember(Order = 9)]
        [ApiMember(Description = "分类的标签集合（使用英文分号分隔）")]
        public string Tags { get; set; }

        /// <summary>
        ///     是否自动发布。（如果不发布则保存为草稿）
        /// </summary>
        [DataMember(Order = 10)]
        [ApiMember(Description = "是否自动发布（如果不发布则保存为草稿）")]
        public bool? AutoPublish { get; set; }
    }

    /// <summary>
    ///     更新一个帖子的响应。
    /// </summary>
    [DataContract]
    public class PostUpdateResponse : IHasResponseStatus
    {
        /// <summary>
        ///     帖子信息。
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "帖子信息")]
        public PostDto Post { get; set; }

        /// <summary>
        ///     处理响应的状态。
        /// </summary>
        [DataMember(Order = 2)]
        [ApiMember(Description = "处理响应的状态")]
        public ResponseStatus ResponseStatus { get; set; }
    }
}