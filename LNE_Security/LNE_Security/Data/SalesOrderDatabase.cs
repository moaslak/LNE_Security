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
    SqlConnection sqlConnection = new DatabaseConnection().SetSqlConnection("LNE_Security");

    public List<OrderLine> GetOrderLines(UInt32 OrderID)
    {
        List<OrderLine> orderLines = new List<OrderLine>();
        string query = "SELECT * FROM [dbo].[OrderlIne] WHERE OrderID = '" + OrderID + "'";
        sqlConnection.Open();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);

        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            OrderLine line = new OrderLine();
            line.OLID = Convert.ToUInt16(reader.GetValue(0));
            line.Product.PID = Convert.ToUInt16(reader.GetValue(1));
            line.Quantity = Convert.ToDouble(reader.GetValue(2));
            line.OrderID = Convert.ToUInt16(reader.GetValue(3));
            string state = reader.GetValue(4).ToString();
            switch (state)
            {
                case "Closed":
                    line.State = OrderLine.States.Closed;
                    break;
                case "Packed":
                    line.State = OrderLine.States.Packed;
                    break;
                case "Confirmed":
                    line.State = OrderLine.States.Confirmed;
                    break;
                case "Created":
                    line.State = OrderLine.States.Created;
                    break;
                default:
                    line.State = OrderLine.States.Created;
                    break;
            }  
            orderLines.Add(line);
        }
        reader.Close();
        sqlConnection.Close();
        return orderLines;

    }
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

    private OrderLine NewOrderLine(UInt32 OrderID)
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
        orderline.State = OrderLine.States.Created;
        sqlConnection.Open();
        string query = @"INSERT INTO [dbo].[Orderline]( [PID], [Quantity], [OrderID], [Status]) VALUES(
        '" + selectedProduct.PID + "','" + quantity.ToString() + "','" + OrderID+ "','" + orderline.State.ToString() + "')";
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

        //selectedProduct.AmountInStorage = amountInStore - quantity; //TODO: trækkes fra i Storage, når state == packed
        Database.Instance.EditProduct(selectedProduct.PID, selectedProduct);

        return orderline;
    }
    public void NewSalesOrder(Customer customer, UInt16 companyID)
    {
        SalesOrder salesOrder = new SalesOrder();
        Console.WriteLine();
        Console.WriteLine("New sales order");

        salesOrder.CID = customer.CID;
        salesOrder.FullName = customer.ContactInfo.FullName;
        salesOrder.CompanyID = companyID;
        SqlConnection sqlConnection = new DatabaseConnection().SetSqlConnection("LNE_Security");

        DateTime randomDate = RandomDay(); // Need OrderID for Orderlines
        string query = "INSERT INTO[dbo].[SalesOrder] (OrderTime, ContactInfoID, CID, CompanyID) VALUES('" + 
            randomDate.ToString("s").Replace("T", " ") + "', '"+ customer.ContactInfoID.ToString() + "', '" +
            customer.CID.ToString() + "',' " + salesOrder.CompanyID +"')";
        
        SqlCommand cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();

        List<SalesOrder> salesOrders = GetSalesOrders(customer);
        foreach(SalesOrder so in salesOrders)
        {
            if (so.OrderTime == randomDate && salesOrder.CID == customer.CID)
            {
                salesOrder.OrderID = so.OrderID;
                salesOrder.OrderTime = DateTime.Now;
            }
                
        }
        bool Done = false;
        do
        {
            salesOrder.OrderLines.Add(NewOrderLine(salesOrder.OrderID));
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

        salesOrder.State = SalesOrder.States.Created;
        query = @"UPDATE [dbo].[SalesOrder]
            SET [OrderTime] = '" + salesOrder.OrderTime.ToString("s").Replace("T", " ") +
            
          "', [ContactInfoID] = '" + customer.ContactInfoID.ToString() +
          "', [CID] = '" + customer.CID +
          "', [CompanyID] = '" + companyID.ToString() +
          "', [Price] = '" + salesOrder.TotalPrice + 
          "', [State] = '" + salesOrder.State.ToString() + "'  WHERE OrderID = '" + salesOrder.OrderID+"'";
        cmd = new SqlCommand(query, sqlConnection);
        sqlConnection.Open();
        
        //execute the SQLCommand
        reader = cmd.ExecuteReader();
        reader.Close();

        //close connection
        sqlConnection.Close();
    }

    public OrderLine SelectOrderline(UInt16 OLID, UInt32 OrderID)
    {
        List<OrderLine> orderLines = GetOrderLines(OrderID);
        foreach(OrderLine line in orderLines)
        {
            if(line.OLID == OLID)
                return line;
        }
        Console.WriteLine("Could not find orderling with OLID: " + OLID);
        return null;
    }

    public void EditOrderlineState(UInt16 OLID, string state)
    {
        string query = @"UPDATE [dbo].[Orderline]
        SET[Status] = '" + state +
                    "' WHERE OLID =" + OLID.ToString();
        sqlConnection.Open();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);

        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();
        sqlConnection.Close();
    }

    public void EditOrderline(UInt16 OLID, OrderLine editedOrderline)
    {
        string query = @"UPDATE [dbo].[Orderline]
        SET[PID] = " + editedOrderline.PID.ToString() +
                    ",[Quantity] =" + editedOrderline.Quantity.ToString() +
                    ",[OrderID] = " + editedOrderline.OrderID.ToString() +
                    ",[Status] = '" + editedOrderline.State.ToString() +
                    "' WHERE OLID =" + OLID.ToString();
        sqlConnection.Open();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);

        SqlDataReader reader = cmd.ExecuteReader();
        reader.Close();
        sqlConnection.Close();
    }
    public void EditSalesOrder(SalesOrder editedSalesOrder)
    {
        if(editedSalesOrder.State == SalesOrder.States.Closed || editedSalesOrder.State == SalesOrder.States.Canceled)
        {
            Console.WriteLine("Cannot edit Salesorder with state: " + editedSalesOrder.ToString());
            Console.WriteLine("Press a key to return");
            Console.ReadKey();
            return;
        }
        if(editedSalesOrder != null)
        {
            UInt32 orderID = editedSalesOrder.OrderID;
            //SalesOrder editedSalesOrder = SelectSalesOrder(orderId, customer);

            if (editedSalesOrder.CompletionTime.ToString() == "01-01-0001 00:00:00")
                editedSalesOrder.CompletionTime = null;
            string query = @"UPDATE [dbo].[SalesOrder]
                SET [OrderTime] = '" + editedSalesOrder.OrderTime.ToString("s").Replace("T", " ") + "'" +
                    ", [CompletionTime] = '" + editedSalesOrder.CompletionTime.ToString() + "'" +
                //",[CID] = '" + editedSalesOrder.CID + "'" +
                ",[ContactInfoID] = '" + editedSalesOrder.ContactInfoID + "'" +
                ",[CompanyID] = '" + editedSalesOrder.CompanyID + "'" +
                ",[Price] = '" + editedSalesOrder.TotalPrice + "'" +
                ",[State] = '" + editedSalesOrder.State.ToString() +"'" +
                " WHERE OrderID = " + orderID;
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();

            //execute the SQLCommand
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();

            //close connection
            sqlConnection.Close();
        }
        
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

    public SalesOrder SelectSalesOrder(UInt32 orderID)
    {
        string query = @"SELECT * FROM [dbo].[SalesOrder]";
        query = query + "WHERE OrderID = " + orderID;

        sqlConnection.Open();
        SqlCommand cmd = new SqlCommand(query, sqlConnection);

        SqlDataReader reader = cmd.ExecuteReader();
        SalesOrder salesOrder = new SalesOrder();
        DateTime dateTime = new DateTime();
        while (reader.Read())
        {
            
            salesOrder.OrderID = Convert.ToUInt16(reader.GetValue(0).ToString());
            string dateTimeString = reader.GetValue(1).ToString();
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
                salesOrder.ContactInfoID = Convert.ToUInt16(reader.GetValue(3));
            }
            catch (InvalidCastException ex)
            {
                salesOrder.ContactInfoID = 0;
            }

            salesOrder.CID = (ushort)(Convert.ToUInt16(reader.GetValue(4)));
            salesOrder.CompanyID = (ushort)(Convert.ToUInt16(reader.GetValue(5)));
            try
            {
                salesOrder.TotalPrice = (double)Convert.ToDouble(reader.GetValue(6));
            }
            catch (InvalidCastException ex)
            {
                salesOrder.TotalPrice = 0;
            }
            try
            {
                string state = reader.GetValue(7).ToString();
                switch (state)
                {
                    case "Created":
                        salesOrder.State = SalesOrder.States.Created;
                        break;
                    case "Confirmed":
                        salesOrder.State = SalesOrder.States.Confirmed;
                        break;
                    case "Packed":
                        salesOrder.State = SalesOrder.States.Packed;
                        break;
                    case "Closed":
                        salesOrder.State = SalesOrder.States.Closed;
                        break;
                    case "Canceled":
                        salesOrder.State = SalesOrder.States.Canceled;
                        break;
                }
            }
            catch (ArgumentNullException ex)
            {
                salesOrder.State = SalesOrder.States.Error;
            }

            salesOrder.FullName = contactInfo.FullName;
        }
        reader.Close();
        sqlConnection.Close();

        return salesOrder;
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
                salesOrder.ContactInfoID = Convert.ToUInt16(reader.GetValue(3));
            }
            catch (InvalidCastException ex)
            {
                salesOrder.ContactInfoID = 0;
            }

            salesOrder.CID = (ushort)(Convert.ToUInt16(reader.GetValue(4)));
            salesOrder.CompanyID = (ushort)(Convert.ToUInt16(reader.GetValue(5)));
            try
            {
                salesOrder.TotalPrice = (double)Convert.ToDouble(reader.GetValue(6));
            }
            catch (InvalidCastException ex)
            {
                salesOrder.TotalPrice = 0;
            }
            try
            {
                string state = reader.GetValue(7).ToString();
                switch (state)
                {
                    case "Created":
                        salesOrder.State = SalesOrder.States.Created;
                        break;
                    case "Confirmed":
                        salesOrder.State = SalesOrder.States.Confirmed;
                        break;
                    case "Packed":
                        salesOrder.State = SalesOrder.States.Packed;
                        break;
                    case "Closed":
                        salesOrder.State = SalesOrder.States.Closed;
                        break;
                    case "Canceled":
                        salesOrder.State = SalesOrder.States.Canceled;
                        break;
                    case "Incomplete":
                        salesOrder.State = SalesOrder.States.Incomplete;
                        break;
                    case "Error":
                        salesOrder.State = SalesOrder.States.Error;
                        break;
                }
            }
            catch (ArgumentNullException ex)
            {
                salesOrder.State = SalesOrder.States.Error;
            }

            salesOrder.FullName = contactInfo.FullName;
            
            salesOrders.Add(salesOrder);
        }
        reader.Close();
        sqlConnection.Close();
        
        return salesOrders;
    }

    public List<SalesOrder> GetSalesOrders(UInt16 CID)
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
            dateTimeString = reader.GetValue(2).ToString();
            try
            {
                DateTime.TryParse(dateTimeString, out dateTime);
                salesOrder.CompletionTime = dateTime;
            }
            catch(Exception ex)
            {
                salesOrder.CompletionTime = null;
            }
            try
            {
                salesOrder.ContactInfoID = (ushort)(Convert.ToUInt16(reader.GetValue(3)));
            }
            catch(InvalidCastException ex)
            {
                salesOrder.ContactInfoID = 0;
            }
            try
            {
                salesOrder.CID = (ushort)(Convert.ToUInt16(reader.GetValue(4).ToString()));
            }
            catch(InvalidCastException ex)
            {
                salesOrder.CID = 0;
            }
            try
            {
                salesOrder.CompanyID = (ushort)Convert.ToDouble(reader.GetValue(5));
            }
            catch (InvalidCastException ex)
            {
                salesOrder.CompanyID = 0;
            }
            try
            {
                salesOrder.TotalPrice = Convert.ToDouble(reader.GetValue(6));
            }
            catch(InvalidCastException ex)
            {
                salesOrder.TotalPrice = 0;
            }
            try
            {
                string state = reader.GetValue(7).ToString();
                switch (state)
                {
                    case "Created":
                        salesOrder.State = SalesOrder.States.Created;
                        break;
                    case "Confirmed":
                        salesOrder.State = SalesOrder.States.Confirmed;
                        break;
                    case "Packed":
                        salesOrder.State = SalesOrder.States.Packed;
                        break;
                    case "Closed":
                        salesOrder.State = SalesOrder.States.Closed;
                        break;
                    case "Canceled":
                        salesOrder.State = SalesOrder.States.Canceled;
                        break;
                }
            }
            catch(ArgumentNullException ex)
            {
                salesOrder.State = SalesOrder.States.Error;
            }
            salesOrders.Add(salesOrder);
        }
        reader.Close();
        sqlConnection.Close();

        return salesOrders;
    }

    public List<SalesOrder> GetSalesOrders(string state)
    {
        List<SalesOrder> salesOrders = new List<SalesOrder>();

        string dateTimeString = "";
        DateTime dateTime = new DateTime();

        string query = @"SELECT * FROM [dbo].[SalesOrder]";
        query = query + " WHERE State = '" + state +"'";
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
                salesOrder.ContactInfoID = Convert.ToUInt16(reader.GetValue(3));
            }
            catch (InvalidCastException ex)
            {
                salesOrder.ContactInfoID = 0;
            }

            salesOrder.CID = (ushort)(Convert.ToUInt16(reader.GetValue(4)));
            salesOrder.CompanyID = (ushort)(Convert.ToUInt16(reader.GetValue(5)));
            try
            {
                salesOrder.TotalPrice = (double)Convert.ToDouble(reader.GetValue(6));
            }
            catch (InvalidCastException ex)
            {
                salesOrder.TotalPrice = 0;
            }

            salesOrder.FullName = contactInfo.FullName;

            salesOrders.Add(salesOrder);
        }
        reader.Close();
        sqlConnection.Close();

        return salesOrders;
    }
    private DateTime RandomDay()
    {
        Random gen = new Random();
        DateTime start = new DateTime(1995, 1, 1);
        int range = (DateTime.Today - start).Days;
        return start.AddDays(gen.Next(range));
    }
}

