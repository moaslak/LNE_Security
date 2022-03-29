using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace LNE_Security;

public partial class Database : Product
{
    // TODO: implementér singleton
    public static Company Instance { get; private set; }
    Product product = new Product();
    List<Product> productsDb = new List<Product>();
    List<Database> databases = new List<Database>();
    public Database()
    {
        
    }
    public Database(uint id, string productName)
    {
        product.ID = id;
        product.ProductName = productName;
        productsDb.ForEach(p => p.ID = id);
        productsDb.ForEach(p => p.ProductName = productName);
        
    }

    public Database Insert()
    {
        productsDb.Add(product);
        return this;
    }
    public Database Update()
    {
        product = new Product();
        ID = product.ID;
        return Insert();
    }

    public Database Delete()
    {
        product.ID--;
        productsDb.Remove(product);
        return this;

    }

        public SqlConnection SetSqlConnection()
        {
            SqlConnectionStringBuilder SqlConnectionStringBuilder = new SqlConnectionStringBuilder();

            SqlConnectionStringBuilder.DataSource = ".";
            SqlConnectionStringBuilder.ConnectTimeout = 5;

            SqlConnectionStringBuilder["Trusted_Connection"] = true;

            SqlConnectionStringBuilder.InitialCatalog = "LNE_Security";
            SqlConnection SqlConnection = new SqlConnection(SqlConnectionStringBuilder.ConnectionString);
            return SqlConnection;
        }

        public List<Company> GetCompanies(SqlConnection sqlConnection)
        {
            List<Company> companies = new List<Company>();
            Company company = new Company();
            sqlConnection.Open();
            string query = "SELECT * FROM [dbo].[Company]";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);

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
            sqlConnection.Close();

            return companies;
        }
    /*static Company()
    {
        Instance = new Company();
        
    }*/

}
    

