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
             //Clean the screen
            
            ListPage<Company> CompanyListPage = new ListPage<Company>();
            CompanyListPage.Add(company);
            
            Title = "Company Screen";
            Clear(this);
            CompanyListPage.AddColumn("Company name", "CompanyName");
            CompanyListPage.AddColumn("Country", "Country");
            CompanyListPage.AddColumn("Currency", "Currency");
            Company selected = CompanyListPage.Select(); // TODO: Der bruges dobbelt ENTER tryk. Ét bør være rigeligt.
            Console.WriteLine("Selection: " + selected.CompanyName);
            Console.WriteLine("F1 - Details");
            Console.WriteLine("F2 - New company");
            Console.WriteLine("Esc - Close App");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.F1:
                    CompanyDetailsScreen details = new CompanyDetailsScreen(selected);
                    ScreenHandler.Display(details);
                    break;
                case ConsoleKey.F2:
                    Console.WriteLine("IMPLEMENT NEW COMPANY");
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
