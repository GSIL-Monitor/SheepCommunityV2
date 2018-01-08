using Quartz;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Mappers
{
    public static class SchedulerMetaDataToSchedulerDtoMapper
    {
        public static SchedulerDto MapToSchedulerDto(this SchedulerMetaData schedulerMetaData)
        {
            var schedulerDto = new SchedulerDto
                               {
                                   InStandbyMode = schedulerMetaData.InStandbyMode,
                                   JobStoreClustered = schedulerMetaData.JobStoreClustered,
                                   JobStoreSupportsPersistence = schedulerMetaData.JobStoreSupportsPersistence,
                                   JobStoreType = schedulerMetaData.JobStoreType.FullName,
                                   NumberOfJobsExecuted = schedulerMetaData.NumberOfJobsExecuted,
                                   RunningSince = schedulerMetaData.RunningSince,
                                   SchedulerInstanceId = schedulerMetaData.SchedulerInstanceId,
                                   SchedulerName = schedulerMetaData.SchedulerName,
                                   SchedulerRemote = schedulerMetaData.SchedulerRemote,
                                   SchedulerType = schedulerMetaData.SchedulerType.FullName,
                                   Shutdown = schedulerMetaData.Shutdown,
                                   Started = schedulerMetaData.Started,
                                   ThreadPoolSize = schedulerMetaData.ThreadPoolSize,
                                   ThreadPoolType = schedulerMetaData.ThreadPoolType.FullName,
                                   Version = schedulerMetaData.Version
                               };
            return schedulerDto;
        }
    }
}