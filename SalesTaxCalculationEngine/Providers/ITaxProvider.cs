using SalesTaxCalculationEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxCalculationEngine.Providers
{
    /// <summary>
    /// ITaxProvider is the interface to which various tax providers shall adhere. The framework for this exercise is designed such that multiple providers can be created for multiple states.
    /// In this fashion, the framework becomes extensible beyond the initial criteria that the sales tax for a transaction in NC be calculated.
    /// A unique provider can be created for each state, providing the application a greater degree of flexibility as it matures and gives the business room to grow beyond a singular state's sales tax calculations.
    /// Implementation frees the classes to use the best (and possibly only) method to obtain its tax rate data. 
    /// Some providers may rely on a relational-database back end. Some might support reading from XML files. In this exercise, the NC tax provider will "scraped" from a web page provided by the
    /// North Carolina Department of Revenue. 
    /// </summary>
    public interface ITaxProvider
    {
        Task<List<TaxRateEntity>> CreateTaxRateEntityListFromService();
        double CalculateSalesTax(double amount, double rate);
    }
}
