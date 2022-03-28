using LNE_Security.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security
{
    public class CompanyScreen : ScreenHandler
    {
        private Company company { get; set; }
        
        public CompanyScreen(Company Company) : base(Company)
        {
            this.company = Company;
        }
        protected override void Draw()
        {            
            ListPage<Company> CompanyListPage = new ListPage<Company>();

            CompanyListPage.Add(company);
            
            Title = company.CompanyName + " Company Screen";
            Clear(this);
            CompanyListPage.AddColumn("Company name", "CompanyName");
            CompanyListPage.AddColumn("Country", "Country");
            CompanyListPage.AddColumn("Currency", "Currency");
            Console.WriteLine("Choose company");
            Company selected = CompanyListPage.Select();
            Console.WriteLine("Selection: " + selected.CompanyName);
            Console.WriteLine("F1 - New company");
            Console.WriteLine("F2 - Edit");
            Console.WriteLine("F10 - To Main menu");
            Console.WriteLine("Esc - Close App");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.F1:
                    Console.WriteLine("IMPLEMENT NEW COMPANY");
                    break;
                case ConsoleKey.F2:
                    EditCompnayScreen editScreen = new EditCompnayScreen(company);
                    ScreenHandler.Display(editScreen);
                    break;


                case ConsoleKey.F11:
                    CompanyDetailsScreen details = new CompanyDetailsScreen(selected);
                    ScreenHandler.Display(details);
                    break;
                
                case ConsoleKey.F10:
                    MainMenuScreen menu = new MainMenuScreen(company);
                    ScreenHandler.Display(menu);
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
