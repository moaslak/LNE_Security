using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

    public class StorageScreen : ScreenHandler
    {

	private Company company { get; set; }
	public StorageScreen(Company Company) : base(Company)
	{
		this.company = Company;
	}

    class Options
    {
        public string Option { get; set; }
        public string KeyPress { get; set; }
        public Options(string option, string keyPress)
        {
            KeyPress = keyPress;
            Option = option;


        }
    }
    protected override void Draw()
    {
        Title = company.CompanyName + " Storage Screen";
        Clear(this);
        ListPage<SalesOrder> storageListPage = new ListPage<SalesOrder>();

        Console.WriteLine("F1 - Not comfirmed sales orders");
        Console.WriteLine("F2 - Pick order");
        Console.WriteLine("F3 - Place products");
        Console.WriteLine("F10 - To Main menu");
        Console.WriteLine("Esc - Close App");

        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.F1:
                ConfirmSalesOrders();
                break;
            case ConsoleKey.F2:
                Pick();
                break;
            case ConsoleKey.F8:
                Put();
                break;
            case ConsoleKey.F10:
                ScreenHandler.Display(new MainMenuScreen(company));
                break;
            case ConsoleKey.Escape:
                Environment.Exit(0);
                break;
        }
    }

    private void ConfirmSalesOrders()
    {
        List<SalesOrder> salesOrders = Database.Instance.GetSalesOrders("Created");
        ListPage<SalesOrder> salesOrdersListPage = new ListPage<SalesOrder>();
        SalesOrder selected = new SalesOrder();
        if (salesOrders.Count > 0)
        {
            foreach (SalesOrder salesOrder in salesOrders)
            {
                salesOrder.FullName = (Database.Instance.SelectCustomer(salesOrder.CID)).ContactInfo.FullName;
                salesOrdersListPage.Add(salesOrder);
                
            }
            salesOrdersListPage.AddColumn("Order ID", "OrderID");
            salesOrdersListPage.AddColumn("Customer", "FullName");
            Console.WriteLine("Choose Company");
            selected = salesOrdersListPage.Select();

            List<OrderLine> orderLines = Database.Instance.GetOrderLines(selected.OrderID);
            
            ListPage<OrderLine> ordersListPage = new ListPage<OrderLine>();
            foreach (OrderLine orderLine in orderLines)
            {
                orderLine.PID = orderLine.Product.PID;
                ordersListPage.Add(orderLine);
            }
                

            OrderLine orderline = new OrderLine();
            ordersListPage.AddColumn("PID", "PID");
            ordersListPage.AddColumn("Quantity", "Quantity");

            Console.WriteLine("F1 - Confirm sales order");
            Console.WriteLine("F2 - Inspect orderlines");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.F1:
                    foreach (OrderLine orderLine in orderLines)
                        Database.Instance.EditOrderlineState(orderLine.OLID, "Confirmed"); // TODO: SalesOrder state update
                    break;
                case ConsoleKey.F2:
                    orderline = ordersListPage.Select();
                    ViewOrderline(orderline);
                    break;
                default:
                    Console.WriteLine("Not a valid selection");
                    break;
            }
        }
    }

    private void ViewOrderline(OrderLine orderLine)
    {


        Product product = Database.Instance.SelectProduct(orderLine.PID);
        ListPage<Product> productListPage = new ListPage<Product>();
        productListPage.Add(product);
        productListPage.AddColumn("Product name", "ProductName");
        productListPage.AddColumn("Amount in storage", "AmountInStorage");
        productListPage.AddColumn("Location", "LocationString");
        productListPage.Draw();
    }

    private void Pick()
    {
        new NotImplementedException();
    }

    private void Put()
    {
        new NotImplementedException();
    }

}
