using System;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.Services
{
    public interface IEventValidator
    {
        Category ParseCategory(string categoryString);
        DateTime ParseDate(string dateString, string fieldName);
        void ValidateDates(DateTime earlierDate, DateTime laterDate, string earlierDateString, string endDateString);
        (int pageNumber, int pageSize) PageFilter(int pageNumber, int pageSize);
        void ValidateRequiredField(string fieldValue, string fieldName);
        void ValidateUpdatedCapacity(int currentCapacity, int newCapacity);
        void ValidateUpdatedFee(decimal currentFee, decimal newFee);
    }
}