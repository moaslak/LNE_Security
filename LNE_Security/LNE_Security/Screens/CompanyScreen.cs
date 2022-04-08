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
        if(CompanyList.Count > 0)
        {
            foreach (Company company in CompanyList)
                CompanyListPage.Add(company);
            CompanyListPage.AddColumn("Company name", "CompanyName");
            CompanyListPage.AddColumn("Country", "Country");
            CompanyListPage.AddColumn("Currency", "Currency");
            Console.WriteLine("Choose Company");
            selected = CompanyListPage.Select();
            Console.WriteLine("Selection: " + Database.Instance.SelectCompany(selected.CompanyID).CompanyName);
        }
        
            
        
        Console.WriteLine("F1 - New Company");
        Console.WriteLine("F2 - Edit Company");
        Console.WriteLine("F8 - Delete Company");
        Console.WriteLine("F10 - To Main menu");
        Console.WriteLine("Esc - Close App");

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
                break;
            case ConsoleKey.F10:
                MainMenuScreen menu = new MainMenuScreen(selected);
                ScreenHandler.Display(menu);
                break;
            case ConsoleKey.Escape:
                Environment.Exit(0);
                break;
        }
    }
}
