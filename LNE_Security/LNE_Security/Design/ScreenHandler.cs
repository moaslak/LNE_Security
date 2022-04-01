using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECHCOOL.UI;

namespace LNE_Security
{

    public class ScreenHandler : Screen
    {
        private Company company1;

        static Company company { get; set; }
        static Product product { get; set; }

        static ContactInfo contactInfo { get; set; }

        static Customer customer { get; set; }

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
        public Person Person { get; set; }


        public override string Title { get; set; }

        protected override void Draw()
        {

        }
    }
}
