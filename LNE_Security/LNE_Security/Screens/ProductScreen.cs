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

            ListPage<Product> productListPage = new ListPage<Product>();
            ListPage<String> selectedList = new ListPage<String>();
            List<Product> products = Database.Instance.GetProducts();
            foreach(Product product in products)
                productListPage.Add(product);

            Product selectedProduct = new Product();

            productListPage.AddColumn("Product Number", "ProductNumber",10);
            productListPage.AddColumn("Product Name", "ProductName", 25);
            productListPage.AddColumn("Amount In Storage", "AmountInStorage");
            productListPage.AddColumn("Cost Price", "CostPrice");
            productListPage.AddColumn("Sales Price", "SalesPrice");
            if (products.Count == 0)
                productListPage.Draw();
            else
                selectedProduct = productListPage.Select();

            Console.WriteLine("Selection : " + selectedProduct.ProductName);
            Console.WriteLine("Enter - product details");
            Console.WriteLine("F1 - New product");
            Console.WriteLine("F8 - Delete product");
            Console.WriteLine("F10 - Back");
            Console.WriteLine("Esc - Close app");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Enter:
                    ScreenHandler.Display(new ProductDetailsScreen(selectedProduct)); // kunne man kalde ProductDetailsScreen direkte i Display()? ScreenHandler.Display(new ProdcuctDetailsScreen(product)); Erklæring er måske ikke nødvendig.
                    break;
                case ConsoleKey.F1:
                    Database.Instance.NewProduct();
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
                case ConsoleKey.F8:
                    Database.Instance.DeleteProduct(selectedProduct.ID);
                    break;
                case ConsoleKey.F10:
                    ScreenHandler.Display(new MainMenuScreen(company));
                    break;
                default:
                    break;
            }
            
        }
    }
}
