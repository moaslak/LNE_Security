using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace LNE_Security;

partial class Database
{
    public void NewAddress(ContactInfo contactInfo)
    {
        string query = @"INSERT INTO [dbo].[Address]
           ([StreetName]
           ,[HouseNumber]
           ,[City]
           ,[ZipCode]
           ,[Country])
            VALUES
           ('" + contactInfo.Address.StreetName.ToString() +
           "','" + contactInfo.Address.HouseNumber.ToString() +
           "','" + contactInfo.Address.City.ToString() +
           "','" + contactInfo.Address.Country.ToString() + "')";

        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
    }
    /// <summary>
    /// new address to database
    /// </summary>
    /// <param name="address"></param>
    /// <returns>AddressID</returns>
    public UInt16 NewAddress(Address address)
    {
        UInt16 addressID = 0;
        string query = @"INSERT INTO [dbo].[Address]
           ([StreetName]
           ,[HouseNumber]
           ,[City]
           ,[ZipCode]
           ,[Country])
            VALUES
           ('" + address.StreetName.ToString() +
           "','" + address.HouseNumber.ToString() +
           "','" + address.City.ToString() +
           "','" + address.ZipCode.ToString() +
           "','" + address.Country.ToString() + "')";

        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();
        
        query = @"SELECT * FROM [dbo].[Address]
                WHERE StreetName = '" + address.StreetName.ToString() +
                "' AND HouseNumber = '" + address.HouseNumber.ToString() +
                "' AND City = '" + address.City.ToString() +
                "' AND ZipCode = '" + address.ZipCode.ToString() +
                "' AND Country = '" + address.Country.ToString()+ "'";
        cmd = new SqlCommand(query, sqlConnection);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            addressID = Convert.ToUInt16(reader.GetValue(0));
        }
        reader.Close();
        //close connection
        sqlConnection.Close();

        return addressID;
    }

    public Address SelectAddress(ContactInfo contactInfo)
    {
        
        string query = "SELECT * FROM [dbo].[Address] WHERE AddressID = " + contactInfo.AddressId.ToString();
        sqlConnection.Open();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        Address address = new Address();
        SqlDataReader reader = cmd.ExecuteReader();//execute the SQLCommand
        
        while (reader.Read())
        {
            address.StreetName = reader.GetValue(1).ToString();
            address.HouseNumber = reader.GetValue(2).ToString();
            address.City = reader.GetValue(3).ToString();
            address.ZipCode = reader.GetValue(4).ToString();
            address.Country = reader.GetValue(5).ToString();
        }

        reader.Close();

        //close connection
        sqlConnection.Close();

        return address;
    }

    public List<Address> GetAddresses()
    {
        List<Address> addresses = new List<Address>();
        string query = "SELECT * FROM [dbo].[Address]";
        sqlConnection.Open();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        SqlDataReader reader = cmd.ExecuteReader();//execute the SQLCommand
        while (reader.Read())
        {
            Address newAddress = new Address();
            newAddress.AddressID = Convert.ToUInt16(reader.GetValue(0));
            newAddress.StreetName = reader.GetValue(1).ToString();
            newAddress.HouseNumber = reader.GetValue(2).ToString();
            newAddress.City = reader.GetValue(3).ToString();
            newAddress.ZipCode = reader.GetValue(4).ToString();
            newAddress.Country = reader.GetValue(5).ToString();
            addresses.Add(newAddress);
        }
        reader.Close();

        //close connection
        sqlConnection.Close();
        return addresses;
    }
}
