﻿using System.Collections.Generic;
using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Posts.Entities;

namespace Sheep.ServiceInterface.Posts.Mappers
{
    public static class PostToPostDtoMapper
    {
        public static PostDto MapToPostDto(this Post post, IUserAuth author, bool commented)
        {
            if (post.Meta == null)
            {
                post.Meta = new Dictionary<string, string>();
            }
            var postDto = new PostDto
                          {
                              Id = post.Id,
                              Title = post.Title,
                              Summary = post.Summary,
                              PictureUrl = post.PictureUrl,
                              ContentType = post.ContentType,
                              Content = post.Content,
                              ContentUrl = post.ContentUrl,
                              Tags = post.Tags,
                              Status = post.Status,
                              CreatedDate = post.CreatedDate.ToUnixTime(),
                              ModifiedDate = post.ModifiedDate.ToUnixTime(),
                              IsPublished = post.IsPublished,
                              PublishedDate = post.PublishedDate?.ToUnixTime(),
                              IsFeatured = post.IsFeatured,
                              Author = author.MapToBasicUserDto(),
                              ViewsCount = post.ViewsCount,
                              BookmarksCount = post.BookmarksCount,
                              CommentsCount = post.CommentsCount,
                              LikesCount = post.LikesCount,
                              RatingsCount = post.RatingsCount,
                              RatingsAverageValue = post.RatingsAverageValue,
                              SharesCount = post.SharesCount,
                              AbuseReportsCount = post.AbuseReportsCount,
                              Commented = commented
                          };
            return postDto;
        }
    }
}