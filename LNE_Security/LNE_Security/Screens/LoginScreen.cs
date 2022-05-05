﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

public class LoginScreen : ScreenHandler
{
    private Company Company = new Company();
    public LoginScreen()
    {

    }

    protected override void Draw()
    {
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

