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
        
        static string BACK = "Back";
        static string NEW = "New Company";
        private Company company { get; set; }
        public CompanyScreen(Company Company) : base(Company)
        {
            this.company = Company;
        }

        

        protected override void Draw()
        {
             //Clean the screen
            
            ListPage<Company> CompanyListPage = new ListPage<Company>();
            ListPage<String> selectModeListPage = new ListPage<String>();

            CompanyListPage.Add(company);

            selectModeListPage.Add(NEW);
            selectModeListPage.Add(BACK);
            //string selectMode = selectModeListPage.Select();
            Company selected;
            do
            {
                Title = "Company Screen";
                Clear(this);
                CompanyListPage.AddColumn("Company name", "CompanyName");
                CompanyListPage.AddColumn("Country", "Country");
                CompanyListPage.AddColumn("Currency", "Currency");
                selected = CompanyListPage.Select(); // TODO: Der bruges dobbelt ENTER tryk. Ét bør være rigeligt.
            } while(!(Console.ReadKey().Key == ConsoleKey.Enter));
            
            
            
            switch (selected.CompanyName)
            {
                case "LNE Security":
                    Console.WriteLine("Selection: " + selected.CompanyName);
                    break;
                case "New Company":
                    Console.WriteLine("IMPLEMENT NEW COMPANY");
                    break;
                case "Back":
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
