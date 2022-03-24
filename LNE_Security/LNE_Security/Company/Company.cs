using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public class Company
    {
        public Storage Storage
        {
            get => default;
            set
            {
            }
        }

        public Sales Sales
        {
            get => default;
            set
            {
            }
        }

        public Database Database
        {
            get => default;
            set
            {
            }
        }

        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string StreetName { get; set; }
        public enum Currencies { DKK, USD, blank }
        public Currencies Currency { get; set; }
        public string CVR
        {
            get => default;
            set
            {
            }
        }
        

        public string HouseNumber
        {
            get => default;
            set
            {
            }
        }

        public string ZipCode
        {
            get => default;
            set
            {
            }
        }

        public string City
        {
            get => default;
            set
            {
            }
        }

        public Company(string companyName, string streetName, string country)
        {
            CompanyName = companyName;
            StreetName = streetName;
            Country = country;
        }
        public Company(string companyName)
        {
            CompanyName = companyName;
        }
    }
}