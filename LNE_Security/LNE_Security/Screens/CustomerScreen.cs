using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

public class CustomerScreen : ScreenHandler
{
    ContactInfo contact { get; set; }
    Address address { get; set; }
    private Person person { get; set; }
    public CustomerScreen(Person person) : base(person)
    {
        this.Person = person;
    }

    public CustomerScreen()
    {
    }

    protected override void Draw()
    {
        ListPage<Person> PersonListPage = new ListPage<Person>();
        ListPage<ContactInfo> ContactListPage = new ListPage<ContactInfo>();
        PersonListPage.Add(person);
        string name = $"{person.ContactInfo.FirstName} {person.ContactInfo.LastName}";
        Title = person.ContactInfo.FullName + " Customer name";
        Clear(this);
        ContactListPage.AddColumn("Customer ID", "ID");
        ContactListPage.AddColumn("Customer Name", "FullName");
        ContactListPage.AddColumn("Phonenumber", "Phonenumber");
        ContactListPage.AddColumn("Email", "Email");
        Console.WriteLine("Choose Customer");
        ContactInfo selected = ContactListPage.Select();

        Console.WriteLine("Selection: " + selected.FullName);
        Console.WriteLine("F1 - New Customer");
        Console.WriteLine("F2 - Edit");
        Console.WriteLine("F10 - To Main menu");
        Console.WriteLine("Esc - Close App");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.F1:
               
                Console.WriteLine("IMPLEMENT NEW COMPANY");
                break;
            case ConsoleKey.F2:
                EditCompnayScreen editScreen = new EditCompnayScreen(person);
                ScreenHandler.Display(editScreen);
                break;
            case ConsoleKey.F10:
                MainMenuScreen menu = new MainMenuScreen(person);
                ScreenHandler.Display(menu);
                break;
            case ConsoleKey.Escape:
                Environment.Exit(0);
                break;
        }
    }
}
