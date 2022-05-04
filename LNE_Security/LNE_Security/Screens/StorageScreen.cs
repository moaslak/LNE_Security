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
    private bool loggedIn = false;
    private string userName { get; set;}
    private string password { get; set;}
    Employee employee { get; set; }
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
        if(loggedIn == false)
        {
            Console.WriteLine("Employee login");
            Console.WriteLine("Enter user name: ");
            userName = Console.ReadLine();

            try
            {
                employee = Database.Instance.SelectEmployee(userName);
                if(employee != null && employee.CompanyID == company.CompanyID)
                {
                    int passwordCheck = 0;
                    bool passwordConfirmed = false;
                    do
                    {
                        Console.WriteLine("Enter password");
                        if (Console.ReadLine() == employee.Password)
                        {
                            passwordConfirmed = true;
                            loggedIn = true;
                            Console.WriteLine("Login succesful");
                        }

                        else
                        {
                            Console.WriteLine("Incorrect password");
                            passwordCheck++;
                            if (passwordCheck == 3)
                                passwordConfirmed = true;
                        }

                    } while (!(passwordConfirmed));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid login credentials");
            }
        }
        if(loggedIn == true)
        {
            Title = company.CompanyName + " Storage Screen";
            
            Clear(this);
            ListPage<SalesOrder> storageListPage = new ListPage<SalesOrder>();
            Console.WriteLine("Logged in as: " + employee.UserName);
            Console.WriteLine();
            Console.WriteLine("F1 - Not comfirmed sales orders");
            Console.WriteLine("F2 - Pick order");
            Console.WriteLine("F3 - Place/Remove products");
            Console.WriteLine("F10 - To Main menu");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.F1:
                    ConfirmSalesOrders();
                    break;
                case ConsoleKey.F2:
                    Pick();
                    break;
                case ConsoleKey.F3:
                    Put();
                    break;
                case ConsoleKey.F10:
                    ScreenHandler.Display(new MainMenuScreen(company));
                    break;
            }
        }
        else
        {
            Console.WriteLine("Not logged in. Press a key to return to main menu");
            Console.ReadKey();
            ScreenHandler.Display(new MainMenuScreen(company));
        }
        
    }

    /// <summary>
    /// Confirms selected sales order that have state = Created 
    /// </summary>
    private void ConfirmSalesOrders()
    {
        List<SalesOrder> salesOrders = Database.Instance.GetSalesOrders("Created");
        ListPage<SalesOrder> salesOrdersListPage = new ListPage<SalesOrder>();
        SalesOrder selected = new SalesOrder();
        if (salesOrders.Count > 0)
        {

            int maxFullnameLength = 0;
            foreach (SalesOrder salesOrder in salesOrders)
            {
                salesOrder.FullName = (Database.Instance.SelectCustomer(salesOrder.CID)).ContactInfo.FullName;
                salesOrdersListPage.Add(salesOrder);
                if (salesOrder.FullName.Length > maxFullnameLength)
                    maxFullnameLength = salesOrder.FullName.Length;

            }
            salesOrdersListPage.AddColumn("Order ID", "OrderID", "Order ID".Length);
            salesOrdersListPage.AddColumn("Customer", "FullName", maxFullnameLength);
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
            ordersListPage.AddColumn("PID", "PID", 5);
            ordersListPage.AddColumn("Quantity", "Quantity", 6);

            Console.WriteLine("F1 - Confirm sales order");
            Console.WriteLine("F2 - Inspect orderlines");
            Console.WriteLine("F8 - Back");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.F1:
                    foreach (OrderLine orderLine in orderLines)
                        Database.Instance.EditOrderlineState(orderLine.OLID, "Confirmed");
                    selected.State = SalesOrder.States.Confirmed;
                    Database.Instance.EditSalesOrder(selected);
                    Console.WriteLine($"Sales order with OrderId: {selected.OrderID} confirmed");
                    break;
                case ConsoleKey.F2:
                    orderline = ordersListPage.Select();
                    ViewOrderline(orderline);
                    break;
                case ConsoleKey.F8:
                    break;
                default:
                    Console.WriteLine("Not a valid selection");
                    break;
            }
        }
        else
        {
            Console.WriteLine("No new sales orders");
        }
    }

    /// <summary>
    /// view selected orderline
    /// </summary>
    /// <param name="orderLine"></param>
    private void ViewOrderline(OrderLine orderLine)
    {
        Product product = Database.Instance.SelectProduct(orderLine.PID);
        ListPage<Product> productListPage = new ListPage<Product>();
        productListPage.Add(product);
        productListPage.AddColumn("Product name", "ProductName", product.ProductName.Length);
        productListPage.AddColumn("Amount in storage", "AmountInStorage", "Amount in storage".Length);
        productListPage.AddColumn("Location", "LocationString", "Location".Length);
        productListPage.Draw();
    }

    /// <summary>
    /// picks orderlines that have state = Confirmed || Incomplete. When picked, amount is regulated in database.
    /// </summary>
    private void Pick()
    {
        List<SalesOrder> salesOrders = Database.Instance.GetSalesOrders("Confirmed");
        List<SalesOrder> incompleteOrders = Database.Instance.GetSalesOrders("Incomplete");

        foreach(SalesOrder salesOrder in incompleteOrders)
        {
            salesOrders.Add(salesOrder);
        }

        ListPage<SalesOrder> salesOrderListpage = new ListPage<SalesOrder>();
        foreach (SalesOrder so in salesOrders)
        {
            so.FullName = (Database.Instance.SelectCustomer(so.CID)).ContactInfo.FullName;
            salesOrderListpage.Add(so);
        }
        if (salesOrders.Count > 0)
        {
            int maxFullnameLength = 0;
            int maxOrderIDLength = 0;
            foreach(SalesOrder so in salesOrders)
            {
                if (so.FullName.Length > maxFullnameLength)
                    maxFullnameLength = so.FullName.Length;
                if(so.OrderID.ToString().Length > maxOrderIDLength)
                    maxOrderIDLength = so.OrderID.ToString().Length;
            }

            salesOrderListpage.AddColumn("Order ID", "OrderID", ColumnLength("Order ID", maxOrderIDLength));
            salesOrderListpage.AddColumn("Customer", "FullName", ColumnLength("Customer", maxFullnameLength));
            SalesOrder salesOrder = salesOrderListpage.Select();

            List<OrderLine> orderLines = Database.Instance.GetOrderLines(salesOrder.OrderID);
            ListPage<OrderLine> listPage = new ListPage<OrderLine>();

            int count = 0;
            int maxOLIDLength = 0;
            foreach (OrderLine orderLine in orderLines)
            {
                if (orderLine.State == OrderLine.States.Confirmed || orderLine.State == OrderLine.States.Created)
                {
                    orderLine.PID = orderLine.Product.PID;
                    orderLine.Product = Database.Instance.SelectProduct(orderLine.PID);
                    listPage.Add(orderLine);
                    count++;
                    if(orderLine.OLID.ToString().Length > maxOLIDLength)
                        maxOrderIDLength=orderLine.OLID.ToString().Length;
                }
            }
            if (count > 0)
            {
                listPage.AddColumn("OLID", "OLID", ColumnLength("OLID", maxOrderIDLength));
                listPage.AddColumn("Status", "State", "Incomplete".Length);
                OrderLine selectedOrderLine = listPage.Select();

                if (selectedOrderLine.State != OrderLine.States.Packed)
                {
                    Product product = Database.Instance.SelectProduct(selectedOrderLine.PID);
                    ListPage<Product> productListPage = new ListPage<Product>();
                    productListPage.Add(product);
                    productListPage.AddColumn("PID", "PID", ColumnLength("PID", product.PID.ToString()));
                    productListPage.AddColumn("Product number", "ProductNumber", ColumnLength("Product number", product.ProductNumber.ToString()));
                    productListPage.AddColumn("Product name", "ProductName", ColumnLength("Product name", product.ProductName));
                    productListPage.AddColumn("Amount in storage", "AmountInStorage", ColumnLength("Amount in storage", product.AmountInStorage.ToString()));
                    productListPage.AddColumn("Location", "LocationString", ColumnLength("Location", product.LocationString));
                    productListPage.AddColumn("Description", "Description", ColumnLength("Description", product.Description));
                    
                    Product selectedProduct = productListPage.Select();

                    Console.WriteLine("Confirm pick? (y)es/(n)o");
                    selectedOrderLine.pickedBy = employee.UserName;
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.Y:
                            Console.WriteLine();
                            if (selectedOrderLine.Quantity > selectedOrderLine.Product.AmountInStorage)
                            {
                                Console.WriteLine("Not enough in storage. Cannot pack orderline");
                                selectedOrderLine.State = OrderLine.States.Incomplete;
                            }
                            else
                            {
                                selectedProduct.AmountInStorage = selectedProduct.AmountInStorage - selectedOrderLine.Quantity;
                                selectedOrderLine.State = OrderLine.States.Packed;
                                Database.Instance.EditProduct(selectedProduct.PID, selectedProduct);
                                Database.Instance.EditOrderline(selectedOrderLine.OLID, selectedOrderLine);
                                Console.WriteLine($"Orderline with OLID {selectedOrderLine.OLID} packed");
                            }
                            break;
                        case ConsoleKey.N:
                            Console.WriteLine();
                            Console.WriteLine("Orderline not picked");
                            break;
                        default:
                            Console.WriteLine("Orderline not picked");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Orderline is already packed");
                }
            }
            int sameStateCount = 0;
            foreach (OrderLine line in orderLines)
            {

                if (line.State == OrderLine.States.Packed)
                    sameStateCount++;
            }
            if (sameStateCount != orderLines.Count)
            {
                salesOrder.State = SalesOrder.States.Incomplete;
                Database.Instance.EditSalesOrder(salesOrder);
            }
            else
            {
                salesOrder.State = SalesOrder.States.Packed;
                Database.Instance.EditSalesOrder(salesOrder);
            }
        }
        else
            Console.WriteLine("No confirmed sales orders in database");
    }

    /// <summary>
    /// inserts products to the database
    /// </summary>
    private void Put()
    {
        List<Product> products = Database.Instance.GetProducts();
        ListPage<Product> listPageProducts = new ListPage<Product>();

        foreach(Product product in products)
        {
            listPageProducts.Add(product);
        }

        listPageProducts.AddColumn("PID", "PID");
        listPageProducts.AddColumn("Product name", "ProductName");
        listPageProducts.AddColumn("Location", "LocationString");
        listPageProducts.AddColumn("Amount in storage", "AmountInStorage");
        Product selectedProduct = listPageProducts.Select();

        Console.WriteLine("Insert on a new location? (y)es/(n)o)");
        bool newLoc = false;
        switch (Console.ReadKey().Key)
        {
            
            case ConsoleKey.Y:
                newLoc = true;
                UInt16 row = 0;
                char section = '0';
                byte shelve = 0;
                Console.WriteLine();
                Console.WriteLine("New location");
                do
                {
                    Console.Write("Enter row: ");
                } while (!(UInt16.TryParse(Console.ReadLine(), out row)));
                do
                {
                    Console.Write("Enter section: ");
                } while (!(char.TryParse(Console.ReadLine(), out section)));
                do
                {
                    Console.Write("Enter shelve: ");
                } while (!(byte.TryParse(Console.ReadLine(), out shelve)));
                selectedProduct.Location.Section = section;
                selectedProduct.Location.Shelve = shelve;
                selectedProduct.Location.Row = row;
                selectedProduct.LocationString = selectedProduct.Location.Location2String(selectedProduct.Location);
                Database.Instance.NewProduct(selectedProduct);
                Console.WriteLine("Location: " + selectedProduct.LocationString);
                break;
            case ConsoleKey.N:
                Console.WriteLine();
                Console.WriteLine("Location: " + selectedProduct.LocationString);
                break;
            default:
                Console.WriteLine();
                Console.WriteLine("Location: " + selectedProduct.LocationString);
                break;
        }

        char choice = '0';
        if (!newLoc)
        {
            do
            {
                Console.WriteLine();
                Console.WriteLine("(A)dd or (R)emove items?");
                Console.WriteLine();
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.A:
                        choice = 'a';
                        break;
                    case ConsoleKey.R:
                        choice = 'r';
                        break;
                    default:
                        break;
                }
            } while (!(choice.Equals('a') || choice.Equals('r')));

            if (choice == 'a')
            {
                double add = 0;
                do
                {
                    Console.WriteLine();
                    Console.Write("Add: ");
                } while (!(double.TryParse(Console.ReadLine(), out add)));
                selectedProduct.AmountInStorage = selectedProduct.AmountInStorage + add;
                Database.Instance.EditProduct(selectedProduct.PID, selectedProduct);
                Console.WriteLine(add + " " + selectedProduct.ProductName + " added to location: " + selectedProduct.LocationString);
            }
            if (choice.Equals('r'))
            {
                double remove = 0;
                do
                {
                    do
                    {
                        Console.WriteLine();
                        Console.WriteLine("Amount in storage: " + selectedProduct.AmountInStorage);
                        Console.Write("Remove: ");
                    } while (!(double.TryParse(Console.ReadLine(), out remove)));
                } while (remove > selectedProduct.AmountInStorage);
                selectedProduct.AmountInStorage = selectedProduct.AmountInStorage - remove;
                Database.Instance.EditProduct(selectedProduct.PID, selectedProduct);
                Console.WriteLine(remove + " " + selectedProduct.ProductName + " removed to location: " + selectedProduct.LocationString);
            }
        }
        else
        {
            double add = 0;
            do
            {
                Console.Write("Add: ");
            } while (!(double.TryParse(Console.ReadLine(), out add)));
            selectedProduct.AmountInStorage = selectedProduct.AmountInStorage + add;
            Database.Instance.EditProduct(selectedProduct.PID, selectedProduct);
            Console.WriteLine(add + " " + selectedProduct.ProductName + " added to location: " + selectedProduct.LocationString);
        }
    } 
}
