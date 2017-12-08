using System.Collections.Generic;
using Sheep.Model.Read.Entities;
using Sheep.ServiceModel.Volumes.Entities;

namespace Sheep.ServiceInterface.Volumes.Mappers
{
    public static class VolumeAnnotationToVolumeAnnotationDtoMapper
    {
        public static VolumeAnnotationDto MapToVolumeAnnotationDto(this VolumeAnnotation volumeAnnotation)
        {
            if (volumeAnnotation.Meta == null)
            {
                volumeAnnotation.Meta = new Dictionary<string, string>();
            }
            var volumeAnnotationDto = new VolumeAnnotationDto
                                      {
                                          Id = volumeAnnotation.Id,
                                          VolumeNumber = volumeAnnotation.VolumeNumber,
                                          Number = volumeAnnotation.Number,
                                          Title = volumeAnnotation.Title,
                                          Annotation = volumeAnnotation.Annotation
                                      };
            return volumeAnnotationDto;
        }
    }
}