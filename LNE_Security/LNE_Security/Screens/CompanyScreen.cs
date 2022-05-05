using LNE_Security.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using System.Data.SqlClient;
using LNE_Security;
using LNE_Security.Data;

namespace LNE_Security;

public class CompanyScreen : ScreenHandler
{
    private Company company { get; set; }
    
    public CompanyScreen(Company Company) : base(Company)
    {

        this.company = Company;
    }        
    
    protected override void Draw()
    {
        Title = "Company Screen";
        Clear(this);
        ListPage<Company> CompanyListPage = new ListPage<Company>();
        List<Company> CompanyList = Database.Instance.GetCompanies();
        Company selected = new Company();
        
        int maxCountryLength = 0;
        int maxCurrencyLength = 0;
        foreach (Company company in CompanyList)
        {
            if (company.Country.Length > maxCountryLength)
                maxCountryLength = company.Country.Length;
            if (company.Currency.ToString().Length > maxCurrencyLength)
                maxCurrencyLength = company.Currency.ToString().Length;
        }

        if (CompanyList.Count > 0)
        {
            foreach (Company company in CompanyList)
                CompanyListPage.Add(company);
            CompanyListPage.AddColumn("Company name", "CompanyName", ColumnLength("Company name", company.CompanyName));
            CompanyListPage.AddColumn("Country", "Country", maxCountryLength);
            CompanyListPage.AddColumn("Currency", "Currency", ColumnLength("Currency", maxCurrencyLength));
            Console.WriteLine("Choose Company");
            selected = CompanyListPage.Select();
            
        }
        if (selected != null)
        {
            Console.WriteLine("Selection: " + Database.Instance.SelectCompany(selected.CompanyID).CompanyName);
            Console.WriteLine("F1 - New Company");
            Console.WriteLine("F2 - Edit Company");
            Console.WriteLine("F8 - Delete Company");
            Console.WriteLine("F10 - To Main menu");


            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.F1:
                    Database.Instance.newCompany();
                    break;
                case ConsoleKey.F2:
                    ScreenHandler.Display(new EditCompnayScreen(selected));
                    break;
                case ConsoleKey.F8:
                    Database.Instance.DeleteCompany(selected.CompanyID);
                    Database.Instance.DeleteContactInfo(selected.ContactInfoID);
                    break;
                case ConsoleKey.F10:
                    
                    ScreenHandler.Display(new MainMenuScreen(this.company));
                    break;
            }
        }
           
    }
}
