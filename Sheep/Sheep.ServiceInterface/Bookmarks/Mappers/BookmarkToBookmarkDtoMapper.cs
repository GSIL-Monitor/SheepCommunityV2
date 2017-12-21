using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Bookmarks.Entities;

namespace Sheep.ServiceInterface.Bookmarks.Mappers
{
    public static class BookmarkToBookmarkDtoMapper
    {
        public static BookmarkDto MapToBookmarkDto(this Bookmark bookmark, IUserAuth user, string title)
        {
            var bookmarkDto = new BookmarkDto
                              {
                                  ParentType = bookmark.ParentType,
                                  ParentId = bookmark.ParentId,
                                  ParentTitle = title,
                                  User = user?.MapToBasicUserDto(),
                                  CreatedDate = bookmark.CreatedDate.ToUnixTime()
                              };
            return bookmarkDto;
        }
    }
}