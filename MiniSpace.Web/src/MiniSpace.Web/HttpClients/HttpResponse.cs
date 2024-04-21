namespace MiniSpace.Web.HttpClients
{
    public class HttpResponse<T>
    {
        public T Content { get; set; }
        public string ErrorMessage { get; set; }
        
        public HttpResponse(T content)
        {
            Content = content;
        }
        
        public HttpResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}