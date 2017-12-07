using System.Collections.Generic;
using System.Linq;
using Sheep.Model.Read.Entities;
using Sheep.ServiceModel.Volumes.Entities;

namespace Sheep.ServiceInterface.Volumes.Mappers
{
    public static class VolumeToVolumeDtoMapper
    {
        public static VolumeDto MapToVolumeDto(this Volume volume, IEnumerable<VolumeAnnotation> volumeAnnotations)
        {
            if (volume.Meta == null)
            {
                volume.Meta = new Dictionary<string, string>();
            }
            var volumeDto = new VolumeDto
                            {
                                Id = volume.Id,
                                Number = volume.Number,
                                Title = volume.Title,
                                Abbreviation = volume.Abbreviation,
                                ChaptersCount = volume.ChaptersCount,
                                SubjectsCount = volume.SubjectsCount,
                                Annotations = volumeAnnotations?.Select(va => va.MapToVolumeAnnotationDto()).ToList() ?? new List<VolumeAnnotationDto>()
                            };
            return volumeDto;
        }
    }
}