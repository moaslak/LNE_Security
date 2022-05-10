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

        foreach(Company companyItem in CompanyList)
        {
            if(companyItem.CompanyID == Id)
                return companyItem;
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
            company.CompanyID = (ushort)(Convert.ToUInt16(reader.GetValue(0)));
            company.CompanyName = reader.GetValue(1).ToString();
            string cur = reader.GetValue(2).ToString();
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
            company.CVR = reader.GetValue(3).ToString();
            company.ContactInfoID = Convert.ToUInt16(reader.GetValue(4));
            company.Role = Convert.ToInt32(reader.GetValue(5));
            company.Password = reader.GetValue(6).ToString();

            companies.Add(company);
        }
        reader.Close();

        //close connection
        sqlConnection.Close();
        
        for(int i = 0; i< companies.Count; i++)
        {
            ContactInfo contactInfo = Database.Instance.SelectContactInfo(companies[i]);
            companies[i].contactInfo = contactInfo;
            companies[i].Country = contactInfo.Address.Country;
            companies[i].StreetName = contactInfo.Address.StreetName;
            companies[i].City = contactInfo.Address.City;
            companies[i].HouseNumber = contactInfo.Address.HouseNumber;
            companies[i].ZipCode = contactInfo.Address.ZipCode;
            companies[i].FirstName = contactInfo.FirstName;
            companies[i].LastName = contactInfo.LastName;
            companies[i].Email = contactInfo.Email;
            companies[i].PhoneNumber = contactInfo.PhoneNumber;
        }
        
        return companies;
    }

    public void EditCompany(UInt16 ID, Company editedCompany)
    {
        SqlConnection sqlConnection = databaseConnection.SetSqlConnection("LNE_Security");
        string query = @"UPDATE [dbo].[Company]
            SET[CompanyName] = '" + editedCompany.CompanyName +
            "',[Currency] = '" + editedCompany.Currency +
            "',[CVR] = '" + editedCompany.CVR + 
            "',[Password] = '" + editedCompany.Password + "' WHERE CompanyID = " + editedCompany.CompanyID;
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();
        
        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();
        query = @"UPDATE [dbo].[ContactInfo] SET[FirstName] = '" + editedCompany.FirstName +
            "', [LastName] = '" + editedCompany.LastName +
            "', [Email] = '" + editedCompany.Email +
            "', [PhoneNumber] = '" + editedCompany.PhoneNumber +
            "' WHERE ContactInfoID = " + editedCompany.ContactInfoID;
        cmd = new SqlCommand(query, sqlConnection);

        //execute the SQLCommand
        reader = cmd.ExecuteReader();
        reader.Close();

        query = @"UPDATE [dbo].[Address] SET[StreetName] = '" + editedCompany.StreetName +
            "', [HouseNumber] = '" + editedCompany.HouseNumber +
            "', [ZipCode] = '" + editedCompany.ZipCode +
            "', [City] = '" + editedCompany.City +
            "', [Country] = '" + editedCompany.Country + "' WHERE AddressID = " + editedCompany.contactInfo.AddressId;
        cmd = new SqlCommand(query, sqlConnection);

        //execute the SQLCommand
        reader = cmd.ExecuteReader();
        reader.Close();
        //close connection

        sqlConnection.Close();
    }

    public void DeleteCompany(UInt16 ID)
    {
        string query = "DELETE FROM [dbo].[Company] WHERE CompanyID = " + ID.ToString();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();

        if (SelectCompany(ID) == null)
            Console.WriteLine("Company with ID = " + ID + " was succesfully deleted");
        else
            Console.WriteLine("Could not find company to delete");
    }
    public void newCompany(Company admin)
    {
        Address address = new Address();
        address.StreetName = admin.StreetName;
        address.HouseNumber = admin.HouseNumber;
        address.ZipCode = admin.ZipCode;
        address.City = admin.City;
        address.Country = admin.Country;
        address.AddressID = Database.Instance.NewAddress(address);
        admin.contactInfo.FirstName = admin.FirstName;
        admin.contactInfo.LastName = admin.LastName;
        admin.contactInfo.Email = admin.Email;
        admin.contactInfo.PhoneNumber = admin.PhoneNumber;
        admin.ContactInfoID = Database.Instance.NewContactInfo(address, admin);

        string query = @"INSERT INTO [dbo].[Company]
        ([CompanyName]
        ,[Currency]
        ,[CVR]
        ,[ContactInfoID]
        ,[Role]
        ,[Password]) VALUES('" + admin.CompanyName +
        "','" + admin.Currency +
        "','" + admin.CVR +
        "','" + admin.ContactInfoID +
        "','" + admin.Role.ToString() +
        "','" + admin.Password + "')";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        query = "SELECT CompanyID FROM [dbo].[Company] WHERE CompanyName = '" + admin.CompanyName + "'";
        cmd = new SqlCommand(query, sqlConnection);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            admin.CompanyID = Convert.ToUInt16(reader.GetValue(0));
        }
        //close connection
        sqlConnection.Close();
    }
    public void newCompany()
    {
        Console.WriteLine();
        Console.WriteLine("New company");
        Company newCompany = new Company();

        SqlConnection sqlConnection = new DatabaseConnection().SetSqlConnection("LNE_Security");

        string cur = "";
        bool curOK = false;

        Console.Write("Enter company name: ");
        newCompany.CompanyName = Console.ReadLine();
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
        Console.Write("Enter CVR: ");
        newCompany.CVR = Console.ReadLine();
        Console.WriteLine("Address");
        Console.Write("Enter street name: ");
        newCompany.contactInfo.Address.StreetName = Console.ReadLine();
        Console.Write("Enter number: ");
        newCompany.contactInfo.Address.HouseNumber = Console.ReadLine();
        Console.Write("Enter city: ");
        newCompany.contactInfo.Address.City = Console.ReadLine();
        Console.Write("Enter zipcode: ");
        newCompany.contactInfo.Address.ZipCode = Console.ReadLine();
        Console.Write("Enter country: ");
        newCompany.contactInfo.Address.Country = Console.ReadLine();
        Console.WriteLine("Contact Info");
        Console.Write("Contact First name: ");
        newCompany.contactInfo.FirstName = Console.ReadLine();
        Console.Write("Contact Last name: ");
        newCompany.contactInfo.LastName = Console.ReadLine();
        Console.Write("Contact email: ");
        newCompany.contactInfo.Email = Console.ReadLine();
        Console.Write("Contact phonenumber: ");
        newCompany.Role = 1;
        newCompany.contactInfo.PhoneNumber = Console.ReadLine();
        Console.Write("Enter password: ");
        newCompany.Password = newCompany.GetPassword();
        Address address = new Address();
        address.StreetName = newCompany.contactInfo.Address.StreetName;
        address.HouseNumber = newCompany.contactInfo.Address.HouseNumber;
        address.City = newCompany.contactInfo.Address.City;
        address.Country = newCompany.contactInfo.Address.Country;
        address.ZipCode = newCompany.contactInfo.Address.ZipCode;

        ContactInfo contactInfo = new ContactInfo();
        contactInfo.FirstName = newCompany.contactInfo.FirstName;
        contactInfo.LastName= newCompany.contactInfo.LastName;
        contactInfo.Email= newCompany.contactInfo.Email;
        contactInfo.PhoneNumber= newCompany.contactInfo.PhoneNumber;

        address.AddressID = Database.Instance.NewAddress(address);
        newCompany.ContactInfoID = Database.Instance.NewContactInfo(address, newCompany);
        
        string query = @"INSERT INTO [dbo].[Company]
        ([CompanyName]
        ,[Currency]
        ,[CVR]
        ,[ContactInfoID]
        ,[Role]
        ,[Password]) VALUES('" + newCompany.CompanyName + 
        "','" + newCompany.Currency + 
        "','" + newCompany.CVR + 
        "','" + newCompany.ContactInfoID + 
        "','" + newCompany.Role.ToString() +
        "','" + newCompany.Password + "')";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        query = "SELECT CompanyID FROM [dbo].[Company] WHERE CompanyName = '" + newCompany.CompanyName + "'";
        cmd = new SqlCommand(query, sqlConnection);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            newCompany.CompanyID = Convert.ToUInt16(reader.GetValue(0));
        }
        //close connection
        sqlConnection.Close();
        Console.WriteLine();
        Console.WriteLine("Company: " + newCompany.CompanyName + " with CompanyID: " + newCompany.CompanyID + " created with success");
        Console.WriteLine("Press enter to continue");
    }
}
