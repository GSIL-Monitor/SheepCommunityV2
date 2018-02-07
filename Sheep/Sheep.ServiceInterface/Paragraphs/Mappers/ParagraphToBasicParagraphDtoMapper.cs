using System.Collections.Generic;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceModel.Paragraphs.Entities;

namespace Sheep.ServiceInterface.Paragraphs.Mappers
{
    public static class ParagraphToBasicParagraphDtoMapper
    {
        public static BasicParagraphDto MapToBasicParagraphDto(this Paragraph paragraph, Volume volume, Chapter chapter)
        {
            if (paragraph.Meta == null)
            {
                paragraph.Meta = new Dictionary<string, string>();
            }
            var paragraphDto = new BasicParagraphDto
                               {
                                   Id = paragraph.Id,
                                   VolumeNumber = paragraph.VolumeNumber,
                                   VolumeTitle = volume?.Title,
                                   ChapterNumber = paragraph.ChapterNumber,
                                   ChapterTitle = chapter?.Title,
                                   SubjectNumber = paragraph.SubjectNumber,
                                   Number = paragraph.Number,
                                   Content = paragraph.Content
                               };
            return paragraphDto;
        }
    }
}