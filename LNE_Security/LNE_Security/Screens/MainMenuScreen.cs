using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

public class MainMenuScreen : ScreenHandler
{
    class Options
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

        
    public MainMenuScreen(Company Company) : base(Company)
    {
        this.company = Company;
    }
    public MainMenuScreen(Product Product) : base(Product)
    {
        this.product = Product;
    }

    public MainMenuScreen()
    {

    }
    protected override void Draw()
    {
        if (company == null)
            Title = "ERP System";
        else
            Title = company.CompanyName + " ERP System";
        Clear(this);
        ListPage<Options> MenuOptions = new ListPage<Options>();
        MenuOptions.AddColumn("Option", "Option", "Sales order screen".Length);
        MenuOptions.Add(new Options("Company screen", "F1"));
        MenuOptions.Add(new Options("Customer screen", "F2"));
        MenuOptions.Add(new Options("Employee screen", "F3"));
        MenuOptions.Add(new Options("Product screen", "F4"));
        MenuOptions.Add(new Options("Sales order screen", "F5"));
        MenuOptions.Add(new Options("Storage screen", "F6"));
        MenuOptions.Add(new Options("Close App", "ESC"));
        Console.WriteLine();

        Options selected = MenuOptions.Select();
        
        if (selected != null)
        {
            switch (selected.KeyPress)
            {
                case "F1":
                    ScreenHandler.Display(new CompanyScreen(company));
                    break;
                case "F2":
                    ScreenHandler.Display(new CustomerScreen(company));
                    break;
                case "F3":
                    ScreenHandler.Display(new EmployeeScreen(company));
                    break;
                case "F4":
                    ScreenHandler.Display(new ProductScreen(company));
                    break;
                case "F5":
                    if (company == null)
                        Console.WriteLine("Select company first");
                    else
                        ScreenHandler.Display(new SalesOrderScreen(company, customer));
                    break;
                case "F6":
                    if (company == null)
                        Console.WriteLine("Select company first");
                    else
                        ScreenHandler.Display(new StorageScreen(company));
                    break;
                case "ESC":
                    Environment.Exit(0);
                    break;
            }
        }
    }
}

