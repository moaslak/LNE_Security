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

        ProductListPage.AddColumn("Product number", "ProductNumber", ColumnLength("Product Number", product.ProductNumber.ToString()));
        ProductListPage.AddColumn("Product name", "ProductName", ColumnLength("Product name", product.ProductName));
        ProductListPage.AddColumn("Description", "Description", ColumnLength("Description", product.Description));
        ProductListPage.AddColumn("Cost price " + company.Currency.ToString(), "CostPrice", ColumnLength("Cost price " + company.Currency.ToString(), product.SalesPrice.ToString()));
        ProductListPage.AddColumn("Sales price " + company.Currency.ToString(), "SalesPrice", ColumnLength("Sales Price " + company.Currency.ToString(), product.CostPrice.ToString()));
        ProductListPage.AddColumn("Unit", "Unit", ColumnLength("Unit", product.Unit.ToString()));
        if (product.Unit != Product.Units.hours)
        {
            ProductListPage.AddColumn("Amount in storage", "AmountInStorage", ColumnLength("Amount in storage", product.AmountInStorage.ToString()));
            ProductListPage.AddColumn("Location", "LocationString", ColumnLength("Location", product.LocationString));
        }

        ProductListPage.AddColumn("Profit percent", "ProfitPercent", ColumnLength("Profit Percent", product.ProfitPercent.ToString()));
        ProductListPage.AddColumn("Profit " + company.Currency.ToString(), "Profit", ColumnLength("Profit " + company.Currency.ToString(), company.Currency.ToString()));
        if (company.Role == 0)
            ProductListPage.AddColumn("Company ID", "CompanyID", ColumnLength("Company ID", product.CompanyID.ToString()));
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

