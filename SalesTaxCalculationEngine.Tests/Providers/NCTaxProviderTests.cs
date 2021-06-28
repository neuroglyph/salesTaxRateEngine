using Microsoft.Extensions.Configuration;
using Moq;
using SalesTaxCalculationEngine.Providers;
using SalesTaxCalculationEngine.Services;
using Xunit;

namespace SalesTaxCalculationEngine.Tests.Providers
{
    public class NCTaxProviderTests
    {
        private readonly Mock<IConfiguration> mockConfig = MockGenerator.CreateConfigurationMock();
   
        /// <summary>
        /// Test must be async, as testing an async method
        /// Yay, expected HTML response
        /// </summary>
        [Fact]
        public async void CreateTaxRateEntityListFromService_PopulatesBasedOnExpectedResponse()
        {
            Mock<ITaxRateService> mockService = MockGenerator.CreateServiceMockWithExpectedResponse();
            var ncTaxProvider = new NCTaxProvider(mockConfig.Object, mockService.Object);
            var entities = await ncTaxProvider.CreateTaxRateEntityListFromService();
            Assert.NotNull(entities);
            Assert.True(entities.Count == 100);
           
        }

        /// <summary>
        /// Test must be async, as testing an async method
        /// Oops! The page content changed, did we handle it gracefully?
        /// </summary>
        [Fact]
        public async void CreateTaxRateEntityListFromService_PopulatesBasedOnUnexpectedResponse()
        {
            Mock<ITaxRateService> mockService = MockGenerator.CreateServiceMockWithUnExpectedResponse();
            var ncTaxProvider = new NCTaxProvider(mockConfig.Object, mockService.Object);
            var entities = await ncTaxProvider.CreateTaxRateEntityListFromService();
            Assert.NotNull(entities);
            Assert.True(entities.Count == 0);

        }
    }
}
