namespace Astravent.Web.Wasm.HttpClients
{
    public class HttpResponse<T>
    {
        public T Content { get; set; }
        public HttpResponse() { }
        public ErrorMessage ErrorMessage { get; set; }
        public bool IsSuccessStatusCode { get; set; }

        public HttpResponse(T content)
        {
            Content = content;
            IsSuccessStatusCode = true;
        }
        
        public HttpResponse(ErrorMessage errorMessage)
        {
            ErrorMessage = errorMessage;
            IsSuccessStatusCode = false;
        }
    }
}
