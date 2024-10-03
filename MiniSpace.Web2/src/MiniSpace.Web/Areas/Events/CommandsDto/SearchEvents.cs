using System;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Areas.Events.CommandsDto
{
    public class SearchEvents
    {
        public string Name { get; set; }
        public string Organizer { get; set; }
        public Guid OrganizationId { get; set; }
        public string Category { get; set; }
        public string State { get; set; }
        public IEnumerable<Guid> Friends { get; set; }
        public string FriendsEngagementType { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public PageableDto Pageable { get; set; }

        public override string ToString()
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(Name)) queryParams.Add($"Name={Name}");
            if (!string.IsNullOrEmpty(Organizer)) queryParams.Add($"Organizer={Organizer}");
            if (OrganizationId != Guid.Empty) queryParams.Add($"OrganizationId={OrganizationId}");
            if (!string.IsNullOrEmpty(Category)) queryParams.Add($"Category={Category}");
            if (!string.IsNullOrEmpty(State)) queryParams.Add($"State={State}");
            if (Friends != null && Friends.Any()) queryParams.Add($"Friends={string.Join(",", Friends)}");
            if (!string.IsNullOrEmpty(FriendsEngagementType)) queryParams.Add($"FriendsEngagementType={FriendsEngagementType}");
            if (!string.IsNullOrEmpty(DateFrom)) queryParams.Add($"DateFrom={DateFrom}");
            if (!string.IsNullOrEmpty(DateTo)) queryParams.Add($"DateTo={DateTo}");
            if (Pageable != null) queryParams.Add($"Pageable.Page={Pageable.Page}&Pageable.Size={Pageable.Size}");

            return string.Join("&", queryParams);
        }
    }
}
