using LNE_Security.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using static LNE_Security.Database;

namespace LNE_Security.Screens;

public class LoginScreen : ScreenHandler
{
    private Company Company { get; set; }   
    public LoginScreen()
    {
        
    }

    protected override void Draw()
    {
        do
        {
            Console.Clear();
            Console.Write("Enter username/company name: ");
            Company.CompanyName = Console.ReadLine();

            List<Company> list = Database.Instance.GetCompanies();

            foreach (Company company in list)
            {
                if (company.CompanyName == Company.CompanyName)
                {
                    Console.Write("Enter password: ");
                    Company.Password = Console.ReadLine();

                    if (company.Password == Company.Password)
                    {
                        ScreenHandler.Display(new MainMenuScreen(Company));
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

