using System;

namespace MiniSpace.Services.Reports.Core.Entities
{
    public class Student(Guid id, int activeReports)
    {
        public Guid Id { get; private set; } = id;
        public int ActiveReports { get; private set; } = activeReports;
    }
}