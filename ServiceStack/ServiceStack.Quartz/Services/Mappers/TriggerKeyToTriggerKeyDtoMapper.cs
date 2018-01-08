using Quartz;
using ServiceStack.Quartz.Services.Models.Entities;

namespace ServiceStack.Quartz.Services.Mappers
{
    public static class TriggerKeyToTriggerKeyDtoMapper
    {
        public static TriggerKeyDto MapToTriggerKeyDto(this TriggerKey triggerKey)
        {
            var triggerKeyDto = new TriggerKeyDto
                                {
                                    Group = triggerKey.Group,
                                    Name = triggerKey.Name
                                };
            return triggerKeyDto;
        }
    }
}