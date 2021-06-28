using SalesTaxCalculationEngine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxCalculationEngine.Providers
{
    /// <summary>
    /// Interface for provider factories
    /// </summary>
    public interface IProviderFactory
    {
        ITaxProvider Create(States state);
    }
}
