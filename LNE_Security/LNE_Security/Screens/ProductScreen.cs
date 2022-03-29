using LNE_Security.Screens;
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
        private Company company { get; set; }

    public ProductScreen(Product product) : base(product)
    {
        this.product = product;
    }

        protected override void Draw()
        {
            Title = "Product";
            Clear(this);
            //Mock product
            Product product = new Product();
            product.ProductNumber = 1;
            product.ProductName = "Hest";
            product.AmountInStorage = 1337;
            product.CostPrice = 5;
            product.SalesPrice = 12;
            product.Location.Section = 'A';
            product.Location.Shelve = 125;
            product.Location.Row = 4;
            product.Unit = Product.Units.piece;
            double ProfitPercent = product.CalculateProfitPercent(product.SalesPrice, product.ProductNumber);

            ListPage<Product> productListPage = new ListPage<Product>();
            ListPage<String> selectedList = new ListPage<String>();
            productListPage.Add(product);
            Product selectedProduct = new Product();

            productListPage.AddColumn("Product Number", "ProductNumber");
            productListPage.AddColumn("Product Name", "ProductName");
            productListPage.AddColumn("Amount In Storage", "AmountInStorage");
            productListPage.AddColumn("Cost Price", "CostPrice");
            productListPage.AddColumn("Sales Price", "SalesPrice");
            selectedProduct = productListPage.Select();

            Console.WriteLine("Selection : " + selectedProduct.ProductName);
            Console.WriteLine("Enter - product details");
            Console.WriteLine("F10 - Back");
            Console.WriteLine("Esc - Close app");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Enter:
                    ScreenHandler.Display(new ProductDetailsScreen(selectedProduct)); // kunne man kalde ProductDetailsScreen direkte i Display()? ScreenHandler.Display(new ProdcuctDetailsScreen(product)); Erklæring er måske ikke nødvendig.
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
                case ConsoleKey.F10:
                    ProductScreen productScreen = new ProductScreen(product);
                    ScreenHandler.Display(productScreen);
                    break;
                default:
                    break;
            }
            
        }
    }
}
