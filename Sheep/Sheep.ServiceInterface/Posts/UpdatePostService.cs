﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Aliyun.Green;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using Aliyun.OSS.Util;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Extensions;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Common.Settings;
using Sheep.Model.Content;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Posts.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Posts;

namespace Sheep.ServiceInterface.Posts
{
    /// <summary>
    ///     更新一个帖子服务接口。
    /// </summary>
    public class UpdatePostService : ChangePostService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(UpdatePostService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置阿里云内容安全服务客户端。
        /// </summary>
        public IGreenClient GreenClient { get; set; }

        /// <summary>
        ///     获取及设置阿里云对象存储客户端。
        /// </summary>
        public IOss OssClient { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置更新一个帖子的校验器。
        /// </summary>
        public IValidator<PostUpdate> PostUpdateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置帖子的存储库。
        /// </summary>
        public IPostRepository PostRepo { get; set; }

        /// <summary>
        ///     获取及设置评论的存储库。
        /// </summary>
        public ICommentRepository CommentRepo { get; set; }

        /// <summary>
        ///     获取及设置点赞的存储库。
        /// </summary>
        public ILikeRepository LikeRepo { get; set; }

        #endregion

        #region 更新一个帖子

        /// <summary>
        ///     更新一个帖子。
        /// </summary>
        public async Task<object> Put(PostUpdate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    PostUpdateValidator.ValidateAndThrow(request, ApplyTo.Put);
            //}
            var existingPost = await PostRepo.GetPostAsync(request.PostId);
            if (existingPost == null)
            {
                throw HttpError.NotFound(string.Format(Resources.PostNotFound, request.PostId));
            }
            var currentUserId = GetSession().UserAuthId.ToInt(0);
            //if (existingPost.AuthorId != currentUserId)
            //{
            //    throw HttpError.Unauthorized(Resources.LoginAsAuthorRequired);
            //}
            var currentUser = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(currentUserId.ToString());
            if (currentUser == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, currentUserId));
            }
            var newPost = new Post();
            newPost.PopulateWith(existingPost);
            newPost.Meta = existingPost.Meta == null ? new Dictionary<string, string>() : new Dictionary<string, string>(existingPost.Meta);
            newPost.AuthorId = currentUserId;
            newPost.GroupId = request.GroupId;
            newPost.Title = request.Title?.Replace("\"", "'");
            newPost.Summary = request.Summary?.Replace("\"", "'");
            newPost.ContentType = request.ContentType;
            newPost.Content = request.Content?.Replace("\"", "'");
            newPost.ContentUrl = request.ContentUrl;
            newPost.Tags = request.Tags.IsNullOrEmpty() ? new List<string>() :
                               request.Tags.Replace(",", ";").Replace("，", ";").Replace("；", ";").Split(';').Select(x => x.Replace("”", string.Empty).Replace("“", string.Empty).Replace("\"", string.Empty).Trim()).ToList();
            newPost.IsPublished = request.AutoPublish ?? false;
            string pictureUrl = null;
            if (!request.SourcePictureUrl.IsNullOrEmpty())
            {
                var imageBuffer = await request.SourcePictureUrl.GetBytesFromUrlAsync();
                if (imageBuffer != null && imageBuffer.Length > 0)
                {
                    using (var imageStream = new MemoryStream(imageBuffer))
                    {
                        var md5Hash = OssUtils.ComputeContentMd5(imageStream, imageStream.Length);
                        var path = $"posts/{newPost.Id}/pictures/{Guid.NewGuid():N}.{request.SourcePictureUrl.GetImageUrlExtension()}";
                        var objectMetadata = new ObjectMetadata
                                             {
                                                 ContentMd5 = md5Hash,
                                                 ContentType = request.SourcePictureUrl.GetImageUrlExtension().GetImageContentType(),
                                                 ContentLength = imageBuffer.Length,
                                                 CacheControl = "max-age=604800"
                                             };
                        try
                        {
                            await OssClient.PutObjectAsync(AppSettings.GetString(AppSettingsOssNames.OssBucket), path, imageStream, objectMetadata);
                            pictureUrl = $"{AppSettings.GetString(AppSettingsOssNames.OssUrl)}/{path}";
                        }
                        catch (OssException ex)
                        {
                            Log.WarnFormat("Failed with error code: {0}; Error info: {1}. RequestID:{2}\tHostID:{3}", ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                            throw new HttpError(HttpStatusCode.InternalServerError, ex.ErrorCode, ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Log.WarnFormat("Failed with error info: {0}", ex.Message);
                            throw new HttpError(HttpStatusCode.InternalServerError, ex.Message);
                        }
                    }
                }
            }
            else
            {
                var imageFile = Request.Files.FirstOrDefault(file => file.ContentLength > 0);
                if (imageFile != null)
                {
                    using (var imageStream = imageFile.InputStream)
                    {
                        var md5Hash = OssUtils.ComputeContentMd5(imageStream, imageStream.Length);
                        var path = $"posts/{newPost.Id}/pictures/{Guid.NewGuid():N}.{imageFile.FileName.GetImageFileExtension()}";
                        var objectMetadata = new ObjectMetadata
                                             {
                                                 ContentMd5 = md5Hash,
                                                 ContentType = imageFile.ContentType,
                                                 ContentLength = imageFile.ContentLength,
                                                 CacheControl = "max-age=604800"
                                             };
                        try
                        {
                            await OssClient.PutObjectAsync(AppSettings.GetString(AppSettingsOssNames.OssBucket), path, imageStream, objectMetadata);
                            pictureUrl = $"{AppSettings.GetString(AppSettingsOssNames.OssUrl)}/{path}";
                        }
                        catch (OssException ex)
                        {
                            Log.WarnFormat("Failed with error code: {0}; Error info: {1}. RequestID:{2}\tHostID:{3}", ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                            throw new HttpError(HttpStatusCode.InternalServerError, ex.ErrorCode, ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Log.WarnFormat("Failed with error info: {0}", ex.Message);
                            throw new HttpError(HttpStatusCode.InternalServerError, ex.Message);
                        }
                    }
                }
            }
            newPost.PictureUrl = pictureUrl.IsNullOrEmpty() ? existingPost.PictureUrl : pictureUrl;
            if (!newPost.Title.IsNullOrEmpty() && !await AliyunHelper.IsTextValidAsync(GreenClient, newPost.Title, "post", "edit"))
            {
                throw HttpError.Forbidden(string.Format(Resources.InvalidTitle, newPost.Title));
            }
            if (!newPost.Summary.IsNullOrEmpty() && !await AliyunHelper.IsTextValidAsync(GreenClient, newPost.Summary, "post", "edit"))
            {
                throw HttpError.Forbidden(string.Format(Resources.InvalidSummary, newPost.Summary));
            }
            if (!newPost.Content.IsNullOrEmpty() && !await AliyunHelper.IsTextValidAsync(GreenClient, newPost.Content, "post", "edit"))
            {
                throw HttpError.Forbidden(string.Format(Resources.InvalidContent, newPost.Content));
            }
            if (!newPost.PictureUrl.IsNullOrEmpty() && !await AliyunHelper.IsImageValidAsync(GreenClient, newPost.PictureUrl))
            {
                throw HttpError.Forbidden(string.Format(Resources.InvalidPicture, newPost.PictureUrl));
            }
            var post = await PostRepo.UpdatePostAsync(existingPost, newPost);
            await PostRepo.UpdatePostContentQualityAsync(post.Id, PostRepo.CalculatePostContentQuality(post));
            var commentsCount = await CommentRepo.GetCommentsCountByParentAsync(post.Id, currentUserId, null, null, null, "审核通过");
            ResetCache(post);
            return new PostUpdateResponse
                   {
                       Post = post.MapToPostDto(currentUser, commentsCount > 0)
                   };
        }

        #endregion
    }
}