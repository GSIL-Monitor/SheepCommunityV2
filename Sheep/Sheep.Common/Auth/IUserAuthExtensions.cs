using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;

namespace Sheep.Common.Auth
{
    public static class IUserAuthExtensions
    {
        /// <summary>
        ///     计算收藏的得分。
        /// </summary>
        /// <param name="userAuth">用户身份。</param>
        /// <returns>得分。</returns>
        public static float CalculateBookmarksScore(this IUserAuth userAuth)
        {
            userAuth.Meta = userAuth.Meta ?? new Dictionary<string, string>();
            return Math.Min(1.0f, userAuth.Meta.GetValueOrDefault("BookmarksCountInPeriod").ToInt() / 10.0f);
        }

        /// <summary>
        ///     计算评论的得分。
        /// </summary>
        /// <param name="userAuth">用户身份。</param>
        /// <returns>得分。</returns>
        public static float CalculateCommentsScore(this IUserAuth userAuth)
        {
            userAuth.Meta = userAuth.Meta ?? new Dictionary<string, string>();
            return Math.Min(1.0f, userAuth.Meta.GetValueOrDefault("CommentsCountInPeriod").ToInt() / 20.0f);
        }

        /// <summary>
        ///     计算点赞的得分。
        /// </summary>
        /// <param name="userAuth">用户身份。</param>
        /// <returns>得分。</returns>
        public static float CalculateLikesScore(this IUserAuth userAuth)
        {
            userAuth.Meta = userAuth.Meta ?? new Dictionary<string, string>();
            return Math.Min(1.0f, userAuth.Meta.GetValueOrDefault("LikesCountInPeriod").ToInt() / 50.0f);
        }

        /// <summary>
        ///     计算评分的得分。
        /// </summary>
        /// <param name="userAuth">用户身份。</param>
        /// <returns>得分。</returns>
        public static float CalculateRatingsScore(this IUserAuth userAuth)
        {
            userAuth.Meta = userAuth.Meta ?? new Dictionary<string, string>();
            return Math.Min(1.0f, userAuth.Meta.GetValueOrDefault("RatingsCountInPeriod").ToInt() / 10.0f);
        }

        /// <summary>
        ///     计算分享的得分。
        /// </summary>
        /// <param name="userAuth">用户身份。</param>
        /// <returns>得分。</returns>
        public static float CalculateSharesScore(this IUserAuth userAuth)
        {
            userAuth.Meta = userAuth.Meta ?? new Dictionary<string, string>();
            return Math.Min(1.0f, userAuth.Meta.GetValueOrDefault("SharesCountInPeriod").ToInt() / 10.0f);
        }

        ///// <summary>
        /////     计算滥用举报的得分。
        ///// </summary>
        ///// <param name="userAuth">用户身份。</param>
        ///// <returns>得分。</returns>
        //public static float CalculateAbuseReportsScore(this IUserAuth userAuth)
        //{
        //    return Math.Min(1.0f, userAuth.AbuseReportsCount / 5.0f);
        //}

        ///// <summary>
        /////     计算内容质量的得分。
        ///// </summary>
        ///// <param name="userAuth">用户身份。</param>
        ///// <param name="featuredWeight">精选的权重。</param>
        ///// <param name="tagsWeight">标签的权重。</param>
        ///// <param name="viewsWeight">查看的权重。</param>
        ///// <param name="bookmarksWeight">收藏的权重。</param>
        ///// <param name="commentsWeight">评论的权重。</param>
        ///// <param name="likesWeight">点赞的权重。</param>
        ///// <param name="ratingsWeight">评分的权重。</param>
        ///// <param name="sharesWeight">分享的权重。</param>
        ///// <param name="decayHalfLife">得分的半衰期。（天）</param>
        ///// <returns>得分。</returns>
        //public static float CalculateContentQuality(this IUserAuth userAuth, float featuredWeight = 1.0f, float tagsWeight = 1.0f, float viewsWeight = 1.0f, float bookmarksWeight = 1.0f, float commentsWeight = 1.0f, float likesWeight = 1.0f, float ratingsWeight = 1.0f, float sharesWeight = 1.0f, int decayHalfLife = 30)
        //{
        //    var baseScore = CalculateFeaturedScore(userAuth) * featuredWeight + CalculateTagsScore(userAuth) * tagsWeight + CalculateViewsScore(userAuth) * viewsWeight + CalculateBookmarksScore(userAuth) * bookmarksWeight + CalculateCommentsScore(userAuth) * commentsWeight + CalculateLikesScore(userAuth) * likesWeight + CalculateRatingsScore(userAuth) * ratingsWeight + CalculateSharesScore(userAuth) * sharesWeight;
        //    if (!userAuth.PublishedDate.HasValue || userAuth.PublishedDate.Value >= DateTime.UtcNow)
        //    {
        //        return baseScore;
        //    }
        //    var decayHalfLifeHours = decayHalfLife * 24.0;
        //    var passedHours = DateTime.UtcNow.Subtract(userAuth.PublishedDate.Value).TotalHours;
        //    return (float) (baseScore * Math.Pow(0.5, passedHours / decayHalfLifeHours));
        //}
    }
}