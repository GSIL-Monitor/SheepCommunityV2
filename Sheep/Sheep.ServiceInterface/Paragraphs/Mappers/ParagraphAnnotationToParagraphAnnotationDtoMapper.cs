using System.Collections.Generic;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceModel.Paragraphs.Entities;

namespace Sheep.ServiceInterface.Paragraphs.Mappers
{
    public static class ParagraphAnnotationToParagraphAnnotationDtoMapper
    {
        public static ParagraphAnnotationDto MapToParagraphAnnotationDto(this ParagraphAnnotation paragraphAnnotation)
        {
            if (paragraphAnnotation.Meta == null)
            {
                paragraphAnnotation.Meta = new Dictionary<string, string>();
            }
            var paragraphAnnotationDto = new ParagraphAnnotationDto
                                         {
                                             Id = paragraphAnnotation.Id,
                                             VolumeNumber = paragraphAnnotation.VolumeNumber,
                                             ChapterNumber = paragraphAnnotation.ChapterNumber,
                                             ParagraphNumber = paragraphAnnotation.ParagraphNumber,
                                             Number = paragraphAnnotation.Number,
                                             Title = paragraphAnnotation.Title,
                                             Annotation = paragraphAnnotation.Annotation
                                         };
            return paragraphAnnotationDto;
        }
    }
}