using MiniSpacePwa.HttpClients;

namespace MiniSpacePwa.Areas.Http
{
    public interface IErrorMapperService
    {
        string MapError(ErrorMessage error);
    }
}
