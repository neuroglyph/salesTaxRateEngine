using SalesTaxCalculationEngine.Models;
using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace SalesTaxCalculationEngine.Parsers
{
    public interface INodeParser
    {
         void ParseChildNodes(HtmlNode node, ref List<TaxRateEntity> collection);
    }
}
