namespace MiniSpace.Services.Reactions.Core.Exceptions
{
    public abstract class DomainException(string message) : Exception(message)
    {
        public virtual string Code { get; }
    }
}
