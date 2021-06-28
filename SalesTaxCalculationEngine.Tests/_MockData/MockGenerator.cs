using Microsoft.Extensions.Configuration;
using Moq;
using SalesTaxCalculationEngine.Enums;
using SalesTaxCalculationEngine.Providers;
using SalesTaxCalculationEngine.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SalesTaxCalculationEngine.Tests
{
    public class MockGenerator
    {
        private static readonly Assembly _currentAssembly = Assembly.GetExecutingAssembly();

        public static Mock<IConfiguration> CreateConfigurationMock()
        {
            Mock<IConfigurationSection> mockUrlSection = new Mock<IConfigurationSection>();
            mockUrlSection.Setup(x => x.Value).Returns("http://www.someurl.com");

            Mock<IConfigurationSection> mockSSlProtocolsSection = new Mock<IConfigurationSection>();
            mockSSlProtocolsSection.Setup(x => x.Value).Returns("Tls12|Tls11|Tls");

            Mock<IConfigurationSection> mockDecompressionMethodsSection = new Mock<IConfigurationSection>();
            mockDecompressionMethodsSection.Setup(x => x.Value).Returns("GZip|Deflate");


            Mock <IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "SalesTaxURLs:NC"))).Returns(mockUrlSection.Object);
            mockConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "HttpClientConfig:SSlProtocols"))).Returns(mockSSlProtocolsSection.Object);
            mockConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "HttpClientConfig:DecompressionMethods"))).Returns(mockDecompressionMethodsSection.Object);

            return mockConfig;
        }

        public static Mock<IProviderFactory> CreateMockProviderFactory()
        {
            List<Models.TaxRateEntity> taxRateEntities = new List<Models.TaxRateEntity> {
                new Models.TaxRateEntity { County = "Wake", Rate = 7.5 }
            };

            Mock<ITaxProvider> mockTaxProvider = new Mock<ITaxProvider>();
            mockTaxProvider.Setup(x => x.CreateTaxRateEntityListFromService()).ReturnsAsync(taxRateEntities);
            mockTaxProvider.Setup(x => x.CalculateSalesTax(It.IsAny<double>(), It.IsAny<double>())).Returns(1.75);

            Mock<IProviderFactory> mockProviderFactory = new Mock<IProviderFactory>();
            mockProviderFactory.Setup(x => x.Create(It.IsAny<States>())).Returns(mockTaxProvider.Object);



            return mockProviderFactory;
        }

        public static Mock<ITaxRateService> CreateServiceMockWithExpectedResponse()
        {
            var resourceName = "SalesTaxCalculationEngine.Tests._EmbeddedResources.NCTaxRatePageValid.html";
            string contents;

            using (Stream stream = _currentAssembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    contents = reader.ReadToEnd();
                }
            }

            Mock<ITaxRateService> rateServiceMock = new Mock<ITaxRateService>();
            rateServiceMock.Setup(x => x.GetResponseAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(contents);
            return rateServiceMock;
        }

        public static Mock<ITaxRateService> CreateServiceMockWithUnExpectedResponse()
        {
            var resourceName = "SalesTaxCalculationEngine.Tests._EmbeddedResources.NCTaxRatePageInvalid.html";
            string contents;

            using (Stream stream = _currentAssembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    contents = reader.ReadToEnd();
                }
            }

            Mock<ITaxRateService> rateServiceMock = new Mock<ITaxRateService>();
            rateServiceMock.Setup(x => x.GetResponseAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(contents);
            return rateServiceMock;
        }
    }
}
