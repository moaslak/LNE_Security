using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security
{
    public class ProductScreen : ScreenHandler
    {
        private Product product { get; set; }

        public ProductScreen(Product product) : base(product)
        {
            this.product = product;
        }

        protected override void Draw()
        {
            ListPage<Product> productListPage = new ListPage<Product>();
            ListPage<String> selectedList = new ListPage<String>();
            productListPage.Add(product);
            Product selectedProduct = new Product();

            do
            {
                Title = "Product";
                Clear(this);
                productListPage.AddColumn("Product Number", "ProductNumber");
                productListPage.AddColumn("Product Name", "ProductName");
                productListPage.AddColumn("Amount In Storage", "AmountInStorage");
                productListPage.AddColumn("Company Price", "CompanyPrice");
                productListPage.AddColumn("Sales Price", "SalesPrice");
                productListPage.AddColumn("Profit In Percent", "CalculateProfit");
            } while (!(Console.ReadKey().Key == ConsoleKey.Enter));

            switch (selectedProduct.ProductName)
            {
                case "Sweet Pickles":
                    Console.WriteLine("Selected: " + selectedProduct.ProductName);
                    break;
                case "Back":
                    Environment.Exit(0);
                    break;
            }
        }



    }
}
