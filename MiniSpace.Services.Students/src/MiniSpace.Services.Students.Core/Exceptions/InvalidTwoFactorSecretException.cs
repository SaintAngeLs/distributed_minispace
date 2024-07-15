namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class InvalidTwoFactorSecretException : DomainException
    {
        public override string Code { get; } = "invalid_two_factor_secret";
        public Guid Id { get; }

        public InvalidTwoFactorSecretException(Guid id) : base($"Student with id: {id} has an invalid two-factor authentication secret.")
        {
            Id = id;
        }
    }
}