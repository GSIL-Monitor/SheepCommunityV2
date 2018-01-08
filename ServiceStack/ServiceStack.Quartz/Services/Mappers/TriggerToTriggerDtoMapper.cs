using Quartz;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Mappers
{
    public static class TriggerToTriggerDtoMapper
    {
        public static TriggerDto MapToTriggerDto(this ITrigger trigger)
        {
            var triggerDto = new TriggerDto
                             {
                                 Key = trigger.Key.MapToTriggerKeyDto(),
                                 JobKey = trigger.JobKey.MapToJobKeyDto(),
                                 Description = trigger.Description,
                                 CalendarName = trigger.CalendarName,
                                 StartTimeUtc = trigger.StartTimeUtc,
                                 EndTimeUtc = trigger.EndTimeUtc,
                                 HasMillisecondPrecision = trigger.HasMillisecondPrecision,
                                 MisfireInstruction = trigger.MisfireInstruction,
                                 Priority = trigger.Priority,
                                 MayFireAgain = trigger.GetMayFireAgain(),
                                 NextFireTimeUtc = trigger.GetNextFireTimeUtc(),
                                 PreviousFireTimeUtc = trigger.GetPreviousFireTimeUtc(),
                                 FinalFireTimeUtc = trigger.FinalFireTimeUtc,
                                 JobDataMap = trigger.JobDataMap.MapToJobDataMapDto()
                             };
            return triggerDto;
        }
    }
}