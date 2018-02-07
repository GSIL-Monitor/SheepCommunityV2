using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceInterface.Books.Mappers;
using Sheep.ServiceInterface.Chapters.Mappers;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceInterface.Volumes.Mappers;
using Sheep.ServiceModel.ChapterReads.Entities;

namespace Sheep.ServiceInterface.ChapterReads.Mappers
{
    public static class ChapterReadToChapterReadDtoMapper
    {
        public static ChapterReadDto MapToChapterReadDto(this ChapterRead chapterRead, Book book, Volume volume, Chapter chapter, IUserAuth user)
        {
            var chapterReadDto = new ChapterReadDto
                                 {
                                     Book = book?.MapToBasicBookDto(),
                                     Volume = volume?.MapToBasicVolumeDto(),
                                     Chapter = chapter?.MapToBasicChapterDto(volume),
                                     User = user?.MapToBasicUserDto(),
                                     CreatedDate = chapterRead.CreatedDate.ToUnixTime()
                                 };
            return chapterReadDto;
        }
    }
}