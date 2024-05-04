using System;
using Convey.CQRS.Commands;
using MiniSpace.Services.Comments.Application.Dto;

namespace MiniSpace.Services.Comments.Application.Commands
{
    public class SearchComments : ICommand
    {
        public Guid ContextId { get; set; }
        public string CommentContext { get; set; }
        public PageableDto Pageable { get; set; }
    }
}