using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using LNE_Security;

namespace LNE_Security.Screens;

public class CustomerScreen : ScreenHandler
{

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
        Console.WriteLine("New Customer");
        Console.WriteLine();
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
        newCustomer.ContactInfo = contactInfo;
        newCustomer.Address = address;
        Database.Instance.NewCustomer(contactInfo, address);
    }

    private void viewCustomer(Customer customer)
    {
        Console.Write("Name: " + customer.FullName + "\n");
        Console.Write("Address: " + customer.ContactInfo.Address.AddressLine(customer));
    }

    protected override void Draw()
    {
        ListPage<Customer> CustomerListPage = new ListPage<Customer>();
        ListPage<ContactInfo> ContactListPage = new ListPage<ContactInfo>();
        List<Customer> customers = Database.Instance.GetCustomers();
        
        foreach(Customer customer in customers)
        {
            customer.FullName = customer.ContactInfo.FullName;
            CustomerListPage.Add(customer);
        }
 
        //string name = $"{Customer.ContactInfo.FirstName} {Customer.ContactInfo.LastName}";
        //Title = Customer.ContactInfo.FullName + " Customer name";
        Title = "Customer screen";
        Clear(this);

        Console.WriteLine("Choose Customer");
        CustomerListPage.AddColumn("Customer ID", "ID");
        CustomerListPage.AddColumn("Customer Name", "FullName");
        CustomerListPage.AddColumn("Phonenumber", "PhoneNumbers");
        CustomerListPage.AddColumn("Email", "Email");
        Customer selected = CustomerListPage.Select();
        

        Console.WriteLine("Selection: " + selected.ContactInfo.FullName);
        Console.WriteLine("F1 - New Customer");
        //Console.WriteLine("F2 - Edit");
        Console.WriteLine("F2 - View/Edit Customer");
        Console.WriteLine("F8 - Delete Customer");
        Console.WriteLine("F10 - To Main menu");
        Console.WriteLine("Esc - Close App");
        Console.WriteLine();
        
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.F1:
                newCustomer();
                Console.WriteLine("Press enter to continue");
                break;
            case ConsoleKey.F2:
                ScreenHandler.Display(new EditCustomerScreen(selected));
                break;
            case ConsoleKey.F10:
                ScreenHandler.Display(new MainMenuScreen(Customer));
                break;
            case ConsoleKey.F8:
                Database.Instance.DeleteCustomer(selected.ID);
                break;
            case ConsoleKey.Escape:
                Environment.Exit(0);
                break;
        }
    }
}
