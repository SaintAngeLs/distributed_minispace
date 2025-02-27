using System;
using System.Collections.Generic;
using System.Linq;
using Astravent.Web.Wasm.Models.Layout;
using MudBlazor;

namespace Astravent.Web.Wasm.Areas.Menu
{
    #nullable enable
    public class MenuService : IMenuService
    {
        // Example lists of components.
        // In a real application these might be loaded from configuration,
        // reflection, or a database.
        private readonly List<MudComponent> _components;
        private readonly List<MudComponent> _apiComponents;

        public MenuService()
        {
            // Populate the Components list with sample data.
            _components = new List<MudComponent>
            {
                new MudComponent { Type = typeof(MudAlert), Name = "MudAlert"},
                new MudComponent { Type = typeof(MudButton), Name = "MudButton" },
                new MudComponent { Type = typeof(MudTable<object>), Name = "MudTable" },
                // Add more components as needed...
            };

            // Populate the Api list with sample data.
            _apiComponents = new List<MudComponent>
            {
                new MudComponent { Type = typeof(MudAlert), Name = "API MudAlert" },
                new MudComponent { Type = typeof(MudButton), Name = "API MudButton"},
                // Add more API-related components...
            };
        }

        public IEnumerable<MudComponent> Components => _components;

        public IEnumerable<MudComponent> Api => _apiComponents;

        /// <summary>
        /// Returns the parent component of the component corresponding to the specified type.
        /// </summary>
        public MudComponent? GetParent(Type? type)
        {
            if (type == null)
                return null;

            // Find the component corresponding to the given type.
            var comp = GetComponent(type);
            return comp;
        }

        /// <summary>
        /// Returns the MudComponent that matches the specified type.
        /// </summary>
        public MudComponent? GetComponent(Type? type)
        {
            if (type == null)
                return null;

            // Look for a matching component in the Components collection.
            return _components.FirstOrDefault(c => c.Type == type)
                   ?? _apiComponents.FirstOrDefault(c => c.Type == type);
        }

        /// <summary>
        /// Returns the component name (as defined in our lists) for the given type name.
        /// </summary>
        public string? GetComponentName(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return null;

            // First try to find in Components.
            var comp = _components.FirstOrDefault(c => string.Equals(c.Name, typeName, StringComparison.OrdinalIgnoreCase))
                    ?? _apiComponents.FirstOrDefault(c => string.Equals(c.Name, typeName, StringComparison.OrdinalIgnoreCase));
            return comp?.Name;
        }
    }
}
