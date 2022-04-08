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
        public ProductScreen(Company Company) : base(Company)
        {
            this.company = Company;
        }
        protected override void Draw()
        {
            Title = "Product";
            Clear(this);

            ListPage<Product> productListPage = new ListPage<Product>();
            ListPage<String> selectedList = new ListPage<String>();
            List<Product> products = Database.Instance.GetProducts();
            Product selectedProduct = new Product();
            if (products.Count > 0)
            {
                foreach (Product product in products)
                    productListPage.Add(product);

                productListPage.AddColumn("Product Number", "ProductNumber", 10);
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
            }
     
            Console.WriteLine("F1 - New product");
            Console.WriteLine("F2 - Edit product");
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
                case ConsoleKey.F2:
                    ScreenHandler.Display(new EditProductScreen(selectedProduct));
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
                case ConsoleKey.F8:
                    Database.Instance.DeleteProduct(selectedProduct.PID);
                    break;
                case ConsoleKey.F10:
                    ScreenHandler.Display(new MainMenuScreen(this.company));
                    break;
                default:
                    break;
            }
            
        }
    }
}
