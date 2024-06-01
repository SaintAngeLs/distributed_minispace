using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Application.Dto;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class SearchPosts : ICommand
    {
        public Guid StudentId { get; set; }
        public PageableDto Pageable { get; set; }
    }
}