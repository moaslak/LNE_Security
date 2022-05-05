using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECHCOOL.UI;

namespace LNE_Security
{

    public class ScreenHandler : Screen
    {

        protected static Company company { get; set; }
        static Product product { get; set; }

        static ContactInfo contactInfo { get; set; }

        static Customer customer { get; set; }
        static Employee employee { get; set; }

        static List<SalesOrder> salesOrders { get; set; }

        public ScreenHandler(Company Company)
        {
            company = Company;
        }
        public ScreenHandler(Product Product)
        {
            product = Product;
        }
        public ScreenHandler(Product Product, Company Company)
        {
            product = Product;
            company = Company;
        }
        public ScreenHandler(Customer Customer, Company Company)
        {
            customer = Customer;
            company = Company;
        }

        public ScreenHandler(Employee Employee, Company Company)
        {
            employee = Employee;
            company = Company;
        }

        public ScreenHandler(Person person)
        {
            this.Person = person;
        }
        public ScreenHandler(ContactInfo contact)
        {
            contact = contactInfo;
        }
        public ScreenHandler()
        {
        }

        public ScreenHandler(Customer Customer)
        {
            customer = Customer;

        }

        public ScreenHandler(List<SalesOrder> SalesOrders)
        {
            salesOrders = SalesOrders;
        }
        public Person Person { get; set; }


        public override string Title { get; set; }

        protected override void Draw()
        {

        }

        protected int ColumnLength(string title, string data)
        {
            int length = title.Length;
            if(data.Length > length)
                length = data.Length;
            return length;
        }

        protected int ColumnLength(string title, int length)
        {
            if (length > title.Length)
                return length;
            return title.Length;
        }

        protected string GetPassword()
        {
            string pwd = "";
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Length > 0)
                    {
                        pwd = pwd.Remove(pwd.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (i.KeyChar != '\u0000') // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
                {
                    pwd = pwd + (i.KeyChar).ToString();
                    Console.Write("*");
                }
            }
            return pwd;
        }
    }
}
