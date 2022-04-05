using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LNE_Security.Data;
using LNE_Security.Screens;
using TECHCOOL.UI;

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
        ListPage<Product> productListPage = new ListPage<Product>();
        ListPage<String> selectedList = new ListPage<String>();
        foreach (Product product in productList)
            productListPage.Add(product);

        Product selectedProduct = new Product();

        productListPage.AddColumn("Product Number", "ProductNumber", 10);
        productListPage.AddColumn("Product Name", "ProductName", 25);
        productListPage.AddColumn("Amount In Storage", "AmountInStorage");
        productListPage.AddColumn("Cost Price", "CostPrice");
        productListPage.AddColumn("Sales Price", "SalesPrice");
        if (productList.Count == 0)
            productListPage.Draw();
        else
            selectedProduct = productListPage.Select();
        Console.WriteLine("Selection : " + selectedProduct.ProductName);
        orderline.Product = selectedProduct;
        double quantity = 0;
        do
        {
            Console.Write("How many: ");
        } while (!(double.TryParse(Console.ReadLine(), out quantity)));
        double amountInStore = selectedProduct.AmountInStorage;
        if (quantity > amountInStore)
        {
            Console.WriteLine("Not enough in store.");
            Console.WriteLine("Amount in store: " + amountInStore);

            char choice = '0';
            do
            {
                Console.WriteLine("Add available to orderline? (Y)es/(N)o");
            } while (!(char.TryParse(Console.ReadLine(), out choice)) && Char.ToLower(choice) != 'y' || Char.ToLower(choice) != 'n');

            switch (choice)
            {
                case 'y':
                    quantity = amountInStore;
                    orderline.Quantity = quantity;
                    selectedProduct.AmountInStorage = amountInStore - quantity;
                    Database.Instance.EditProduct(selectedProduct.ID, selectedProduct);
                    break;
                case 'n':
                    orderline.Product = null;
                    break;
            }
        }

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
            Console.WriteLine("Press 'Esc' to stop adding orderlines");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Escape:
                    Done = true;
                    break;
                default:

                    break;
            }
        } while (!Done);

        SqlConnection sqlConnection = new DatabaseConnection().SetSqlConnection();

        // TODO: QUERY FÆRDIGGØRES
        string query = @"INSERT INTO[dbo].[SalesOrder]
            ([Date]
          ,[CID]
          ,[FullName]
          ,[Price])
            VALUES
           (< Date, datetime,>
           ,< CID, int,>
           ,< FullName, varchar(64),>
           ,< Price, money,>)";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
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

