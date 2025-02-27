using Astravent.Web.Wasm.Models.Layout;

namespace Astravent.Web.Wasm.Areas.Menu;

#nullable enable
public interface IMenuService
{
    IEnumerable<MudComponent> Components { get; }

    IEnumerable<MudComponent> Api { get; }

    MudComponent? GetParent(Type? type);

    MudComponent? GetComponent(Type? type);

    /// <summary>
    /// Gets a component by its type name.
    /// </summary>
    /// <param name="typeName">The name of the type, such as <c>MudAlert</c>.</param>
    /// <returns>The matching component, or <c>null</c> if none was found.</returns>
    string? GetComponentName(string typeName);
}