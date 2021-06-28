using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SalesTaxCalculationEngine.Controllers;
using SalesTaxCalculationEngine.Models;
using SalesTaxCalculationEngine.Providers;
using SalesTaxCalculationEngine.Services;
using System.Threading.Tasks;
using Xunit;

namespace SalesTaxCalculationEngine.Tests.Controllers
{   
    public class TaxCollectorTests
    {
        private readonly Mock<IConfiguration> mockConfig = MockGenerator.CreateConfigurationMock();
        private readonly Mock<IProviderFactory> mockProviderFactory = MockGenerator.CreateMockProviderFactory();

        [Theory]
        [InlineData("Wyoming", "Wake", 500)]
        public async Task CalculateSalesTax_UnknownState_Returns_NotFoundObjectResult(string state, string county, double transactionAmount)
        {
            var controller = new TaxController(mockProviderFactory.Object, mockConfig.Object);

            var result = await controller.CalculateSalesTax(state, county, transactionAmount);

            var viewResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Theory]
        [InlineData("NC", "InvalidCounty", 45.00)]
        public async Task CalculateSalesTax_UnknownCounty_Returns_StatusCode500(string state, string county, double transactionAmount)
        {
            var controller = new TaxController(mockProviderFactory.Object, mockConfig.Object);

            var result = await controller.CalculateSalesTax(state, county, transactionAmount);

            var viewResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Theory]
        [InlineData("NC", "Wake", 45.00)]
        public async Task CalculateSalesTax_ValidParameters_Returns_StatusCode500(string state, string county, double transactionAmount)
        {
            var controller = new TaxController(mockProviderFactory.Object, mockConfig.Object);

            var result = await controller.CalculateSalesTax(state, county, transactionAmount);

            var viewResult = Assert.IsType<JsonResult>(result);
            Assert.NotNull(viewResult.Value);
            var model = Assert.IsType<TaxResponseModel>(viewResult.Value);
            Assert.Equal(county, model.County);
            Assert.Equal(transactionAmount, model.TransactionAmount);
            Assert.Equal(1.75, model.SalesTaxTotal); // assigned in MockGenerator.CreateMockProviderFactory
        }
    }
}
