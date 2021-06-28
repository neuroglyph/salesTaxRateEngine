using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxCalculationEngine.Services
{
    /// <summary>
    /// Interface for services designed to return sales tax data
    /// </summary>
    public interface ITaxRateService
    {
        Task<string> GetResponseAsync(string url, string sslProtocols, string decompressionMethods);
    }
}
