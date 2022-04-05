using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using LNE_Security;

namespace LNE_Security;

public partial class Database : Product
{
    // TODO: implementér singleton
    // TODO: getContactInfo(personID)
    public static Database Instance { get; private set; }

    static Database()
    {
        Instance = new Database();
    }
   
    /*public Database(uint id, string productName)
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
    */    
    
    //public SqlConnection RemoveSqlCompany(ushort id, Company company)
    //{
        
    //    company.Id = id;
    //    SqlConnection sqlConnection = new SqlConnection();
    //    sqlConnection.Database.Remove(id);
    //    return sqlConnection;
    //}
    
    Address address = new Address();
    ContactInfo contactInfo = new ContactInfo();

    public ContactInfo GetContactInfo(Person person, SqlConnection sqlConnection)
    {
        UInt16 personID = person.ID;
        ContactInfo contactInfo = new ContactInfo();
        sqlConnection.Open();

        string query = "SELECT * FROM [dbo].[ContactInfo] WHERE PersonId = " + personID.ToString();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        SqlDataReader reader = cmd.ExecuteReader();

        StringBuilder stringBuilder = new StringBuilder();
        while(reader.Read())
        {
            for(int i=0; i<=reader.FieldCount-1; i++)
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
    
     

    
}
