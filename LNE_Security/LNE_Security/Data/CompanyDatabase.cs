using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LNE_Security.Data;

namespace LNE_Security;

partial class Database
{
    public Company SelectCompany(UInt16 Id)
    {
        List<Company> CompanyList = GetCompanies();
        Company company = new Company();

        foreach(Company companyItem in CompanyList)
        {
            if(company.Id == Id)
                return company;
        }
        return null;
    }
    public List<Company> GetCompanies()
    {
        List<Company> companies = new List<Company>();
        
        sqlConnection.Open();
        string query = "SELECT * FROM [dbo].[Company]";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);

        SqlDataReader reader = cmd.ExecuteReader();
        //execute the SQLCommand

        while (reader.Read()) // each loop reads a row from the query.
        {
            Company company = new Company();
            company.Id = (ushort)(Convert.ToUInt16(reader.GetValue(0)));
            company.CompanyName = reader.GetValue(1).ToString();
            company.Country = reader.GetValue(2).ToString();
            company.StreetName = reader.GetValue(3).ToString();
            company.HouseNumber = reader.GetValue(4).ToString();
            company.City = reader.GetValue(5).ToString();
            company.ZipCode = reader.GetValue(6).ToString();

            string cur = reader.GetValue(7).ToString();
            switch (cur)
            {
                case "DKK":
                    company.Currency = Company.Currencies.DKK;
                    break;
                case "USD":
                    company.Currency = Company.Currencies.USD;
                    break;
                case "EUR":
                    company.Currency = Company.Currencies.EUR;
                    break;
                case "YEN":
                    company.Currency = Company.Currencies.YEN;
                    break;
                default:
                    company.Currency = Company.Currencies.EUR;
                    break;
            }
            company.CVR = reader.GetValue(8).ToString();
            companies.Add(company);
        }

        reader.Close();

        //close connection
        sqlConnection.Close();

        return companies;
    }

    public void EditCompany(UInt16 ID, Company editedCompany)
    {
        SqlConnection sqlConnection = databaseConnection.SetSqlConnection();
        string query = @"UPDATE [dbo].[Company]
            SET[CompanyName] = '" + editedCompany.CompanyName + "'" +
            ",[Country] = '" + editedCompany.Country + "'" +
            ",[StreetName] = '" + editedCompany.StreetName + "'" +
            ",[HouseNumber] = '" + editedCompany.HouseNumber + "'" +
            ",[City] = '" + editedCompany.City + "'" +
            ",[ZipCode] = '" + editedCompany.ZipCode + "'" +
            ",[Currency] = '" + editedCompany.Currency + "'" +
            ",[CVR] = '" + editedCompany.CVR + "' WHERE Id = " + editedCompany.Id;
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
    }

    public void DeleteCompany(UInt16 ID)
    {
        string query = "DELETE FROM [dbo].[Company] WHERE Id = " + ID.ToString();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();

        if (SelectCustomer(ID) == null)
            Console.WriteLine("Company with ID = " + ID + " was succesfully deleted");
        else
            Console.WriteLine("Could not find company to delete");
    }

    public void newCompany()
    {
        Console.WriteLine();
        Console.WriteLine("New company");
        Company newCompany = new Company();

        SqlConnection sqlConnection = new DatabaseConnection().SetSqlConnection();

        string cur = "";
        bool curOK = false;

        Console.Write("Enter company name: ");
        newCompany.CompanyName = Console.ReadLine();
        Console.Write("Enter street name: ");
        newCompany.StreetName = Console.ReadLine();
        Console.Write("Enter number: ");
        newCompany.HouseNumber = Console.ReadLine();
        Console.Write("Enter city: ");
        newCompany.City = Console.ReadLine();
        Console.Write("Enter zipcode: ");
        newCompany.ZipCode = Console.ReadLine();
        Console.Write("Enter country: ");
        newCompany.Country = Console.ReadLine();
        Console.Write("Enter CVR: ");
        newCompany.CVR = Console.ReadLine();
        do
        {
            Console.Write("Choose currency DKK, USD, EUR or YEN: ");
            cur = Console.ReadLine();
            switch (cur)
            {
                case "DKK":
                    newCompany.Currency = Company.Currencies.DKK;
                    curOK = true;
                    break;
                case "USD":
                    newCompany.Currency = Company.Currencies.USD;
                    curOK = true;
                    break;
                case "EUR":
                    newCompany.Currency = Company.Currencies.EUR;
                    curOK = true;
                    break;
                case "YEN":
                    newCompany.Currency = Company.Currencies.YEN;
                    curOK = true;
                    break;
                default:
                    break;
            }
        } while (!curOK);

        string query = @"INSERT INTO [dbo].[Company]
        ([CompanyName]
        ,[Country]
        ,[StreetName]
        ,[HouseNumber]
        ,[City]
        ,[ZipCode]
        ,[Currency]
        ,[CVR])";
        query = query + " VALUES(";
        query = query + "'" + newCompany.CompanyName + "'" + ",";
        query = query + "'" + newCompany.Country + "'" + ",";
        query = query + "'" + newCompany.StreetName + "'" + ",";
        query = query + "'" + newCompany.HouseNumber + "'" + ",";
        query = query + "'" + newCompany.City + "'" + ",";
        query = query + "'" + newCompany.ZipCode + "'" + ",";
        query = query + "'" + cur + "'" + ",";
        query = query + "'" + newCompany.CVR + "'";
        query = query + ")";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
    }
}
