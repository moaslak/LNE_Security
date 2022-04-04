using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LNE_Security.Data;

namespace LNE_Security;

partial class Database
{
    SqlConnection SqlConnection = new DatabaseConnection().SetSqlConnection();
    public List<Company> GetCompanies(Company company)
    {
        List<Company> companies = new List<Company>();
        SqlConnection.Open();
        string query = "SELECT * FROM [dbo].[Company]";
        SqlCommand cmd = new SqlCommand(query, SqlConnection);

        SqlDataReader reader = cmd.ExecuteReader();
        //execute the SQLCommand


        StringBuilder stringBuilder = new StringBuilder();
        while (reader.Read()) // each loop reads a row from the query.
        {
            for (int i = 0; i <= reader.FieldCount - 1; i++) // used to read through each column.
            {
                stringBuilder.Append(reader.GetValue(i)); // get the name and value for the columns.
            }
            stringBuilder.Append("\n");
            company.Id = (ushort)(Convert.ToUInt16(stringBuilder[0]) - 48);
            company.CompanyName = stringBuilder[1].ToString();
            company.Country = stringBuilder[2].ToString();
            company.StreetName = stringBuilder[3].ToString();
            company.HouseNumber = stringBuilder[4].ToString();
            company.City = stringBuilder[5].ToString();
            company.ZipCode = stringBuilder[6].ToString();
            company.Currency = Company.Currencies.DKK; // TODO: Orden denne
            company.CVR = stringBuilder[8].ToString();
            companies.Add(company);
        }

        reader.Close();

        //close connection
        SqlConnection.Close();

        return companies;
    }
}
