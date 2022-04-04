using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace LNE_Security;

public class Customer : Person
{
    Person _person;

    public UInt16 Id { get; set; }
    public void NewCustomer(ContactInfo contactInfo,
        Database database, Address address)
    {
        SqlConnection sqlconnection =  database.SetSqlConnection();

        string query = @"INSERT INTO [dbo].[Customer]
        ([FirstName]
        ,[LastName]
        ,[Address])";

        query = query + " VALUES(";
        query = query + "'" + contactInfo.FirstName + "'" + ",";
        query = query + "'" + contactInfo.LastName + "'" + ",";
        query = query + "'" + address.StreetName + "," + address.HouseNumber + "," + address.ZipCode + "," + address.City + "," + address.Country + "')";
        SqlCommand cmd = new SqlCommand(query, sqlconnection);
        sqlconnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlconnection.Close();
    }

    public override Person DeletePerson(ContactInfo contactInfo, Database database, Address address)
    {
        database = new Database();
        contactInfo = new ContactInfo();
        address = new Address();
        _person.DeletePerson(contactInfo, database, address);
        return null;
    }

    public override Person GetPerson(ContactInfo contactInfo)
    {
        contactInfo = new ContactInfo();
        return _person;
    }

    public override Person UpdatePerson(ContactInfo contactInfo, Database database, Address address)
    {
        contactInfo = new ContactInfo();
        database = new Database();
        address = new Address();
        return _person;
    }

    public string CreateFullName(string FirstName, string LastName)
    {
        return FirstName + " " + LastName;
    }
}
