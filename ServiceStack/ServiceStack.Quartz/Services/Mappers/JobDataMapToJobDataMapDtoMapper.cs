using Quartz;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Mappers
{
    public static class JobDataMapToJobDataMapDtoMapper
    {
        public static JobDataMapDto MapToJobDataMapDto(this JobDataMap jobDataMap)
        {
            var jobDataMapDto = new JobDataMapDto
                                {
                                    Count = jobDataMap.Count,
                                    Dirty = jobDataMap.Dirty,
                                    IsEmpty = jobDataMap.IsEmpty,
                                    Data = jobDataMap.WrappedMap.ToJson()
                                };
            return jobDataMapDto;
        }
    }
}