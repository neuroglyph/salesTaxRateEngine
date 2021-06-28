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
    public class FourteenNodeParser : NodeParser
    {
        /// <summary>
        ///  When a node has 14 children, the following holds true:
        ///     county names will be found at indices 0, 6, and 10
        ///     sales tax rates will be found at indices 2, 8, and 12
        /// </summary>
        /// <param name="node">node as HtmlNode</param>
        /// <param name="collection">List of TaxRateEntities</param>
        public override void ParseChildNodes(HtmlNode node, ref List<TaxRateEntity> collection)
        {
            collection.Add(CreateTaxRateEntity(node.ChildNodes[0].InnerText, ConvertRateStringToDouble(node.ChildNodes[2].InnerText)));
            collection.Add(CreateTaxRateEntity(node.ChildNodes[6].InnerText, ConvertRateStringToDouble(node.ChildNodes[8].InnerText)));
            collection.Add(CreateTaxRateEntity(node.ChildNodes[10].InnerText, ConvertRateStringToDouble(node.ChildNodes[12].InnerText)));
        }
    }
}
