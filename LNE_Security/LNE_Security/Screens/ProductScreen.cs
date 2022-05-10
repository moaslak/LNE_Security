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

        public ProductScreen(Company Company, Product Product)
        {
            this.company = Company;
            this.product = Product;
        }
        protected override void Draw()
        {
            Title = "Product";
            Clear(this);

            ListPage<Product> productListPage = new ListPage<Product>();
            ListPage<String> selectedList = new ListPage<String>();
            List<Product> products = Database.Instance.GetProducts(company);
            Product selectedProduct = new Product();

            int maxProductNumberLength = 0;
            int maxProductNameLength = 0;
            int maxAmountInStorageLength = 0;
            int maxCompanyIDLength = 0;
            if (products.Count > 0)
            {
                foreach (Product product in products)
                {
                    productListPage.Add(product);
                    if(product.ProductNumber.ToString().Length > maxProductNumberLength)
                        maxProductNumberLength = product.ProductNumber.ToString().Length;
                    if(product.ProductName.Length > maxProductNameLength)
                        maxProductNameLength = product.ProductName.Length;
                    if(product.AmountInStorage.ToString().Length > maxAmountInStorageLength)
                        maxAmountInStorageLength = product.AmountInStorage.ToString().Length;
                    if(product.CompanyID.ToString().Length > maxCompanyIDLength)
                        maxCompanyIDLength = product.CompanyID.ToString().Length;
                }
                    

                productListPage.AddColumn("Product number", "ProductNumber", ColumnLength("Product Number", maxProductNumberLength));
                productListPage.AddColumn("Product name", "ProductName", ColumnLength("Product name", maxProductNameLength));
                productListPage.AddColumn("Amount in storage", "AmountInStorage", ColumnLength("Amount in storage", maxAmountInStorageLength));
                productListPage.AddColumn("Cost price " + company.Currency.ToString(), "CostPrice", "Cost price ".Length + company.Currency.ToString().Length);
                productListPage.AddColumn("Sales price " + company.Currency.ToString(), "SalesPrice", "Sales price ".Length + company.Currency.ToString().Length);
                if (company.Role == 0)
                    productListPage.AddColumn("Company ID", "CompanyID", ColumnLength("Company ID", maxCompanyIDLength));
                if (products.Count == 0)
                    productListPage.Draw();
                else
                    selectedProduct = productListPage.Select();
            }
            if(selectedProduct != null)
            {
                Console.WriteLine("Selection : " + selectedProduct.ProductName);
                Console.WriteLine("Enter - product details");

                Console.WriteLine("F1 - New product");
                Console.WriteLine("F2 - Edit product");
                Console.WriteLine("F8 - Delete product");
                Console.WriteLine("F10 - Back");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        ScreenHandler.Display(new ProductDetailsScreen(selectedProduct, company));
                        break;
                    case ConsoleKey.F1:
                        Database.Instance.NewProduct(company);
                        break;
                    case ConsoleKey.F2:
                        ScreenHandler.Display(new EditProductScreen(selectedProduct, company));
                        break;
                    case ConsoleKey.F8:
                        Database.Instance.DeleteProduct(selectedProduct.PID, company);
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
}
