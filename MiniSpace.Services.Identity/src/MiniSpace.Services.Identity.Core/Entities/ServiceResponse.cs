namespace MiniSpace.Services.Identity.Core.Entities
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public static ServiceResponse<T> SuccessResponse(T data) => new ServiceResponse<T> { Success = true, Data = data };
        public static ServiceResponse<T> FailureResponse(string errorMessage) => new ServiceResponse<T> { Success = false, ErrorMessage = errorMessage };
    }
}