using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LNE_Security.Data;
using System.Data.SqlClient;

namespace LNE_Security
{
    partial class Database
    {
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
                product.ProductNumber = Convert.ToInt32(reader.GetValue(0));
                product.ProductName = reader.GetValue(1).ToString();
                product.SalesPrice = Convert.ToDouble(reader.GetValue(2));
                product.CostPrice = Convert.ToDouble(reader.GetValue(3));
                product.AmountInStorage = Convert.ToDouble(reader.GetValue(4));
                product.LocationString = reader.GetValue(5).ToString();

                string unitString = reader.GetValue(6).ToString();
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
                product.Description = reader.GetValue(7).ToString();
                product.Profit = product.CalculateProfit(product.SalesPrice, product.CostPrice);
                product.ProfitPercent = product.CalculateProfitPercent(product.SalesPrice, product.CostPrice);
                products.Add(product);
            }

            reader.Close();

            //close connection
            sqlConnection.Close();
            return products;
        }
    }
}
