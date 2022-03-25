using Xunit;
using System;

namespace LNE_Security_Test_Suite
{
    public class ProductTestCase
    {
        LNE_Security.Product product = new LNE_Security.Product();

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

    public class SalesOrderTestCase
    {
        [Fact]
        public void CalculateTotalPriceTest()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CalculateVATSTest()
        {
            throw new NotImplementedException();
        }
    }
}