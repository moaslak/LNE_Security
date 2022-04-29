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

    private List<OrderLine.States> statesToList()
    {
        List<OrderLine.States> list = Enum.GetValues(typeof(OrderLine.States)).Cast<OrderLine.States>().ToList();
        return list;
    }

    private (List<OrderLine>, bool) EditOrderLines(UInt32 OrderID)
    {
        bool succes = false;
        List<OrderLine> orderLines = Database.Instance.GetOrderLines(OrderID);
        if (orderLines.Count == 0)
        {
            return (orderLines, succes);
        }

        ListPage<OrderLine> orderLineListPage = new ListPage<OrderLine>();

        orderLineListPage.AddColumn("OLID", "OLID");
        orderLineListPage.AddColumn("Product", "PID");
        orderLineListPage.AddColumn("Quantity", "Quantity");
        orderLineListPage.AddColumn("Status", "State");

        foreach (OrderLine orderline in orderLines)
        {
            orderline.PID = orderline.Product.PID;
            orderLineListPage.Add(orderline);
            if (orderline.State == OrderLine.States.Closed || orderline.State == OrderLine.States.Canceled)
            {
                Console.WriteLine("Cannot edit Orderline with state: " + orderline.State.ToString());
                Console.WriteLine("Press a key to return");
                Console.ReadKey();
                return (orderLines, succes);
            }
        }
        OrderLine selected = orderLineListPage.Select(); // TODO: finish edit

        ListPage<Options> OptionListPage = new ListPage<Options>();
        OptionListPage.AddColumn("Edit", "Option");
        OptionListPage.Add(new Options("OrderID", "OrderID"));
        OptionListPage.Add(new Options("Product", "Product"));
        OptionListPage.Add(new Options("Quantity", "Quantity"));
        OptionListPage.Add(new Options("Status", "State"));
        Options option = OptionListPage.Select();

        string newValue = "";
        UInt32 newUint = 0;
        double newDouble = 0;
        if (option.Option != "Status")
        {
            Console.Write("Enter new " + option.Option.ToString() + ": ");
            newValue = Console.ReadLine();

            UInt32.TryParse(newValue, out newUint);
            Double.TryParse(newValue, out newDouble);
        }

        switch (option.Option)
        {
            case "OrderID":
                selected.OrderID = newUint;
                break;
            case "Product":
                selected.PID = newUint;
                break;
            case "Quantity":
                Product product = Database.Instance.SelectProduct(selected.PID);
                Console.WriteLine("Amount in storage: " + product.AmountInStorage);
                if(product.AmountInStorage < newDouble)
                {
                    Console.WriteLine("Not enough in storage!!");
                    succes = false;
                    return (orderLines, succes);
                }
                else
                    selected.Quantity = newDouble;
                break;
            case "Status":
                List<OrderLine.States> stateList = statesToList();
                ListPage<Options> listPage = new ListPage<Options>();
                listPage.AddColumn("Status", "Option");
                foreach (OrderLine.States state in stateList)
                {
                    listPage.Add(new Options(state.ToString(), state.ToString()));
                }
                Options selectedState = listPage.Select();
                switch (selectedState.Option)
                {
                    case "Created":
                        selected.State = OrderLine.States.Created;
                        break;
                    case "Confirmed":
                        selected.State = OrderLine.States.Confirmed;
                        break;
                    case "Packed":
                        selected.State = OrderLine.States.Packed;
                        break;
                    case "Closed":
                        selected.State = OrderLine.States.Closed;
                        break;
                    case "Incomplete":
                        selected.State = OrderLine.States.Incomplete;
                        break;
                }
                break;
        }
        for (int i = 0; i < orderLines.Count; i++)
        {
            if (orderLines[i].OLID == selected.OLID)
            {
                orderLines[i] = selected;
                Database.Instance.EditOrderline(selected.OLID, selected);
            }
        } 
        succes = true;

        return (orderLines, succes);
    }

    private SalesOrder EditSalesOrder(Options selected, SalesOrder selectedSalesOrder)
    {
        string newValue = "";
        if(selected.Option != "Completion Time" && selected.Option != "Orderlines")
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
                    throw new NotImplementedException();
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
            case "Total price":
                selectedSalesOrder.TotalPrice = newDouble;
                success = true;
                break;
            case "Orderlines":
                (selectedSalesOrder.OrderLines, success) = EditOrderLines(selectedSalesOrder.OrderID);
                break;
            default:
                break;
        }
        
        for (int i = 0; i < this.salesOrders.Count; i++)
        {
            if( selectedSalesOrder.OrderLines.Count == 0)
            {
                Console.WriteLine("no orderlines in sales order");// TODO: Denne kaldes når man ænder først gang
                return selectedSalesOrder;
            }
            if (this.salesOrders[i].OrderID == selectedSalesOrder.OrderID && success)
            {
                List<OrderLine> orderLines = Database.Instance.GetOrderLines(selectedSalesOrder.OrderID);
                Console.WriteLine("Sales order with orderId " + selectedSalesOrder.OrderID + " edited");
                return selectedSalesOrder;
            }
            if(this.salesOrders[i].OrderID == selectedSalesOrder.OrderID && !success)
            {
                Console.WriteLine("Sales order not edited");
                return selectedSalesOrder;
            }
            
        }
        Console.WriteLine("Could not find sales order to edit");
        
        return selectedSalesOrder;
    }

    protected override void Draw()
    {
        Company company = Database.Instance.SelectCompany(salesOrders[0].CompanyID);
        Customer customer = new Customer();
        do
        {
            Title = "Edit sales order screen";
            Clear(this);
            ListPage<SalesOrder> SalesOrderListPage = new ListPage<SalesOrder>();

            int fullNameMaxLength = 0;

            foreach(SalesOrder salesOrder in salesOrders)
            {
                salesOrder.OrderLines = Database.Instance.GetOrderLines(salesOrder.OrderID);
                salesOrder.TotalPrice = salesOrder.CalculateTotalPrice(salesOrder.OrderLines);
                SalesOrderListPage.Add(salesOrder);
                customer = Database.Instance.SelectCustomer(salesOrder.CID);
                if(salesOrder.FullName.Length > fullNameMaxLength)
                    fullNameMaxLength = salesOrder.FullName.Length;
            }
            
            for (int i = 0; i < salesOrders.Count; i++)
            {
                salesOrders[i].OrderLines = Database.Instance.GetOrderLines(salesOrders[i].OrderID);
                for (int j = 0; j < salesOrders[i].OrderLines.Count; j++)
                {
                    salesOrders[i].OrderLines[j].PID = salesOrders[i].OrderLines[j].Product.PID;
                    salesOrders[i].OrderLines[j].Product = Database.Instance.SelectProduct(salesOrders[i].OrderLines[j].PID);
                }
                salesOrders[i].TotalPrice = salesOrders[i].CalculateTotalPrice(salesOrders[i].OrderLines);
            }
            
            SalesOrderListPage.AddColumn("ID", "OrderID", 10);
            SalesOrderListPage.AddColumn("Order time", "OrderTime", salesOrders[0].OrderTime.ToString().Length);
            SalesOrderListPage.AddColumn("Customer Id", "CID", "Customer Id".Length);
            SalesOrderListPage.AddColumn("Name", "FullName", fullNameMaxLength);
            SalesOrderListPage.AddColumn("Price " + company.Currency.ToString(), "TotalPrice", "Price ".Length + 3);
            SalesOrderListPage.AddColumn("State", "State", 9);
            SalesOrder selectedSalesOrder = SalesOrderListPage.Select();

            ListPage<Options> optionsListPage = new ListPage<Options>();

            optionsListPage.AddColumn("Edit", "Option");
            //optionsListPage.Add(new Options("Customer Id", selectedSalesOrder.CID.ToString()));
            optionsListPage.Add(new Options("Total price", selectedSalesOrder.TotalPrice.ToString()));
            optionsListPage.Add(new Options("Completion Time", selectedSalesOrder.CompletionTime.ToString()));
            optionsListPage.Add(new Options("Orderlines", selectedSalesOrder.OrderLines.ToString()));

            optionsListPage.Add(new Options("Back", "NO EDIT"));
            Options selected = optionsListPage.Select();

            if (selected.Value != "NO EDIT")
            {
                selectedSalesOrder = EditSalesOrder(selected, selectedSalesOrder);
                
                if (selectedSalesOrder.OrderLines != null)
                {
                    Console.WriteLine("Press a key to update another parameter");
                    Database.Instance.EditSalesOrder(selectedSalesOrder);
                }
            }
            else
            {
                break;
            }
            Console.WriteLine("Press ESC to return to Sales Order screen");
            Database.Instance.EditSalesOrder(selectedSalesOrder);
            company = Database.Instance.SelectCompany(selectedSalesOrder.CompanyID);
        } while ((Console.ReadKey().Key != ConsoleKey.Escape));

        ScreenHandler.Display(new SalesOrderScreen(company, customer));
    }
}
