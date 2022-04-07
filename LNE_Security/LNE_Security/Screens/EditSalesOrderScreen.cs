using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

internal class EditSalesOrderScreen : ScreenHandler
{
    private class Options
    {
        public string Option { get; set; }
        public string Value { get; set; }
        public Options(string option, string value)
        {
            Value = value;
            Option = option;
        }
    }

    private List<SalesOrder> salesOrders;
    public EditSalesOrderScreen(List<SalesOrder> SalesOrders) : base(SalesOrders)
    {
        this.salesOrders = SalesOrders;
    }

    private SalesOrder EditSalesOrder(Options selected, SalesOrder selectedSalesOrder)
    {
        Console.Write("New value: ");
        string newValue = Console.ReadLine();
        UInt16 newInt = 0;
        UInt16.TryParse(newValue, out newInt);
        bool success = false;
        switch (selected.Option)
        {
            case "Customer Id":
                
                List<Customer> customers = Database.Instance.GetCustomers();
                foreach(Customer customer in customers)
                {
                    if(customer.ID == newInt)
                    {
                        selectedSalesOrder.CID = newInt;
                        Console.WriteLine("Customer id changed");
                        success = true;
                        break;
                    }
                }
                if (!success)
                    Console.WriteLine("Could not find customer id. No entry change.");
                break;
            default:
                break;
        }
        for(int i = 0; i < this.salesOrders.Count; i++)
        {
            if (this.salesOrders[i].OrderID == selectedSalesOrder.OrderID && success)
            {
                Console.WriteLine("Sales order with orderId " + selectedSalesOrder.OrderID + " edited");
                return this.salesOrders[i];
            }
                
        }
        Console.WriteLine("Could not find sales order to edit");
        return selectedSalesOrder;
    }

    protected override void Draw()
    {
        Company company = new Company();
        Customer customer = new Customer();
        do
        {
            Title = "Edit sales order screen";
            Clear(this);
            ListPage<SalesOrder> SalesOrderListPage = new ListPage<SalesOrder>();

            SalesOrderListPage.AddColumn("ID", "OrderID");
            SalesOrderListPage.AddColumn("Order time", "OrderTime");
            SalesOrderListPage.AddColumn("Customer Id", "CID");
            SalesOrderListPage.AddColumn("Name", "FullName");
            SalesOrderListPage.AddColumn("Price", "TotalPrice");

            foreach(SalesOrder salesOrder in salesOrders)
            {
                SalesOrderListPage.Add(salesOrder);
                customer = Database.Instance.SelectCustomer(salesOrder.CID);
            }
            SalesOrder selectedSalesOrder = SalesOrderListPage.Select();

            ListPage<Options> optionsListPage = new ListPage<Options>();

            optionsListPage.AddColumn("Edit", "Option");
            optionsListPage.Add(new Options("Customer Id", selectedSalesOrder.CID.ToString()));

            optionsListPage.Add(new Options("Back", "NO EDIT"));
            Options selected = optionsListPage.Select();

            // TODO: EDIT SALESORDER!!!

            if (selected.Value != "NO EDIT")
            {
                selectedSalesOrder = EditSalesOrder(selected, selectedSalesOrder);
                Console.WriteLine("Press a key to update another parameter"); // TODO: Denne skal gerne væk
                Database.Instance.EditSalesOrder(selectedSalesOrder, selectedSalesOrder.OrderID);
            }
            else
            {
                break;
            }
            Console.WriteLine("Press ESC to return to Sales Order screen");

        } while ((Console.ReadKey().Key != ConsoleKey.Escape));

        ScreenHandler.Display(new SalesOrderScreen(company, customer));
    }
}
