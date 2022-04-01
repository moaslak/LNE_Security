using Xunit;
using System;
using LNE_Security;
using LNE_Security.Screens;

namespace LNE_Security_Test_Suite
{
    public class EditCompanyTestCase
    {
        static Person company = new Person("LNE Security", "Navn Gade", "1b", "1337", "Aalborg", "Denmark");
        EditCompnayScreen editCompanyScreen = new EditCompnayScreen(company);
        EditCompnayScreen.Options options = new EditCompnayScreen.Options("Option", "Value");

        [Fact]
        public void EditCompanyNameTest()
        {
            Assert.Equal("LNE Security", company.CompanyName);
            options.Option = "Company name";
            options.Value = "asd";
            editCompanyScreen.EditCompanyTesting(options);
            Assert.Equal("asd", company.CompanyName);
        }

        [Fact]
        public void EditStreetNameTest()
        {
            Assert.Equal("Navn Gade", company.StreetName);
            options.Option = "Street name";
            options.Value = "asd";
            editCompanyScreen.EditCompanyTesting(options);
            Assert.Equal("asd", company.StreetName);
        }

        [Fact]
        public void EditHouseNumberTest()
        {
            Assert.Equal("1b", company.HouseNumber);
            options.Option = "House number";
            options.Value = "asd";
            editCompanyScreen.EditCompanyTesting(options);
            Assert.Equal("asd", company.HouseNumber);
        }

        [Fact]
        public void EditZipCodeTeset()
        {
            Assert.Equal("1337", company.ZipCode);
            options.Option = "Zip code";
            options.Value = "asd";
            editCompanyScreen.EditCompanyTesting(options);
            Assert.Equal("asd", company.ZipCode);
        }

        [Fact]
        public void EditCityTest()
        {
            Assert.Equal("Aalborg", company.City);
            options.Option = "City";
            options.Value = "asd";
            editCompanyScreen.EditCompanyTesting(options);
            Assert.Equal("asd", company.City);
        }

        [Fact]
        public void EditCountryTest()
        {
            Assert.Equal("Denmark", company.Country);
            options.Option = "Country";
            options.Value = "asd";
            editCompanyScreen.EditCompanyTesting(options);
            Assert.Equal("asd", company.Country);
        }

        [Fact]
        public void EditCurrencyTest()
        {
            Assert.Equal("DKK", company.Currency.ToString());
            options.Option = "Currency";
            options.Value = "USD";
            editCompanyScreen.EditCompanyTesting(options);
            Assert.Equal("USD", company.Currency.ToString());
        }
    }
    public class ProductTestCase
    {
        Product product = new Product();

        [Fact]
        public void CalculateProfitTest()
        {
            Assert.Equal(0, product.CalculateProfit(10, 10));
            Assert.Equal(5, product.CalculateProfit(10, 5));
            Assert.Equal(-5, product.CalculateProfit(5, 10));
        }

        [Fact]
        public void CalculateProfitPercentTest()
        {
            Assert.Equal(100, product.CalculateProfitPercent(10, 10));
            Assert.Equal(0, product.CalculateProfitPercent(10, 0));
            Assert.Equal(0, product.CalculateProfitPercent(0, 10));
            Assert.Equal(1000, product.CalculateProfitPercent(10, 1));
            Assert.Equal(200, product.CalculateProfitPercent(2, 1));
            Assert.Equal(100, product.CalculateProfitPercent(1, 1));
            Assert.Equal(300, product.CalculateProfitPercent(3, 1));
            Assert.Equal(325, product.CalculateProfitPercent(3.25, 1));
            Assert.Equal(90, product.CalculateProfitPercent(0.9,1));
            Assert.Equal(0, product.CalculateProfitPercent(-1, 1));
            Assert.Equal(0, product.CalculateProfitPercent(1, -1));
        }
    }

    
    public class OrderLineTestCase
    {
        OrderLine orderLine = new OrderLine();
        
        [Fact]
        public void CalculateLinePriceTest()
        {
            orderLine.Product.SalesPrice = 10;
            orderLine.Quantity = 1;
            Assert.Equal(10, orderLine.CalculateLinePrice(orderLine.Product));
            orderLine.Quantity = 10;
            Assert.Equal(100, orderLine.CalculateLinePrice(orderLine.Product));
            orderLine.Quantity = 0;
            Assert.Equal(0, orderLine.CalculateLinePrice(orderLine.Product));
            orderLine.Quantity = 10;
            orderLine.Product.SalesPrice = -10;
            Assert.Equal(0, orderLine.CalculateLinePrice(orderLine.Product));

            orderLine.Product = null;
            Assert.Equal(0, orderLine.CalculateLinePrice(orderLine.Product));
        }
    }

    public class SalesOrderTestCase
    {
        SalesOrder salesOrder = new SalesOrder();
        OrderLine orderLine = new OrderLine();
        [Fact]
        public void CalculateTotalPriceTest()
        {
            OrderLine orderLine2 = new OrderLine();
            orderLine2.Product.SalesPrice = 10;
            orderLine2.Quantity = 10;

            orderLine.Product.SalesPrice = 10;
            orderLine.Quantity = 1;
            salesOrder.OrderLines.Add(orderLine);
            Assert.Equal(10, salesOrder.CalculateTotalPrice(salesOrder.OrderLines));

            
            salesOrder.OrderLines.Add(orderLine2);
            Assert.Equal(110, salesOrder.CalculateTotalPrice(salesOrder.OrderLines));
            orderLine2.Product.SalesPrice = -1;
            Assert.Equal(0, salesOrder.CalculateTotalPrice(salesOrder.OrderLines));
        }

        [Fact]
        public void CalculateVATSTest()
        {
            orderLine.Product.SalesPrice = 10;
            orderLine.Quantity = 1;
            Assert.Equal(12.5, salesOrder.CalculateVATS(1.25, 10));
        }
    }

    public class CompanyTest
    {
        [Fact]
        public void NewCompanyTest()
        {
            throw new NotImplementedException();
        } 
    }

    public class PersonTest
    {
        [Fact]
        public void NewPersonTest()
        {
            throw new NotImplementedException();
        }
        [Fact]
        public void UpdatePersonTest()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetPersonTest()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void DeletePersonTest()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CombineNamesTest()
        {
            throw new NotImplementedException();
        }
    }

    public class SalesTest
    {
        [Fact]
        public void NewOrderTest()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetOrderTest()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetAllOrdersTest()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void UpdateOrderTest()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void DeleteOrderTest()
        {
            throw new NotImplementedException();
        }
    }

    public class StorageTest
    {
        [Fact]
        public void PickTest()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void PutTest()
        {
            throw new NotImplementedException();
        }
    }
}