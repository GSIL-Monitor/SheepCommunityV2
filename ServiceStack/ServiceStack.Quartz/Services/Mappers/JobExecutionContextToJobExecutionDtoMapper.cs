using Quartz;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Mappers
{
    public static class JobExecutionContextToJobExecutionDtoMapper
    {
        public static JobExecutionDto MapToJobExecutionDto(this IJobExecutionContext jobExecutionContext)
        {
            var jobExecutionDto = new JobExecutionDto
                                  {
                                      FireInstanceId = jobExecutionContext.FireInstanceId,
                                      JobRunTime = jobExecutionContext.JobRunTime,
                                      FireTimeUtc = jobExecutionContext.FireTimeUtc,
                                      ScheduledFireTimeUtc = jobExecutionContext.ScheduledFireTimeUtc,
                                      NextFireTimeUtc = jobExecutionContext.NextFireTimeUtc,
                                      PreviousFireTimeUtc = jobExecutionContext.PreviousFireTimeUtc,
                                      Recovering = jobExecutionContext.Recovering,
                                      RecoveringTriggerKey = jobExecutionContext.Recovering ? jobExecutionContext.RecoveringTriggerKey.MapToTriggerKeyDto() : null,
                                      RefireCount = jobExecutionContext.RefireCount,
                                      Trigger = jobExecutionContext.Trigger.MapToTriggerDto()
                                  };
            return jobExecutionDto;
        }
    }
}