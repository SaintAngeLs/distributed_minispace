using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Models.Events
{
    public class SearchEventModel
    {
        public string Name { get; set; }
        public string Organizer { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public PageableDto Pageable { get; set; }
    }
}