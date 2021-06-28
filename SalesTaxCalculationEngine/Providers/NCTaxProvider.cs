using SalesTaxCalculationEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Security.Authentication;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using SalesTaxCalculationEngine.Services;
using SalesTaxCalculationEngine.Parsers;

namespace SalesTaxCalculationEngine.Providers
{
    /// <summary>
    /// Implementation of ITaxProvider for North Carolina sales tax calculations. 
    /// This provider "scrapes" sales tax rate data from a web page provided by the North Carolina Department of Revenue.
    /// Its success is based on the following assumptions:
    ///   1. Internet connectivity is guaranteed.
    ///   2. The provided URL remains "up" and pointing to the expected page.
    ///   3. The format of the "scraped" HTML remains constant.
    ///  
    /// Failure of any of these assumptions will result in failure of the provider to correctly bring back data and subsequently, to use it for calculations.
    /// 
    ///  An argument could be made to store the data within a source over which we have greater control (a SQL Server database, for example), thus reducing potential for failure
    ///  due to conditions beyond our control.
    ///  
    ///  However, the point here is, why use our own resources to host data when it can be accessed elsewhere for free. And plus, it's just pretty dang cool.  
    /// </summary>
    public class NCTaxProvider : ITaxProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ITaxRateService _service;

        public NCTaxProvider(IConfiguration configuration, ITaxRateService service)
        {
            _configuration = configuration;
            _service = service;
        }

        /// <summary>
        /// Generates a list of entities (county, rate) for a state. This function is async because it involves a call to remote server.
        /// </summary>
        /// <returns>List of TaxRateEntities</returns>
        public async Task<List<TaxRateEntity>> CreateTaxRateEntityListFromService()
        {
            List<TaxRateEntity> taxRateCollection = new List<TaxRateEntity>();
            try
            {
                // Get configuration values to create HttpClient object
                string url = _configuration.GetValue<string>("SalesTaxURLs:NC");
                string sslProtocols = _configuration.GetValue<string>("HttpClientConfig:SSlProtocols");
                string decompressionMethods = _configuration.GetValue<string>("HttpClientConfig:DecompressionMethods");

                string contents = await _service.GetResponseAsync(url, sslProtocols, decompressionMethods);

                // This emerged during testing. "line return" showing up as node that shifts the correct values in an unexpected way.
                // This is not an ideal way of handling this situation, but for the sake of wrapping things up, it is the most expedient solution.
                contents = contents.Replace("\r\n                                                                        ", string.Empty);

                // Parse response using HtmlAgilityPack classes
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(contents);
                var tableRows = document.DocumentNode.SelectNodes("//div[@class='field-item']//table//tr");

                // If expected HTML table containing NC tax rate data cannot be found, return empty list
                if (tableRows == null)
                    return taxRateCollection;

                // HTML table within this response has an expected format that is known ahead of time. In a more robust application, there would be additional logic implemented
                // to assure correct parsing. However, for the sake of this example application, it should be sufficient to parse explicitly around this known format.

                // Skip the first row, as it is header values
                // Additionally, only process if the childnode in which county is expected is not empty or null
                for (int i = 1; i < tableRows.Count; i++)
                {
                    var node = tableRows[i];

                    // If no child nodes, skip to the next "row"
                    if (node.ChildNodes == null)
                        continue;


                    // Most tableRows have 16 child nodes. However, some have 14 and require slightly different processing
                    var nodeParser = NodeParserFactory.Create(node.ChildNodes.Count);
                    nodeParser.ParseChildNodes(node, ref taxRateCollection);

                }

                // Clean up list by removing any "empty" entities
                taxRateCollection.RemoveAll(t => String.IsNullOrEmpty(t.County.Trim()));

                // Return list sorted alphabetically
                return taxRateCollection.OrderBy(t => t.County).ToList();
            }
            catch (Exception ex)
            {
                // I generally do not like to wrap try...catch statements around the entirety of a loop statement. In this function, we have 3 calls to "ConvertRateStringToDouble" for each
                // pass through the loop. Any of these could conceivably fail - a robust application framework design would prevent the entire application from crashing from one singular point of failure.
                // However, for the purposes of this exercise (and with the knowledge that at the time of design, all data from the service response should succeed), I feel that it is sufficient
                // to provide exception handling at an overall level.

                // I would usually create a logger service and enter exceptions here, along with a notification process to admin alerting them to an issue.
                // For the sake of this exercise, however, let's just re-throw and kick the exception back up to the caller (I know, very very bad form).
                throw ex;
            }
        }

        /// <summary>
        /// Calculates the sales tax for a provided amount. Rounds to 2 decimal places to reflect monetary values.
        ///
        /// Note: The implementation of NC sales tax rules has been greatly simplified for this exercise. In reality, there are a number of additional rules that modify or override the standard "county" 
        ///       sales tax percentage in North Carolina. For example, "Aircraft and Qualified Jet Engines" are subject to a 4.75% state rate with a maximum of $2,500 per article:
        ///       https://www.ncdor.gov/taxes-forms/sales-and-use-tax/aircraft-and-qualified-jet-engines
        ///       
        ///       If this application were intended to be released as a viable product, these additional rules must also be collected and implemented by this provider.
        /// </summary>
        /// <param name="amount">Transaction amount as double</param>
        /// <param name="rate">Sales tax amount as double</param>
        /// <returns></returns>
        public double CalculateSalesTax(double amount, double rate)
        {
            return Math.Round(amount * rate / 100, 2);
        }
    }
}
