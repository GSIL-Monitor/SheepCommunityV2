using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Content.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.AbuseReports.Entities;

namespace Sheep.ServiceInterface.AbuseReports.Mappers
{
    public static class AbuseReportToAbuseReportDtoMapper
    {
        public static AbuseReportDto MapToAbuseReportDto(this AbuseReport report, string title, string pictureUrl, IUserAuth abuseUser, IUserAuth user)
        {
            var reportDto = new AbuseReportDto
                            {
                                Id = report.Id,
                                ParentType = report.ParentType,
                                ParentId = report.ParentId,
                                ParentTitle = title,
                                ParentPictureUrl = pictureUrl,
                                ParentUser = abuseUser?.MapToBasicUserDto(),
                                Status = report.Status,
                                Reason = report.Reason,
                                User = user?.MapToBasicUserDto(),
                                CreatedDate = report.CreatedDate.ToUnixTime(),
                                ModifiedDate = report.ModifiedDate.ToUnixTime()
                            };
            return reportDto;
        }
    }
}