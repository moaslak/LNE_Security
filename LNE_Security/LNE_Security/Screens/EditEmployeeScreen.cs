﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using static LNE_Security.Screens.EditCompnayScreen;

namespace LNE_Security.Screens;

public class EditEmployeeScreen : ScreenHandler
{
    private Person person { get; set; }
    private ContactInfo contact { get; set; }
    private Address address { get; set; }
    private Options options { get; set; }
    private Employee employee { get; set; }
    public EditEmployeeScreen(Person person, ContactInfo contactInfo, Address address) : base(person)
    {
        this.person = person;
        this.contact = contactInfo;
        this.address = address;
    }

    public EditEmployeeScreen(Employee Employee) : base(Employee)
    {
        this.employee = Employee;
        contact = employee.ContactInfo;
        address = contact.Address;
    }

    public EditEmployeeScreen()
    {
    }

    private void EditEmployee(Options selected)
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
                //this.contact.PhoneNumber.Add(newValue);
                this.contact.PhoneNumber = newValue;
                break;
            case "Email":
                this.contact.Email = newValue;
                break;
            default:
                break;
        }
        Employee employee = this.employee;
        Database.Instance.EditEmployee(this.employee, selected.Option);
    }
    protected override void Draw()
    {
        do
        {
            Title = contact.FullName + " Edit Employee Screen";
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

            ContactListPage.AddColumn("Firstname", "FirstName");
            ContactListPage.AddColumn("Lastname", "LastName");
            AddressListPage.AddColumn("Streetname", "StreetName");
            AddressListPage.AddColumn("Housenumber", "HouseNumber");
            AddressListPage.AddColumn("Zipcode", "ZipCode");
            AddressListPage.AddColumn("City", "City");
            AddressListPage.AddColumn("Country", "Country");
            ContactListPage.AddColumn("Phonenumber", "PhoneNumber");
            ContactListPage.AddColumn("Email", "Email");

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


            /*string phoneNumbers = "";
            foreach(string phonenumber in contact.PhoneNumber)
            {
                phoneNumbers = phoneNumbers + phonenumber + "\n";
            }*/

            OptionListPage.Add(new Options("Phonenumber", contact.PhoneNumber));
            OptionListPage.Add(new Options("Email", contact.Email));
            OptionListPage.Add(new Options("Back", "NO EDIT"));
            Options selected = OptionListPage.Select();

            if (selected.Value != "NO EDIT")
            {
                EditEmployee(selected);
            }
            else
            {
                break;
            }
            Console.WriteLine("Press ESC to return to Company screen");
        } while ((Console.ReadKey().Key != ConsoleKey.Escape));

        ScreenHandler.Display(new EmployeeScreen());
    }
}