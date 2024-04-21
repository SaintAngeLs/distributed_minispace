namespace MiniSpace.Web.HttpClients
{
    public class HttpResponse<T>
    {
        public T Content { get; set; }
        public ErrorMessage ErrorMessage { get; set; }
        
        public HttpResponse(T content)
        {
            Content = content;
        }
        
        public HttpResponse(ErrorMessage errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}