using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
namespace Test_IGuideModels
{
   [DataContract]
    public class Contact
    {
        [DataMember(Name="phone")]
        public string phone { get; set; }

        [DataMember(Name = "formattedPhone")]
        public string formattedPhone { get; set; }

        [DataMember(Name = "twitter")]
        public string twitter { get; set; }

       [DataMember(Name = "facebook")]
        public string facebook { get; set; }

       [DataMember(Name = "facebookUsername")]
        public string facebookUsername { get; set; }

       [DataMember(Name = "facebookName")]
        public string facebookName { get; set; }
    }

    [DataContract]
    public class Location
    {
        [DataMember(Name = "address")]
        public string address { get; set; }

        [DataMember(Name = "crossStreet")]
        public string crossStreet { get; set; }

        [DataMember(Name = "lat")]
        public double lat { get; set; }

        [DataMember(Name = "lng")]
        public double lng { get; set; }

        [DataMember(Name = "distance")]
        public int distance { get; set; }

        [DataMember(Name = "cc")]
        public string cc { get; set; }

        [DataMember(Name = "city")]
        public string city { get; set; }

        [DataMember(Name = "state")]
        public string state { get; set; }

        [DataMember(Name = "country")]
        public string country { get; set; }

        [DataMember(Name = "formattedAddress")]
        public List<string> formattedAddress { get; set; }

        [DataMember(Name = "postalCode")]
        public string postalCode { get; set; }
    }


    [DataContract]
    public class VenuePage
    {
        [DataMember(Name = "id")]
        public string id { get; set; }
    }
    [DataContract]
    public class Venue
    {
        [DataMember(Name = "id")]
        public string id { get; set; }

        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "contact")]
        public Contact contact { get; set; }

        [DataMember(Name = "location")]
        public Location location { get; set; }
    }
    [DataContract]
    public class Response
    {
        [DataMember(Name = "venues")]
        public List<Venue> venues { get; set; }
    }
    [DataContract]
    public class RootObject
    {
        [DataMember(Name = "response")]
        public Response response { get; set; }
    }
}
