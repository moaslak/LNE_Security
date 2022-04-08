using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace LNE_Security.Data
{
    public class DatabaseConnection
    {
        public SqlConnection SetSqlConnection(String database)
        {
            SqlConnectionStringBuilder SqlConnectionStringBuilder = new SqlConnectionStringBuilder();

            SqlConnectionStringBuilder.DataSource = ".";
            SqlConnectionStringBuilder.ConnectTimeout = 5;

            SqlConnectionStringBuilder["Trusted_Connection"] = true;

            SqlConnectionStringBuilder.InitialCatalog = database;
            SqlConnection SqlConnection = new SqlConnection(SqlConnectionStringBuilder.ConnectionString);
            return SqlConnection;
        }
    }
}
