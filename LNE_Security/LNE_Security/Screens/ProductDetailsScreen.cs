using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace LNE_Security.Screens;

public class ProductDetailsScreen : ScreenHandler
{
    Product product = new Product();

    public ProductDetailsScreen(Product Product) : base(Product)
    {
        this.product = Product;
    }

    protected override void Draw()
    {
        ListPage<Product> ProductListPage = new ListPage<Product>();
        ProductListPage.Add(product);

        Title = product.ProductName + "Product Screen";
        Clear(this);

        product.Profit = product.CalculateProfit(product.SalesPrice, product.CostPrice);
        product.ProfitPercent = product.CalculateProfitPercent(product.SalesPrice, product.CostPrice);

        ProductListPage.AddColumn("Product Number", "ProductNumber");
        ProductListPage.AddColumn("Product Name", "ProductName");
        ProductListPage.AddColumn("Description", "Description");
        ProductListPage.AddColumn("Cost Price", "CostPrice");
        ProductListPage.AddColumn("Sales Price", "SalesPrice");
        ProductListPage.AddColumn("Unit", "Unit");
        if (product.Unit != Product.Units.hours)
        {
            product.LocationString = product.Location.Location2String(product.Location);
            string loc = product.LocationString;
            ProductListPage.AddColumn("Amount In Storage", "AmountInStorage");
            ProductListPage.AddColumn("Location", "LocationString");
        }

        ProductListPage.AddColumn("Profit Percent", "ProfitPercent");
        ProductListPage.AddColumn("Profit", "Profit");
        ProductListPage.Draw();

        Console.WriteLine("F2 - Edit product");
        Console.WriteLine("F10 - Back");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.F10:
                ScreenHandler.Display(new ProductScreen(product));
                break;
            case ConsoleKey.F2:
                ScreenHandler.Display(new EditProductScreen(product));

                break;
            default:
                break;
        }
    }
}

