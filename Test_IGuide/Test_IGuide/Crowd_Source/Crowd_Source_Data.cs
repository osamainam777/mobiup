using Test_IGuideModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Test_IGuide.Crowd_Source
{
    public class Crowd_Source_Data
    {
        RootObject resp;
        public RootObject FourSqaure()
        {
            string url = "https://api.foursquare.com/v2/venues/search?"+
                "client_id=LLPSVMZVUIADQUXZHYEVP23GAEKPVRFM12B31PJUSWSK0LR3"
                +"&"+
                "client_secret=VOFT4O2511X4FKO1JBUEYZU4Q0S5KOJZVSA3W3V5IJF40KOQ"
                +"&"+
                "v=20130815"
                +"&"+
                "ll=31.422437640561725, 73.07725667953491"
                +"&"+
                "categoryId=4d4b7104d754a06370d81259";
            try
            {
                using(WebClient web = new WebClient())
                {
                    var stream = web.DownloadString(url);
                     resp = JsonConvert.DeserializeObject<RootObject>(stream.ToString());
                     return resp;
                    
                }
            }
            catch (Exception cd)
            {
                return null;  
            }
        }
        }
    
}