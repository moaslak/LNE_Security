using System;
using TECHCOOL;

namespace LNE_Security
{
    class Program
    {
        public Menu Menu
        {
            get => default;
            set
            {
            }
        }

        public static void Main(string[] args)
        {
            ScreenHandler screenHandler = new ScreenHandler();
            Company company = new Company();
            company.CompanyName = "test";
            screenHandler.Company = company;
            ScreenHandler.Display(screenHandler);
        }
    }
}