using Quartz;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Mappers
{
    public static class JobDetailToJobDtoMapper
    {
        public static JobDto MapToJobDto(this IJobDetail jobDetail, TriggerDto[] triggers)
        {
            var jobDto = new JobDto
                         {
                             Key = jobDetail.Key.MapToJobKeyDto(),
                             Description = jobDetail.Description,
                             JobType = jobDetail.JobType.FullName,
                             ConcurrentExecutionDisallowed = jobDetail.ConcurrentExecutionDisallowed,
                             Durable = jobDetail.Durable,
                             PersistJobDataAfterExecution = jobDetail.PersistJobDataAfterExecution,
                             RequestsRecovery = jobDetail.RequestsRecovery,
                             JobDataMap = jobDetail.JobDataMap.MapToJobDataMapDto(),
                             Triggers = triggers
                         };
            return jobDto;
        }
    }
}