using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LNE_Security;
using LNE_Security.Data;

namespace LNE_Security;

partial class Database
{
    DatabaseConnection databaseConnection = new DatabaseConnection();
    
    private Customer customer { get; set; }
    public List<Customer> GetCustomers()
    {
        SqlConnection sqlConnection = databaseConnection.SetSqlConnection("LNE_Security");
        List<Customer> customers = new List<Customer>();

        string query = @"SELECT * FROM [dbo].[Customer]";
        sqlConnection.Open();

        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Customer customer = new Customer();
            customer.CID = Convert.ToUInt16(reader.GetValue(0));
            customer.ContactInfoID = Convert.ToUInt16(reader.GetValue(1));
            customers.Add(customer);
        }
        reader.Close();
        sqlConnection.Close();
        foreach(Customer customer in customers)
        {
            query = "SELECT * FROM [dbo].[ContactInfo] WHERE ContactInfoID = '" + customer.ContactInfoID + "'";
            sqlConnection.Open();
            cmd = new SqlCommand(query, sqlConnection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customer.ContactInfo.FirstName = reader.GetValue(1).ToString();
                customer.ContactInfo.LastName = reader.GetValue(2).ToString();
                customer.ContactInfo.Email = reader.GetValue(3).ToString();
                customer.ContactInfo.PhoneNumber = reader.GetValue(4).ToString();
                customer.ContactInfo.AddressId = Convert.ToUInt16(reader.GetValue(5));
            }
            reader.Close();
            sqlConnection.Close();

            query = "SELECT * FROM [dbo].[Address] WHERE AddressID = '" + customer.ContactInfo.AddressId.ToString() + "'";
            sqlConnection.Open();
            cmd = new SqlCommand(query, sqlConnection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customer.ContactInfo.Address.StreetName = reader.GetValue(1).ToString();
                customer.ContactInfo.Address.HouseNumber = reader.GetValue(2).ToString();
                customer.ContactInfo.Address.ZipCode = reader.GetValue(3).ToString();
                customer.ContactInfo.Address.City = reader.GetValue(4).ToString();
                customer.ContactInfo.Address.Country = reader.GetValue(5).ToString();
            }
            reader.Close();
            sqlConnection.Close();
        }

        return customers;
    }

    public Customer SelectCustomer(UInt16 CID) 
    {
        List<Customer> customers = GetCustomers();

        foreach(Customer customer in customers)
        {
            if(customer.CID == CID) return customer;

        }
        return null;
    }

    public void EditCustomer(Customer editedCustomer, string option)
    {
        SqlConnection sqlConnection = databaseConnection.SetSqlConnection("LNE_Security");
        string query = "UPDATE [dbo].[ContactInfo] SET[FirstName] = '" + editedCustomer.ContactInfo.FirstName + "' " +
            ",[LastName] = '" + editedCustomer.ContactInfo.LastName + "', [Email] = '" + editedCustomer.ContactInfo.Email +
            "', [PhoneNumber] = '" + editedCustomer.ContactInfo.PhoneNumber + "' WHERE ContactInfoID = '" + editedCustomer.ContactInfoID +"'";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        query = "UPDATE [dbo].[Address] SET[StreetName] = '" + editedCustomer.ContactInfo.Address.StreetName +
            "', [HouseNumber] = '" + editedCustomer.ContactInfo.Address.HouseNumber +
            "', [City] = '" + editedCustomer.ContactInfo.Address.City +
            "', [ZipCode] = '" + editedCustomer.ContactInfo.Address.ZipCode +
            "', [Country] = '" + editedCustomer.ContactInfo.Address.Country + "' WHERE AddressID = '" + editedCustomer.ContactInfo.AddressId + "'";
        
        cmd = new SqlCommand(query, sqlConnection);

        //execute the SQLCommand
        reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
    }

    public void DeleteCustomer(UInt16 CID)
    {
        SqlConnection sqlConnection = databaseConnection.SetSqlConnection("LNE_Security");
        string query = "DELETE FROM [dbo].[Customer] WHERE CID = " + CID.ToString();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();

        if (SelectCustomer(CID) == null)
            Console.WriteLine("Customer with CID = " + CID + " was succesfully deleted");
        else
            Console.WriteLine("Could not find customer to delete");
    }

    public void NewCustomer(Customer customer)
    {
        SqlConnection sqlConnection = databaseConnection.SetSqlConnection("LNE_Security");
        // TODO: fix query
        string query = @"INSERT INTO [dbo].[Customer] 
            (
            [ContactInfoID])
            VALUES('" + customer.ContactInfoID.ToString() + "')";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
        /*string CID = "";
        query = "SELECT CID FROM Customer WHERE FirstName = '" + contactInfo.FirstName + "' AND LastName = '" + contactInfo.LastName
            +"'";
        sqlConnection.Open();
        cmd = new SqlCommand(query, sqlConnection);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            CID = reader.GetValue(0).ToString();
        }
        reader.Close();
        query = @"INSERT INTO [dbo].[Person]
           ([Type]
           ,[ContactInfoID])
            VALUES
           ('Customer'" +
           ",'" + CID + "')";
        cmd = new SqlCommand(query, sqlConnection);
        reader = cmd.ExecuteReader();
        reader.Close();
        sqlConnection.Close();*/
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

    /* // TODO: bruges denne?
    public SqlConnection DeleteCustomer(ushort id, Person person)
    {
        person.ID = id;
        SqlConnection sqlConnection = new SqlConnection();
        sqlConnection.Database.Remove(id);
        return sqlConnection;
    }
    */

    /*public List<Customer> InsertCustomer(SqlConnection sqlConnection)
    {
        List<Customer> customer = new List<Customer>();
        string query = "INSERT INTO dbo.Customer(id, firstname, lastname," +
            " phonenumber, email, streetname, housenumber, city, zipcode, country) " +
            "VALUES(@id, @firstname, @lastname, @phonenumber, @email, @streetname," +
            " @housenumber, @city, @zipcode, @country";
        using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
        {
            cmd.Parameters.AddWithValue("@id", customer.ID);
            cmd.Parameters.AddWithValue("@firstname", customer.ContactInfo.FirstName);
            cmd.Parameters.AddWithValue("@lastname", customer.ContactInfo.LastName);
            cmd.Parameters.AddWithValue("@phonenumber", customer.ContactInfo.PhoneNumber);
            cmd.Parameters.AddWithValue("@email", customer.ContactInfo.Email);
            cmd.Parameters.AddWithValue("@streetname", customer.Address.StreetName);
            cmd.Parameters.AddWithValue("@housenumber", customer.Address.HouseNumber);
            cmd.Parameters.AddWithValue("@city", customer.Address.City);
            cmd.Parameters.AddWithValue("@zipcode", customer.Address.ZipCode);
            cmd.Parameters.AddWithValue("@country", customer.Address.Country);
            sqlConnection.Open();
            int result = cmd.ExecuteNonQuery();
        }
        return customer;
    }*/

    /*
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

        person.ContactInfo = GetContactInfo(person, sqlConnection);
        person.ContactInfo.FirstName = stringBuilder[1].ToString();
        person.ContactInfo.LastName = stringBuilder[2].ToString();
        person.ContactInfo.PhoneNumber.Add(stringBuilder[3].ToString()); // TODO: implementér loop
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
*/

}
