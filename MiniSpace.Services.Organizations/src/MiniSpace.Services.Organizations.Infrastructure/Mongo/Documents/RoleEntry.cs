using MiniSpace.Services.Organizations.Core.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    public class RoleEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.Document)]
        public Dictionary<string, bool> Permissions { get; set; }

        public static RoleEntry FromEntity(Role role)
        {
            return new RoleEntry
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                Permissions = role.Permissions.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value)
            };
        }

        public Role ToEntity()
        {
            return new Role(
                Id,
                Name,
                Description,
                Permissions.ToDictionary(kvp => Enum.Parse<Permission>(kvp.Key), kvp => kvp.Value)
            );
        }
    }
}
