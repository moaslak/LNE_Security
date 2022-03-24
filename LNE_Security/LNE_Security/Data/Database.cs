using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public class Database : Product
    {
        // TODO: implementér singleton
        public static Company Instance { get; private set; }
        Product product = new Product();
        List<Product> products = new List<Product>();
        List<Database> databases = new List<Database>();
        public Database()
        {

        }
        public Database(uint id, string productName)
        {
            product.ID = id;
            product.ProductName = productName;
            products.ForEach(p => p.ID = id);
            products.ForEach(p => p.ProductName = productName);
            
        }

        public Database Insert()
        {
            products.Add(product);
            return this;
        }
        public Database Update()
        {
            product = new Product();
            ID = product.ID;
            return Insert();
        }

        public Database Delete()
        {
            product.ID--;
            products.Remove(product);
            return this;
        }
        /*static Company()
        {
            Instance = new Company();
            
        }*/
    }
}