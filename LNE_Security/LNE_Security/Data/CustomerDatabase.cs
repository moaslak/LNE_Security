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
        SqlConnection sqlConnection = databaseConnection.SetSqlConnection();
        List<Customer> customers = new List<Customer>();

        string query = @"SELECT * FROM [dbo].[Customer]";
        sqlConnection.Open();

        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Customer customer = new Customer();
            customer.Address = new Address();
            customer.ID = Convert.ToUInt16(reader.GetValue(0));
            customer.ContactInfo.FirstName = reader.GetValue(1).ToString();
            customer.ContactInfo.LastName = reader.GetValue(2).ToString();
            string[] addressString = reader.GetValue(3).ToString().Split(",");

            customer.ContactInfo.Address.StreetName = addressString[0];
            customer.ContactInfo.Address.HouseNumber = addressString[1];
            customer.ContactInfo.Address.ZipCode = addressString[2];
            customer.ContactInfo.Address.City = addressString[3];
            customer.ContactInfo.Address.Country = addressString[4];
            customers.Add(customer);
        }
        reader.Close();
        sqlConnection.Close();
        return customers;
    }

    public Customer SelectCustomer(UInt16 ID) 
    {
        List<Customer> customers = GetCustomers();

        foreach(Customer customer in customers)
        {
            if(customer.ID == ID) return customer;

        }
        return null;
    }

    public void EditCustomer(UInt16 ID, Customer editedCustomer)
    {
        SqlConnection sqlConnection = databaseConnection.SetSqlConnection();
        string query = "UPDATE [dbo].[Customer] SET[FirstName] = '" + editedCustomer.ContactInfo.FirstName + "' " +
            ",[LastName] = '"+ editedCustomer.ContactInfo.LastName +"' ,[Address] = '" + editedCustomer.ContactInfo.Address.commaSeperatedAddress(editedCustomer)
            + "' WHERE Id = " + ID.ToString();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
    }

    public void DeleteCustomer(UInt16 ID)
    {
        SqlConnection sqlConnection = databaseConnection.SetSqlConnection();
        string query = "DELETE FROM [dbo].[Customer] WHERE Id = " + ID.ToString();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();

        if (SelectCustomer(ID) == null)
            Console.WriteLine("Customer with ID = " + ID + " was succesfully deleted");
        else
            Console.WriteLine("Could not find customer to delete");
    }

    public void NewCustomer(ContactInfo contactInfo, Address address)
    {
        SqlConnection sqlConnection = databaseConnection.SetSqlConnection();

        string query = @"INSERT INTO [dbo].[Customer]
            ([FirstName]
            ,[LastName]
            ,[Address])";

        query = query + " VALUES(";
        query = query + "'" + contactInfo.FirstName + "'" + ",";
        query = query + "'" + contactInfo.LastName + "'" + ",";
        query = query + "'" + address.StreetName + "," + address.HouseNumber + "," + address.ZipCode + "," + address.City + "," + address.Country + "')";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
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
