using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxCalculationEngine.Services
{
    /// <summary>
    /// This service began as the NC rate provider. However, "scraping" the webpage needs to be performed in an interface-implementing class so we can build it into unit testing.
    /// </summary>
    public class WebScraperService : ITaxRateService
    {
        IConfiguration _configuration;
        public WebScraperService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// "Scrapes" the webpage for tax rate data. I went back and forth on whether to pass in url, sslProtocols, and decompressionMethods as parameters. However, there
        /// may be some services created later on that do not require these - forcing them to implement a method with unnecessary parameters is not good practice. Best to keep it
        /// as abstract as possible.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetResponseAsync(string url, string sslProtocols, string decompressionMethods)
        {
           
            if (String.IsNullOrEmpty(url))
                throw new Exception("Service URL cannot be determined");

            var client = Utilities.CreateClient(url, sslProtocols, decompressionMethods);
            if (client == null)
            {
                // This is clumsy. If I had more time, I would refactor to return something meaningful to the caller without crashing the application by exception.
                throw new Exception("Unable to obtain list of tax rates.");
            }
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
