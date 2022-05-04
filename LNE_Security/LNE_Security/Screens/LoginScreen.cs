using LNE_Security.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using static LNE_Security.Database;

namespace LNE_Security.Screens;

public class LoginScreen
{
    public class Options
    {
        public string Option { get; set; }
        public string KeyPress { get; set; }
        public Options(string option, string keyPress)
        {
            KeyPress = keyPress;
            Option = option;


        }
    }


    private Company company { get; set; }
    private Product product { get; set; }
    private Customer customer = new Customer();
    private Person person;
    private LNE_Security.Person.UserLogins userLogins;
    public LoginScreen(Person user) 
    {
        this.userLogins = user;
    }

    protected override void Draw()
    {
        ListPage<LNE_Security.Person.UserLogins> userListPage = new ListPage<LNE_Security.Person.UserLogins>();
        
        Console.Write("Username: ");
        userLogins.UserName = Console.ReadLine();
        Console.Write("Password: ");
        userLogins.Password = Console.ReadLine();

        if(userLogins.UserName == "Admin")
        {
            Title = "Admin";
            Clear(this);

            ListPage<Options> MenuOptions = new ListPage<Options>();
            MenuOptions.AddColumn("Option", "Option");
            MenuOptions.Add(new Options("Company screen", "F1"));
            MenuOptions.Add(new Options("Customer screen", "F2"));
            MenuOptions.Add(new Options("Employee screen", "F3"));
            MenuOptions.Add(new Options("Product screen", "F4"));
            MenuOptions.Add(new Options("Sales order screen", "F5"));
            MenuOptions.Add(new Options("Close App", "ESC"));

            Options selected = MenuOptions.Select();

            switch (selected.KeyPress)
            {
                case "F1":
                    ScreenHandler.Display(new CompanyScreen(company));
                    break;
                case "F2":
                    ScreenHandler.Display(new CustomerScreen(customer));
                    break;
                case "F3":
                    Console.WriteLine("NOT IMPLEMENTET");
                    break;
                case "F4":
                    ScreenHandler.Display(new ProductScreen(product));
                    break;
                case "F5":
                    if (company == null)
                        Console.WriteLine("Select company first");
                    else
                        ScreenHandler.Display(new SalesOrderScreen(company, customer));
                    break;
                case "ESC":
                    Environment.Exit(0);
                    break;
            }
        }
        else
        {
            Title = company.CompanyName;
            Clear(this);

            ListPage<Options> MenuOptions = new ListPage<Options>();
            MenuOptions.AddColumn("Option", "Option");
            MenuOptions.Add(new Options("Customer screen", "F2"));
            MenuOptions.Add(new Options("Employee screen", "F3"));
            MenuOptions.Add(new Options("Product screen", "F4"));
            MenuOptions.Add(new Options("Sales order screen", "F5"));
            MenuOptions.Add(new Options("Close App", "ESC"));

            Options selected = MenuOptions.Select();

            switch (selected.KeyPress)
            {
               
                case "F2":
                    ScreenHandler.Display(new CustomerScreen(customer));
                    break;
                case "F3":
                    Console.WriteLine("NOT IMPLEMENTET");
                    break;
                case "F4":
                    ScreenHandler.Display(new ProductScreen(product));
                    break;
                case "F5":
                    if (company == null)
                        Console.WriteLine("Select company first");
                    else
                        ScreenHandler.Display(new SalesOrderScreen(company, customer));
                    break;
                case "ESC":
                    Environment.Exit(0);
                    break;
            }
        }
    }
    }
}
