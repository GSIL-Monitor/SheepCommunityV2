using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Views.Entities;

namespace Sheep.ServiceInterface.Views.Mappers
{
    public static class ViewToViewDtoMapper
    {
        public static ViewDto MapToViewDto(this View view, IUserAuth user, string title)
        {
            var viewDto = new ViewDto
                          {
                              ParentType = view.ParentType,
                              ParentId = view.ParentId,
                              ParentTitle = title,
                              User = user?.MapToBasicUserDto(),
                              CreatedDate = view.CreatedDate.ToUnixTime()
                          };
            return viewDto;
        }
    }
}