using HtmlAgilityPack;
using SalesTaxCalculationEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalesTaxCalculationEngine.Parsers
{
    public abstract class NodeParser : INodeParser
    {
        #region abstract members that each inheriting class must implement
        /// <summary>
        ///  Force each inheriting class to implement its own parsing logic
        /// </summary>
        /// <param name="nodes">nodes as HtmlNodeCollection</param>
        /// <returns></returns>
        public abstract void ParseChildNodes(HtmlNode node, ref List<TaxRateEntity> collection);
        #endregion

        #region concrete members that do not require each implementing class to override
        /// <summary>
        /// Takes a string of format "X.XX%" and converts it to a double value by removing the "%" and converting remaining string to double
        /// In a more robust application, additional logic would be required to handle occurrences where the incoming rate parameter does not
        /// parse successfully as double. However, at the time of design, it has been determined that at all incoming values should successfully
        /// convert to double via the logic implemented here. For the purpose of this exercise, we shall just try to parse and throw an exception if it fails.
        /// </summary>
        /// <param name="rate">Rate as string including percentage sign</param>
        /// <returns>Rate as double</returns>
        protected double ConvertRateStringToDouble(string rate)
        {
            double rateToReturn;
            // Remove all non-numeric (alpha) characters.
            string convertedRate = Regex.Replace(rate, "[^0-9.]", "");

            // This is not, generally speaking, a good idea. Just because rate may be empty, does not necessarily mean that it should equate to zero.
            // However, the case should be evaluated.
            if (String.IsNullOrEmpty(convertedRate))
                return 0;

            // Some will return "7", for example. Round to 2 decimal places for consistency.
            if (Double.TryParse(convertedRate, out rateToReturn))
                return Math.Round(rateToReturn, 2);
            else
                throw new Exception(String.Format("Rate: {0} cannot be parsed to double", rate));
        }

        /// <summary>
        /// Creates a new TaxRateEntity based on incoming parameterized data
        /// </summary>
        /// <param name="county">County as string</param>
        /// <param name="rate">Rate as double</param>
        /// <returns>TaxRateEntity</returns>
        protected TaxRateEntity CreateTaxRateEntity(string county, double rate)
        {
            return new TaxRateEntity
            {
                County = county,
                Rate = rate
            };
        }

        #endregion


    }
}
