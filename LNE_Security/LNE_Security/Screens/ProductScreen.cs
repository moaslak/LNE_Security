using LNE_Security.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security;

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

       
        Title = product.ProductName + "Product";
        Clear(this);
        productListPage.AddColumn("Product Number", "ProductNumber");
        productListPage.AddColumn("Product Name", "ProductName");
        productListPage.AddColumn("Amount In Storage", "AmountInStorage");
        productListPage.AddColumn("Company Price", "CompanyPrice");
        productListPage.AddColumn("Sales Price", "SalesPrice");
        productListPage.AddColumn("Profit In Percent", "CalculateProfit");
        Product selected = productListPage.Select();
        Console.WriteLine("Selection: " + selected.ProductName);
        Console.WriteLine("F5 - Product Details");
        Console.WriteLine("ESCAPE - Back");

        switch (Console.ReadKey().Key)
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

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Enter:
                    ProductDetailsScreen detailsScreen = new ProductDetailsScreen(product);
                    ScreenHandler.Display(detailsScreen); // kunne man kalde ProductDetailsScreen direkte i Display()? ScreenHandler.Display(new ProdcuctDetailsScreen(product)); Erklæring er måske ikke nødvendig.
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
                case ConsoleKey.F11:
                    ProductDetailsScreen detailsScreen = new ProductDetailsScreen(product);
                    ScreenHandler.Display(detailsScreen);
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
