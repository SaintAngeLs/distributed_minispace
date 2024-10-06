using System;
using System.Threading.Tasks;
using Paralax.Persistence.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MiniSpace.Services.Identity.Infrastructure.Mongo.Documents;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Identity.Infrastructure.Mongo
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IApplicationBuilder UseMongo(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var users = scope.ServiceProvider.GetService<IMongoRepository<UserDocument, Guid>>().Collection;
                var userBuilder = Builders<UserDocument>.IndexKeys;
                Task.Run(async () => await users.Indexes.CreateOneAsync(
                    new CreateIndexModel<UserDocument>(userBuilder.Ascending(i => i.Email),
                        new CreateIndexOptions
                        {
                            Unique = true
                        })));
            }

            return builder;
        }
    }
}