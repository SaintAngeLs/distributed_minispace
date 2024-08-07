using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MiniSpace.APIGateway.Ocelot
{
    internal static class ApplicationBuilderExtensions
    {
        private const string OperationHeader = "X-Operation";
        private const string ResourceIdKey = "resource-id";
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static void SetResourceId(this JObject jObject, string id)
        {
            var idProperty = jObject.Property("id", StringComparison.InvariantCultureIgnoreCase);
            if (idProperty is null)
            {
                jObject.Add("id", id);
                return;
            }

            idProperty.Value = id;
        }

        public static void SetResourceIdFoRequest(this HttpContext context, string id)
            => context.Items.TryAdd(ResourceIdKey, id);

        public static string GetResourceIdFoRequest(this HttpContext context)
            => context.Items.TryGetValue(ResourceIdKey, out var id) ? id as string : string.Empty;

        public static void SetOperationHeader(this HttpResponse response, string id)
            => response.Headers[OperationHeader] = $"operations/{id}";
    }
}