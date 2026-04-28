using log4net;

namespace QSS.POS.Front.UI.Utils
{
    public class LogFactory : ILogFactory
    {
        public ILog CreateLogger<T>()
        {
            return LogManager.GetLogger(typeof(T));
        }
    }
}
