# salesTaxRateEngine
Summary
=======
Important information:

The endpoint is "tax/calculateSalesTax" and expects 3 parameters:

1. state: The application framework is designed to be expanded beyond North Carolina and can support additional states. At this time, only "NC" is implemented. Submission of 
          other values will result in a 404 "not found" error with a message to the user wrapped in json:
          
          {
             "Error": {
             "rawValue": null,
             "attemptedValue": null,
             "errors": [
               {
                 "exception": null,
                 "errorMessage": "'va' is not a valid state or is not currently supported at this time."
               }
             ],
             "validationState": 1,
             "isContainerNode": false,
             "children": null
             }
         }
               
2. county: The link in provided email for NC sales tax data unfortunately did not work for me. I found an alternative source on the NC Dept of Revenue web site
                  
           https://www.ncdor.gov/taxes-forms/sales-and-use-tax/sales-and-use-tax-rates-other-information/sales-and-use-tax-rates-effective-october-1-2020

           This data is  categorized by "county", so the application is designed around this. Submission of any value other than one of the counties for NC specified on the page 
           will also result in a 404 "not found" error with message to user wrapped in json:
           
           {
            "Error": {
              "rawValue": null,
              "attemptedValue": null,
              "errors": [
                {
                  "exception": null,
                  "errorMessage": "'washington ' is not a valid county or is not currently supported at this time."
                }
              ],
              "validationState": 1,
              "isContainerNode": false,
              "children": null
            }
          }
                 
3. transactionAmount: The amount upon which sales tax must be calculated. Successful response (valid state, valid county, valid transaction amount) will result in a json 
                      response containing tax calculation as well as county and determined rate that was used. Successful request/response will result in a json body of 
                      following format:
                   
                      {
                        "county": "wake",
                        "transactionAmount": 4,
                        "salesTaxTotal": 0.29
                      }

Although the end point is an HTTP POST, parameters are encoded on the query string as follows:
   https://salestaxservice.azurewebsites.net/Tax/calculateSalesTax?state=nc&county=wake&transactionAmount=4

Swagger
=======
The application for the coding exercise is posted to azure.websites for review - it can be accessed at the following:
   https://salestaxservice.azurewebsites.net/swagger/index.html

Swagger is a cool tool that allows you to test api endpoints that I've incorporated into my application. Users can test the end point by submitting values for its parameters.
