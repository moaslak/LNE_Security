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
    private Company company { get; set; }
    public CustomerScreen(Company Company) : base(Company)
    {
        this.company = Company;
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
        Console.Write("Enter email: ");
        contactInfo.Email = Console.ReadLine();
        Console.Write("Enter phonenumber: ");
        contactInfo.PhoneNumber = Console.ReadLine();
        
        Console.Write("Enter street name: ");
        contactInfo.Address.StreetName = Console.ReadLine();
        Console.Write("Enter house number: ");
        contactInfo.Address.HouseNumber = Console.ReadLine();
        Console.Write("Enter zip code: ");
        contactInfo.Address.ZipCode = Console.ReadLine();
        Console.Write("Enter city: ");
        contactInfo.Address.City = Console.ReadLine();
        Console.Write("Enter country: ");
        contactInfo.Address.Country = Console.ReadLine();
        contactInfo.AddressId = Database.Instance.NewAddress(contactInfo.Address);
        
        Customer newCustomer = new Customer();
        newCustomer.ContactInfoID = Database.Instance.NewContactInfo(contactInfo);
        newCustomer.ContactInfo = contactInfo;
        newCustomer.Address = contactInfo.Address;
        Database.Instance.NewCustomer(newCustomer);
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
        int maxFullnameLength = 0;
        int maxEmailLength = 0;
        int maxPhoneNumberLength = 0;
        foreach (Customer customer in customers)
        {
            try
            {
                customer.FullName = customer.ContactInfo.FullName;
                customer.Email = customer.ContactInfo.Email;
                customer.PhoneNumber = customer.ContactInfo.PhoneNumber;
                CustomerListPage.Add(customer);
            }
            catch (System.NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            } 
            if(customer.FullName.Length > maxFullnameLength)
                maxFullnameLength = customer.FullName.Length;
            if(customer.Email.Length > maxEmailLength)
                maxEmailLength = customer.Email.Length;
            if(customer.PhoneNumber.Length > maxPhoneNumberLength)
                maxPhoneNumberLength = customer.PhoneNumber.Length;
        }
 
        Title = "Customer screen";
        Clear(this);
        Customer selected = new Customer();


        if(customers.Count != 0)
        {
            Console.WriteLine("Choose Customer");
            CustomerListPage.AddColumn("Customer ID", "CID", "Customer ID".Length);
            CustomerListPage.AddColumn("Customer Name", "FullName", maxFullnameLength);
            CustomerListPage.AddColumn("Phonenumber", "PhoneNumber", maxPhoneNumberLength);
            CustomerListPage.AddColumn("Email", "Email", maxEmailLength);
            selected = CustomerListPage.Select();
            
        }
        if (selected != null)
        {
            Console.WriteLine("Selection: " + selected.ContactInfo.FullName);
            Console.WriteLine("F1 - New Customer");
            Console.WriteLine("F2 - View/Edit Customer");
            Console.WriteLine("F8 - Delete Customer");
            Console.WriteLine("F10 - To Main menu");
            Console.WriteLine();

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.F1:
                    newCustomer();
                    Console.WriteLine("Press enter to continue");
                    break;
                case ConsoleKey.F2:
                    ScreenHandler.Display(new EditCustomerScreen(selected, company));
                    break;
                case ConsoleKey.F10:
                    ScreenHandler.Display(new MainMenuScreen(this.company));
                    break;
                case ConsoleKey.F8:
                    Database.Instance.DeleteCustomer(selected.CID); //TODO: kan ikke slette hvis brugeren har en salgsordre, DELETE ON CASCADE
                    break;
            }
        } 
    }
}
