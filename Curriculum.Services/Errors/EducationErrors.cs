using Curriculum.Core.Entities;
using Curriculum.Core.Results;

namespace Curriculum.Services.Errors;
    
public sealed record EducationNotFoundError : NotFoundError<Education>
{
    public EducationNotFoundError(Guid id, IDictionary<string, object?>? httpExtensions = null)
        : base($"Education with id {id} not found.",  httpExtensions)
    { }
    
    public EducationNotFoundError(string institution, IDictionary<string, object?>? httpExtensions = null)
        : base($"Education with institution '{institution}' not found.",  httpExtensions)
    { }
}