using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

public class CustomerDetails : ScreenHandler
{
    private Customer customer { get; set; }
	private Company company { get; set; }
    public CustomerDetails(Customer Customer, Company Company) : base(Customer, Company)
    {
        this.customer = Customer;
		this.company = Company;
    }

	protected override void Draw()
	{
		ListPage<Customer> CustomerlistPage = new ListPage<Customer>();
		ListPage<SalesOrder> SalesListPage = new ListPage<SalesOrder>();
		List<SalesOrder> salesOrders = Database.Instance.GetSalesOrders(customer);

		ContactInfo contactInfo = Database.Instance.SelectContactInfo(customer);
		Address address = Database.Instance.SelectAddress(contactInfo);
		customer.FullAddress = contactInfo.FullAddress;
		DateTime? newestCompletedOrder = new DateTime();
		foreach(SalesOrder SalesOrder in salesOrders)
        {
			if (SalesOrder.CompletionTime > newestCompletedOrder)
            {
				newestCompletedOrder = SalesOrder.CompletionTime;
			}
			customer.newestOrder = newestCompletedOrder;
				
        }
		
		CustomerlistPage.Add(customer);

		ListPage<string> SelectedList = new ListPage<string>();

		Title = customer.FullName + " Customer Details";
		Clear(this);

		CustomerlistPage.AddColumn("Customer name", "FullName", ColumnLength("Customer name", customer.FullName));
		CustomerlistPage.AddColumn("Address", "FullAddress", ColumnLength("Address", customer.ContactInfo.FullAddress));
		CustomerlistPage.AddColumn("Last purchase", "newestOrder", newestCompletedOrder.ToString().Length);
		CustomerlistPage.Draw();

		Console.WriteLine("Selection: " + customer.FullName);
		Console.WriteLine("F2 - Edit");
		Console.WriteLine("F8 - Delete");
		Console.WriteLine("F10 - Back");
		CustomerScreen customerScreen = new CustomerScreen(company);

		switch (Console.ReadKey().Key)
		{
			case ConsoleKey.F10:
				ScreenHandler.Display(customerScreen);
				break;
			case ConsoleKey.F2:
				ScreenHandler.Display(new EditCustomerScreen(customer, company));
				break;
			case ConsoleKey.F8:
				Database.Instance.DeleteCustomer(customer.CID);
				break;
			default:
				break;
		}
	}
}
