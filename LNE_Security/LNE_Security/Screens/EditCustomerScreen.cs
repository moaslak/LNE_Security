using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using static LNE_Security.Screens.EditCompnayScreen;

namespace LNE_Security.Screens;

public class EditCustomerScreen : ScreenHandler
{
    private Person person { get; set; }
    private ContactInfo contact { get; set; }
    private Address address { get; set; }
    private Options options { get; set; }
    private Customer customer { get; set; }
    private Company company { get; set; }
    public EditCustomerScreen(Person person, ContactInfo contactInfo, Address address) : base(person)
    {
        this.person = person;
        this.contact = contactInfo;
        this.address = address;
    }

    public EditCustomerScreen(Customer Customer) : base(Customer)
    {
        this.customer = Customer;
        contact = customer.ContactInfo;
        address = contact.Address;
    }
    public EditCustomerScreen(Customer Customer, Company Company) : base(Customer, Company)
    {
        this.customer = Customer;
        contact = customer.ContactInfo;
        address = contact.Address;
        this.company = Company;
    }

    public EditCustomerScreen()
    {
    }

    private void EditCustomer(Options selected)
    {
        string newValue = "";
        Console.Write("Enter new " + selected.Option.ToString() + ": ");
        newValue = Console.ReadLine();
        string zipcode = address.ZipCode.ToString();
        switch (selected.Option)
        {
            case "Firstname":
                this.contact.FirstName = newValue;
                break;
            case "Lastname":
                this.contact.LastName = newValue;
                break;
            case "Streetname":
                this.address.StreetName = newValue;
                break;
            case "Housenumber":
                this.address.HouseNumber = newValue;
                break;
            case "Zipcode":
                this.address.ZipCode = newValue;
                break;
            case "City":
                this.address.City = newValue;
                break;
            case "Phonenumber":
                this.contact.PhoneNumber = newValue;
                break;
            case "Email":
                this.contact.Email = newValue;
                break;
            default:
                break;
        }
        Customer customer = this.customer;
        Database.Instance.EditCustomer(this.customer, selected.Option);
    }
    protected override void Draw()
    {
        do
        {
            Title = contact.FullName + " Edit Customer Screen";
            Clear(this);

            ListPage<Person> PersonListPage = new ListPage<Person>();
            ListPage<ContactInfo> ContactListPage = new ListPage<ContactInfo>();
            ListPage<Address> AddressListPage = new ListPage<Address>();

            Console.WriteLine("Firstname: " + contact.FirstName);
            Console.WriteLine("Lastname: " + contact.LastName);
            Console.WriteLine("Streetname: " + contact.Address.StreetName);
            Console.WriteLine("Housenumber: " + contact.Address.HouseNumber);
            Console.WriteLine("Zipcode: " + contact.Address.ZipCode);
            Console.WriteLine("City: " + contact.Address.City);
            Console.WriteLine("Country: " + contact.Address.Country);
            Console.WriteLine("Phonenumber: " + contact.PhoneNumber);
            Console.WriteLine("Email: " + contact.Email);

            ListPage<Options> OptionListPage = new ListPage<Options>();
            string zipCode = address.ZipCode.ToString();
            OptionListPage.AddColumn("Edit", "Option");
            OptionListPage.Add(new Options("Firstname", contact.FirstName));
            OptionListPage.Add(new Options("Lastname", contact.LastName));
            OptionListPage.Add(new Options("Streetname", address.StreetName));
            OptionListPage.Add(new Options("Housenumber", address.HouseNumber));
            OptionListPage.Add(new Options("Zipcode", address.ZipCode));
            OptionListPage.Add(new Options("City", address.City));
            OptionListPage.Add(new Options("Country", address.Country));
            OptionListPage.Add(new Options("Phonenumber", contact.PhoneNumber));
            OptionListPage.Add(new Options("Email", contact.Email));
            
            OptionListPage.Add(new Options("Back", "NO EDIT"));
            Options selected = OptionListPage.Select();

            if(selected.Value != "NO EDIT")
            {
                EditCustomer(selected);
            }
            else
            {
                break;
            }
            Console.WriteLine("Press ESC to return to Company screen");
        } while ((Console.ReadKey().Key != ConsoleKey.Escape));

        CustomerScreen customerScreen = new CustomerScreen(company);
        ScreenHandler.Display(customerScreen);
    }
}
