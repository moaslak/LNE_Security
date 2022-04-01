using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

public class CustomerDetails : ScreenHandler
{
    private ContactInfo contactInfo { get; set; }
    public CustomerDetails(ContactInfo contact) : base(contact)
    {
        this.contactInfo = contact;
    }

	protected override void Draw()
	{
		ListPage<ContactInfo> CustomerlistPage = new ListPage<ContactInfo>();
		CustomerlistPage.Add(contactInfo);

		ListPage<string> SelectedList = new ListPage<string>();

		Title = contactInfo.FullName + " Customer Details";
		Clear(this);

		CustomerlistPage.AddColumn("Customer name", "FullName");
		CustomerlistPage.AddColumn("Address", "FullAddress");
		//CustomerlistPage.AddColumn("Last Purchase", "?");
		ContactInfo selected = CustomerlistPage.Select();

		Console.WriteLine("Selection: " + selected.FullName);
		Console.WriteLine("F1 - Back");
		Console.WriteLine("F2 - Edit");
		CustomerScreen customerScreen = new CustomerScreen();
		EditCustomerScreen editCustomerScreen = new EditCustomerScreen();

		switch (Console.ReadKey().Key)
		{
			case ConsoleKey.F1:
				ScreenHandler.Display(customerScreen);
				break;
			case ConsoleKey.F2:
				ScreenHandler.Display(editCustomerScreen);
				break;
			default:
				break;
		}
	}
}
