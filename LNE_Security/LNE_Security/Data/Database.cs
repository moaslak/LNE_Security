using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

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
        /*static Company()
        {
            Instance = new Company();
            
        }*/

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
    public SqlConnection RemoveSqlCompany(ushort id)
    {
        Company company = new Company();
        company.Id = id;
        SqlConnection sqlConnection = new SqlConnection();
        sqlConnection.Database.Remove(id);
        return sqlConnection;
    }
    private Person person { get; set; }
    Address address = new Address();
    ContactInfo contactInfo = new ContactInfo();
    public List<Person> GetCustomers(SqlConnection sqlConnection)
    {
        List<Person> persons = new List<Person>();
        sqlConnection.Open();
        string query = "SELECT *, FROM [dbo].[Customer]";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        SqlDataReader reader = cmd.ExecuteReader();

        StringBuilder stringBuilder = new StringBuilder();
        while (reader.Read())
        {
            for (int i = 0; i <= reader.FieldCount - 1; i++)
            {
                stringBuilder.Append(reader.GetValue(i));
            }
            stringBuilder.Append("/n");
            person.ID = (ushort)(Convert.ToUInt16(stringBuilder[0]) - 48);
            person.ContactInfo.FirstName = stringBuilder[1].ToString();
            person.ContactInfo.LastName = stringBuilder[2].ToString();
            person.ContactInfo.PhoneNumber = stringBuilder[3].ToString();
            person.ContactInfo.Email = stringBuilder[4].ToString();
            person.Address.StreetName = stringBuilder[5].ToString();
            person.Address.HouseNumber = stringBuilder[6].ToString();
            person.Address.City = stringBuilder[7].ToString();
            person.Address.ZipCode.ToString();
            person.Address.Country = stringBuilder[9].ToString();
            persons.Add(person);
        }

        reader.Close();

        sqlConnection.Close();

        return persons;
    }
    public List<Customer> GetCustomers()
    {
        SqlConnection sqlConnection = SetSqlConnection();
        List<Customer> customers = new List<Customer>();

        string query = @"SELECT * FROM [dbo].[Customer]";
        sqlConnection.Open();

        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Customer customer = new Customer();
            customer.Address = new Address();
            customer.ID = Convert.ToUInt16(reader.GetValue(0).ToString());
            customer.FirstName = reader.GetValue(1).ToString();
            customer.LastName = reader.GetValue(2).ToString();
            customer.Address.StreetName = reader.GetValue(3).ToString();
            customer.Address.HouseNumber = reader.GetValue(4).ToString();
            customer.Address.ZipCode = reader.GetValue(5).ToString();
            customer.Address.City = reader.GetValue(6).ToString();
            customer.Address.Country = reader.GetValue(7).ToString();
            customers.Add(customer);
        }
        reader.Close();
        sqlConnection.Close();
        return customers;

    }
    public List<Person> InsertCustomer(SqlConnection sqlConnection)
    {
        List<Person> persons = new List<Person>();
        string query = "INSERT INTO dbo.Customer(id, firstname, lastname," +
            " phonenumber, email, streetname, housenumber, city, zipcode, country) " +
            "VALUES(@id, @firstname, @lastname, @phonenumber, @email, @streetname," +
            " @housenumber, @city, @zipcode, @country";
        using(SqlCommand cmd = new SqlCommand(query, sqlConnection))
        {
            cmd.Parameters.AddWithValue("@id", ID);
            cmd.Parameters.AddWithValue("@firstname", person.ContactInfo.FirstName);
            cmd.Parameters.AddWithValue("@lastname", person.ContactInfo.LastName);
            cmd.Parameters.AddWithValue("@phonenumber", person.ContactInfo.PhoneNumber);
            cmd.Parameters.AddWithValue("@email", person.ContactInfo.Email);
            cmd.Parameters.AddWithValue("@streetname", person.Address.StreetName);
            cmd.Parameters.AddWithValue("@housenumber", person.Address.HouseNumber);
            cmd.Parameters.AddWithValue("@city", person.Address.City);
            cmd.Parameters.AddWithValue("@zipcode", person.Address.ZipCode);
            cmd.Parameters.AddWithValue("@country", person.Address.Country);
            sqlConnection.Open();
            int result = cmd.ExecuteNonQuery();
        }
        return persons;
    }
    
    

    public List<SalesOrder> GetSalesOrders(SqlConnection sqlConnection, Customer customer)
    {
        List<SalesOrder> salesOrders = new List<SalesOrder>();
        SalesOrder salesOrder = new SalesOrder();
        string dateTimeString = "";
        DateTime dateTime = new DateTime();
        string query = @"SELECT * FROM [dbo].[SalesOrder]";
        query = query + "WHERE CID = " + customer.ID;
        sqlConnection.Open();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            salesOrder.OrderID = Convert.ToUInt16(reader.GetValue(0).ToString());
            dateTimeString = reader.GetValue(1).ToString();
            try
            {
                DateTime.TryParse(dateTimeString, out dateTime);
                salesOrder.OrderTime = dateTime;
            }
            catch (Exception ex)
            {
                salesOrder.OrderTime = null;
            }
            
            salesOrder.CID = (ushort)(Convert.ToUInt16(reader.GetValue(2)));
            salesOrder.FullName = reader.GetValue(3).ToString();
            salesOrder.TotalPrice = (double)Convert.ToDouble(reader.GetValue(4));
            salesOrders.Add(salesOrder);
        }
        reader.Close();
        sqlConnection.Close();
        return salesOrders;
    }   

    public List<Person> UpdateCustomer(SqlConnection sqlConnection)
    {
        List<Person> people = new List<Person>();
        string query = "UPDATE Customer(id, firstname, lastname," +
            " phonenumber, email, streetname, housenumber, city, zipcode, country) " +
            "VALUES(@id, @firstname, @lastname, @phonenumber, @email, @streetname," +
            " @housenumber, @city, @zipcode, @country";
        return people;
    }

    public SqlConnection DeleteCustomer(ushort id, Person person)
    {
        person.ID = id;
        SqlConnection sqlConnection = new SqlConnection();
        sqlConnection.Database.Remove(id);
        return sqlConnection;
    }
}
