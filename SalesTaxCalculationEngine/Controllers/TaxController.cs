using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalesTaxCalculationEngine.Enums;
using SalesTaxCalculationEngine.Models;
using SalesTaxCalculationEngine.Providers;

namespace SalesTaxCalculationEngine.Controllers
{
    /// <summary>
    /// Main controller for application. I am taking the approach of creating a framework that will be extensible beyond just the state of NC. 
    /// In this way, should the opportunity present itself down the line, additional "regional" states can be introduced into the application.
    /// In order to achieve this, a factory class is used to generate the appropriate provider for a specified state (provided through the service as parameter).
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TaxController : ControllerBase
    {
        private readonly IProviderFactory _providerFactory;
        private readonly IConfiguration _configuration;

        public TaxController(IProviderFactory providerFactory, IConfiguration configuration)
        {
            _providerFactory = providerFactory;
            _configuration = configuration;
        }

        [HttpPost("calculateSalesTax")]
        public async Task<ActionResult> CalculateSalesTax([FromQuery] string state,[FromQuery] string county,[FromQuery] double transactionAmount)
        {
            var validationMessages = new ModelStateDictionary();

            // I would not generally just wrap everything in a try...catch but it's getting late - for the purposes of this exercise, should suffice.
            try
            { 
                // Does "state" parameter exist in the enum "States"?
                States parsedState;
                if (!Enum.TryParse<States>(state, true, out parsedState))
                {
                    validationMessages.AddModelError("Error", String.Format("'{0}' is not a valid state or is not currently supported at this time.", state));
                    return NotFound(validationMessages);
                }

                // Create the appropriate state's provider (if exists)
                var taxProvider = _providerFactory.Create(parsedState);
                if (taxProvider == null)
                {
                    validationMessages.AddModelError("Error", String.Format("'{0}' is not a valid state or is not currently supported at this time.", state));
                    return NotFound(validationMessages);
                }

                var rateEntities = await taxProvider.CreateTaxRateEntityListFromService();

                // Here is a potential for error. This controller action provides a parameterized string for "county", which needs to match up to
                // the value contained in the tax provider's list. Should our intrepid user fail to specific the county correctly in the query, 
                // it will not be possible to determine the correct sales tax rate to use and the application will not perform as expected.
                var countyToUse = rateEntities.FirstOrDefault(r => r.County.ToUpper() == county.ToUpper());
                if (countyToUse == null)
                {
                    validationMessages.AddModelError("Error", String.Format("'{0}' is not a valid county or is not currently supported at this time.", county));
                    return NotFound(validationMessages);
                }

                var salesTaxTotal = taxProvider.CalculateSalesTax(transactionAmount, countyToUse.Rate);

                var response = new TaxResponseModel
                {
                    State = state,
                    County = county,
                    TransactionAmount = transactionAmount,
                    SalesTaxTotal = salesTaxTotal
                };

                var result = new JsonResult(response);
                return result;   
            }
            catch(Exception ex)
            {
                // Super generic "Internal server error" response
                return StatusCode(500);
            }
        }
    }
}
