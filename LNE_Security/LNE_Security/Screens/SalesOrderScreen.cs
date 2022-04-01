using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECHCOOL.UI;
using System.Data.SqlClient;

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

    // mock sales order
    SalesOrder salesOrder = new SalesOrder();
    SalesOrder salesOrder2 = new SalesOrder();
    List<SalesOrder> salesOrders = new List<SalesOrder>();
    OrderLine orderLine = new OrderLine();
    OrderLine orderLine2 = new OrderLine();
    List<OrderLine> orderLines = new List<OrderLine>();

    Customer selectedCustomer = new Customer();
    
    
    Database database = new Database();
    
    protected override void Draw()
    {
        Title = company.CompanyName + " Sales order screen";
        Clear(this);
        ListPage<SalesOrder> salesOrderListPage = new ListPage<SalesOrder>();
        SqlConnection sqlConnection = database.SetSqlConnection();
        salesOrders = database.GetSalesOrders(sqlConnection, selectedCustomer);
        
        foreach (SalesOrder salesOrder in salesOrders)
        {
            if(salesOrder.CID == selectedCustomer.ID)
                salesOrderListPage.Add(salesOrder);
        }
        
        salesOrderListPage.AddColumn("Sales order id", "OrderID",20);
        salesOrderListPage.AddColumn("Date", "OrderTime", 20);
        salesOrderListPage.AddColumn("CID", "CID", 20);
        salesOrderListPage.AddColumn("Name", "FullName", 20);
        salesOrderListPage.AddColumn("Price", "TotalPrice", 20);
        salesOrderListPage.Draw();

        List<Customer> Customers = database.GetCustomers();
        
        ListPage<Customer> customerListPage = new ListPage<Customer>();

        foreach (Customer Customer in Customers)
        {
            customerListPage.Add(Customer);
        }

        customerListPage.AddColumn("CID", "ID");
        selectedCustomer = customerListPage.Select();
        Console.WriteLine("Options???");

    }
}
