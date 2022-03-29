using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECHCOOL.UI;

namespace LNE_Security;

public class SalesOrderScreen : ScreenHandler
{
    private Company company { get; set; }
    public SalesOrderScreen(Company Company) : base(Company)
    {
        this.company = Company;
    }

    // mock sales order
    SalesOrder salesOrder = new SalesOrder();
    OrderLine orderLine = new OrderLine();
    OrderLine orderLine2 = new OrderLine();
    List<OrderLine> orderLines = new List<OrderLine>();

    protected override void Draw()
    {
        Title = company.CompanyName + " Sales order screen";
        Clear(this);

        ListPage<SalesOrder> salesOrderListPage = new ListPage<SalesOrder>();

        // TODO: SQL query
        // select * from salesorder
        // where company = this.company
        orderLine.Product.ProductName = "hest";
        orderLine.Product.ID = 1;
        orderLine.Product.ProductNumber = 1;
        orderLine.Quantity = 3;
        orderLine.Product.SalesPrice = 10;
        orderLine.Product.CostPrice = 5;
        orderLines.Add(orderLine);
        orderLine2.Product.ProductName = "fest";
        orderLine2.Product.ID = 12;
        orderLine2.Product.ProductNumber = 11;
        orderLine2.Quantity = 32;
        orderLine2.Product.SalesPrice = 1220;
        orderLine2.Product.CostPrice = 51;
        orderLines.Add(orderLine2);
        salesOrder.OrderID = 1;
        salesOrder.OrderTime = DateTime.Now;
        salesOrder.OrderLines = orderLines;
        salesOrder.TotalPrice = salesOrder.CalculateTotalPrice(orderLines);

        salesOrderListPage.AddColumn("Sales order id", "OrderID", "Sales order id".Length + 5);
        salesOrderListPage.AddColumn("Country", "Country");
        salesOrderListPage.AddColumn("Currency", "Currency");
        salesOrderListPage.Draw();
        Console.ReadKey();



    }
}
