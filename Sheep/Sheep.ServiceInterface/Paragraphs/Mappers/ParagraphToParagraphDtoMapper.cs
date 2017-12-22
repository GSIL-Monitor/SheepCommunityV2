using System.Collections.Generic;
using System.Linq;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceModel.Paragraphs.Entities;

namespace Sheep.ServiceInterface.Paragraphs.Mappers
{
    public static class ParagraphToParagraphDtoMapper
    {
        public static ParagraphDto MapToParagraphDto(this Paragraph paragraph, bool commented, IEnumerable<ParagraphAnnotation> paragraphAnnotations)
        {
            if (paragraph.Meta == null)
            {
                paragraph.Meta = new Dictionary<string, string>();
            }
            var paragraphDto = new ParagraphDto
                               {
                                   Id = paragraph.Id,
                                   VolumeNumber = paragraph.VolumeNumber,
                                   ChapterNumber = paragraph.ChapterNumber,
                                   SubjectNumber = paragraph.SubjectNumber,
                                   Number = paragraph.Number,
                                   Content = paragraph.Content,
                                   ViewsCount = paragraph.ViewsCount,
                                   BookmarksCount = paragraph.BookmarksCount,
                                   CommentsCount = paragraph.CommentsCount,
                                   LikesCount = paragraph.LikesCount,
                                   RatingsCount = paragraph.RatingsCount,
                                   RatingsAverageValue = paragraph.RatingsAverageValue,
                                   SharesCount = paragraph.SharesCount,
                                   Commented = commented,
                                   Annotations = paragraphAnnotations?.Select(va => va.MapToParagraphAnnotationDto()).ToList() ?? new List<ParagraphAnnotationDto>()
                               };
            return paragraphDto;
        }
    }
}