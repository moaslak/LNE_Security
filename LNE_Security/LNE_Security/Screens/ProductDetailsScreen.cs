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
    private Company company { get; set; }


    public ProductDetailsScreen(Product Product, Company Company) : base(Product, Company)
    {
        this.product = Product;
        this.company = Company;
    }
    public ProductDetailsScreen(Product Product) : base(Product)
    {
        this.product = Product;
    }

    protected override void Draw()
    {
        ListPage<Product> ProductListPage = new ListPage<Product>();
        ProductListPage.Add(product);

        Title = product.ProductName + " Product Screen";
        Clear(this);

        product.Profit = product.CalculateProfit(product.SalesPrice, product.CostPrice);
        product.ProfitPercent = product.CalculateProfitPercent(product.SalesPrice, product.CostPrice);

        ProductListPage.AddColumn("Product Number", "ProductNumber", "Product Number".Length);
        ProductListPage.AddColumn("Product Name", "ProductName", 20);
        ProductListPage.AddColumn("Description", "Description", 20);
        ProductListPage.AddColumn("Cost Price " + company.Currency.ToString(), "CostPrice", "Cost Price ".Length + 3);
        ProductListPage.AddColumn("Sales Price " + company.Currency.ToString(), "SalesPrice", "Sales Price ".Length + 3);
        ProductListPage.AddColumn("Unit", "Unit", 6);
        if (product.Unit != Product.Units.hours)
        {
            ProductListPage.AddColumn("Amount In Storage", "AmountInStorage", "Amount In Storage".Length);
            ProductListPage.AddColumn("Location", "LocationString", "Location".Length);
        }

        ProductListPage.AddColumn("Profit Percent", "ProfitPercent", "Profit Percent".Length);
        ProductListPage.AddColumn("Profit " + company.Currency.ToString(), "Profit", +"Profit ".Length + 3);
        ProductListPage.Draw();

        Console.WriteLine("F2 - Edit product");
        Console.WriteLine("F10 - Back");
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.F10:
                ScreenHandler.Display(new ProductScreen(company, product));
                break;
            case ConsoleKey.F2:
                ScreenHandler.Display(new EditProductScreen(product, company));

                break;
            default:
                break;
        }
    }
}

