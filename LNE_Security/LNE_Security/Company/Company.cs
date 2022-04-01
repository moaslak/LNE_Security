using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LNE_Security;

public class Company
{
    private SqlConnection sqlConnection;

    public Storage Storage
    {
        get => default;
        set
        {
        }
    }

    public Sales Sales
    {
        get => default;
        set
        {
        }
    }

    public Database Database
    {
        get => default;
        set
        {
        }
    }


    public string? CompanyName { get; set; }
    public string? Country { get; set; }
    public string? StreetName { get; set; }
    public enum Currencies { DKK, USD, EUR, YEN }
    public Currencies? Currency { get; set; }
    public string? CVR { get; set; }        
    public string? HouseNumber { get; set; }

    public string? ZipCode { get; set; }


    public string? City { get; set; }

    public UInt16 Id { get; set; }

    public Company()
    {
        return;
    }


    public Company(string companyName, string streetName, string country)
    {
        CompanyName = companyName;
        StreetName = streetName;
        Country = country;

    }
    public Company(string companyName)
    {
        CompanyName = companyName;

    }

    public Company(string companyName, string streetName, string houseNumber, string zipCode,
        string city, string country)
    {
        CompanyName = companyName;
        StreetName= streetName;
        HouseNumber = houseNumber;
        ZipCode = zipCode;
        City = city;
        Country = country;
    }

    List<Company>? CompanyList { get; set; }
    public Company RemoveCompany(Company company)
    {
        SqlConnection sqlConnection = SetSqlConnection(Id);
        CompanyName = null;
        StreetName = null;
        HouseNumber = null;
        ZipCode = null;
        City = null;
        Country = null;
        CVR = null;
        return this;
    }

    private SqlConnection SetSqlConnection(ushort id)
    {
        sqlConnection.Database.Remove(id);
        return sqlConnection;
    }
}
