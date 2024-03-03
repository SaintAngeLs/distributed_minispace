using System.Threading.Tasks;
using Pacco.Services.Operations.Api.DTO;

namespace Pacco.Services.Operations.Api.Services
{
    public interface IHubService
    {
        Task PublishOperationPendingAsync(OperationDto operation);
        Task PublishOperationCompletedAsync(OperationDto operation);
        Task PublishOperationRejectedAsync(OperationDto operation);
    }
}