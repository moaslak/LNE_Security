using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LNE_Security.Data;

namespace LNE_Security
{
    partial class Database
    {
        SqlConnection sqlConnection = new DatabaseConnection().SetSqlConnection();
        public List<SalesOrder> GetSalesOrders(Customer customer)
        {
            List<SalesOrder> salesOrders = new List<SalesOrder>();

            string dateTimeString = "";
            DateTime dateTime = new DateTime();
            string query = @"SELECT * FROM [dbo].[SalesOrder]";
            query = query + "WHERE CID = " + customer.ID;
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                SalesOrder salesOrder = new SalesOrder();
                salesOrder.OrderID = Convert.ToUInt16(reader.GetValue(0).ToString());
                dateTimeString = reader.GetValue(1).ToString();
                try
                {
                    DateTime.TryParse(dateTimeString, out dateTime);
                    salesOrder.OrderTime = dateTime;
                }
                catch (Exception ex)
                {
                    salesOrder.OrderTime = null;
                }

                salesOrder.CID = (ushort)(Convert.ToUInt16(reader.GetValue(2)));
                salesOrder.FullName = reader.GetValue(3).ToString();
                salesOrder.TotalPrice = (double)Convert.ToDouble(reader.GetValue(4));
                salesOrders.Add(salesOrder);
            }
            reader.Close();
            sqlConnection.Close();
            return salesOrders;
        }
    }
}
