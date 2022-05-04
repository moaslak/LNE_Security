using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

public class EditCompnayScreen : ScreenHandler
{
    public class Options
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

    private List<Company.Currencies> currenciesToList()
    {
        List<Company.Currencies> list = Enum.GetValues(typeof(Company.Currencies)).Cast<Company.Currencies>().ToList();
        return list;
    }

    private void EditCompany(Options selected)
    {
        string newValue = "";
        if(selected.Option != "Currency")
        {
            Console.Write("New value: ");
            newValue = Console.ReadLine();
        }
        else
        {
            List<Company.Currencies> currencies = currenciesToList();
            
            ListPage<Options> listPage = new ListPage<Options>();
            listPage.AddColumn("Currency", "Option");
            foreach(Company.Currencies cur in currencies)
            {
                listPage.Add(new Options(cur.ToString(), cur.ToString()));
            }
            selected = listPage.Select();

            switch (selected.Value)
            {
                case "DKK":
                    company.Currency = Company.Currencies.DKK;
                    break;
                case "USD":
                    company.Currency = Company.Currencies.USD;
                    break;
                case "YEN":
                    company.Currency = Company.Currencies.YEN;
                    break;
                case "EUR":
                    company.Currency = Company.Currencies.EUR;
                    break;
                default:
                    break;
            }
        }

        if (newValue == null)
            newValue = "";
        switch (selected.Option)
        {
            case "Company name":
                this.company.CompanyName = newValue;
                break;
            case "CVR":
                this.company.CVR = newValue;
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
            case "Email":
                this.company.Email = newValue;
                break;
            case "Phonenumber":
                this.company.PhoneNumber = newValue;
                break;
            case "First Name":
                this.company.FirstName = newValue;
                break;
            case "Last Name":
                this.company.LastName = newValue;
                break;
            default:
                break;
        }

        Company editedCompany = this.company;
        Database.Instance.EditCompany(editedCompany.CompanyID, this.company);
    }

    /// <summary>
    /// Modified version of EditCompany().
    /// Only used for unit test
    /// </summary>
    /// <param name="selected"></param>
    public void EditCompanyTesting(Options selected)
    {
        string newValue = selected.Value;

        if (selected.Option == "Currency")
        {
            List<Company.Currencies> currencies = currenciesToList();
            ListPage<Options> listPage = new ListPage<Options>();
            listPage.AddColumn("Currency", "Option");
            foreach (Company.Currencies cur in currencies)
            {
                listPage.Add(new Options(cur.ToString(), cur.ToString()));


            }


                switch (selected.Value)
                {
                    case "DKK":
                        company.Currency = Company.Currencies.DKK;
                        break;
                    case "USD":
                        company.Currency = Company.Currencies.USD;
                        break;
                    case "YEN":
                        company.Currency = Company.Currencies.YEN;
                        break;
                    case "EUR":
                        company.Currency = Company.Currencies.EUR;
                        break;
                    default:
                        break;
                }
        }

        switch (selected.Option)
        {
            case "Company name":
                this.company.CompanyName = newValue;
                break;
            case "CVR":
                this.company.CVR = newValue;
                break;
            case "Street name":
                this.company.contactInfo.Address.StreetName = newValue;
                break;
            case "House number":
                this.company.contactInfo.Address.HouseNumber = newValue;
                break;
            case "Zip code":
                this.company.contactInfo.Address.ZipCode = newValue;
                break;
            case "City":
                this.company.contactInfo.Address.City = newValue;
                break;
            case "Country":
                this.company.contactInfo.Address.Country = newValue;
                break;
            default:
                break;
        }
    }
    protected override void Draw()
    {
        do
        {
            Title = company.CompanyName + " Edit company screen";
            Clear(this);
            ListPage<Company> CompanyListPage = new ListPage<Company>();
            CompanyListPage.AddColumn("Company name", "CompanyName", ColumnLength("Company name", company.CompanyName));
            CompanyListPage.AddColumn("CVR", "CVR", ColumnLength("CVR", company.CVR));
            CompanyListPage.AddColumn("First name", "FirstName", ColumnLength("First name", company.FirstName));
            CompanyListPage.AddColumn("Last name", "LastName", ColumnLength("Last name", company.LastName));
            CompanyListPage.AddColumn("Email", "Email", ColumnLength("Email", company.Email));
            CompanyListPage.AddColumn("Phonenumber", "PhoneNumber", ColumnLength("Phonenumber", company.PhoneNumber));
            CompanyListPage.AddColumn("Street name", "StreetName", ColumnLength("Street name", company.StreetName));
            CompanyListPage.AddColumn("House number", "HouseNumber", ColumnLength("House number", company.HouseNumber));
            CompanyListPage.AddColumn("Zip code", "ZipCode", ColumnLength("Zip code", company.ZipCode));
            CompanyListPage.AddColumn("City", "City", ColumnLength("City", company.City));
            CompanyListPage.AddColumn("Country", "Country", ColumnLength("Country", company.Country));
            CompanyListPage.AddColumn("Currency", "Currency", "Currency".Length);
            CompanyListPage.Add(company);
            CompanyListPage.Draw();

            ListPage<Options> optionsListPage = new ListPage<Options>();

            optionsListPage.AddColumn("Edit", "Option");
            optionsListPage.Add(new Options("Company name", company.CompanyName));
            optionsListPage.Add(new Options("CVR", company.CVR));
            optionsListPage.Add(new Options("First Name", company.contactInfo.FirstName));
            optionsListPage.Add(new Options("Last Name", company.contactInfo.LastName));
            optionsListPage.Add(new Options("Email", company.contactInfo.Email));
            optionsListPage.Add(new Options("Phonenumber", company.contactInfo.PhoneNumber));
            optionsListPage.Add(new Options("Street name", company.StreetName));
            optionsListPage.Add(new Options("House number", company.HouseNumber));
            optionsListPage.Add(new Options("Zip code", company.ZipCode));
            optionsListPage.Add(new Options("City", company.City));
            optionsListPage.Add(new Options("Country", company.Country));
            optionsListPage.Add(new Options("Currency", company.Currency.ToString()));
            optionsListPage.Add(new Options("Back", "NO EDIT"));
            Options selected = optionsListPage.Select();
            
           
            
            if(selected.Value != "NO EDIT")
            {
                EditCompany(selected);
                Console.WriteLine("Press a key to update another parameter");
            }
            else
            {
                break;
            }
            Console.WriteLine("Press ESC to return to Company screen");

        } while ((Console.ReadKey().Key != ConsoleKey.Escape));

        CompanyScreen companyScreen = new CompanyScreen(company);
        ScreenHandler.Display(companyScreen);
        
    }
}
