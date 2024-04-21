using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Http
{
    public class ErrorMapperService : IErrorMapperService
    {
        public string MapError(ErrorMessage error)
        {
            switch (error.Code)
            {
                case "invalid_credentials":
                    return "Failed to sign in. Please check your credentials and try again.";
                default:
                    return "Something went wrong. Please try again later.";
            }
        }
    }
}