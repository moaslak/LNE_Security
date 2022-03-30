using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECHCOOL.UI;

namespace LNE_Security
{

    public class ScreenHandler : Screen
    {
        static Person company { get; set; }
        static Product product { get; set; }
        public ScreenHandler(Person Company)
        {
            company = Company;
        }
        public ScreenHandler(Product Product)
        {
            product = Product;
        }
        public ScreenHandler(Product Product, Person Company)
        {
            product = Product;
            company = Company;
        }


        public Person Person { get; set; }


        public override string Title { get; set; }

        protected override void Draw()
        {

        }
    }
}
