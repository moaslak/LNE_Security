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


  






    // TODO: TITLES!!!

    static List<Company> companyList = new List<Company>();

    public static void Main(string[] args)
    {
        companyList = Database.Instance.GetCompanies();
        //InvoiceMockTest(salesOrder);   

        //MainMenuScreen mainMenu = new MainMenuScreen(companyList[0]);
        MainMenuScreen mainMenu = new MainMenuScreen();
        ScreenHandler.Display(mainMenu);

    }
}
