using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using MiniSpace.Services.Events.Application.Exceptions;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
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
            var formats = new[]
            {
                "yyyy-MM-ddTHH:mm:ssZ",
                "yyyy-MM-ddTHH:mm:ss.fffZ",
                "yyyy-MM-ddTHH:mm:ss",
                "yyyy-MM-ddTHH:mm:ss.fff"
            };

            if (!DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out DateTime date))
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
        
        public EventEngagementType ParseEngagementType(string engagementTypeString)
        {
            if (!Enum.TryParse<EventEngagementType>(engagementTypeString, true, out var engagementType))
            {
                throw new InvalidEventEngagementTypeException(engagementTypeString);
            }
            return engagementType;
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
        
        public void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > 300)
                throw new InvalidEventNameException(name);
        }
        
        public void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description) || description.Length > 1000)
                throw new InvalidEventDescriptionException(description);
        }
        
        public void ValidateMediaFiles(List<Guid> mediaFiles)
        {
            if (mediaFiles.Count > 5)
                throw new InvalidNumberOfEventMediaFilesException(mediaFiles.Count);
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
        
        public State? RestrictState(State? state)
        {
            if (state != State.Published && state != State.Archived)
                return null;
            return state;
        }
    }
}