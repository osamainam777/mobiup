using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using LinqToTwitter;
using System.Data.SqlClient;
using System.Data;


namespace Test_IGuide.Tweets_Followers
{
     
    public class GetTweets
    {
        SqlConnection sc = new SqlConnection("Data Source=OSAMA-PC;Initial Catalog=IGuide;Integrated Security=True");
        SqlCommand cmd;
        static List<Status> currentTweets;
        static SingleUserAuthorizer authorizer =
         new SingleUserAuthorizer
         {
             CredentialStore = new
                SingleUserInMemoryCredentialStore
             {
                 ConsumerKey =
                    "Y7MG3WyT5mctqdsZdzYnLqoYd",
                 ConsumerSecret =
                    "o2mZ72P4xWUoqKrNhYKaUCttlzgPACm5Xdrjqj3Gjx7RWBFKdj",
                 AccessToken =
                    "715592655850364928-YvmwMmbrnhhhU4JMYWWSqMP1UAcwD5h",
                 AccessTokenSecret =
                    "v6RnKTT9MRt1q3tPSQcn3jKHxDGj0H923ec66ZQ57oAIk"
             }
         };
        public void T_F(string _screenName)
        {
            List<String> UserID = new List<string>();
            List<String> UserName = new List<string>();
            List<String> ScreenName = new List<string>();
            List<String> TweetText = new List<string>();
            int i = 0;
       
                sc.Open();
                GetTweetsFunction(_screenName); // Change Screen Name here 
                currentTweets.ForEach(name => UserID.Add(name.User.UserIDResponse));
                currentTweets.ForEach(name => ScreenName.Add(name.ScreenName.ToString()));
                currentTweets.ForEach(name => TweetText.Add(name.Text));
                currentTweets.ForEach(name => UserName.Add(name.User.Name));
                string query = "Insert into TweetInfo (ID,UserID,TweetText,UserScreenName,UserName) values (@ID,@UserID,@Text,@UserScreenName,@UserName)";

                cmd = new SqlCommand(query, sc);

                cmd.Parameters.Add("@ID", SqlDbType.Int);
                cmd.Parameters.Add("@UserID", SqlDbType.Int);
                cmd.Parameters.Add("@UserScreenName", SqlDbType.VarChar);
                cmd.Parameters.Add("@Text", SqlDbType.VarChar);
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar);

                foreach (var tt in TweetText.Zip(UserName, Tuple.Create))
                {
                    i++;
                    cmd.Parameters["@ID"].Value = i;
                    cmd.Parameters["@UserID"].Value = UserID[0];
                    cmd.Parameters["@UserScreenName"].Value = ScreenName[0].ToString();
                    cmd.Parameters["@UserName"].Value = tt.Item2;
                    cmd.Parameters["@Text"].Value = tt.Item1;
                    cmd.ExecuteNonQuery();
                }
                GetFollowersFunction(_screenName, UserID[0].ToString()); // Change Screen Name here 
           
            sc.Close();  

        }
        private void GetTweetsFunction(string name)
        {
            var twitterContext = new TwitterContext(authorizer);

            var tweets = (from tweet in twitterContext.Status
                          where tweet.Type == StatusType.User &&
                          tweet.ScreenName == name &&
                          tweet.Count == 200
                          select tweet);
            currentTweets = tweets.ToList();
        }
        private static void GetFollowersFunction(string name, string userid)
        {
            SqlConnection scf = new SqlConnection("Data Source=OSAMA-PC;Initial Catalog=IGuide;Integrated Security=True");
            SqlCommand cmdf;

            List<string> FollowerScreenName = new List<string>();
            List<string> FollowerID = new List<string>();
               scf.Open();
                var twitterContext = new TwitterContext(authorizer);
                var friendship = (from friend in twitterContext.Friendship
                                  where friend.Type == FriendshipType.FollowersList &&
                                  friend.ScreenName == name &&
                                  friend.Count == 200
                                  select friend).SingleOrDefault();
                friendship.Users.ForEach(friend => FollowerScreenName.Add(friend.ScreenNameResponse));
                friendship.Users.ForEach(friend => FollowerID.Add(friend.UserIDResponse));
           
           
                string queryfollow = "Insert into FollowersList (UserID,FollowerID,FollowerScreenName) values (@UserID,@FollowerID,@FollowerScreenName)";

                cmdf = new SqlCommand(queryfollow, scf);
                cmdf.Parameters.Add("@UserID", SqlDbType.VarChar);
                cmdf.Parameters.Add("@FollowerID", SqlDbType.VarChar);
                cmdf.Parameters.Add("@FollowerScreenName", SqlDbType.VarChar);

                foreach (var fi in FollowerID.Zip(FollowerScreenName, Tuple.Create))
                {
                    cmdf.Parameters["@UserID"].Value = userid;
                    cmdf.Parameters["@FollowerID"].Value = fi.Item1;
                    cmdf.Parameters["@FollowerScreenName"].Value = fi.Item2;
                    cmdf.ExecuteNonQuery();
                }
                scf.Close();
           
        }



    }
}