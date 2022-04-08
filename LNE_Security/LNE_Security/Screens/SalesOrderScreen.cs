using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECHCOOL.UI;
using System.Data.SqlClient;
using LNE_Security.Data;
using LNE_Security.Screens;

namespace LNE_Security;

public class SalesOrderScreen : ScreenHandler
{
    private Company company { get; set; }
    private Customer customer { get; set; }
    public SalesOrderScreen(Company Company, Customer Customer) : base(Company)
    {
        this.company = Company;
        this.customer = Customer;
    }

    Customer selected = new Customer();
    
    
    protected override void Draw()
    {
        Title = company.CompanyName + " Sales order screen";
        Clear(this);

        List<Customer> Customers = Database.Instance.GetCustomers();

        ListPage<Customer> customerListPage = new ListPage<Customer>();

        foreach (Customer Customer in Customers)
        {
            customerListPage.Add(Customer);
        }

        customerListPage.AddColumn("Select customer", "ID");
        selected = customerListPage.Select();

        ListPage<SalesOrder> salesOrderListPage = new ListPage<SalesOrder>();
        SqlConnection sqlConnection = new DatabaseConnection().SetSqlConnection();
        List<SalesOrder> salesOrders = Database.Instance.GetSalesOrders(selected);
        
        foreach (SalesOrder salesOrder in salesOrders)
        {
            if(salesOrder.CID == selected.ID)
                salesOrderListPage.Add(salesOrder);
        }
        
        salesOrderListPage.AddColumn("Sales order id", "OrderID",20);
        salesOrderListPage.AddColumn("Date", "OrderTime", 20);
        salesOrderListPage.AddColumn("CID", "CID", 20);
        salesOrderListPage.AddColumn("Name", "FullName", 20);
        salesOrderListPage.AddColumn("Price", "TotalPrice", 20);
        salesOrderListPage.Draw();

        Console.WriteLine("F1 - New Sales Order");
        Console.WriteLine("F2 - Edit Sales Order");
        Console.WriteLine("F3 - Get Sales Order for Customer");
        Console.WriteLine("F7 - Delete Sales Orders by customer id");
        Console.WriteLine("F8 - Delete Sales Orders");
        Console.WriteLine("F10 - Back");
        Console.WriteLine("Esc - Close App");
        
        UInt16 CID = 0;
        switch (Console.ReadKey().Key)
        {
            
            case ConsoleKey.F1:
                Database.Instance.NewSalesOrder(selected);
                break;
            case ConsoleKey.F2:
                ScreenHandler.Display(new EditSalesOrderScreen(salesOrders));
                break;
            case ConsoleKey.F3:
                do
                {
                    Console.Write("Get Sales Srders for which custumer ID: ");
                } while (!(UInt16.TryParse(Console.ReadLine(), out CID)));
                Console.WriteLine();
                showSalesOrders(Database.Instance.SelectCustomer(CID));
                Console.WriteLine("Press a key to continue");
                Console.ReadKey();
                break;
            case ConsoleKey.F7:
                do
                {
                    Console.Write("Delete Sales Orders for which custumer ID: ");
                } while (!(UInt16.TryParse(Console.ReadLine(), out CID)));
                Console.WriteLine();
                Database.Instance.DeleteSalesOrdersByCID(CID);
                break;
            case ConsoleKey.F8:
                Database.Instance.DeleteSalesOrder(DeleteSalesOrderOption(salesOrders),Database.Instance.SelectCustomer(selected.ID));
                Console.WriteLine("Press a enter to continue");
                break;
            case ConsoleKey.F10:
                ScreenHandler.Display(new MainMenuScreen(selected));
                break;
            case ConsoleKey.Escape:
                Environment.Exit(0);
                break;
            default:
                break;
        }
        Draw();
        
    }

    private void showSalesOrders(Customer customer)
    {
        if(customer == null)
        {
            Console.WriteLine("Customer not found");
            return;
        }
            
        ListPage<SalesOrder> salesOrderListPage = new ListPage<SalesOrder>();
        List<SalesOrder> salesOrders = Database.Instance.GetSalesOrders(customer);
        salesOrderListPage.AddColumn("Sales order id", "OrderID", 20);
        salesOrderListPage.AddColumn("Date", "OrderTime", 20);
        salesOrderListPage.AddColumn("CID", "CID", 20);
        salesOrderListPage.AddColumn("Name", "FullName", 20);
        salesOrderListPage.AddColumn("Price", "TotalPrice", 20);
        foreach(SalesOrder salesOrder in salesOrders)
        {
            salesOrderListPage.Add(salesOrder);
        }
        salesOrderListPage.Draw();
    }

    private UInt32 DeleteSalesOrderOption(List<SalesOrder> salesOrders)
    {
        ListPage<SalesOrder> SalesOrderListPage = new ListPage<SalesOrder>();

        SalesOrderListPage.AddColumn("ID", "OrderID");
        SalesOrderListPage.AddColumn("Order time", "OrderTime");
        SalesOrderListPage.AddColumn("Customer Id", "CID");
        SalesOrderListPage.AddColumn("Name", "FullName");
        SalesOrderListPage.AddColumn("Price", "TotalPrice");

        foreach (SalesOrder salesOrder in salesOrders)
        {
            SalesOrderListPage.Add(salesOrder);

        }
        UInt32 orderID = SalesOrderListPage.Select().OrderID;
        return orderID;
    }
}
