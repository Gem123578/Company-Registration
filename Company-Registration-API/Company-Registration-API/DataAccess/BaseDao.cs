using System.Transactions;

namespace QPSOS.Web.API.DataAccess
{
    public class BaseDao
    {
        public static TransactionScope GetReadUncommittedScope()
        {
            return new TransactionScope(
        TransactionScopeOption.Required,
        new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted,
            Timeout = TransactionManager.DefaultTimeout
        },
        TransactionScopeAsyncFlowOption.Enabled
    );

        }
    }
}
