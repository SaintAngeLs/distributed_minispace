namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class InvalidRatingValueException : DomainException
    {
        public override string Code { get; } = "invalid_rating_value";
        public int Rating { get; }

        public InvalidRatingValueException(int rating) : base($"Invalid rating value: {rating}. It must be between 1 and 5.")
        {
            Rating = rating;
        }
    }
}