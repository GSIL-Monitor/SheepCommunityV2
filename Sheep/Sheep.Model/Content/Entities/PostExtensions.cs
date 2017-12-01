using System;

namespace Sheep.Model.Content.Entities
{
    public static class PostExtensions
    {
        /// <summary>
        ///     计算精选的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        public static float CalculateFeaturedScore(this Post post)
        {
            return post.IsFeatured ? 1.0f : 0.0f;
        }

        /// <summary>
        ///     计算标签的得分。
        /// </summary>
        /// <returns>得分。</returns>
        public static float CalculateTagsScore(this Post post)
        {
            if (post.Tags == null)
            {
                return 0.0f;
            }
            return 0.8f + 0.2f * (Math.Min(5.0f, post.Tags.Count) / 5.0f);
        }

        /// <summary>
        ///     计算查看的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        public static float CalculateViewsScore(this Post post)
        {
            return Math.Min(1.0f, post.ViewsCount / 1000.0f);
        }

        /// <summary>
        ///     计算收藏的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        public static float CalculateBookmarksScore(this Post post)
        {
            return Math.Min(1.0f, post.BookmarksCount / 10.0f);
        }

        /// <summary>
        ///     计算评论的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        public static float CalculateCommentsScore(this Post post)
        {
            return Math.Min(1.0f, post.CommentsCount / 50.0f);
        }

        /// <summary>
        ///     计算点赞的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        public static float CalculateLikesScore(this Post post)
        {
            return Math.Min(1.0f, post.LikesCount / 100.0f);
        }

        /// <summary>
        ///     计算评分的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        public static float CalculateRatingsScore(this Post post)
        {
            return post.RatingsCount >= 5 ? post.RatingsAverageValue : 0.6f;
        }

        /// <summary>
        ///     计算分享的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        public static float CalculateSharesScore(this Post post)
        {
            return Math.Min(1.0f, post.SharesCount / 10.0f);
        }

        /// <summary>
        ///     计算滥用举报的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <returns>得分。</returns>
        public static float CalculateAbuseReportsScore(this Post post)
        {
            return Math.Min(1.0f, post.AbuseReportsCount / 5.0f);
        }

        /// <summary>
        ///     计算内容质量的得分。
        /// </summary>
        /// <param name="post">帖子。</param>
        /// <param name="featuredWeight">精选的权重。</param>
        /// <param name="tagsWeight">标签的权重。</param>
        /// <param name="viewsWeight">查看的权重。</param>
        /// <param name="bookmarksWeight">收藏的权重。</param>
        /// <param name="commentsWeight">评论的权重。</param>
        /// <param name="likesWeight">点赞的权重。</param>
        /// <param name="ratingsWeight">评分的权重。</param>
        /// <param name="sharesWeight">分享的权重。</param>
        /// <param name="decayHalfLife">得分的半衰期。（天）</param>
        /// <returns>得分。</returns>
        public static float CalculateContentQuality(this Post post, float featuredWeight = 1.0f, float tagsWeight = 1.0f, float viewsWeight = 1.0f, float bookmarksWeight = 1.0f, float commentsWeight = 1.0f, float likesWeight = 1.0f, float ratingsWeight = 1.0f, float sharesWeight = 1.0f, int decayHalfLife = 30)
        {
            var baseScore = CalculateFeaturedScore(post) * featuredWeight + CalculateTagsScore(post) * tagsWeight + CalculateViewsScore(post) * viewsWeight + CalculateBookmarksScore(post) * bookmarksWeight + CalculateCommentsScore(post) * commentsWeight + CalculateLikesScore(post) * likesWeight + CalculateRatingsScore(post) * ratingsWeight + CalculateSharesScore(post) * sharesWeight;
            if (!post.PublishedDate.HasValue || post.PublishedDate.Value >= DateTime.UtcNow)
            {
                return baseScore;
            }
            var decayHalfLifeHours = decayHalfLife * 24.0;
            var passedHours = DateTime.UtcNow.Subtract(post.PublishedDate.Value).TotalHours;
            return (float) (baseScore * Math.Pow(0.5, passedHours / decayHalfLifeHours));
        }
    }
}