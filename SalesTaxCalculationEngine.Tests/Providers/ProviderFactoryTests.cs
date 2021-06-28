using Microsoft.Extensions.Configuration;
using Moq;
using SalesTaxCalculationEngine.Enums;
using SalesTaxCalculationEngine.Providers;
using SalesTaxCalculationEngine.Services;
using Xunit;

namespace SalesTaxCalculationEngine.Tests.Providers
{
    /// <summary>
    /// Test cases for the ProviderFactory class. 
    /// As there is only one function, "Create" the number of tests are limited.
    /// </summary>
    public class ProviderFactoryTests 
    {
        private readonly Mock<IConfiguration> mockConfig = MockGenerator.CreateConfigurationMock();
        private readonly Mock<ITaxRateService> mockService = MockGenerator.CreateServiceMockWithExpectedResponse();

        [Theory]
        [InlineData(States.NC)]
        public void ProviderFactory_Create_ValidStateEnum_ReturnsProvider(States state)
        {
            var providerFactory = new ProviderFactory(mockConfig.Object, mockService.Object);
            var entity = providerFactory.Create(state);

            Assert.NotNull(entity);
        }

        [Theory]
        [InlineData(States.Va)]
        public void ProviderFactory_Create_InValidStateEnum_ReturnsNull(States state)
        { 
            var providerFactory = new ProviderFactory(mockConfig.Object, mockService.Object);
            var entity = providerFactory.Create(state);

            Assert.Null(entity);
        }
    }
}
