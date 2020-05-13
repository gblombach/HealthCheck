/*
 * TITLE: HealthCheck Example
 * Abstract: HTTPS Status Code return for health check against two URLS
 *          Return 501 to caller if either URL check is not Status Code 200
 * Author: George Blombach, george.blombach@microsoft.com
 * Date: 05/10/2020
 *
 */

using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HealthCheck.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {

            /*
            *   DEFINE URLS HERE
            */

            String urlOne = "http://www.contoso.com";
            String urlTwo = "http://www.microsoft.com";

            //  Return to caller HTTP Status Code for Health Check on both URLs
            if (GetPage(urlOne) == 200 && GetPage(urlTwo) == 200)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
            }
        }

        public static int GetPage(String url)
        {
            try
            {
                // Creates an HttpWebRequest for the specified URL
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                // Sends the HttpWebRequest and waits for a response
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    
                    // Releases the resources of the response
                    myHttpWebResponse.Close();
                    return 200;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
                                     myHttpWebResponse.StatusDescription);
                    // Releases the resources of the response
                    myHttpWebResponse.Close();
                    return 501;
                }
                
            }
            catch (WebException e)
            {
                System.Diagnostics.Debug.WriteLine("\r\nWebException Raised. The following error occurred : {0}", e.Status);
                return 501;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("\nThe following Exception was raised : {0}", e.Message);
                return 501;
            }
        }
        
    }
}
