using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using static LNE_Security.Screens.EditCompnayScreen;

namespace LNE_Security.Screens
{
public class EditProductScreen : ScreenHandler
{
        public class Options
        {
            public string Option { get; set; }
            public string Value { get; set; }
            public Options(string option, string value)
            {
                Value = value;
                Option = option;
            }
        }
    private Product product;
        public EditProductScreen(Product Product) : base(Product)
        {
            this.product = Product;
        }

        private List<Product.Units> UnitsToList()
        {
            List<Product.Units> list = Enum.GetValues(typeof(Product.Units)).Cast<Product.Units>().ToList();
            return list;
        }

    private void EditProduct(Options selected)
    {
        string newValue = "";
            UInt16 newInt = 0;
            double newDouble = 0;
            if (selected.Option == "Unit")
            {
                List<Product.Units> units = UnitsToList();

                ListPage<Options> listPage = new ListPage<Options>();
                listPage.AddColumn("Unit", "Option");
                foreach (Product.Units unit in units)
                {
                    listPage.Add(new Options(unit.ToString(), unit.ToString()));
                }
                selected = listPage.Select();

                switch (selected.Value)
                {
                    case "piece":
                        product.Unit = Product.Units.piece;
                        break;
                    case "meter":
                        product.Unit = Product.Units.meter;
                        break;
                    case "hours":
                        product.Unit = Product.Units.hours;
                        break;
                    default:
                        break;
                }
            }
            if(selected.Option == "Location")
            {
                char newChar = '0';
                do
                {
                    Console.Write("Enter row(postive number): ");
                } while (!(UInt16.TryParse(Console.ReadLine(), out newInt)));
                product.Location.Row = newInt;
                do
                {
                    do
                    {
                        Console.Write("Enter section(letter): ");
                    } while (!(Char.TryParse(Console.ReadLine(), out newChar)) && !(Char.IsLetter(newChar)));
                    product.Location.Section = newChar;
                }while (!(Char.IsLetter(newChar)));
                do
                {
                    Console.Write("Enter shelve(postive number): ");
                } while (!(UInt16.TryParse(Console.ReadLine(), out newInt)));
                product.Location.Shelve = Convert.ToByte(newInt);
                product.LocationString = product.Location.Location2String(product.Location);
            }
            else
            {
                Console.Write("New value: ");
                newValue = Console.ReadLine();
                UInt16.TryParse(newValue, out newInt);
                Double.TryParse(newValue, out newDouble);
            }

        if (newValue == null)
            newValue = "";

            // location og unit mangler
        switch (selected.Option)
        {
                case "Product Name":
                this.product.ProductName = newValue;
                break;
                case "Product Number":
                    this.product.ProductNumber = newInt;
                break;
                case "Sales Price":
                    this.product.SalesPrice = newDouble;
                break;
                case "Cost Price":
                    this.product.CostPrice = newDouble;
                break;
                case "Amount In Storage":
                    this.product.AmountInStorage = newDouble;
                break;
                case "Description":
                    this.product.Description = newValue;
                break;
            default:
                break;
            }
        }       
        protected override void Draw()
        {
            
            do
            {
                ListPage<Product> ProductListPage = new ListPage<Product>();
                ProductListPage.Add(product);

                Title = product.ProductName + " Product Screen";
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

                ListPage<Options> optionsListPage = new ListPage<Options>();

                optionsListPage.AddColumn("Edit", "Option");
                optionsListPage.Add(new Options("Product Number", product.ProductNumber.ToString()));
                optionsListPage.Add(new Options("Product Name", product.ProductName));
                optionsListPage.Add(new Options("Description", product.Description));
                optionsListPage.Add(new Options("Cost Price", product.CostPrice.ToString()));
                optionsListPage.Add(new Options("Sales Price", product.SalesPrice.ToString()));
                optionsListPage.Add(new Options("Unit", product.Unit.ToString()));
                if (product.Unit != Product.Units.hours)
                {
                    optionsListPage.Add(new Options("Amount In Storage", product.AmountInStorage.ToString()));
                    optionsListPage.Add(new Options("Location", product.LocationString));
                }   
                optionsListPage.Add(new Options("Back", "NO EDIT"));
                Options selected = optionsListPage.Select();

                if (selected.Value != "NO EDIT")
                {
                    EditProduct(selected);
                    Console.WriteLine("Press a key to update another parameter"); // TODO: Denne skal gerne væk
                }
                else
                {
                    break;
        }
                Console.WriteLine("Press ESC to return to Product screen");

            } while ((Console.ReadKey().Key != ConsoleKey.Escape));

            ScreenHandler.Display(new ProductDetailsScreen(product));

        }
    }
}