using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Http
{
    public interface IErrorMapperService
    {
        string MapError(ErrorMessage error);
    }
}

