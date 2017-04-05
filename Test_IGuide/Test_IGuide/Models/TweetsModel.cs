using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test_IGuideModels
{
    public class TweetsModel
    {
        public string ScreenNameTweets { get; set; }
        public List<Int64> UserID { get; set; }
        public List<string> UserScreenName { get; set; }
        public List<string> UserName { get; set; }
        public List<string> TweetText { get; set; }
        public int CounterID { get; set; }
    }
}