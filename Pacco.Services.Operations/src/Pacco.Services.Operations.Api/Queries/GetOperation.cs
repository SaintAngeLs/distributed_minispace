using Convey.CQRS.Queries;
using Pacco.Services.Operations.Api.DTO;

namespace Pacco.Services.Operations.Api.Queries
{
    public class GetOperation : IQuery<OperationDto>
    {
        public string OperationId { get; set; }
    }
}