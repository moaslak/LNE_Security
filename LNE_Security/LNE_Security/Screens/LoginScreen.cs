using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

public class LoginScreen : ScreenHandler
{
    Company Company = new Company();
    public LoginScreen()
    {

    }

    protected override void Draw()
    {
        List<Company> companies = Database.Instance.GetCompanies();
        bool adminOK = false;
        foreach (Company company in companies)
        {
            if(company.Role == 0)
                adminOK = true;
        }
        if (!adminOK)
        {
            Company admin = new Company();
            admin.Role = 0;
            admin.CompanyName = "admin";
            admin.StreetName = "admin";
            admin.HouseNumber = "admin";
            admin.PhoneNumber = "admin";
            admin.City = "admin";
            admin.ZipCode = "admin";
            admin.Currency = Company.Currencies.DKK;
            admin.Country = "admin";
            admin.Email = "admin";
            admin.CVR = "admin";
            admin.FirstName = "admin";
            admin.LastName = "admin";
            admin.Password = "admin";
            admin.DatabaseName = "LNE_Security";
            Database.Instance.newCompany(admin);
        }
        do
        {
            Console.Clear();
            Console.Write("Enter username/company name: ");
            this.Company.CompanyName = Console.ReadLine();

            List<Company> list = Database.Instance.GetCompanies();

            foreach (Company company in list)
            {
                if (company.CompanyName == this.Company.CompanyName)
                {
                    Console.Write("Enter password: ");
                    this.Company.Password = GetPassword();

                    if (company.Password == this.Company.Password)
                    {
                        ScreenHandler.Display(new MainMenuScreen(company));
                    }
                }
            }
            Console.WriteLine("Invalid credentials");
            Console.WriteLine("Press ESC to close the app. Press another key to retry login");
        } while (!(Console.ReadKey().Key == ConsoleKey.Escape));
        Console.WriteLine("App closing...");
        Thread.Sleep(1000);
        Environment.Exit(0);
    }
}

