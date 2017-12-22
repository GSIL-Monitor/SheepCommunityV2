using System.Collections.Generic;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceModel.Chapters.Entities;

namespace Sheep.ServiceInterface.Chapters.Mappers
{
    public static class ChapterAnnotationToChapterAnnotationDtoMapper
    {
        public static ChapterAnnotationDto MapToChapterAnnotationDto(this ChapterAnnotation chapterAnnotation)
        {
            if (chapterAnnotation.Meta == null)
            {
                chapterAnnotation.Meta = new Dictionary<string, string>();
            }
            var chapterAnnotationDto = new ChapterAnnotationDto
                                       {
                                           Id = chapterAnnotation.Id,
                                           VolumeNumber = chapterAnnotation.VolumeNumber,
                                           ChapterNumber = chapterAnnotation.ChapterNumber,
                                           Number = chapterAnnotation.Number,
                                           Title = chapterAnnotation.Title,
                                           Annotation = chapterAnnotation.Annotation
                                       };
            return chapterAnnotationDto;
        }
    }
}