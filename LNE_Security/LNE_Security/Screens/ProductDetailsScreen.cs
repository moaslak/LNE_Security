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

        ProductListPage.AddColumn("Product Number", "ProductNumber");
        ProductListPage.AddColumn("Product Name", "ProductName");
        ProductListPage.AddColumn("Description", "Description");
        ProductListPage.AddColumn("Cost Price", "CostPrice");
        ProductListPage.AddColumn("Sales Price", "SalesPrice");
        // TODO: Lokation af product
        // TODO: Enhed
        ProductListPage.AddColumn("Amount In Storage", "AmountInStorage");
        ProductListPage.AddColumn("Calculate Profit Percent", "CalculateProfitPercent");
        ProductListPage.AddColumn("Calculate Profit", "CalculateProfit");

        Product selected = ProductListPage.Select();

        Console.WriteLine("Selection" + selected.ProductName);
        Console.WriteLine("F1 - Back");

        ProductScreen productScreen = new ProductScreen(selected);

        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.F1:
                ScreenHandler.Display(productScreen);
                break;
            default:
                break;
        }
    }
}
