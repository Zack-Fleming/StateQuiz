using System;

namespace StateQuiz.pages
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void Quiz_Redirect(object sender, EventArgs e) { Response.Redirect("/Pages/Quiz.aspx"); }

        protected void Login_Redirect(object sender, EventArgs e) { Response.Redirect("/Pages/LoginRegister.aspx"); }
    }
}