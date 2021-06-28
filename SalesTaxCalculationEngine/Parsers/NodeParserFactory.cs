using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxCalculationEngine.Parsers
{
    public class NodeParserFactory
    {
        /// <summary>
        /// Not the best class names, but it's time to wrap this up.
        /// </summary>
        /// <param name="nodeCount"></param>
        /// <returns></returns>
        public static INodeParser Create(int nodeCount)
        {
            switch (nodeCount)
            {
                case 8:
                    return new EightNodeParser();
                case 9:
                    return new NineNodeParser();
                case 14:
                    return new FourteenNodeParser();
                case 16:
                    return new SixteenNodeParser();
                default:
                    return null;
            }
        }
    }
}
