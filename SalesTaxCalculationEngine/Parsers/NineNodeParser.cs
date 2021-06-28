using HtmlAgilityPack;
using SalesTaxCalculationEngine.Models;
using System.Collections.Generic;

namespace SalesTaxCalculationEngine.Parsers
{
    /// <summary>
    /// Unfortunate side effect of scraping the web page for NC tax rate data per county is that the HTML table
    /// column count (translated into DOM-type nodes) is not uniform. However, it is predictable based on
    /// review of page HTML. 
    /// </summary>
    public class NineNodeParser : NodeParser
    {
        /// <summary>
        ///  When a node has 9 children, the following holds true:
        ///     county names will be found at indices 0, 3, and 6
        ///     sales tax rates will be found at indices 1, 4, and 7
        /// </summary>
        /// <param name="node">node as HtmlNode</param>
        /// <param name="collection">List of TaxRateEntities</param>
        public override void ParseChildNodes(HtmlNode node, ref List<TaxRateEntity> collection)
        {
            collection.Add(CreateTaxRateEntity(node.ChildNodes[0].InnerText, ConvertRateStringToDouble(node.ChildNodes[1].InnerText)));
            collection.Add(CreateTaxRateEntity(node.ChildNodes[3].InnerText, ConvertRateStringToDouble(node.ChildNodes[4].InnerText)));
            collection.Add(CreateTaxRateEntity(node.ChildNodes[6].InnerText, ConvertRateStringToDouble(node.ChildNodes[7].InnerText)));
        }
    }
}
