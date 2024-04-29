using System;
using System.Globalization;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Services
{
    public class EventValidator: IEventValidator
    {
        private readonly string _expectedFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public Category ParseCategory(string categoryString)
        {
            if (!Enum.TryParse<Category>(categoryString, true, out var category))
            {
                throw new InvalidEventCategoryException(categoryString);
            }
            return category;
        }

        public DateTime ParseDate(string dateString, string fieldName)
        {
            if (!DateTime.TryParseExact(dateString, _expectedFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime date))
            {
                throw new InvalidEventDateTimeException(fieldName, dateString);
            }
            return date;
        }
        
        public State ParseState(string stateString)
        {
            if (!Enum.TryParse<State>(stateString, true, out var state))
            {
                throw new InvalidEventStateException(stateString);
            }
            return state;
        }

        public void ValidateDates(DateTime earlierDate, DateTime laterDate,  string earlierDateField, string laterDateField)
        {
            if (laterDate <= earlierDate)
                throw new InvalidEventDateTimeOrderException(earlierDate, laterDate,
                    earlierDateField, laterDateField);
        }
        
        public (int pageNumber, int pageSize) PageFilter(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 0 ? 0 : pageNumber;
            pageSize = pageSize > 10 ? 10 : pageSize;
            return (pageNumber, pageSize);
        }
        
        public void ValidateRequiredField(string fieldValue, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
                throw new EmptyRequiredEventFieldException(fieldName);
        }
        
        public void ValidateCapacity(int capacity)
        {
            if (capacity <= 0 || capacity > 1000)
                throw new InvalidEventCapacityException(capacity);
        }
        
        public void ValidateFee(decimal fee)
        {
            if (fee < 0.0m || fee > 1000.0m)
                throw new InvalidEventFeeException(fee);
        }
        
        public void ValidateUpdatedCapacity(int currentCapacity, int newCapacity)
        {
            if (newCapacity < currentCapacity)
                throw new InvalidUpdatedEventCapacityException(currentCapacity, newCapacity);
        }
        
        public void ValidateUpdatedFee(decimal currentFee, decimal newFee)
        {
            if (newFee > currentFee)
                throw new InvalidUpdatedEventFeeException(currentFee, newFee);
        }
    }
}