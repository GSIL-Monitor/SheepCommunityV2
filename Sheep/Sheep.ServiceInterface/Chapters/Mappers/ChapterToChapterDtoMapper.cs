﻿using System.Collections.Generic;
using System.Linq;
using Sheep.Model.Read.Entities;
using Sheep.ServiceModel.Chapters.Entities;

namespace Sheep.ServiceInterface.Chapters.Mappers
{
    public static class ChapterToChapterDtoMapper
    {
        public static ChapterDto MapToChapterDto(this Chapter chapter, IEnumerable<ChapterAnnotation> chapterAnnotations)
        {
            if (chapter.Meta == null)
            {
                chapter.Meta = new Dictionary<string, string>();
            }
            var chapterDto = new ChapterDto
                             {
                                 Id = chapter.Id,
                                 VolumeNumber = chapter.VolumeNumber,
                                 Number = chapter.Number,
                                 Title = chapter.Title,
                                 Content = chapter.Content,
                                 ParagraphsCount = chapter.ParagraphsCount,
                                 ViewsCount = chapter.ViewsCount,
                                 BookmarksCount = chapter.BookmarksCount,
                                 CommentsCount = chapter.CommentsCount,
                                 LikesCount = chapter.LikesCount,
                                 RatingsCount = chapter.RatingsCount,
                                 RatingsAverageValue = chapter.RatingsAverageValue,
                                 SharesCount = chapter.SharesCount,
                                 Annotations = chapterAnnotations?.Select(va => va.MapToChapterAnnotationDto()).ToList() ?? new List<ChapterAnnotationDto>()
                             };
            return chapterDto;
        }
    }
}