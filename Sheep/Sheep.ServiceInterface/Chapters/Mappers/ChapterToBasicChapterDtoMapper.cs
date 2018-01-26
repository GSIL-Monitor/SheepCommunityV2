using System.Collections.Generic;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceModel.Chapters.Entities;

namespace Sheep.ServiceInterface.Chapters.Mappers
{
    public static class ChapterToBasicChapterDtoMapper
    {
        public static BasicChapterDto MapToBasicChapterDto(this Chapter chapter)
        {
            if (chapter.Meta == null)
            {
                chapter.Meta = new Dictionary<string, string>();
            }
            var chapterDto = new BasicChapterDto
                             {
                                 Id = chapter.Id,
                                 Number = chapter.Number,
                                 Title = chapter.Title
                             };
            return chapterDto;
        }
    }
}