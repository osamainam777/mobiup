using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test_IGuide.Models
{
    public class FollowersModel
    {
        public string ScreenName { get; set; }
        public List<Int64> UserID { get; set; }
        public List<Int64> FollowerID { get; set; }
        public List<string> FollowerScreenName { get; set; }
    }
}