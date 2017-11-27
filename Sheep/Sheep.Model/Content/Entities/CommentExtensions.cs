using System;

namespace Sheep.Model.Content.Entities
{
    public static class CommentExtensions
    {
        /// <summary>
        ///     计算精选的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <returns>得分。</returns>
        public static float CalculateFeaturedScore(this Comment comment)
        {
            return comment.IsFeatured ? 1.0f : 0.0f;
        }

        /// <summary>
        ///     计算回复的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <returns>得分。</returns>
        public static float CalculateRepliesScore(this Comment comment)
        {
            return Math.Min(1.0f, comment.RepliesCount / 5.0f);
        }

        /// <summary>
        ///     计算投票的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <returns>得分。</returns>
        public static float CalculateVotesScore(this Comment comment)
        {
            return Math.Min(1.0f, comment.YesVotesCount / 10.0f) - Math.Min(1.0f, comment.NoVotesCount / 10.0f);
        }

        /// <summary>
        ///     计算内容质量的得分。
        /// </summary>
        /// <param name="comment">评论。</param>
        /// <param name="featuredWeight">精选的权重。</param>
        /// <param name="repliesWeight">收藏的权重。</param>
        /// <param name="votesWeight">投票的权重。</param>
        /// <returns>得分。</returns>
        public static float CalculateContentQuality(this Comment comment, float featuredWeight = 1.0f, float repliesWeight = 1.0f, float votesWeight = 1.0f)
        {
            return CalculateFeaturedScore(comment) * featuredWeight + CalculateRepliesScore(comment) * repliesWeight + CalculateVotesScore(comment) * votesWeight;
        }
    }
}