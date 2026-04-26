using log4net;

namespace QPSOS.Web.API.Utils
{
    public interface ILogFactory
    {
        ILog CreateLogger<T>();
    }
}
