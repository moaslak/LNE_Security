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

        static Company company = new Company("LNE Security", "Navn Gade 1", "Denmark");
        

        public static void Main(string[] args)
        {
            if(company.Country == "Denmark")
                company.Currency = Company.Currencies.DKK;
            else
                company.Currency = Company.Currencies.USD;
            CompanyScreen companyScreen = new CompanyScreen(company);
            CompanyDetails details = new CompanyDetails(company);
            ScreenHandler.Display(companyScreen);
        }
    }
}