using System;
using System.Threading.Tasks;
using Pacco.Services.Operations.Api.DTO;
using Pacco.Services.Operations.Api.Types;

namespace Pacco.Services.Operations.Api.Services
{
    public interface IOperationsService
    {
        event EventHandler<OperationUpdatedEventArgs> OperationUpdated;
        Task<OperationDto> GetAsync(string id);

        Task<(bool updated, OperationDto operation)> TrySetAsync(string id, string userId, string name,
            OperationState state, string code = null, string reason = null);
    }
}