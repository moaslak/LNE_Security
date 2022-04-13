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
    SqlConnection sqlConnection = new DatabaseConnection().SetSqlConnection("LNE_Security");
    public void DeleteSalesOrdersByCID(UInt16 CID)
    {
        string query = "DELETE FROM [dbo].[SalesOrder] WHERE CID = " + CID.ToString();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);

        List<SalesOrder> salesOrders = GetSalesOrders(CID);
        sqlConnection.Open();
        if (salesOrders.Count > 0)
        {
            Console.WriteLine(salesOrders.Count.ToString() + " entries will be deleted. This CANNOT be undone");
        }
        char choice = '0';
        do
        {
            Console.WriteLine("Delete entries for customer " + CID +"? (Y)es/(N)o");
        } while (!(char.TryParse(Console.ReadLine(), out choice)) && (Char.ToLower(choice) != 'y' || Char.ToLower(choice) != 'n'));

        switch (choice)
        {
            case 'y':
                //execute the SQLCommand
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Close();

                //close connection
                sqlConnection.Close();
                Console.WriteLine("Entries deleted");
                break;
            case 'n':
                Console.WriteLine("Delete aborted");
                break;
        }
        Console.WriteLine("Press a key to continue");
        Console.ReadKey();
    }

    public void DeleteSalesOrder(UInt32 ID, Customer customer)
    {
        string query = "DELETE FROM [dbo].[SalesOrder] WHERE OrderID = " + ID.ToString();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
        SalesOrder salesOrder = SelectSalesOrder(ID, customer);
        if (salesOrder == null)
        {
            Console.WriteLine("Sales order with ID = " + ID + " was succesfully deleted");
            Console.WriteLine("Press a key to continue");
            Console.ReadKey();
        }
            
        
        else
            Console.WriteLine("Could not find sales order to delete");
    }

    private OrderLine NewOrderLine()
    {
        Console.Clear();
        List<Product> productList = Database.Instance.GetProducts();
        if (productList.Count == 0) return null;

        OrderLine orderline = new OrderLine();
        List<UInt16> OLIDs = new List<UInt16>();
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
            } while (!(char.TryParse(Console.ReadLine(), out choice)) && (Char.ToLower(choice) != 'y' || Char.ToLower(choice) != 'n'));

            switch (choice)
            {
                case 'y':
                    quantity = amountInStore;
                    break;
                case 'n':
                    orderline.Product = null;
                    return orderline;
            }
        }
        orderline.Quantity = quantity;

        sqlConnection.Open();
        string query = @"INSERT INTO [dbo].[Orderline]( [PID], [Quantity]) VALUES(
        '" + selectedProduct.PID + "','" + quantity.ToString() + "')";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();
        query = "SELECT OLID FROM [Orderline]";
        cmd = new SqlCommand(query, sqlConnection);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            OLIDs.Add(Convert.ToUInt16(reader.GetValue(0)));
        }
        sqlConnection.Close();

        if (OLIDs.Count > 0 && OLIDs.Count > 1)
        {
            foreach (UInt16 id in OLIDs)
            {
                if (id > orderline.OLID)
                    orderline.OLID = id;
            }
        }
        else
            orderline.OLID = 1;

        selectedProduct.AmountInStorage = amountInStore - quantity;
        Database.Instance.EditProduct(selectedProduct.PID, selectedProduct);

        return orderline;
    }
    public void NewSalesOrder(Customer customer, UInt16 companyID)
    {
        SalesOrder salesOrder = new SalesOrder();
        Console.WriteLine();
        Console.WriteLine("New sales order");

        salesOrder.OrderTime = DateTime.Now;
        salesOrder.CID = customer.CID;
        salesOrder.FullName = customer.ContactInfo.FullName;
        
        bool Done = false;
        do
        {
            salesOrder.OrderLines.Add(NewOrderLine());
            Console.WriteLine("Press 'Esc' to stop adding orderlines");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Escape:
                    Done = true;
                    //salesOrder.OLID = salesOrder.OrderLines[0].OLID; // TODO: fix this
                    break;
                default:
                    break;
            }
        } while (!Done);

        salesOrder.TotalPrice = salesOrder.CalculateTotalPrice(salesOrder.OrderLines);
        SqlConnection sqlConnection = new DatabaseConnection().SetSqlConnection("LNE_Security");

        string query = @"INSERT INTO[dbo].[SalesOrder]
            ([OrderTime]
          ,[ContactInfoID]
          ,[CID]
          ,[CompanyID]
          ,[Price])
            VALUES
           (" + "'" + salesOrder.OrderTime.ToString("s").Replace("T"," ") +
           //"','" + salesOrder.OLID.ToString() +
           "','" + customer.ContactInfoID.ToString() +
           "','" + customer.CID +
           "', '" + companyID.ToString() +
           "','" + salesOrder.CalculateTotalPrice(salesOrder.OrderLines) + "')";
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();

        //execute the SQLCommand
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
    }

    public void EditSalesOrder(SalesOrder editedSalesOrder)
    {
        UInt32 orderID = editedSalesOrder.OrderID;
        //SalesOrder editedSalesOrder = SelectSalesOrder(orderId, customer);
        
        if (editedSalesOrder.CompletionTime.ToString() == "01-01-0001 00:00:00")
            editedSalesOrder.CompletionTime = null;
        string query = @"UPDATE [dbo].[SalesOrder]
                SET [OrderTime] = '" + editedSalesOrder.OrderTime.ToString("s").Replace("T", " ") + "'" +
                ", [CompletionTime] = '" + editedSalesOrder.CompletionTime.ToString() + "'" +
            //",[CID] = '" + editedSalesOrder.CID + "'" +
            //",[ContactInfoID] = '" + editedSalesOrder.ContactInfoID + "'" +
            //",[CompanyID] = '" + editedSalesOrder.CompanyID + "'" +
            //",[OLID] = '" + editedSalesOrder.OLID + "'" +
            ",[Price] = '" + editedSalesOrder.TotalPrice + "'" +
            " WHERE OrderID = " + orderID;
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
            if (salesOrder.OrderID == orderId)
                return salesOrder;
        }
        return null;
    }
    public List<SalesOrder> GetSalesOrders(Customer customer)
    {
        List<SalesOrder> salesOrders = new List<SalesOrder>();

        string dateTimeString = "";
        DateTime dateTime = new DateTime();
        ContactInfo contactInfo = SelectContactInfo(customer);

        string query = @"SELECT * FROM [dbo].[SalesOrder]";
        query = query + "WHERE CID = " + customer.CID;
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
            catch (FormatException ex)
            {
                salesOrder.OrderTime = DateTime.MinValue;
            }
            dateTimeString = reader.GetValue(2).ToString();
            try
            {
                DateTime.TryParse(dateTimeString, out dateTime);
                salesOrder.CompletionTime = dateTime;
            }
            catch (FormatException ex)
            {
                salesOrder.CompletionTime = null;
            }
            try
            {
                salesOrder.OLID = Convert.ToUInt16(reader.GetValue(3));
            }
            catch(InvalidCastException ex)
            {
                Console.WriteLine("OLID not set");
                salesOrder.OLID = 0;
            }
            salesOrder.ContactInfoID = Convert.ToUInt16(reader.GetValue(4));
            salesOrder.CID = (ushort)(Convert.ToUInt16(reader.GetValue(5)));
            salesOrder.TotalPrice = (double)Convert.ToDouble(reader.GetValue(6));
            salesOrder.FullName = contactInfo.FullName;
            salesOrders.Add(salesOrder);
        }
        reader.Close();
        sqlConnection.Close();
        
        return salesOrders;
    }

    public List<SalesOrder> GetSalesOrders(UInt16 CID) //TODO: bruges denne?
    {
        List<SalesOrder> salesOrders = new List<SalesOrder>();

        string dateTimeString = "";
        DateTime dateTime = new DateTime();
        string query = @"SELECT * FROM [dbo].[SalesOrder]";
        query = query + "WHERE CID = " + CID;
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
                salesOrder.OrderTime = DateTime.MinValue;
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

