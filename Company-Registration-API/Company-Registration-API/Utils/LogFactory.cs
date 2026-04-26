using log4net;

namespace QPSOS.Web.API.Utils
{
    public class LogFactory : ILogFactory
    {
        public ILog CreateLogger<T>()
        {
            return LogManager.GetLogger(typeof(T));
        }
    }
}
