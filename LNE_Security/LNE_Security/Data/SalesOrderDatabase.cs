using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LNE_Security.Data;

namespace LNE_Security;

partial class Database
{
    // TODO: finish sales order database
    SqlConnection sqlConnection = new DatabaseConnection().SetSqlConnection();

    private OrderLine NewOrderLine()
    {
        List<Product> productList = Database.Instance.GetProducts();
        OrderLine orderline = new OrderLine();

        // TODO: List products and select from list
        orderline.Product = productList[0];
        orderline.Quantity = 3;

        return orderline;
        
    }
    public void NewSalesOrder(Customer customer)
    {
        SalesOrder salesOrder = new SalesOrder();
        Console.WriteLine();
        Console.WriteLine("New sales order");

        salesOrder.OrderTime = DateTime.Now;
        salesOrder.CID = customer.ID;
        salesOrder.FullName = customer.ContactInfo.FullName;

        // TODO: add products
        bool Done = false;
        do
        {
            salesOrder.OrderLines.Add(NewOrderLine());

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Escape:
                    Done = true;
                    break;
                default:
                    Console.WriteLine("Press 'Esc' to stop adding orderlines");
                    break;
            }
        } while (!Done);
    }
    public void EditSalesOrder(UInt32 orderId, Customer customer)
    {
        SalesOrder editedSalesOrder = SelectSalesOrder(orderId, customer);
        string query = @"UPDATE [dbo].[SalesOrder]
                SET [Date] = '" + editedSalesOrder.OrderTime + "'" +
            ",[CID] = '" + editedSalesOrder.CID + "'" +
            ",[FullName] = '" + editedSalesOrder.FullName + "'" +
            ",[Price] = '" + editedSalesOrder.TotalPrice + "'" +
            " WHERE id = " + orderId;
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
    }
        
    public SalesOrder SelectSalesOrder(UInt32 orderId, Customer customer)
    {
        List<SalesOrder> SalesOrderList = GetSalesOrders(customer);
        SalesOrder salesOrder = new SalesOrder();

        foreach (SalesOrder SalesOrderItem in SalesOrderList)
        {
            if (salesOrder.OrderID == ID)
                return salesOrder;
        }
        return null;
    }
    public List<SalesOrder> GetSalesOrders(Customer customer)
    {
        List<SalesOrder> salesOrders = new List<SalesOrder>();

        string dateTimeString = "";
        DateTime dateTime = new DateTime();
        string query = @"SELECT * FROM [dbo].[SalesOrder]";
        query = query + "WHERE CID = " + customer.ID;
        sqlConnection.Open();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            SalesOrder salesOrder = new SalesOrder();
            salesOrder.OrderID = Convert.ToUInt16(reader.GetValue(0).ToString());
            dateTimeString = reader.GetValue(1).ToString();
            try
            {
                DateTime.TryParse(dateTimeString, out dateTime);
                salesOrder.OrderTime = dateTime;
            }
            catch (Exception ex)
            {
                salesOrder.OrderTime = null;
            }

            salesOrder.CID = (ushort)(Convert.ToUInt16(reader.GetValue(2)));
            salesOrder.FullName = reader.GetValue(3).ToString();
            salesOrder.TotalPrice = (double)Convert.ToDouble(reader.GetValue(4));
            salesOrders.Add(salesOrder);
        }
        reader.Close();
        sqlConnection.Close();
        return salesOrders;
    }
}
