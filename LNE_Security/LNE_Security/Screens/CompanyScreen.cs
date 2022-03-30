using LNE_Security.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using System.Data.SqlClient;

namespace LNE_Security;

public class CompanyScreen : ScreenHandler
{
    private Company company { get; set; }
    
    public CompanyScreen(Company Company) : base(Company)
    {

        this.company = Company;
    }        
    private void newCompany()
    {
        Database database = new Database();
        Company newCompany = new Company();
            
        SqlConnection sqlConnection = database.SetSqlConnection();

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
        query = query + "'" + newCompany.CVR +"'";
        query = query + ")";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();


        //return newCompany;
    }
    protected override void Draw()
    {            
        ListPage<Company> CompanyListPage = new ListPage<Company>();

        CompanyListPage.Add(company);
            
        Title = company.CompanyName + " Company Screen";
        Clear(this);
        CompanyListPage.AddColumn("Company name", "CompanyName");
        CompanyListPage.AddColumn("Country", "Country");
        CompanyListPage.AddColumn("Currency", "Currency");
        Console.WriteLine("Choose company");
        Company selected = CompanyListPage.Select();
            
        Console.WriteLine("Selection: " + selected.CompanyName);
        Console.WriteLine("F1 - New company");
        Console.WriteLine("F2 - Edit");
        Console.WriteLine("F10 - To Main menu");
        Console.WriteLine("Esc - Close App");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.F1:
                newCompany();
                Console.WriteLine("IMPLEMENT NEW COMPANY");
                break;
            case ConsoleKey.F2:
                EditCompnayScreen editScreen = new EditCompnayScreen(company);
                ScreenHandler.Display(editScreen);
                break;
            case ConsoleKey.F10:
                MainMenuScreen menu = new MainMenuScreen(company);
                ScreenHandler.Display(menu);
                break;
            case ConsoleKey.Escape:
                Environment.Exit(0);
                break;
        }
    }
}
