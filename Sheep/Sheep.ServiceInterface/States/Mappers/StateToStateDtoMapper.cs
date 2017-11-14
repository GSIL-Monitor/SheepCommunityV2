using Sheep.Model.Geo.Entities;
using Sheep.ServiceModel.States.Entities;

namespace Sheep.ServiceInterface.States.Mappers
{
    public static class StateToStateDtoMapper
    {
        public static StateDto MapToStateDto(this State state)
        {
            var stateDto = new StateDto
                           {
                               Id = state.Id,
                               Name = state.Name
                           };
            return stateDto;
        }
    }
}