namespace MiniSpace.Services.Reports.Application.Exceptions
{
    public class AppException : Exception
    {
        public virtual string Code { get; }
        
        protected AppException(string message) : base(message)
        {
        }
    }
}
