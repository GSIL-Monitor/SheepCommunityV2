﻿using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Posts.Validators
{
    /// <summary>
    ///     查询并列举一组帖子基本信息的校验器。
    /// </summary>
    public class BasicPostListValidator : AbstractValidator<PostList>
    {
        public static readonly HashSet<string> ContentTypes = new HashSet<string>
                                                              {
                                                                  "图文",
                                                                  "音频",
                                                                  "视频"
                                                              };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "Title",
                                                              "CreatedDate",
                                                              "ModifiedDate",
                                                              "PublishedDate",
                                                              "ViewsCount",
                                                              "BookmarksCount",
                                                              "CommentsCount",
                                                              "LikesCount",
                                                              "RatingsCount",
                                                              "RatingsAverageValue",
                                                              "SharesCount",
                                                              "AbuseReportsCount",
                                                              "ContentQuality"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="PostListValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BasicPostListValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ContentType).Must(contentType => ContentTypes.Contains(contentType)).WithMessage(x => string.Format(Resources.ContentTypeRangeMismatch, ContentTypes.Join(","))).When(x => !x.ContentType.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }

    /// <summary>
    ///     根据作者查询并列举一组帖子基本信息的校验器。
    /// </summary>
    public class BasicPostListByAuthorValidator : AbstractValidator<PostListByAuthor>
    {
        public static readonly HashSet<string> ContentTypes = new HashSet<string>
                                                              {
                                                                  "图文",
                                                                  "音频",
                                                                  "视频"
                                                              };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "Title",
                                                              "CreatedDate",
                                                              "ModifiedDate",
                                                              "PublishedDate",
                                                              "ViewsCount",
                                                              "BookmarksCount",
                                                              "CommentsCount",
                                                              "LikesCount",
                                                              "RatingsCount",
                                                              "RatingsAverageValue",
                                                              "SharesCount",
                                                              "AbuseReportsCount",
                                                              "ContentQuality"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="PostListByAuthorValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BasicPostListByAuthorValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ContentType).Must(contentType => ContentTypes.Contains(contentType)).WithMessage(x => string.Format(Resources.ContentTypeRangeMismatch, ContentTypes.Join(","))).When(x => !x.ContentType.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }

    /// <summary>
    ///     根据已关注的作者列表查询并列举一组帖子基本信息的校验器。
    /// </summary>
    public class BasicPostListByFollowingOwnersValidator : AbstractValidator<PostListByFollowingOwners>
    {
        public static readonly HashSet<string> ContentTypes = new HashSet<string>
                                                              {
                                                                  "图文",
                                                                  "音频",
                                                                  "视频"
                                                              };

        public static readonly HashSet<string> OrderBys = new HashSet<string>
                                                          {
                                                              "Title",
                                                              "CreatedDate",
                                                              "ModifiedDate",
                                                              "PublishedDate",
                                                              "ViewsCount",
                                                              "BookmarksCount",
                                                              "CommentsCount",
                                                              "LikesCount",
                                                              "RatingsCount",
                                                              "RatingsAverageValue",
                                                              "SharesCount",
                                                              "AbuseReportsCount",
                                                              "ContentQuality"
                                                          };

        /// <summary>
        ///     初始化一个新的<see cref="PostListByFollowingOwnersValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BasicPostListByFollowingOwnersValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.ContentType).Must(contentType => ContentTypes.Contains(contentType)).WithMessage(x => string.Format(Resources.ContentTypeRangeMismatch, ContentTypes.Join(","))).When(x => !x.ContentType.IsNullOrEmpty());
                                     RuleFor(x => x.OrderBy).Must(orderBy => OrderBys.Contains(orderBy)).WithMessage(x => string.Format(Resources.OrderByRangeMismatch, OrderBys.Join(","))).When(x => !x.OrderBy.IsNullOrEmpty());
                                 });
        }
    }
}