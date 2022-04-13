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
        string newValue = "";
        if(selected.Option != "Completion Time")
        {
            Console.Write("New value: ");
            newValue = Console.ReadLine();
        }

        UInt16 newInt = 0;
        UInt16.TryParse(newValue, out newInt);
        bool success = false;
        double newDouble = 0;
        double.TryParse(newValue, out newDouble);
        switch (selected.Option)
        {
            case "Customer Id":
                
                List<Customer> customers = Database.Instance.GetCustomers();
                foreach(Customer customer in customers)
                {
                    if(customer.CID.ToString() == selected.Value)
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
            case "Completion Time":

                DateTime temp;
                do
                {
                    Console.Write("Enter completetion time(yyyy-mm-dd hh:mm:ss): ");
                } while (!(DateTime.TryParse(Console.ReadLine(), out temp)));
                selectedSalesOrder.CompletionTime = temp;
                success = true;
                break;
            case "Total price": //TODO: Denne virker ikke
                selectedSalesOrder.TotalPrice = newDouble;
                success = true;
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
            //optionsListPage.Add(new Options("Customer Id", selectedSalesOrder.CID.ToString()));
            optionsListPage.Add(new Options("Total price", selectedSalesOrder.TotalPrice.ToString()));
            optionsListPage.Add(new Options("Completion Time", selectedSalesOrder.CompletionTime.ToString()));

            optionsListPage.Add(new Options("Back", "NO EDIT"));
            Options selected = optionsListPage.Select();

            if (selected.Value != "NO EDIT")
            {
                selectedSalesOrder = EditSalesOrder(selected, selectedSalesOrder);
                Console.WriteLine("Press a key to update another parameter"); // TODO: Denne skal gerne væk
                Database.Instance.EditSalesOrder(selectedSalesOrder);
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
