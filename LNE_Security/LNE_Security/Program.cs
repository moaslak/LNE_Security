using System;
using TECHCOOL;

namespace LNE_Security
{
    class Program
    {
        public Menu Menu
        {
            get => default;
            set
            {
            }
        }
        // MOCK DATA
        static Company company = new Company("LNE Security", "Navn Gade 1", "Denmark");
        
        public static void Main(string[] args)
        {
            

            if (company.Country == "Denmark")
                company.Currency = Company.Currencies.DKK;
            else
                company.Currency = Company.Currencies.USD;
            CompanyScreen companyScreen = new CompanyScreen(company);
            ScreenHandler.Display(companyScreen);
        }

        static private OrderLine orderLinesMockTest()
        {
            OrderLine orderLine = new OrderLine();
            Product product = new Product();
            product.ProductNumber = 1;
            product.ProductName = "string>";
            product.SalesPrice = 1;
            product.ID = 1;
            product.Description = "Test";
            product.CompanyPrice = 2;
            orderLine.Product = product;
            orderLine.Price = 2;
            orderLine.Quantity = 3;
            return orderLine;
        }

        static private void InvoiceMockTest()
        {
            SalesOrder SalesOrder = new SalesOrder();
            Customer Customer = new Customer();
            // Invoice mock test
            SalesOrder.TotalPrice = 100;
            SalesOrder.OrderID = 1;
            SalesOrder.CompletionTime = DateTime.Now.AddMonths(1);
            SalesOrder.OrderTime = DateTime.Now;
            Customer.ID = 1;
            OrderLine orderLine = orderLinesMockTest();
            SalesOrder.OrderLines.Add(orderLine);

            Invoice invoice = new Invoice(SalesOrder.OrderID, SalesOrder.OrderTime, SalesOrder.CompletionTime, SalesOrder.TotalPrice, Customer.ID);
            invoice.State = Invoice.States.Created;

            Console.WriteLine("OrderID: " + invoice.OrderID);
            Console.WriteLine("OrderTime: " + invoice.OrderTime);
            Console.WriteLine("CompletionTime: " + invoice.CompletionTime);
            Console.WriteLine("TotalPrice: " + invoice.TotalPrice);
            Console.WriteLine("State: " + invoice.State);
            Console.WriteLine("CustomerID: " + invoice.CustomerID);
            Console.WriteLine("OrderLine: " + invoice.OrderLines.ToArray().ToString());
        }
    }
}