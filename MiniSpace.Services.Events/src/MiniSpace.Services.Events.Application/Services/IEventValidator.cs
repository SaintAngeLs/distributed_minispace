using System;
using System.Collections.Generic;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.Services
{
    public interface IEventValidator
    {
        Category ParseCategory(string categoryString);
        DateTime ParseDate(string dateString, string fieldName);
        State ParseState(string stateString);
        EventEngagementType ParseEngagementType(string engagementTypeString);
        void ValidateDates(DateTime earlierDate, DateTime laterDate, string earlierDateString, string endDateString);
        (int pageNumber, int pageSize) PageFilter(int pageNumber, int pageSize);
        void ValidateName(string name);
        void ValidateDescription(string description);
        void ValidateMediaFiles(List<Guid> mediaFiles);
        void ValidateCapacity(int capacity);
        void ValidateFee(decimal fee);
        void ValidateUpdatedCapacity(int currentCapacity, int newCapacity);
        void ValidateUpdatedFee(decimal currentFee, decimal newFee);
        State? RestrictState(State? state);
    }
}