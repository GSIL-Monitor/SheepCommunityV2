using System.Collections.Generic;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceModel.Chapters.Entities;

namespace Sheep.ServiceInterface.Chapters.Mappers
{
    public static class ChapterToBasicChapterDtoMapper
    {
        public static BasicChapterDto MapToBasicChapterDto(this Chapter chapter, Volume volume)
        {
            if (chapter.Meta == null)
            {
                chapter.Meta = new Dictionary<string, string>();
            }
            var chapterDto = new BasicChapterDto
                             {
                                 Id = chapter.Id,
                                 VolumeNumber = chapter.VolumeNumber,
                                 VolumeTitle = volume?.Title,
                                 Number = chapter.Number,
                                 Title = chapter.Title,
                                 ParagraphsCount = chapter.ParagraphsCount
                             };
            return chapterDto;
        }
    }
}