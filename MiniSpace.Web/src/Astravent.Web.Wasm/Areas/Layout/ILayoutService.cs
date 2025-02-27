using Astravent.Web.Wasm.DTO;

namespace Astravent.Web.Wasm.Areas.Layout;

public interface ILayoutService
{
    bool IsRTL { get; }
    FrontendVersion CurrentFrontendVersion { get; }
    void ToggleRightToLeft();
    Task CycleThemeModeAsync();
}

