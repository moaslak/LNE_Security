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
}
