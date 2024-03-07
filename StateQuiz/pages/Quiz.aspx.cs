using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StateQuiz.pages
{
    public partial class Quiz : System.Web.UI.Page
    {
        private int max_questions, question_num, num_right;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["u"] == null) Response.Redirect("LoginRegister.aspx");
            if (Session["n"] == null) Response.Redirect("Profile.aspx");

            max_questions = (Int32)Session["n"];
            question_num = 0;
            num_right = 0;

            if ((bool)Session["q"] == true)
            {
                Dictionary<string, string> states = new Dictionary<string, string>(),
                            question_states = new Dictionary<string, string>();
                using (SqlConnection connection = new SqlConnection(Site1.db_connect))
                {
                    connection.Open();

                    SqlCommand cmd_get_quest = new SqlCommand("Get_All_Rows", connection);
                    cmd_get_quest.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd_get_quest.Parameters.AddWithValue("@table_name", "State_Data");

                    SqlDataReader reader = cmd_get_quest.ExecuteReader();

                    // add each state info to the dictionary
                    while (reader.Read())
                    {
                        states.Add(
                            Session["m"].ToString().Equals("c") ? reader["StateName"].ToString() : reader["StateCapital"].ToString(),
                            Session["m"].ToString().Equals("c") ? reader["StateCapital"].ToString() : reader["StateName"].ToString());
                    }

                    // create a random dictionary of questions that will be used for the current quiz
                    while (question_states.Count < max_questions)
                    {
                        int index = Site1.rand.Next(0, 50);
                        if (!question_states.ContainsKey(states.ElementAt(index).Key)) question_states.Add(states.ElementAt(index).Key, states.ElementAt(index).Value);
                    }

                    Session["states"] = states;                     // save the dictionaries to a session variable (submitting the quiz reloads the page)
                    Session["question_states"] = question_states;

                    while (question_num < max_questions) // generate the entire quiz
                    {
                        // state question number and question
                        Literal question = new Literal();
                        question.Text = "<p>Question #" + (question_num + 1) + ": " + (Session["m"].ToString().Equals("c") ? "What is the capital of " : "What state has the capital of ") + question_states.ElementAt(question_num).Key + "</p>\n";
                        pane_quiz.Controls.Add(question);

                        // temp array of answers for question
                        string[] answers = { question_states.ElementAt(question_num).Value, "", "", "", "" }; // add the right answer to the array first
                        int i = 1;
                        while (answers.Contains(""))
                        {
                            int index = Site1.rand.Next(0, 50);
                            string answer = states.ElementAt(index).Value;
                            if (!answers.Contains(answer))
                            {
                                answers[i] = answer;
                                i++;
                            }
                        }
                        answers = answers.OrderBy(x => Site1.rand.Next()).ToArray();
                        Session["question" + question_num] = answers; // save the choices to each question

                        for (int n = 0; n < answers.Length; n++) // generate the answer list of the question
                        {
                            RadioButton button = new RadioButton(); // generate the radio button
                            button.Text = answers[n];
                            button.ID = "question" + question_num + "_answer" + n;
                            button.GroupName = "question" + question_num;

                            pane_quiz.Controls.Add(new LiteralControl("<p class = \"compact\">")); // add the radio butto nto the page 
                            pane_quiz.Controls.Add(button);
                            pane_quiz.Controls.Add(new LiteralControl("</p>"));
                            pane_quiz.Controls.Add(new LiteralControl("<br />\n"));
                        }
                        question_num++;
                    }
                    connection.Close();
                }
            }
            else
            {
                while (question_num < max_questions)
                {
                    // state question number and question
                    Literal question = new Literal();
                    question.Text = "<p>Question #" + (question_num + 1) + ": " + (Session["m"].ToString().Equals("c") ? "What is the capital of " : "What state has the capital of ") + ((Dictionary<string, string>)Session["question_states"]).ElementAt(question_num).Key + "</p>\n";
                    pane_quiz.Controls.Add(question);

                    // generate the answer choice list
                    string[] answers = (string[])Session["question" + question_num];

                    for (int n = 0; n < answers.Length; n++)
                    {
                        RadioButton button = new RadioButton(); // generate the radio button
                        button.Text = answers[n];
                        button.ID = "question" + question_num + "_answer" + n;
                        button.GroupName = "question" + question_num;

                        pane_quiz.Controls.Add(new LiteralControl("<p class = \"compact\">")); // add the radio butto nto the page 
                        pane_quiz.Controls.Add(button);
                        pane_quiz.Controls.Add(new LiteralControl("</p>"));
                        pane_quiz.Controls.Add(new LiteralControl("<br />\n"));
                    }

                    question_num++;
                }
            }

            Session["q"] = false;
            question_num = 0;
        }

        protected void Grade_Quiz(object sender, EventArgs e)
        {
            if (btn_submit_quiz.Text.Equals("Submit Quiz"))
            {
                btn_submit_quiz.Text = "Finish Quiz"; // change the text of the button
                while (question_num < max_questions)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        string id = "question" + question_num + "_answer" + i; // generate the id to search for 

                        RadioButton control = pane_quiz.FindControl(id) as RadioButton; // get the input control

                        // set the styling of each answer item 
                        if (!control.Text.Equals(((Dictionary<string, string>)Session["question_states"]).ElementAt(question_num).Value)) control.Attributes["style"] += "text-decoration: line-through;";
                        if (control.Checked) control.Attributes["style"] += "font-weight: bold;";

                        // grade the quiz
                        if (control.Checked && control.Text.Equals(((Dictionary<string, string>)Session["question_states"]).ElementAt(question_num).Value)) // the correct answer
                            num_right++;
                    }
                    question_num++;
                }

                // tell the user their grade
                lit_out.Text += "<p>Your final quiz grade: " + num_right + "/" + max_questions + " or " + ((num_right * 100) / max_questions) + "%</p>";

                // add the quiz grade to the DB
                using (SqlConnection connection = new SqlConnection(Site1.db_connect))
                {
                    connection.Open();

                    SqlCommand cmd_add_quiz = new SqlCommand("Add_Quiz_Result", connection);
                    cmd_add_quiz.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd_add_quiz.Parameters.AddWithValue("@userID", Session["u"].ToString());
                    cmd_add_quiz.Parameters.AddWithValue("@right", num_right);
                    cmd_add_quiz.Parameters.AddWithValue("@total", max_questions);
                    cmd_add_quiz.Parameters.AddWithValue("@date", DateTime.Now);

                    cmd_add_quiz.ExecuteNonQuery();

                    connection.Close();
                }
            }
            else
            {
                Session["n"] = null; // deleting the old quiz data
                Session["states"] = null;
                Session["question_states"] = null;
                for (int i = 0; i < max_questions; i++) Session["question" + i] = null;

                Response.Redirect("Profile.aspx");
            }
        }
    }
}


/* OLD STUFF */
//lit_test.Text += "<p><input runat=\"server\" type=\"radio\" id=\"question" + question_num + "_answer" + n + "\" value=\"" + answers[n] + "\" name=\"question" + question_num + "_answer" + n + "\">";
//lit_test.Text += "<label for=\"question" + question_num + "\">&nbsp;" + answers[n] + "</label></p>\n";

//lit_test.Text += "\n<p><u>Question #" + (question_num + 1) + ":</u> " +  (Session["m"].ToString().Equals("c") ? "What is the capital of " : "What state has the capital of ") + question_states.ElementAt(question_num).Key + "?</p>\n";

//string selected_answer = string.Format("{0}", Request.Form["question" + question_num]); // get the results of an html-based form submittion

//states.Add(reader["StateName"].ToString(), reader["StateCapital"].ToString());