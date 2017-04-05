using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_IGuide.Tweets_Followers;
using Test_IGuideModels;
using Test_IGuide.Crowd_Source;
using Test_IGuide.Models;
using Test_IGuide.NLP;
using System.Data.Sql;
using System.Data.SqlClient;


namespace Test_IGuide.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Tweets(TweetsModel model)
        {
            try
            {
                GetTweets G = new GetTweets();
                G.T_F(model.ScreenNameTweets);
                return Content("Done with Downloading Tweets of the Following Screen Name: " + model.ScreenNameTweets);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return Content("Error Handle: Account does not exist in Twitter Network");
            }
            catch (OverflowException ec)
            {
                return Content("Error Handle: Account exist in Twitter Network but contains NO Tweets\n");
            }
            catch (AggregateException cd)
            {
                return Content("Error Handle: Either the Internet Connection is BAD! or Account and Tweets exist but are Protected in Twitter Network at line 68\n" );
            }
            catch (SqlException se)
            {
                return Content("Error Handle: Sql Conntection(String) or Command(Qurey) Error");
            }
            catch (Exception ee)
            {
                return Content("Error Handle : Some Unknow Exception as occured ::\n" + ee.ToString());
            }

        }
        public ActionResult Tweets()
        {
            ViewBag.Message = "Tweets Retrevial !";
            return View();
        }

        [HttpPost]
        public ActionResult PreProcess(PreprocessModel model)
        {
            try
            {
                Preprocessing p = new Preprocessing();
                p.Pre_processing(model._Name);
                return Content("Done with PreProcessing");
            }
            catch (Exception e)
            {
                return Content(e.ToString());
            }
            

        }
        public ActionResult PreProcess()
        {
            ViewBag.Message = "Start PreProcessing !";

            return View();
        }

        public ActionResult CrowdSource(RootObject model)
        {
            Crowd_Source_Data csd = new Crowd_Source_Data();
            try
            {

                model = csd.FourSqaure();
                if (model == null)
                {
                    return Content("Error handle : Inappropriate Root Object returned");
                }
                else
                {

                    ViewBag.Message = "Map Display";

                    return View("CrowdSource", model);
                }
            }
            catch (Exception ee)
            {
                return Content("Error Handle : Some Unknow Exception as occured :: "+ee.ToString());
            }
        }
        public ActionResult SentimentalAnalysis()
        {
            ViewBag.Message = "Start Analyzing Tweets";
            return View();
        }
    }
}