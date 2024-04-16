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
    }
}