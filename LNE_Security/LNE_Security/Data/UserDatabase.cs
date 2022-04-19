using LNE_Security.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LNE_Security.Data;



namespace LNE_Security.Data;

partial class Database
{
    public List<LNE_Security.Person.UserLogins> GetUser()
    {
        List<LNE_Security.Person.UserLogins> getUserList = new List<LNE_Security.Person.UserLogins>();
        SqlConnection sqlConnection = new SqlConnection();

        sqlConnection.Open();
        string query = "SELECT * FROM [dbo].{UserLogin]";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Person user = new LNE_Security.Person.UserLogins();
            user.Id =(ushort)Convert.ToInt32(reader.GetValue(0));
            user.UserName = reader.GetValue(1).ToString();
            user.Password = reader.GetValue(2).ToString();
            getUserList.Add(user);
        }
        reader.Close();
        sqlConnection.Close();
        return getUserList;
    }

}
