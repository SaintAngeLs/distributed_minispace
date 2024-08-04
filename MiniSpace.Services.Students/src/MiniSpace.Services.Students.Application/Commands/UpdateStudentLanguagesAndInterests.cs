using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class UpdateStudentLanguagesAndInterests : ICommand
    {
        public Guid StudentId { get; }
        public IEnumerable<string> Languages { get; }
        public IEnumerable<string> Interests { get; }

        public UpdateStudentLanguagesAndInterests(Guid studentId, IEnumerable<string> languages, IEnumerable<string> interests)
        {
            StudentId = studentId;
            Languages = languages ?? throw new ArgumentNullException(nameof(languages));
            Interests = interests ?? throw new ArgumentNullException(nameof(interests));
        }
    }
}
