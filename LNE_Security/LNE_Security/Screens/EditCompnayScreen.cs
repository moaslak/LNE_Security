using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens
{
    internal class EditCompnayScreen : ScreenHandler
    {
        class Options
        {
            public string Option { get; set; }
            public string Value { get; set; }
            public Options(string option, string value)
            {
                Value = value;
                Option = option;
            }
        }
        private Company company;
        public EditCompnayScreen(Company Company) : base(Company)
        {
            this.company = Company;
        }

        private void EditCompany(Options selected)
        {
            Console.Write("New value: ");
            string newValue = Console.ReadLine();

            switch (selected.Option)
            {
                case "Company name":
                    this.company.CompanyName = newValue;
                    break;
                case "Street name":
                    this.company.StreetName = newValue;
                    break;
                case "House number":
                    this.company.HouseNumber = newValue;
                    break;
                case "Zip code":
                    this.company.ZipCode = newValue;
                    break;
                case "City":
                    this.company.City = newValue;
                    break;
                case "Country": 
                    this.company.Country = newValue;
                    break;
                case "Currency":
                    break; // TODO: det er en ENUM. LAV DENNE.
                default:
                    break;
            }   
        }

        private void ChangeCurrency(Company company)
        {


        }

        protected override void Draw()
        {
            do
            {
                Title = company.CompanyName + " Edit company screen";
                Clear(this);
                ListPage<Company> CompanyListPage = new ListPage<Company>();

                CompanyListPage.AddColumn("Company name", "CompanyName");
                CompanyListPage.AddColumn("Street name", "StreetName");
                CompanyListPage.AddColumn("House number", "HouseNumber");
                CompanyListPage.AddColumn("Zip code", "ZipCode");
                CompanyListPage.AddColumn("City", "City");
                CompanyListPage.AddColumn("Country", "Country");
                CompanyListPage.AddColumn("Currency", "Currency");
                CompanyListPage.Add(company);
                CompanyListPage.Draw();

                ListPage<Options> optionsListPage = new ListPage<Options>();

                optionsListPage.AddColumn("Edit", "Option");
                optionsListPage.Add(new Options("Company name", company.CompanyName));
                optionsListPage.Add(new Options("Street name", company.StreetName));
                optionsListPage.Add(new Options("House number", company.HouseNumber));
                optionsListPage.Add(new Options("Zip code", company.ZipCode));
                optionsListPage.Add(new Options("City", company.City));
                optionsListPage.Add(new Options("Country", company.Country));
                optionsListPage.Add(new Options("Back", "NO EDIT"));
                Options selected = optionsListPage.Select();
                
                if(selected.Value != "NO EDIT")
                {
                    EditCompany(selected);
                    Console.WriteLine("Press a key to update..."); // TODO: Denne skal gerne væk
                    
                }
                Console.WriteLine("Press ESC to return to Company screen");

            } while ((Console.ReadKey().Key != ConsoleKey.Escape));

            CompanyScreen companyScreen = new CompanyScreen(company);
            ScreenHandler.Display(companyScreen);
            
        }
    }
}
