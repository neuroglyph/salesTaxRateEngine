using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;

namespace SalesTaxCalculationEngine
{
    /// <summary>
    ///  Ideally, this class would be implemented from an interface (without static functions) so that it could be properly unit tested. For the purpose of this exercise, I'm just moving
    ///  code that can be stripped out of the main classes here to clean things up. 
    /// </summary>
    public class Utilities
    {
        public static HttpClient CreateClient(string url, string protocols, string decompressionMethods)
        {
            // If url is empty, no point in continuing
            if (String.IsNullOrEmpty(url))
                return null;

            // If there were time, I could extract a singular generic method to generate a list of enums that 
            // both "protocols" and "decompressionMethods" strings could be passed into (returning a collection of correct enum types).
            // However, it's getting late and I'm ready to wrap this up, so just building separate ones. Consider it something ripe for refactoring down the line. 
            SslProtocols sslProtocols = new SslProtocols();
            if (!String.IsNullOrEmpty(protocols))
            {
                var protocolList = protocols.Split("|");
                foreach (string p in protocolList)
                {
                    SslProtocols convertedProtocol;

                    if (Enum.TryParse(p,out convertedProtocol))
                    {
                        if (Enum.IsDefined(typeof(SslProtocols), convertedProtocol))
                        {
                            sslProtocols |= convertedProtocol;
                        }
                    }
                }
            }

            DecompressionMethods decompressionMethodsLocalized = new DecompressionMethods();
            if (!String.IsNullOrEmpty(decompressionMethods))
            {
                var methodList = decompressionMethods.Split("|");
                foreach (string m in methodList)
                {
                    DecompressionMethods convertedMethods;

                    if (Enum.TryParse(m, out convertedMethods))
                    {
                        if (Enum.IsDefined(typeof(DecompressionMethods), convertedMethods))
                        {
                            decompressionMethodsLocalized |= convertedMethods;
                        }
                    }
                }
            }

            // In order to correctly mimic a secure web request, must set SslProtocols 
            // Additionally, the response returns compressed data, need to explicitly declare AutomaticDecompression to decompress upon return
            // (Note: These declarations have been abstracted out into configuration values - leaving above comments here for informational purposes).
            //  For the purposes of the exercise, no logic around empty enum values is implemented. However, this should be a consideration when refactoring.
            var handler = new HttpClientHandler()
            {
                SslProtocols = sslProtocols,
                AutomaticDecompression = decompressionMethodsLocalized
            };

            var client = new HttpClient(handler);
            client.BaseAddress = new Uri(url);

            // Add headers needed to mimic a web browser
            // Realistically, these should all be configuration values that are passed in similiar to the "protocols" and "decompressionMethods" parameters.
            // However, for now, just hard-coding them for the sake of this exercise. Code section ripe for refactoring down the line.
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.106 Safari/537.36");

            return client;
        }
    }
}
