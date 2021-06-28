using Microsoft.Extensions.Configuration;
using SalesTaxCalculationEngine.Enums;
using SalesTaxCalculationEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxCalculationEngine.Providers
{
    /// <summary>
    /// Factory class to generate and return an ITaxProvider implementation based on parameterized enum value
    /// </summary>
    public class ProviderFactory : IProviderFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ITaxRateService _service;

        public ProviderFactory(IConfiguration configuration, ITaxRateService service)
        {
            _configuration = configuration;
            _service = service;
        }

        /// <summary>
        /// Creates and returns ITaxProvider implementation
        /// </summary>
        /// <param name="state">State enum</param>
        /// <returns>ITaxProvider implementation associated with parameterized enum, null if no association found</returns>
        public ITaxProvider Create(States state)
        {
            switch (state)
            {
                case States.NC:
                    return new NCTaxProvider(_configuration, _service);
                default:
                    return null;
            }
        }
    }
}
