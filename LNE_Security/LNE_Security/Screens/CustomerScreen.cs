using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

public class CustomerScreen : ScreenHandler
{
    Database database = new Database();
    ContactInfo contact { get; set; }
    Address address { get; set; }
    private Customer Customer = new Customer();
    public CustomerScreen(Customer customer) : base(customer)
    {
        this.Customer = customer;
    }

    public CustomerScreen()
    {
    }

    public void newCustomer()
    {
        Address address = new Address();
        ContactInfo contactInfo = new ContactInfo();
        Database database = new Database();
        Console.Write("Enter first name: ");
        contactInfo.FirstName = Console.ReadLine();
        Console.Write("Enter last name: ");
        contactInfo.LastName = Console.ReadLine();
        Console.Write("Enter street name: ");
        address.StreetName = Console.ReadLine();
        Console.Write("Enter house number: ");
        address.HouseNumber = Console.ReadLine();
        Console.Write("Enter zip code: ");
        address.ZipCode = Console.ReadLine();
        Console.Write("Enter city: ");
        address.City = Console.ReadLine();
        Console.Write("Enter country: ");
        address.Country = Console.ReadLine();
        Customer newCustomer = new Customer();
        newCustomer.NewCustomer(contactInfo, database, address);
    }

    protected override void Draw()
    {
        ListPage<Customer> CustomerListPage = new ListPage<Customer>();
        ListPage<ContactInfo> ContactListPage = new ListPage<ContactInfo>();
        List<Customer> customers = database.GetCustomers();
        
        foreach(Customer customer in customers)
        {
            customer.FullName = customer.ContactInfo.FullName;
            CustomerListPage.Add(customer);
        }
 
        //string name = $"{Customer.ContactInfo.FirstName} {Customer.ContactInfo.LastName}";
        //Title = Customer.ContactInfo.FullName + " Customer name";
        Title = "Customer screen";
        Clear(this);


        CustomerListPage.AddColumn("Customer ID", "ID");
        CustomerListPage.AddColumn("Customer Name", "FullName");
        CustomerListPage.AddColumn("Phonenumber", "PhoneNumbers");
        CustomerListPage.AddColumn("Email", "Email");
        Customer selected = CustomerListPage.Select();
        Console.WriteLine("Choose Customer");
        

        Console.WriteLine("Selection: " + selected.ContactInfo.FullName);
        Console.WriteLine("F1 - New Customer");
        Console.WriteLine("F2 - Edit");
        Console.WriteLine("F10 - To Main menu");
        Console.WriteLine("Esc - Close App");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.F1:

                newCustomer();
                break;
            case ConsoleKey.F10:
                MainMenuScreen menu = new MainMenuScreen(Customer);
                ScreenHandler.Display(menu);
                break;
            case ConsoleKey.Escape:
                Environment.Exit(0);
                break;
        }
    }
}
