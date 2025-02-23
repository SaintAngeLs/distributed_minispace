using Paralax.CQRS.Commands;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Students.Application.Commands;

public class UpdateUserLanguagesAndInterests : ICommand
{
    public Guid UserId { get; }
    public IEnumerable<string> Languages { get; }
    public IEnumerable<string> Interests { get; }

    public UpdateUserLanguagesAndInterests(Guid userId, IEnumerable<string> languages, IEnumerable<string> interests)
    {
        UserId = userId;
        Languages = languages ?? throw new ArgumentNullException(nameof(languages));
        Interests = interests ?? throw new ArgumentNullException(nameof(interests));
    }
}