﻿using LNE_Security.Screens;
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
        static Company lne = new Company("LNE Security", "Navn Gade", "1b", "1337", "Aalborg", "Denmark");
        static SalesOrder salesOrder = new SalesOrder();

        static List<Company> companyList = new List<Company>();
        public static void Main(string[] args)
        {

            //InvoiceMockTest(salesOrder);   
            
            foreach(Company company in companyList)
            {
                if (company.Country == "Denmark")
                    company.Currency = Company.Currencies.DKK;
                else
                    company.Currency = Company.Currencies.USD;
            }

            MainMenuScreen mainMenu = new MainMenuScreen(lne);
            ScreenHandler.Display(mainMenu);
            
        }

        private static OrderLine orderLinesMockTest()
        {
            OrderLine orderLine = new OrderLine();
            Product product = new Product();
            product.ProductNumber = 1;
            product.ProductName = "string";
            product.SalesPrice = 1;
            product.ID = 1;
            product.Description = "Test";
            product.CostPrice = 2;
            product.AmountInStorage = 3;
            orderLine.Product = product;
            orderLine.Quantity = 3;
            return orderLine;
        }

         private static void InvoiceMockTest(SalesOrder salesOrder)
        {
            
            Customer Customer = new Customer();
            // Invoice mock test

            OrderLine orderLine = orderLinesMockTest();
            salesOrder.TotalPrice = 100;
            salesOrder.OrderID = 1;
            salesOrder.CompletionTime = DateTime.Now.AddMonths(1);
            salesOrder.OrderTime = DateTime.Now;
            Customer.ID = 1;
            
            Invoice invoice = new Invoice(salesOrder.OrderID, salesOrder.OrderTime, salesOrder.CompletionTime, salesOrder.TotalPrice, Customer.ID);
            invoice.State = Invoice.States.Created;
            invoice.SalesOrder = salesOrder;
            invoice.SalesOrder.OrderLines.Add(orderLine);
            Console.WriteLine("OrderID: " + invoice.OrderID);
            Console.WriteLine("OrderTime: " + invoice.OrderTime);
            Console.WriteLine("CompletionTime: " + invoice.CompletionTime);
            Console.WriteLine("TotalPrice: " + invoice.TotalPrice);
            Console.WriteLine("State: " + invoice.State);
            Console.WriteLine("CustomerID: " + invoice.CustomerID);
            Console.ReadKey();
        }

    }
}