using Paralax.Types;
using MiniSpace.Services.Students.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class EducationDocument
    {
        public string InstitutionName { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}
