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

        List<Customer> Customers = Database.Instance.GetCustomers();
        
        ListPage<Customer> customerListPage = new ListPage<Customer>();

        foreach (Customer Customer in Customers)
        {
            customerListPage.Add(Customer);
        }

        customerListPage.AddColumn("CID", "ID");
        selected = customerListPage.Select();

        Console.WriteLine("F1 - New Sales Order");
        Console.WriteLine("F2 - Edit Sales Order");
        Console.WriteLine("F8 - Delete Sales Order");
        Console.WriteLine("F10 - Back");
        Console.WriteLine("Esc - Close App");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.F1:
                Database.Instance.NewSalesOrder(selected);
                break;
            case ConsoleKey.F2:
                //ScreenHandler.Display(new EditSalesOrderScreen(selected));
                break;
            case ConsoleKey.F8:
                //Database.Instance.DeleteSalesOrder(selected.Id);
                break;
            case ConsoleKey.F10:
                ScreenHandler.Display(new MainMenuScreen(selected));
                break;
            case ConsoleKey.Escape:
                Environment.Exit(0);
                break;
        }
    }
}
