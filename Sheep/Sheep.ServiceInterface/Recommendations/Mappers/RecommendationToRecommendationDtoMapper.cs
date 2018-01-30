using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceModel.Recommendations.Entities;

namespace Sheep.ServiceInterface.Recommendations.Mappers
{
    public static class RecommendationToRecommendationDtoMapper
    {
        public static RecommendationDto MapToRecommendationDto(this Recommendation recommendation, Post post)
        {
            var recommendationDto = new RecommendationDto
                                    {
                                        Id = recommendation.Id,
                                        ContentType = recommendation.ContentType,
                                        ContentId = recommendation.ContentId,
                                        Title = post?.Title,
                                        Summary = post?.Summary,
                                        PictureUrl = post?.PictureUrl,
                                        Position = recommendation.Position,
                                        CreatedDate = recommendation.CreatedDate.ToUnixTime()
                                    };
            return recommendationDto;
        }
    }
}