using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    public class GetPaginatedEventsHandler : IQueryHandler<GetPaginatedEvents, MiniSpace.Services.Events.Application.DTO.PagedResult<EventDto>>
    {
        private readonly IMongoCollection<EventDocument> _eventCollection;
        private readonly IAppContext _appContext;

        public GetPaginatedEventsHandler(IMongoCollection<EventDocument> eventCollection, IAppContext appContext)
        {
            _eventCollection = eventCollection;
            _appContext = appContext;
        }

        public async Task<MiniSpace.Services.Events.Application.DTO.PagedResult<EventDto>> HandleAsync(GetPaginatedEvents query, CancellationToken cancellationToken)
        {
            var filter = Builders<EventDocument>.Filter.Empty;

            var totalItems = await _eventCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            var events = await _eventCollection
                .Find(filter)
                .Skip((query.Page - 1) * query.PageSize)
                .Limit(query.PageSize)
                .ToListAsync(cancellationToken);

            var studentId = _appContext.Identity.Id; 
            var eventDtos = events.Select(e => e.AsDto(studentId)).ToList();

            // Return a paged result with totalItems casted to int
            return new MiniSpace.Services.Events.Application.DTO.PagedResult<EventDto>(eventDtos, query.Page, query.PageSize, (int)totalItems);
        }
    }
}
