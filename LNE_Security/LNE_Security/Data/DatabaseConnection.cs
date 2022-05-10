using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LNE_Security.Data
{
    public class DatabaseConnection
    {
        public SqlConnection SetSqlConnection(string database)
        {
            SqlConnectionStringBuilder SqlConnectionStringBuilder = new SqlConnectionStringBuilder();

            SqlConnectionStringBuilder.DataSource = ".";
            SqlConnectionStringBuilder.ConnectTimeout = 5;

            SqlConnectionStringBuilder["Trusted_Connection"] = true;

            SqlConnectionStringBuilder.InitialCatalog = database;
            SqlConnection SqlConnection = new SqlConnection(SqlConnectionStringBuilder.ConnectionString);
            return SqlConnection;
        }

        public SqlConnection SetSqlConnection()
        {
            SqlConnectionStringBuilder SqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            
            SqlConnectionStringBuilder.DataSource = @"sql.itcn.dk\TCAA";
            SqlConnectionStringBuilder.ConnectTimeout = 5;
            SqlConnectionStringBuilder.UserID = "mort40f4.SKOLE";
            SqlConnectionStringBuilder.Password = "fhq3CCN626";
            string database = File.ReadAllText("..\\Data\\CONFIG.txt"); //TODO: relative path
            database = database.Replace("Database=", "");
            SqlConnectionStringBuilder.InitialCatalog = database;
            SqlConnection SqlConnection = new SqlConnection(SqlConnectionStringBuilder.ConnectionString);
            return SqlConnection;
        }
    }
}
