using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Http
{
    public interface IErrorMapperService
    {
        string MapError(ErrorMessage error);
    }
}

