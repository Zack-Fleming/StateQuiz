using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace StateQuiz.pages
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["u"] == null) Response.Redirect("LoginRegister.aspx");                                      // if user is not logged in, redirect to login page

            using (SqlConnection connection = new SqlConnection(Site1.db_connect))                                  // greet the user and load their information
            {
                connection.Open();

                string first_name = "", last_name = "", user_name = "";                                             // fields to hold user info

                SqlCommand cmd_get_user = new SqlCommand("Get_User_Info", connection);                              // get the user's info
                cmd_get_user.CommandType = CommandType.StoredProcedure;
                cmd_get_user.Parameters.AddWithValue("@user_ID", (Int32)Session["u"]);
                SqlDataReader user_data = cmd_get_user.ExecuteReader();

                while (user_data.Read())
                {
                    first_name = user_data["FirstName"].ToString();
                    last_name  = user_data["LastName"].ToString();
                    user_name  = user_data["UserName"].ToString();
                }
                user_data.Close();

                lit_user.Text =
                    "<p>First Name: " + first_name + " </p>\n" +
                    "<p>Last Name : " + last_name + "</p>\n" +
                    "<p>Full Name : " + first_name + " " + last_name + "</p>\n" + 
                    "<p>User Name : " + user_name + "</p>\n";

                SqlCommand cmd_get_len = new SqlCommand("Get_Table_Len", connection);                               // get the length of the greetings table
                cmd_get_len.CommandType = CommandType.StoredProcedure;
                cmd_get_len.Parameters.AddWithValue("@table_name", "Greetings");
                int len = (Int32)cmd_get_len.ExecuteScalar();

                int index = Site1.rand.Next(1, len + 1);                                                            // generate a random index to the length of the table

                SqlCommand cmd_get_greeting = new SqlCommand("Get_Greeting", connection);
                cmd_get_greeting.CommandType = CommandType.StoredProcedure;
                cmd_get_greeting.Parameters.AddWithValue("@id", index);
                SqlDataReader reader = cmd_get_greeting.ExecuteReader();

                while (reader.Read())                                                                               // greet the user, with a random greeting
                {
                    lit_greeting.Text = "<h1>" + reader["Greeting"] + "(hello in " + reader["Language"] + "), " + user_name + "</h1>";
                }
                reader.Close();

                SqlCommand cmd_past_quiz = new SqlCommand("get_User_Quizes", connection);                           // get the user's last quizes
                cmd_past_quiz.CommandType = CommandType.StoredProcedure;
                cmd_past_quiz.Parameters.AddWithValue("@user_ID", (Int32)Session["u"]);
                SqlDataReader quiz_reader = cmd_past_quiz.ExecuteReader();

                StringBuilder text = new StringBuilder();
                int count = 0, total_score = 0;
                while (quiz_reader.Read())
                {
                    int right = (int)quiz_reader["NumRight"], total = (int)quiz_reader["Total"], score = ((right * 100) / total);

                    text.AppendLine("\t<tr>\n\t\t<td>" + right + "</td>\n\t\t<td>" + total + "</td>\n\t\t<td>" + score + "%</td>\n\t\t<td>" + quiz_reader["DateTaken"] + "</td>\n\t</tr>");
                    
                    count++;
                    total_score += score;
                }
                quiz_reader.Close();

                // if the user has no quizes, show some 'you have no past history' text
                if (text.Length == 0)
                {
                    lit_past_quiz.Text = "<p>Your past quizes will show up here. To start a quiz, use the 'New Quiz' section above.</p>";
                    lit_user.Text += "<p>Num Quizes: 0</p>\n<p>Avg. Score: 0%</p>";
                }
                else
                {
                    lit_past_quiz.Text +=
                        "<table>\n\t<tr>\n\t\t<th>Num Right</th>\n\t\t<th>Num Total</th>\n\t\t<th>Score</th>\n\t\t<th>Quiz date</th>\n\t</tr>\n\t" + text.ToString() + "</table>";
                    lit_user.Text += "<p>Num Quizes: " + count + "</p>\n<p>Avg. Score: " + (total_score / count) + "</p>";
                }

                connection.Close();
            }
        }

        protected void Start_Quiz(object sender, EventArgs e)
        {
            //var selected_mode = Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.GroupName == "r_quiz_mode" && rb.Checked);
            //Session["m"] = selected_mode == null ? string.Empty : selected_mode.Text;
            Session["m"] = (mode_state.Checked) ? "s" : "c";
            Session["q"] = true; 
            //lit_past_quiz.Text = Session["m"].ToString(); // debugging
            Session["n"] = Convert.ToInt32(ddl_quiz_len.SelectedValue);
            Response.Redirect("Quiz.aspx");
        }
    }
}