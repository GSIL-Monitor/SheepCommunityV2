using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Feedbacks.Entities;

namespace Sheep.ServiceInterface.Feedbacks.Mappers
{
    public static class FeedbackToFeedbackDtoMapper
    {
        public static FeedbackDto MapToFeedbackDto(this Feedback feedback, IUserAuth user)
        {
            var feedbackDto = new FeedbackDto
                              {
                                  Id = feedback.Id,
                                  Content = feedback.Content,
                                  Status = feedback.Status,
                                  User = user?.MapToBasicUserDto(),
                                  CreatedDate = feedback.CreatedDate.ToUnixTime(),
                                  ModifiedDate = feedback.ModifiedDate.ToUnixTime()
                              };
            return feedbackDto;
        }
    }
}