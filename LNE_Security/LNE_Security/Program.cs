using LNE_Security.Screens;
using System;
using TECHCOOL;

namespace LNE_Security;

class Program
{
    public Menu Menu
    {
        get => default;
        set
        {
        }
    }

  





    static List<Company> companyList = new List<Company>();

    public static void Main(string[] args)
    {
        companyList = Database.Instance.GetCompanies();
        //InvoiceMockTest(salesOrder);   

        foreach (Company company in companyList)
        {
            if (company.Country == "Denmark")
                company.Currency = Company.Currencies.DKK;
            else
                company.Currency = Company.Currencies.USD;
        }

        MainMenuScreen mainMenu = new MainMenuScreen(companyList[0]);
        ScreenHandler.Display(mainMenu);

    }
}
