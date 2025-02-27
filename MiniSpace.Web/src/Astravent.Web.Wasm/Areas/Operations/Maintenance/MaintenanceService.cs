namespace Astravent.Web.Wasm.Areas.Operations.Maintenance;

public class MaintenanceService : IMaintenanceService
{
    public Task<bool> IsUnderMaintenanceAsync()
    {
        bool maintenanceFlag = false; 
        return Task.FromResult(maintenanceFlag);
    }
}