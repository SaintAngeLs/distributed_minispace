using System;
using Pacco.Services.Operations.Api.DTO;

namespace Pacco.Services.Operations.Api.Services
{
    public class OperationUpdatedEventArgs : EventArgs
    {
        public OperationDto Operation { get; }

        public OperationUpdatedEventArgs(OperationDto operation)
        {
            Operation = operation;
        }
    }
}