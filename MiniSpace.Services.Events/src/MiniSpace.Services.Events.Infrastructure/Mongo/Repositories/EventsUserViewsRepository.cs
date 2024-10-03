using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;
using Paralax.Persistence.MongoDB;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Repositories
{
    public class EventsUserViewsRepository : IEventsUserViewsRepository
    {
        private readonly IMongoRepository<UserEventsViewsDocument, Guid> _repository;

        public EventsUserViewsRepository(IMongoRepository<UserEventsViewsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<EventsViews> GetAsync(Guid userId)
        {
            var document = await _repository.GetAsync(x => x.UserId == userId);
            return document?.ToEntity();
        }

        public async Task AddAsync(EventsViews eventsViews)
        {
            var document = eventsViews.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task UpdateAsync(EventsViews eventsViews)
        {
            var document = eventsViews.AsDocument();
            await _repository.UpdateAsync(document);
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _repository.DeleteAsync(x => x.UserId == userId);
        }
    }
}
