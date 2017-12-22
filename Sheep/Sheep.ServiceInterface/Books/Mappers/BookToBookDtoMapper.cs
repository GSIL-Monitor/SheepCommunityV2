using System.Collections.Generic;
using ServiceStack.Text;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceModel.Books.Entities;

namespace Sheep.ServiceInterface.Books.Mappers
{
    public static class BookToBookDtoMapper
    {
        public static BookDto MapToBookDto(this Book book)
        {
            if (book.Meta == null)
            {
                book.Meta = new Dictionary<string, string>();
            }
            var bookDto = new BookDto
                          {
                              Id = book.Id,
                              Title = book.Title,
                              Summary = book.Summary,
                              PictureUrl = book.PictureUrl,
                              Author = book.Author,
                              Tags = book.Tags,
                              IsPublished = book.IsPublished,
                              PublishedDate = book.PublishedDate?.ToUnixTime(),
                              VolumesCount = book.VolumesCount,
                              BookmarksCount = book.BookmarksCount,
                              RatingsCount = book.RatingsCount,
                              RatingsAverageValue = book.RatingsAverageValue,
                              SharesCount = book.SharesCount
                          };
            return bookDto;
        }
    }
}