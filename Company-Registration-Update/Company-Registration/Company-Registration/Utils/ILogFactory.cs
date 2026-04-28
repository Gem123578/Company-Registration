using log4net;

namespace QSS.POS.Front.UI.Utils
{
    public interface ILogFactory
    {
        ILog CreateLogger<T>();
    }
}
