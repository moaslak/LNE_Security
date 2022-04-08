using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace LNE_Security;

partial class Database
{
    public UInt16 NewContactInfo(ContactInfo contactInfo)
    {
        Address address = contactInfo.Address;
        address.AddressID = contactInfo.AddressId;
        UInt16 contactInfoId = 0;
        string query = @"INSERT INTO [dbo].[ContactInfo]
           ([FirstName]
           ,[LastName]
           ,[Email]
           ,[PhoneNumber]
           ,[AddressID])
     VALUES
           ('" + contactInfo.FirstName.ToString() +
           "','" + contactInfo.LastName.ToString() +
           "','" + contactInfo.Email.ToString() +
           "','" + contactInfo.PhoneNumber.ToString() +
           "','" + address.AddressID + "')";

        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        query = @"SELECT * FROM [dbo].[ContactInfo] 
                WHERE FirstName = '" + contactInfo.FirstName +
                "' AND LastName = '" + contactInfo.LastName +
                "' AND Email = '" + contactInfo.Email +
                "' AND PhoneNumber = '" + contactInfo.PhoneNumber + "'";
        cmd = new SqlCommand(query, sqlConnection);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            contactInfoId = Convert.ToUInt16(reader.GetValue(0));
        }
        reader.Close();

        //close connection
        sqlConnection.Close();
        return contactInfoId;
    }
    public UInt16 NewContactInfo(Address address, Company company)
    {
        UInt16 contactInfoId = 0;
        string query = @"INSERT INTO [dbo].[ContactInfo]
           ([FirstName]
           ,[LastName]
           ,[Email]
           ,[PhoneNumber]
           ,[AddressID])
     VALUES
           ('" + company.contactInfo.FirstName.ToString() +
           "','" + company.contactInfo.LastName.ToString() +
           "','" + company.contactInfo.Email.ToString() +
           "','" + company.contactInfo.PhoneNumber.ToString() +
           "','" + address.AddressID + "')";

        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        query = @"SELECT * FROM [dbo].[ContactInfo] 
                WHERE FirstName = '" + company.contactInfo.FirstName +
                "' AND LastName = '" + company.contactInfo.LastName +
                "' AND Email = '" + company.contactInfo.Email + 
                "' AND PhoneNumber = '" + company.contactInfo.PhoneNumber + "'";
        cmd = new SqlCommand(query, sqlConnection);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            contactInfoId = Convert.ToUInt16(reader.GetValue(0));
        }
        reader.Close();

        //close connection
        sqlConnection.Close();
        return contactInfoId;
    }
    public ContactInfo GetContactInfo(Person person, SqlConnection sqlConnection)
    {
        UInt16 personID = person.ID;
        ContactInfo contactInfo = new ContactInfo();
        sqlConnection.Open();

        string query = "SELECT * FROM [dbo].[ContactInfo] WHERE PersonId = " + personID.ToString();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        SqlDataReader reader = cmd.ExecuteReader();

        StringBuilder stringBuilder = new StringBuilder();
        while (reader.Read())
        {
            for (int i = 0; i <= reader.FieldCount - 1; i++)
            {
                stringBuilder.Append(reader.GetValue(i));
            }
            contactInfo.PersonId = stringBuilder[1]; // TODO: link to database
            contactInfo.FirstName = stringBuilder[2].ToString();
            contactInfo.LastName = stringBuilder[3].ToString();
            contactInfo.FullName = contactInfo.FirstName + " " + contactInfo.LastName;
            contactInfo.Address = address; // TODO: address id=?
            contactInfo.Email = stringBuilder[4].ToString();

            /*string[] Numbers = stringBuilder[5].ToString(); // TODO: to array
            foreach (string phonenumber in Numbers)
                contactInfo.PhoneNumber.Add(phonenumber);
            */

        }
        reader.Close();

        sqlConnection.Close();
        return contactInfo;
    }

    public ContactInfo SelectContactInfo(Company company)
    {
        ContactInfo contactInfo = new ContactInfo();
        sqlConnection.Open();
        string query = "SELECT * FROM [dbo].[ContactInfo] WHERE ContactInfoID = " + company.ContactInfoID;

        SqlCommand cmd = new SqlCommand(query, sqlConnection);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            contactInfo.ContactInfoID = Convert.ToUInt16(reader.GetValue(0));
            contactInfo.FirstName = reader.GetValue(1).ToString();
            contactInfo.LastName = reader.GetValue(2).ToString();
            contactInfo.Email = reader.GetValue(3).ToString();
            contactInfo.PhoneNumber = reader.GetValue(4).ToString();
            contactInfo.AddressId = Convert.ToUInt16(reader.GetValue(5).ToString());
        }
        reader.Close();

        //close connection
        sqlConnection.Close();
        contactInfo.Address = Database.Instance.SelectAddress(contactInfo);

        return contactInfo;
    }

    public ContactInfo SelectContactInfo(Customer customer)
    {
        ContactInfo contactInfo = new ContactInfo();
        sqlConnection.Open();
        string query = "SELECT * FROM [dbo].[ContactInfo] WHERE ContactInfoID = " + customer.ContactInfoID;

        SqlCommand cmd = new SqlCommand(query, sqlConnection);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            contactInfo.ContactInfoID = Convert.ToUInt16(reader.GetValue(0));
            contactInfo.FirstName = reader.GetValue(1).ToString();
            contactInfo.LastName = reader.GetValue(2).ToString();
            contactInfo.Email = reader.GetValue(3).ToString();
            contactInfo.PhoneNumber = reader.GetValue(4).ToString();
            contactInfo.AddressId = Convert.ToUInt16(reader.GetValue(5).ToString());
        }
        reader.Close();

        //close connection
        sqlConnection.Close();
        contactInfo.Address = Database.Instance.SelectAddress(contactInfo);

        return contactInfo;
    }
}
