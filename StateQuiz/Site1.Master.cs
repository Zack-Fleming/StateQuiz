using System;
using System.Configuration;
using System.Web;

namespace StateQuiz
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        // setup some value(s) used by every page
        public static string db_connect = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

        public static Random rand = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache); // do not cache the page

            // show profile navbar item, only when logged in
            if (Session["u"] == null)
            {
                profile.Visible = false;
                logout.Visible = false;
            }
            else
            {
                profile.Visible = true;
                logout.Visible = true;
            }

            // get the currently running page
            string page = HttpContext.Current.CurrentHandler.ToString();
            string[] pieces = page.Split('_');

            //lit_test.Text = "<p>" + page + "</p>"; // debugging
            //lit_test.Text = "<p>";
            //foreach (string piece in pieces) lit_test.Text += piece + ", ";
            //lit_test.Text += "</p>";

            // setting navbar link to active, for the current page
            switch (pieces[1])
            {
                case "loginregister":
                    register.Attributes["class"] = "active left";
                    break;
                case "profile":
                    profile.Attributes["class"] = "active left";
                    break;
                case "home":
                    home.Attributes["class"] = "active right";
                    break;
                default:
                    break;
            }
        }

        protected void User_Logout(object sender, EventArgs e)
        {
            //Session["u"] = null;
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionID", ""));
            Response.Redirect("LoginRegister.aspx");
        }
    }
}