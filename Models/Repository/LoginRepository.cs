using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainVisitor.Repository
{
    public class LoginRepository
    {

        public static DataTable Check()
        {
            string ProviderName = "MySql.Data.MySqlClient";
            string connStr = "Server=core.icp-si.com:33006;Database=door_system;UID=haoyi8409;Password=wu011205";
            string sql = "SELECT * FROM user";
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(ProviderName);
            DbDataAdapter adapter = dbFactory.CreateDataAdapter();
            adapter.SelectCommand = dbFactory.CreateCommand();
            adapter.SelectCommand.CommandText = sql;
            adapter.SelectCommand.Connection = dbFactory.CreateConnection();
            adapter.SelectCommand.Connection.ConnectionString = connStr;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }
}
