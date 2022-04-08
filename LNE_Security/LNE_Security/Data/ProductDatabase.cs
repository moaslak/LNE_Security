using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LNE_Security.Data;
using System.Data.SqlClient;
using TECHCOOL.UI;

namespace LNE_Security
{
    partial class Database
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

        public void EditProduct(UInt32 ID, Product editedProduct)
        {
            SqlConnection sqlConnection = databaseConnection.SetSqlConnection();
            string query = @"UPDATE [dbo].[Product]
            SET[ProductNumber] = " + editedProduct.ProductNumber.ToString() + 
            ",[ProductName] = '" + editedProduct.ProductName + "'" +
                ",[SalesPrice] = " + editedProduct.SalesPrice.ToString().Replace(',','.') +
                ",[CostPrice] = " + editedProduct.CostPrice.ToString().Replace(',', '.') + 
                ",[AmountInStorage] = " + editedProduct.AmountInStorage.ToString().Replace(',', '.') +
                ",[LocationString] = '" + editedProduct.LocationString + "'" +
                ",[Unit] = '" + editedProduct.Unit.ToString() + "' " +
                ",[Description] = '" + editedProduct.Description.ToString() + "' WHERE Id = " + ID.ToString();
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();

            //execute the SQLCommand
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();

            //close connection
            sqlConnection.Close();
        }
        public void NewProduct()
        {
            Console.WriteLine();
            Console.WriteLine("New product");
            Product product = new Product();

            SqlConnection sqlConnection = new DatabaseConnection().SetSqlConnection();

            int number = 0;
            do
            {
                Console.Write("Enter product number: ");
            } while (!(int.TryParse(Console.ReadLine(), out number)));
            product.ProductNumber = number;
            Console.Write("Enter product name: ");
            product.ProductName = Console.ReadLine();
            double salesPrice = 0;
            do
            {
                Console.Write("Enter sales price: ");
            } while(!(double.TryParse(Console.ReadLine(), out salesPrice)));
            product.SalesPrice = salesPrice;
            double costPrice = 0;
            do
            {
                Console.Write("Enter cost price: ");
            } while (!(double.TryParse(Console.ReadLine(), out costPrice)));
            product.CostPrice = costPrice;
            double amount = 0;
            do
            {
                Console.Write("Enter amount in storage: ");
            } while (!(double.TryParse(Console.ReadLine(), out amount)));
            product.AmountInStorage = amount;
            UInt16 row = 0;
            char section = '0';
            byte shelve = 0;
            do
            {
                Console.Write("Enter row: ");
            } while (!(UInt16.TryParse(Console.ReadLine(), out row)));
            do
            {
                Console.Write("Enter section: ");
            } while (!(char.TryParse(Console.ReadLine(), out section)));
            do
            {
                Console.Write("Enter shelve: ");
            } while (!(byte.TryParse(Console.ReadLine(), out shelve)));
            product.Location.Row = row;
            product.Location.Section = section;
            product.Location.Shelve = shelve;
            product.LocationString = product.Location.Location2String(product.Location);
            List<Product.Units> units = unitsToList();

            ListPage<Options> listPage = new ListPage<Options>();
            listPage.AddColumn("Unit", "Option");
            foreach (Product.Units unit in units)
            {
                listPage.Add(new Options(unit.ToString(), unit.ToString()));
            }
            Options selected = listPage.Select();

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
                    product.Unit = Product.Units.piece;
                    break;
            }

            Console.Write("Enter product description: ");
            product.Description = Console.ReadLine();
            product.Profit = product.CalculateProfit(product.SalesPrice, product.CostPrice);
            product.ProfitPercent = product.CalculateProfitPercent(product.SalesPrice, product.CostPrice);

            string query = @"INSERT INTO [dbo].[Product]
           ([ProductNumber]
           ,[ProductName]
           ,[SalesPrice]
           ,[CostPrice]
           ,[AmountInStorage]
           ,[LocationString]
           ,[Unit]
           ,[Description]
           ,[Profit]
           ,[ProfitPersent]) ";
            string VALUES = "VALUES(" + product.ProductNumber.ToString() + ", '" + product.ProductName
                + "', " + product.SalesPrice.ToString() + ", "
                + product.CostPrice.ToString() + ", " + product.AmountInStorage.ToString()
                + ", '" + product.LocationString + "', '" + product.Unit.ToString()
                + "', '" + product.Description + "', " + product.Profit.ToString()
                + ", " + product.ProfitPercent.ToString().Replace(',','.') + ")";
           
            query = query + VALUES;
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();

            //execute the SQLCommand
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();

            //close connection
            sqlConnection.Close();
        }

        private List<Product.Units> unitsToList()
        {
            List<Product.Units> list = Enum.GetValues(typeof(Product.Units)).Cast<Product.Units>().ToList();
            return list;
        }

        public void DeleteProduct(UInt32 ID)
        {
            SqlConnection sqlConnection = databaseConnection.SetSqlConnection();
            string query = "DELETE FROM [dbo].[Product] WHERE Id = " + ID.ToString();
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();

            //execute the SQLCommand
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();

            //close connection
            sqlConnection.Close();

            if (SelectProduct(ID) == null)
                Console.WriteLine("Product with ID = " + ID + " was succesfully deleted");
            else
                Console.WriteLine("Could not find product to delete");
        }

        public Product SelectProduct(UInt32 ID)
        {
            List<Product> products = new List<Product>();
            foreach(Product product in products)
            {
                if(product.ID == ID) return product;
            }
            return null;
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            sqlConnection.Open();
            string query = "SELECT * FROM [dbo].[Product]";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            SqlDataReader reader = cmd.ExecuteReader();
            //execute the SQLCommand

            while (reader.Read()) // each loop reads a row from the query.
            {
                Product product = new Product();
                product.ID = Convert.ToUInt32(reader.GetValue(0));
                product.ProductNumber = Convert.ToInt32(reader.GetValue(1));
                product.ProductName = reader.GetValue(2).ToString();
                product.SalesPrice = Convert.ToDouble(reader.GetValue(3));
                product.CostPrice = Convert.ToDouble(reader.GetValue(4));
                product.AmountInStorage = Convert.ToDouble(reader.GetValue(5));
                product.LocationString = reader.GetValue(6).ToString();

                string unitString = reader.GetValue(7).ToString();
                switch (unitString)
                {
                    case "piece":
                        product.Unit = Units.piece;
                        break;
                    case "meter":
                        product.Unit = Units.meter;
                        break;
                    case "hours":
                        product.Unit = Units.hours;
                        break;
                    default:
                        product.Unit = Units.piece;
                        break;
                }
                product.Description = reader.GetValue(8).ToString();
                product.Profit = product.CalculateProfit(product.SalesPrice, product.CostPrice);
                product.ProfitPercent = product.CalculateProfitPercent(product.SalesPrice, product.CostPrice);
                products.Add(product);
            }

            reader.Close();

            //close connection
            sqlConnection.Close();
            return products;
            Console.WriteLine("Prees a key to continue...");
        }
    }
}
