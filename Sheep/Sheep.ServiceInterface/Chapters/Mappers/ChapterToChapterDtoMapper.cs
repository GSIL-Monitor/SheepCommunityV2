using System.Collections.Generic;
using System.Linq;
using Sheep.Model.Read.Entities;
using Sheep.ServiceInterface.Paragraphs.Mappers;
using Sheep.ServiceModel.Chapters.Entities;
using Sheep.ServiceModel.Paragraphs.Entities;

namespace Sheep.ServiceInterface.Chapters.Mappers
{
    public static class ChapterToChapterDtoMapper
    {
        public static ChapterDto MapToChapterDto(this Chapter chapter, IEnumerable<ChapterAnnotation> chapterAnnotations, IEnumerable<Paragraph> paragraphs, Dictionary<string, int> paragraphCommentsMap)
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
                                 Annotations = chapterAnnotations?.Select(ca => ca.MapToChapterAnnotationDto()).ToList() ?? new List<ChapterAnnotationDto>(),
                                 Paragraphs = paragraphs?.Select(p => p.MapToParagraphDto(paragraphCommentsMap.GetValueOrDefault(p.Id) > 0, new List<ParagraphAnnotation>())).ToList() ?? new List<ParagraphDto>()
                             };
            return chapterDto;
        }
    }
}