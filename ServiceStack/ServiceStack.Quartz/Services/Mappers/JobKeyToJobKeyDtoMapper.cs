using Quartz;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Mappers
{
    public static class JobKeyToJobKeyDtoMapper
    {
        public static JobKeyDto MapToJobKeyDto(this JobKey jobKey)
        {
            var jobKeyDto = new JobKeyDto
                            {
                                Group = jobKey.Group,
                                Name = jobKey.Name
                            };
            return jobKeyDto;
        }
    }
}