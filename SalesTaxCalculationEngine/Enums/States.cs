using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTaxCalculationEngine.Enums
{
    /// <summary>
    /// In a more robust application, this data should be maintained within a source such as a relational database. This would make it easier to modify the entries (add new ones, for example)
    /// when (hopefully!) the business expands to service additional state tax rate calculations. 
    /// 
    /// Storing these values as "baked-in" enum values makes it impossible to change them without recompiling and redeploying the application. In general, not highly desired. 
    /// However, for the purposes of this exercise, placing them here should suffice to provide proof-of-concept.
    /// 
    /// Note: "Va" added to fully flush out the test cases for ProviderFactory, one of which needs to be the case when States enum is passed in that is not coded for.
    /// </summary>
    public enum States
    {
        NC,
        Va
    }
}
