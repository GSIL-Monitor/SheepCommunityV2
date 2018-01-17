using System.Runtime.Serialization;

namespace Sheep.ServiceModel.Comments.Entities
{
    /// <summary>
    ///     评论数量信息。
    /// </summary>
    [DataContract]
    public class CommentCountsDto
    {
        /// <summary>
        ///     评论次数。
        /// </summary>
        [DataMember(Order = 1)]
        public int CommentsCount { get; set; }
    }
}