using System.Collections.Generic;
using Sheep.Model.Bookstore.Entities;
using Sheep.ServiceModel.Volumes.Entities;

namespace Sheep.ServiceInterface.Volumes.Mappers
{
    public static class VolumeToBasicVolumeDtoMapper
    {
        public static BasicVolumeDto MapToBasicVolumeDto(this Volume volume)
        {
            if (volume.Meta == null)
            {
                volume.Meta = new Dictionary<string, string>();
            }
            var volumeDto = new BasicVolumeDto
                            {
                                Id = volume.Id,
                                Number = volume.Number,
                                Title = volume.Title
                            };
            return volumeDto;
        }
    }
}