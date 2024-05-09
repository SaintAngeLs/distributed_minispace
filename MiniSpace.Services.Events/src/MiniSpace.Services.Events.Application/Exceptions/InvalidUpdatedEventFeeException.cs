namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class InvalidUpdatedEventFeeException : AppException
    {
        public override string Code { get; } = "invalid_updated_event_fee";
        public decimal CurrentFee { get; }
        public decimal NewFee { get; }

        public InvalidUpdatedEventFeeException(decimal currentFee, decimal newFee) 
            : base($"Updated fee: {newFee} cannot be greater than current fee: {currentFee}.")
        {
            CurrentFee = currentFee;
            NewFee = newFee;
        }
    }
}