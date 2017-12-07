using System.Collections.Generic;
using Sheep.Model.Read.Entities;
using Sheep.ServiceModel.Subjects.Entities;

namespace Sheep.ServiceInterface.Subjects.Mappers
{
    public static class SubjectToSubjectDtoMapper
    {
        public static SubjectDto MapToSubjectDto(this Subject subject)
        {
            if (subject.Meta == null)
            {
                subject.Meta = new Dictionary<string, string>();
            }
            var subjectDto = new SubjectDto
                             {
                                 Id = subject.Id,
                                 Number = subject.Number,
                                 Title = subject.Title
                             };
            return subjectDto;
        }
    }
}