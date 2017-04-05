using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace Test_IGuide.NLP
{
    public class Preprocessing
    {

        public void Pre_processing(string name)
        {
            Retrive(name);
            RemoveURLFromTweet(name);
        }
        // Reterive Tweets from DB
        public void Retrive(string name)
        {
            SqlCommand comm;
            SqlConnection conn;
            string pattern = "#";
            int endIndex;
            List<int> HashTag = new List<int>();
            List<string> Subjects = new List<string>();
            string LowerCaseTweet;
            string Text;
            int HashTagPosition;
            string User = name;

            conn = new SqlConnection(@"Data Source=OSAMA-PC;Initial Catalog=IGuide;Integrated Security=True");
            conn.Open();
            comm = new SqlCommand("SELECT child FROM Dataset WHERE father= @father", conn);
            comm.Parameters.AddWithValue("@father", name);
            SqlDataReader reader = comm.ExecuteReader();
            List<string> str2 = new List<string>();

            while (reader.Read())
            {
                str2.Add(reader.GetValue(0).ToString());
            }
            reader.Close();
            conn.Close();
            Console.Write(str2.Count);
            //Function call to get Subjects/Interst
            Subjects = GetSubjects();
            foreach (string str4 in str2)
            {

                try
                {
                    //To Check for # Presence
                    Text = str4;
                    LowerCaseTweet = str4;
                    LowerCaseTweet = LowerCaseTweet.ToLower();
                    Match m = Regex.Match(LowerCaseTweet, pattern);
                    if (m.Success)//If it founds Hashtag in Tweet
                    {
                        HashTagPosition = m.Index;//Store Hashtag occurence index
                        List<string> Arr = new List<string>();
                        Arr = CheckForSubjectivenessWithHashTag(LowerCaseTweet, Subjects, HashTagPosition);

                        //IF No match occur from first occurence of Hashtag to End of String
                        if (Arr.Count == 0)
                        {
                            endIndex = HashTagPosition;
                            List<string> Arr2 = new List<string>();
                            Arr2 = CheckForSubjectiveness(LowerCaseTweet, Subjects, endIndex);
                            if (Arr2.Count == 0)
                            {
                                //Console.WriteLine("Here2");
                            }
                            else
                            {
                                string EmptySpcae = "";

                                for (int count = 0; count < Arr2.Count; count++)
                                {

                                    EmptySpcae = Arr2[count] + "," + EmptySpcae;
                                }
                                AddSubjectiveData(Text, User, EmptySpcae);

                            }


                        }
                        //IF Match occur from first occurence of Hashtag to End of String
                        else
                        {
                            string str3 = "";

                            for (int count = 0; count < Arr.Count; count++)
                            {

                                str3 = Arr[count] + "," + str3;
                            }
                            AddSubjectiveData(Text, User, str3);
                        }

                    }
                    //If Hashtag is not avilable in Tweet then it is
                    else
                    {
                        endIndex = LowerCaseTweet.Length;
                        List<string> A = new List<string>();
                        A = CheckForSubjectiveness(LowerCaseTweet, Subjects, endIndex);
                        if (A.Count == 0)
                        {
                            //Console.WriteLine("Here2");
                        }
                        else
                        {
                            string str = "";

                            for (int count = 0; count < A.Count; count++)
                            {

                                str = A[count] + "," + str;
                            }
                            AddSubjectiveData(Text, User, str);

                        }

                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }


        }
        //Checking For Subjectiveness
        public List<string> CheckForSubjectiveness(string str, List<string> subject1, int EndIndex)
        {

            int lengthTweet;
            int lengthSubject;
            string Tweet;
            string subjectstring;

            List<int> Exist = new List<int>();
            List<string> InterestFound = new List<string>();
            //KMP Algorithm
            foreach (string subject in subject1)
            {
                subjectstring = subject;
                subjectstring = subjectstring.ToLower();
                subjectstring = subjectstring.Replace(" ", "");
                subjectstring = " " + subjectstring + " ";

                lengthSubject = subjectstring.Length;
                int i = 0;
                int j = 0;
                int[] Pi = new int[lengthSubject];

                Tweet = str;
                //Console.WriteLine(Tweet);

                lengthTweet = EndIndex;
                PrefixFunction(subjectstring, lengthSubject, Pi);
                //Matching Process
                try
                {
                    while (j == lengthSubject || i < lengthTweet)
                    {

                        if (subjectstring[j] == Tweet[i])
                        {
                            j++;
                            i++;
                        }
                        if (j == lengthSubject)
                        {
                            Exist.Add(i - j);
                            j = Pi[j - 1];
                            InterestFound.Add(subjectstring);
                        }
                        else if (i < lengthTweet && subjectstring[j] != Tweet[i])
                        {
                            if (j != 0)
                            {
                                j = Pi[j - 1];
                            }
                            else
                            {
                                i = i + 1;
                            }
                        }
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return InterestFound;

        }
        //Prefix Function
        public void PrefixFunction(string subject, int LengthSubject, int[] Pi)
        {
            int length = 0;
            int i = 1;
            try
            {
                Pi[0] = 0;
                while (i < LengthSubject)
                {

                    if (subject[i] == subject[length])
                    {

                        length++;
                        Pi[i] = length;
                        i++;
                    }
                    else
                    {

                        if (length != 0)
                        {

                            length = Pi[length - 1];
                        }
                        else
                        {

                            Pi[i] = 0;
                            i++;
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }

        }
        // Getting Subjects list
        public List<string> GetSubjects()
        {
            SqlCommand comm;
            SqlConnection conn;
            conn = new SqlConnection(@"Data Source=OSAMA-PC;Initial Catalog=IGuide;Integrated Security=True");
            conn.Open();
            comm = new SqlCommand("SELECT interest FROM subjects  ", conn);

            SqlDataReader reader = comm.ExecuteReader();
            List<string> Interests = new List<string>();

            while (reader.Read())
            {
                Interests.Add(reader.GetValue(0).ToString());
            }
            reader.Close();
            conn.Close();

            return Interests;

        }
        //KMP When # Found
        public List<string> CheckForSubjectivenessWithHashTag(string str, List<string> subject1, int location)
        {

            int lengthTweet;
            int lengthSubject;
            string Tweet;
            string subjectstring;
            List<int> Exist = new List<int>();
            List<string> InterestFound = new List<string>();
            //KMP Algorithm
            foreach (string subject in subject1)
            {
                int i = location;
                int j = 0;

                Tweet = str;
                lengthTweet = Tweet.Length;
                subjectstring = subject;
                subjectstring = subjectstring.ToLower();
                subjectstring = subjectstring.Replace(" ", "");
                subjectstring = " " + subjectstring + " ";
                lengthSubject = subjectstring.Length;
                int[] Pi = new int[lengthSubject];
                PrefixFunction(subjectstring, lengthSubject, Pi);
                //Matching Process
                while (j == lengthSubject || i < lengthTweet)
                {
                    if (subjectstring[j] == Tweet[i])
                    {
                        j++;
                        i++;
                    }
                    if (j == lengthSubject)
                    {
                        j = Pi[j - 1];
                        InterestFound.Add(subjectstring);
                    }
                    else if (i < lengthTweet && subjectstring[j] != Tweet[i])
                    {
                        if (j != 0)
                        {
                            j = Pi[j - 1];
                        }
                        else
                        {
                            i = i + 1;
                        }
                    }
                }
            }


            return InterestFound;

        }
        //To Add Subjective Tweets in DB
        public void AddSubjectiveData(string TweeText, string User, string Interst)
        {
            SqlCommand comm;
            SqlConnection conn;
            string predata = TweeText;
            //predata = predata.ToLower();

            try
            {
                conn = new SqlConnection(@"Data Source=OSAMA-PC;Initial Catalog=IGuide;Integrated Security=True");
                conn.Open();
                comm = new SqlCommand("INSERT INTO SubjectiveTweets (name,tweet,interests) VALUES (@name,@tweet,@interests)", conn);

                comm.Parameters.AddWithValue("@name", User);
                comm.Parameters.AddWithValue("@tweet", predata);
                comm.Parameters.AddWithValue("@interests", Interst);
                comm.ExecuteNonQuery();
                conn.Close();
                //Console.WriteLine("Reached");

            }
            catch (SqlException se)
            {
                Console.WriteLine(se.Message);
            }

        }
        //Remove URL from tweet if found
        public void RemoveURLFromTweet(string User)
        {
            string TweetUser;
            SqlCommand comm;
            SqlConnection conn;
            List<List<String>> Emoticons = new List<List<String>>();
            List<List<String>> Acronyms = new List<List<String>>();
            //string Interest;
            //Console.Write("Reterived Data:");
            conn = new SqlConnection(@"Data Source=OSAMA-PC;Initial Catalog=IGuide;Integrated Security=True");
            conn.Open();
            comm = new SqlCommand("SELECT tweet,name FROM SubjectiveTweets WHERE name= @name  ", conn);
            comm.Parameters.AddWithValue("@name", User);
            SqlDataReader reader = comm.ExecuteReader();
            List<string> Tweet = new List<string>();

            while (reader.Read())
            {
                Tweet.Add(reader.GetValue(0).ToString());

            }
            reader.Close();
            conn.Close();
            Console.WriteLine("Subjective:" + Tweet.Count);
            //Getting all Required Things Acronym, Emoticon
            Emoticons = GetEmoticon();
            Acronyms = GetAcronyms();

            foreach (string UserTweets in Tweet)
            {
                //TweetUser is a new variable because for each variable can not b assigned local values
                TweetUser = Regex.Replace(UserTweets,
                @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                "||U||");
                TagListChecking(TweetUser, User, Emoticons, Acronyms);
            }

        }


        public void TagListChecking(string Tweet, string User, List<List<String>> Emoticons, List<List<String>> Acronyms)
        {

            string value = null;
            MatchCollection MC = null;
            var variable = new Regex(@"@\w*");
            MC = variable.Matches(Tweet);
            List<string> Taggers = new List<string>();
            foreach (Match m in MC)
            {
                //while (match.Success)
                value = (string)m.Value;
                if (value.Length == 0)
                {
                    //Console.WriteLine("No");
                }
                else
                {
                    //Console.WriteLine("Yes Tagged Persons Found");t
                    Taggers.Add(value);

                }


            }


            //Check If Tagged Persons list contain any value
            if (Taggers.Count == 0)
            {
                ProcessEmoticon(Tweet, User, Emoticons, Acronyms);
            }
            else
            {
                AddTaggedPersonstoDB(User, Tweet, Taggers);
                ReplaceTaggers(Tweet, User, Taggers, Emoticons, Acronyms);

            }

        }
        public void AddTaggedPersonstoDB(string User, string tweet, List<string> TaggedPersons)
        {
            SqlCommand comm;
            SqlConnection conn;
            string str = "";
            try
            {
                for (int z = 0; z < TaggedPersons.Count; z++)
                {
                    //Console.WriteLine("Tagged Persons");
                    str = TaggedPersons[z] + "," + str;

                }
                conn = new SqlConnection(@"Data Source=OSAMA-PC;Initial Catalog=IGuide;Integrated Security=True");
                conn.Open();
                comm = new SqlCommand("INSERT INTO TaggList (UserId,Tweet,TaggedPersons) VALUES (@UserId,@Tweet,@TaggedPersons)", conn);

                comm.Parameters.AddWithValue("@UserId", User);
                comm.Parameters.AddWithValue("@Tweet", tweet);
                comm.Parameters.AddWithValue("@TaggedPersons", str);
                //Console.WriteLine(str);
                comm.ExecuteNonQuery();
                conn.Close();

            }
            catch (SqlException ea)
            {
                Console.WriteLine(ea.Message);
            }



        }
        public void ReplaceTaggers(string Tweet, string User, List<string> TaggedPersons, List<List<String>> Emoticons, List<List<String>> Acronyms)
        {
            string ProcessedTweet = Tweet;
            string Replacement = "||T||";
            string Tag;
            for (int i = 0; i < TaggedPersons.Count; i++)
            {
                //Tag=TaggedPersons[i];
                ProcessedTweet = ProcessedTweet.Replace(TaggedPersons[i], Replacement);

            }

            ProcessEmoticon(ProcessedTweet, User, Emoticons, Acronyms);
        }
        public List<List<String>> GetEmoticon()
        {
            SqlCommand comm;
            SqlConnection conn;
            List<List<String>> Emoticons = new List<List<String>>();
            //string Interest;
            //Console.Write("Reterived Data:");
            conn = new SqlConnection(@"Data Source=OSAMA-PC;Initial Catalog=IGuide;Integrated Security=True");
            conn.Open();
            comm = new SqlCommand("SELECT emoticon,polarity FROM Emoticons  ", conn);

            SqlDataReader reader = comm.ExecuteReader();
            List<string> emotion = new List<string>();
            List<string> polar = new List<string>();

            while (reader.Read())
            {
                emotion.Add(reader.GetValue(0).ToString());
                polar.Add(reader.GetValue(1).ToString());
            }
            reader.Close();
            conn.Close();
            Emoticons.Add(emotion);
            Emoticons.Add(polar);
            //ProcessEmoticon(Tweet, User, emotion, polar);
            return Emoticons;

        }
        //Process Emoticon (Check For Presence in Tweet and Replace Emotion with polarity)
        public void ProcessEmoticon(string Tweet, string User, List<List<String>> Emoticon, List<List<String>> Acronyms)
        {
            bool checkPresence;
            string strEmotion;
            //string strPolarity;

            for (int i = 0; i <= Emoticon[0].Count; i++)
            {
                try
                {
                    strEmotion = Emoticon[0][i];
                    checkPresence = Tweet.Contains(strEmotion);
                    //Console.WriteLine(checkPresence);
                    if (checkPresence == true)
                    {
                        //Console.WriteLine("Matched");
                        Tweet = Tweet.Replace(Emoticon[0][i], Emoticon[1][i]);
                        //Console.WriteLine(polar[i]);
                        //Console.WriteLine(Tweet);
                    }
                    else
                    {
                        //Console.WriteLine("Not Matched");

                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            Tweet = Tweet.ToLower();
            ProcessAcronyms(Tweet, User, Acronyms);


        }
        public List<List<String>> GetAcronyms()
        {
            SqlCommand comm;
            SqlConnection conn;
            List<List<String>> Acronyms = new List<List<String>>();
            //Console.Write("Reterived Data:");

            conn = new SqlConnection(@"Data Source=OSAMA-PC;Initial Catalog=IGuide;Integrated Security=True");
            conn.Open();
            comm = new SqlCommand("SELECT acronym,text1 FROM acronyms  ", conn);

            SqlDataReader reader = comm.ExecuteReader();
            List<string> Acronym = new List<string>();
            List<string> Text = new List<string>();

            while (reader.Read())
            {
                Acronym.Add(reader.GetValue(0).ToString());
                Text.Add(reader.GetValue(1).ToString());
            }
            reader.Close();
            conn.Close();
            Acronyms.Add(Acronym);
            Acronyms.Add(Text);
            //Console.WriteLine(Tweet);
            //Tweet = Tweet.ToLower();
            //ProcessAcronyms(Tweet, User, Acronym, Text);

            return Acronyms;
        }
        public void ProcessAcronyms(string Tweet, string User, List<List<String>> Acronyms)
        {
            bool checkPresence;
            string strAcronym;
            string stringAcronym;

            List<string> Ac = new List<string>();
            List<string> Te = new List<string>();

            //string strPolarity;

            for (int i = 0; i < Acronyms[0].Count; i++)
            {
                try
                {
                    stringAcronym = Acronyms[0][i];
                    strAcronym = @"(^|\s)" + stringAcronym + @"(\s|$)";// COncatination of ^ and $ to check exact match of acronym
                    checkPresence = Regex.IsMatch(Tweet, strAcronym);

                    if (checkPresence == true)
                    {
                        Ac.Add(Acronyms[0][i]);
                        Te.Add(Acronyms[1][i]);

                    }
                    else
                    {
                        //Console.WriteLine("Not Matched");

                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            try
            {
                for (int c = 0; c <= Ac.Count; c++)
                {
                    Tweet = Tweet.Replace(Ac[c], Te[c]);
                    // Console.WriteLine(Tweet);

                }

            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }
            Tweet = RemoveRepeatCharacters(Tweet);
            AddPreprocessedToDB(Tweet, User);

        }
        //Replace Replicas Occuring more than twice
        public string RemoveRepeatCharacters(string TweetText)
        {
            Regex r = new Regex("(.)(?<=\\1\\1\\1)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

            var Tweet = r.Replace(TweetText, "");
            return Tweet;
        }
        //Add Preprocessed tweets to DB 
        public void AddPreprocessedToDB(string Tweet, string User)
        {
            SqlCommand comm;
            SqlConnection conn;
            Tweet = Tweet.Replace("n't", "NOT");
            //Console.WriteLine("HUZAIFA");
            //Console.WriteLine(Tweet);
            conn = new SqlConnection(@"Data Source=OSAMA-PC;Initial Catalog=IGuide;Integrated Security=True");
            conn.Open();
            comm = new SqlCommand("INSERT INTO PreProcessedTweet (name,tweet) VALUES (@name,@tweet)", conn);

            comm.Parameters.AddWithValue("@name", User);
            comm.Parameters.AddWithValue("@tweet", Tweet);
            comm.ExecuteNonQuery();
            conn.Close();


        }

    }
    }
