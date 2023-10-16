using Oracle.ManagedDataAccess.Client;

namespace ASCHM.Context
{
    public interface IHRMDBContext
    {
        OracleCommand GetCommand();
        OracleConnection GetConn();
    }
}
