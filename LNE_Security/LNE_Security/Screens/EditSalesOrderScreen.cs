using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

internal class EditSalesOrderScreen : ScreenHandler
{
    private class Options
    {
        public string Option { get; set; }
        public string Value { get; set; }
        public Options(string option, string value)
        {
            Value = value;
            Option = option;
        }
    }

    private List<SalesOrder> salesOrders;
    public EditSalesOrderScreen(List<SalesOrder> SalesOrders) : base(SalesOrders)
    {
        this.salesOrders = SalesOrders;
    }

    /// <summary>
    /// Gets orderline states
    /// </summary>
    /// <returns>state for orderlines</returns>
    private List<OrderLine.States> statesToList()
    {
        List<OrderLine.States> list = Enum.GetValues(typeof(OrderLine.States)).Cast<OrderLine.States>().ToList();
        return list;
    }

    /// <summary>
    /// Edit orderlines. Bool used to verify edit.
    /// </summary>
    /// <param name="OrderID"></param>
    /// <returns>edited list, bool</returns>
    private (List<OrderLine>, bool) EditOrderLines(UInt32 OrderID)
    {
        bool succes = false;
        List<OrderLine> orderLines = Database.Instance.GetOrderLines(OrderID);
        if (orderLines.Count == 0)
        {
            return (orderLines, succes);
        }

        ListPage<OrderLine> orderLineListPage = new ListPage<OrderLine>();

        orderLineListPage.AddColumn("OLID", "OLID");
        orderLineListPage.AddColumn("Product", "PID");
        orderLineListPage.AddColumn("Quantity", "Quantity");
        orderLineListPage.AddColumn("Status", "State");

        foreach (OrderLine orderline in orderLines)
        {
            orderline.PID = orderline.Product.PID;
            orderLineListPage.Add(orderline);
            if (orderline.State == OrderLine.States.Closed || orderline.State == OrderLine.States.Canceled)
            {
                Console.WriteLine("Cannot edit Orderline with state: " + orderline.State.ToString());
                Console.WriteLine("Press a key to return");
                Console.ReadKey();
                return (orderLines, succes);
            }
        }
        OrderLine selected = orderLineListPage.Select(); // TODO: finish edit

        ListPage<Options> OptionListPage = new ListPage<Options>();
        OptionListPage.AddColumn("Edit", "Option");
        OptionListPage.Add(new Options("OrderID", "OrderID"));
        OptionListPage.Add(new Options("Product", "Product"));
        OptionListPage.Add(new Options("Quantity", "Quantity"));
        OptionListPage.Add(new Options("Status", "State"));
        Options option = OptionListPage.Select();

        string newValue = "";
        UInt32 newUint = 0;
        double newDouble = 0;
        if (option.Option != "Status")
        {
            Console.Write("Enter new " + option.Option.ToString() + ": ");
            newValue = Console.ReadLine();

            UInt32.TryParse(newValue, out newUint);
            Double.TryParse(newValue, out newDouble);
        }

        switch (option.Option)
        {
            case "OrderID":
                selected.OrderID = newUint;
                break;
            case "Product":
                selected.PID = newUint;
                break;
            case "Quantity":
                Product product = Database.Instance.SelectProduct(selected.PID);
                Console.WriteLine("Amount in storage: " + product.AmountInStorage);
                if(product.AmountInStorage < newDouble)
                {
                    Console.WriteLine("Not enough in storage!!");
                    succes = false;
                    return (orderLines, succes);
                }
                else
                    selected.Quantity = newDouble;
                break;
            case "Status":
                List<OrderLine.States> stateList = statesToList();
                ListPage<Options> listPage = new ListPage<Options>();
                listPage.AddColumn("Status", "Option");
                foreach (OrderLine.States state in stateList)
                {
                    listPage.Add(new Options(state.ToString(), state.ToString()));
                }
                Options selectedState = listPage.Select();
                switch (selectedState.Option)
                {
                    case "Created":
                        selected.State = OrderLine.States.Created;
                        break;
                    case "Confirmed":
                        selected.State = OrderLine.States.Confirmed;
                        break;
                    case "Packed":
                        selected.State = OrderLine.States.Packed;
                        break;
                    case "Closed":
                        selected.State = OrderLine.States.Closed;
                        break;
                    case "Incomplete":
                        selected.State = OrderLine.States.Incomplete;
                        break;
                }
                break;
        }
        for (int i = 0; i < orderLines.Count; i++)
        {
            if (orderLines[i].OLID == selected.OLID)
            {
                orderLines[i] = selected;
                Database.Instance.EditOrderline(selected.OLID, selected);
            }
        } 
        succes = true;

        return (orderLines, succes);
    }

    /// <summary>
    /// Edits selected options for sales orders.
    /// Updates database
    /// </summary>
    /// <param name="selected"></param>
    /// <param name="selectedSalesOrder"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private SalesOrder EditSalesOrder(Options selected, SalesOrder selectedSalesOrder)
    {
        string newValue = "";
        if(selected.Option != "Completion Time" && selected.Option != "Orderlines" && selected.Option != "State")
        {
            Console.Write("New value: ");
            newValue = Console.ReadLine();
        }

        UInt16 newInt = 0;
        UInt16.TryParse(newValue, out newInt);
        bool success = false;
        double newDouble = 0;
        double.TryParse(newValue, out newDouble);
        switch (selected.Option)
        {
            case "State":
                List<OrderLine.States> stateList = statesToList();
                ListPage<Options> listPage = new ListPage<Options>();
                listPage.AddColumn("Status", "Option");

                if (selectedSalesOrder.State == SalesOrder.States.Packed)
                    listPage.Add(new Options("Closed", "Closed"));
                listPage.Add(new Options("Canceled", "Canceled"));
                listPage.Add(new Options("Back", "NO EDIT"));
                
                Options selectedState = listPage.Select();
                switch (selectedState.Option)
                {
                    case "Closed":
                        List<OrderLine> ols = Database.Instance.GetOrderLines(selectedSalesOrder.OrderID);
                        foreach (OrderLine ol in ols)
                        {
                            ol.PID = ol.Product.PID;
                            ol.State = OrderLine.States.Closed;
                            Database.Instance.EditOrderline(ol.OLID, ol);
                        }
                        selectedSalesOrder.State = SalesOrder.States.Closed;
                        selectedSalesOrder.CompletionTime = DateTime.Now;
                        CreateHTMLInvoice(selectedSalesOrder);

                        break;
                    case "Canceled":
                        List<OrderLine> ols2 = Database.Instance.GetOrderLines(selectedSalesOrder.OrderID);
                        
                        for(int i = 0; i < ols2.Count; i++)
                        {
                            ols2[i].PID = ols2[i].Product.PID;
                            ols2[i].State = OrderLine.States.Canceled;
                            selectedSalesOrder.OrderLines[i] = ols2[i];
                            Database.Instance.EditOrderline(ols2[i].OLID, ols2[i]);
                        }
                        selectedSalesOrder.State = SalesOrder.States.Error;

                        break;
                    case "NO EDIT":
                        break;
                }
                
                success = true;
                break;
            case "Customer Id":
                List<Customer> customers = Database.Instance.GetCustomers();
                foreach(Customer customer in customers)
                {
                    throw new NotImplementedException();
                    if(customer.CID.ToString() == selected.Value)
                    {
                        selectedSalesOrder.CID = newInt;
                        Console.WriteLine("Customer id changed");
                        success = true;
                        break;
                    }
                }
                if (!success)
                    Console.WriteLine("Could not find customer id. No entry change.");
                break;
            case "Completion Time":
                DateTime temp;
                do
                {
                    Console.Write("Enter completetion time(yyyy-mm-dd hh:mm:ss): ");
                } while (!(DateTime.TryParse(Console.ReadLine(), out temp)));
                selectedSalesOrder.CompletionTime = temp;
                success = true;
                break;
            case "Total price":
                selectedSalesOrder.TotalPrice = newDouble;
                success = true;
                break;
            case "Orderlines":
                (selectedSalesOrder.OrderLines, success) = EditOrderLines(selectedSalesOrder.OrderID);
                break;
            default:
                break;
        }
        List<OrderLine> orderLines = Database.Instance.GetOrderLines(selectedSalesOrder.OrderID);
        for(int i = 0; i < orderLines.Count; i++)
        {
            orderLines[i].Product = Database.Instance.SelectProduct(orderLines[i].Product.PID);
        }
        selectedSalesOrder.TotalPrice = selectedSalesOrder.CalculateTotalPrice(orderLines);
        for (int i = 0; i < this.salesOrders.Count; i++)
        {
            if( selectedSalesOrder.OrderLines.Count == 0)
            {
                Console.WriteLine("no orderlines in sales order");// TODO: Denne kaldes når man ænder først gang
                return selectedSalesOrder;
            }
            if (this.salesOrders[i].OrderID == selectedSalesOrder.OrderID && success)
            {
                Console.WriteLine("Sales order with orderId " + selectedSalesOrder.OrderID + " edited");
                return selectedSalesOrder;
            }
            if(this.salesOrders[i].OrderID == selectedSalesOrder.OrderID && !success)
            {
                Console.WriteLine("Sales order not edited");
                return selectedSalesOrder;
            }
            
        }
        Console.WriteLine("Could not find sales order to edit");
        
        return selectedSalesOrder;
    }

    /// <summary>
    /// Show the screen
    /// </summary>
    protected override void Draw()
    {
        Company company = Database.Instance.SelectCompany(salesOrders[0].CompanyID);
        Customer customer = new Customer();
        do
        {
            Title = "Edit sales order screen";
            Clear(this);
            ListPage<SalesOrder> SalesOrderListPage = new ListPage<SalesOrder>();

            int fullNameMaxLength = 0;

            foreach(SalesOrder salesOrder in salesOrders)
            {
                salesOrder.OrderLines = Database.Instance.GetOrderLines(salesOrder.OrderID);
                salesOrder.TotalPrice = salesOrder.CalculateTotalPrice(salesOrder.OrderLines);
                SalesOrderListPage.Add(salesOrder);
                customer = Database.Instance.SelectCustomer(salesOrder.CID);
                if(salesOrder.FullName.Length > fullNameMaxLength)
                    fullNameMaxLength = salesOrder.FullName.Length;
            }
            
            for (int i = 0; i < salesOrders.Count; i++)
            {
                salesOrders[i].OrderLines = Database.Instance.GetOrderLines(salesOrders[i].OrderID);
                for (int j = 0; j < salesOrders[i].OrderLines.Count; j++)
                {
                    salesOrders[i].OrderLines[j].PID = salesOrders[i].OrderLines[j].Product.PID;
                    salesOrders[i].OrderLines[j].Product = Database.Instance.SelectProduct(salesOrders[i].OrderLines[j].PID);
                }
                salesOrders[i].TotalPrice = salesOrders[i].CalculateTotalPrice(salesOrders[i].OrderLines);
            }
            
            SalesOrderListPage.AddColumn("ID", "OrderID", 10);
            SalesOrderListPage.AddColumn("Order time", "OrderTime", salesOrders[0].OrderTime.ToString().Length);
            SalesOrderListPage.AddColumn("Customer Id", "CID", "Customer Id".Length);
            SalesOrderListPage.AddColumn("Name", "FullName", fullNameMaxLength);
            SalesOrderListPage.AddColumn("Price " + company.Currency.ToString(), "TotalPrice", "Price ".Length + 3);
            SalesOrderListPage.AddColumn("State", "State", 9);
            SalesOrder selectedSalesOrder = SalesOrderListPage.Select();

            ListPage<Options> optionsListPage = new ListPage<Options>();

            optionsListPage.AddColumn("Edit", "Option");
            //optionsListPage.Add(new Options("Customer Id", selectedSalesOrder.CID.ToString()));
            optionsListPage.Add(new Options("Total price", selectedSalesOrder.TotalPrice.ToString()));
            optionsListPage.Add(new Options("Completion Time", selectedSalesOrder.CompletionTime.ToString()));
            optionsListPage.Add(new Options("State", selectedSalesOrder.State.ToString()));
            optionsListPage.Add(new Options("Orderlines", selectedSalesOrder.OrderLines.ToString()));

            optionsListPage.Add(new Options("Back", "NO EDIT"));
            Options selected = optionsListPage.Select();

            if (selected.Value != "NO EDIT")
            {
                selectedSalesOrder = EditSalesOrder(selected, selectedSalesOrder);
                
                if (selectedSalesOrder.OrderLines != null)
                {
                    Console.WriteLine("Press a key to update another parameter");
                    Database.Instance.EditSalesOrder(selectedSalesOrder); 
                }
            }
            else
            {
                break;
            }
            Console.WriteLine("Press ESC to return to Sales Order screen");
            //Database.Instance.EditSalesOrder(selectedSalesOrder);
            company = Database.Instance.SelectCompany(selectedSalesOrder.CompanyID);
        } while ((Console.ReadKey().Key != ConsoleKey.Escape));

        ScreenHandler.Display(new SalesOrderScreen(company, customer));
    }

    /// <summary>
    /// Creates the html for the invoices. Gets data from sales orders that get state = Closed
    /// </summary>
    /// <param name="salesOrder"></param>
    private void CreateHTMLInvoice(SalesOrder salesOrder)
    {
        //TODO: Get relative path
        string path = @"C:\Dropbox\TECHCOLLEGE\Hovedforløb_1\Repository\LNE_Security\LNE_Security\LNE_Security\Templates\Invoice.html";
        string logoPath = @"C:\Dropbox\TECHCOLLEGE\Hovedforløb_1\Repository\LNE_Security\LNE_Security\LNE_Security\Images\LNE_logo.png"; //TODO: find logos!!!
        string invoicePath = @"C:\Dropbox\TECHCOLLEGE\Hovedforløb_1\Repository\LNE_Security\LNE_Security\LNE_Security\Invoices\";
        if (!(Directory.Exists(invoicePath)))
            Directory.CreateDirectory(invoicePath);

        string html2String = File.ReadAllText(path);
        Customer customer = Database.Instance.SelectCustomer(salesOrder.CID);
        customer.CreateFullName(customer.FirstName, customer.LastName);
        ContactInfo contactInfo = Database.Instance.SelectContactInfo(customer);
        Address address = Database.Instance.SelectAddress(contactInfo);
        html2String = html2String.Replace("{OrderID}", "Sales order: " + salesOrder.OrderID.ToString());
        html2String = html2String.Replace("{customer}", contactInfo.FullName);
        html2String = html2String.Replace("{streetNumber}", address.StreetName + " " + address.HouseNumber);
        html2String = html2String.Replace("{cityZip}", address.ZipCode + " " + address.City);
        html2String = html2String.Replace("{country}", address.Country);
        html2String = html2String.Replace("{email}", contactInfo.Email);
        html2String = html2String.Replace("{phone number}", contactInfo.PhoneNumber);

        try
        {
            html2String = html2String.Replace("{logo}", logoPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Logo for invoice not found at: " + logoPath);
        }
        html2String = html2String.Replace("<!--Orderlines Place holder-->", generateHTMLOrderlies(salesOrder));
        
        html2String = html2String.Replace("{Total price}", salesOrder.TotalPrice.ToString());
        html2String = html2String.Replace("{completionTime}", salesOrder.CompletionTime.ToString());
        html2String = html2String.Replace("{packedby}", salesOrder.OrderLines[0].pickedBy.ToString()); //TODO: picked by orderline

        string stop = "";
        File.WriteAllText(invoicePath + "SalesOrder_" + salesOrder.OrderID.ToString() + "_" + salesOrder.CompletionTime.ToString().Substring(0,10) +".html", html2String);
        System.Diagnostics.Process.Start(@"C:\Program Files\Google\Chrome\Application\chrome.exe", invoicePath + "SalesOrder_" + salesOrder.OrderID.ToString() + "_" + salesOrder.CompletionTime.ToString().Substring(0, 10) + ".html");
    }

    /// <summary>
    /// generates html for orderlines in sales order.
    /// </summary>
    /// <param name="salesOrder"></param>
    /// <returns>html string</returns>
    private string generateHTMLOrderlies(SalesOrder salesOrder)
    {
        string htmlStart = "<tbody>";
        string htmlOut = "";
        string htmlEnd = "</tbody>";
        foreach (OrderLine orderLine in salesOrder.OrderLines)
        {
            string html = "<tr><td>{OLID}</td><td>{Product}</td><td>{Quantity}</td><td>{Price each}</td><td>{Sub price}</td></tr>";
            Product product = Database.Instance.SelectProduct(orderLine.Product.PID);
            html = html.Replace("{OLID}", orderLine.OLID.ToString());
            html = html.Replace("{Product}", product.ProductName);
            html = html.Replace("{Quantity}", orderLine.Quantity.ToString());
            html = html.Replace("{Price each}", product.SalesPrice.ToString());
            html = html.Replace("{Sub price}", (product.SalesPrice * orderLine.Quantity).ToString());
            htmlOut = htmlOut + html;
        }
        htmlOut = htmlStart + htmlOut + htmlEnd;
        return htmlOut;
    }
}
