using System.Collections.Generic;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceModel.Books.Entities;

namespace Sheep.ServiceInterface.Books.Mappers
{
    public static class BookToBasicBookDtoMapper
    {
        public static BasicBookDto MapToBasicBookDto(this Book book)
        {
            if (book.Meta == null)
            {
                book.Meta = new Dictionary<string, string>();
            }
            var bookDto = new BasicBookDto
                          {
                              Id = book.Id,
                              Title = book.Title
                          };
            return bookDto;
        }
    }
}