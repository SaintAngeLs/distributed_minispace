namespace Astravent.Web.Wasm.Areas.Operations.Maintenance;

public interface IMaintenanceService
{
    Task<bool> IsUnderMaintenanceAsync();
}