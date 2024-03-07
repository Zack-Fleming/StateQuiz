using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace StateQuiz.pages
{
    public partial class LoginRegister : System.Web.UI.Page
    {
        int error = 0;

        Color error_background = (Color)new WebColorConverter().ConvertFromString("#FFFF8A8A"),
              reg_background   = (Color)new WebColorConverter().ConvertFromString("#FFFFFFFF");

        protected void Page_Load(object sender, EventArgs e) { if (Session["u"] != null) Response.Redirect("Profile.aspx"); }

        protected void Add_User(object sender, EventArgs e)
        {
            txt_first.BackColor     = reg_background;                               // reset backgrounds and error text
            txt_last.BackColor      = reg_background;
            txt_user.BackColor      = reg_background;
            txt_pass.BackColor      = reg_background;
            txt_confirm.BackColor   = reg_background;
            lbl_register_error.Text = "";
            error = 0;                                                              // reset error flag
            
            string  first_name = txt_first.Text,                                    // get the text rfom input field(s)
                    last_name  = txt_last.Text,
                    user_name  = txt_user.Text;

            Required_Field_Check(first_name, txt_first);                            // check required field(s) for input
            Required_Field_Check(last_name, txt_last);
            Required_Field_Check(user_name, txt_user);
            Required_Field_Check(txt_pass.Text, txt_pass);
            Required_Field_Check(txt_confirm.Text, txt_confirm);

            if (error == 1) { Print_Error(1, lbl_register_error); return; }                             // print error if any field(s) are empty

            Password_Check(txt_pass.Text, txt_pass, txt_confirm.Text, txt_confirm); // perform the password checks

            if (error == 1) return;                                                 // exit event, if previous error

            string en = Compute_SHA_256(txt_pass.Text);                             // hash the password

            using (SqlConnection connection = new SqlConnection(Site1.db_connect))  // add the user to the DB
            {
                connection.Open();

                SqlCommand cmd_check = new SqlCommand("User_Check", connection);
                cmd_check.CommandType = System.Data.CommandType.StoredProcedure;
                cmd_check.Parameters.AddWithValue("@user", user_name);

                int result = (Int32)cmd_check.ExecuteScalar();

                if (result != 0) { Print_Error(5, lbl_register_error); return; }                        // if the user exists, print error and break out

                SqlCommand cmd_add = new SqlCommand("Add_User", connection);        // add the user, if they do not exist
                cmd_add.CommandType = System.Data.CommandType.StoredProcedure;
                cmd_add.Parameters.AddWithValue("@first", first_name);
                cmd_add.Parameters.AddWithValue("@last", last_name);
                cmd_add.Parameters.AddWithValue("@user", user_name);
                cmd_add.Parameters.AddWithValue("@pass", en);
                cmd_add.Parameters.Add("@new_id", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd_add.ExecuteNonQuery();                                          // execute uaer add query, and get the inserted ID
                int new_user_ID = Convert.ToInt32(cmd_add.Parameters["@new_id"].Value);

                lbl_register_error.Text = "User sucessfully registered!!!";

                txt_first.Text = "";                               // reset text of input fields
                txt_last.Text = "";
                txt_user.Text = "";
                txt_pass.Text = "";
                txt_confirm.Text = "";

                Session["u"] = new_user_ID;                                         // set user id to session

                Response.Redirect("Profile.aspx");                                  // redirect to user profile page

                connection.Close();
            }
        }

        protected void Login_User(object sender, EventArgs e)
        {
            string user_name = txt_user_login.Text;                                 // get the info from input

            Required_Field_Check(user_name, txt_user_login);                        // check if the required fields are filled in
            Required_Field_Check(txt_pass_login.Text, txt_pass_login);

            if (error == 1) { Print_Error(1, lbl_login_error); return; }

            using (SqlConnection connection = new SqlConnection(Site1.db_connect))
            {
                connection.Open();

                SqlCommand cmd_login = new SqlCommand("User_Login", connection);
                cmd_login.CommandType = CommandType.StoredProcedure;
                cmd_login.Parameters.AddWithValue("@user", user_name);
                cmd_login.Parameters.AddWithValue("@pass", Compute_SHA_256(txt_pass_login.Text));

                int userId = Convert.ToInt32(cmd_login.ExecuteScalar());            // get the ID of the user

                if (userId == 0) { Print_Error(4, lbl_login_error); return; }                        // print error, if ID not found

                txt_user_login.Text = "";                                           // reset input field(s)

                Session["u"] = userId;                                              // set userId session variable, and redirect to profile page
                Response.Redirect("Profile.aspx");

                connection.Close();
            }
        }

        // HELPER METHODS
        private void Required_Field_Check(string input, TextBox control)
        {
            bool isNotValid = string.IsNullOrEmpty(input);

            control.BackColor = (isNotValid ? error_background : reg_background);
            error = ((isNotValid | (error == 1)) ? 1 : 0);
        }

        private string Compute_SHA_256(string input)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++) builder.Append(bytes[i].ToString("X2"));

                return builder.ToString();
            }
        }
        
        private void Print_Error(int code, Label output)
        {
            switch (code)
            {
                case 1:  output.Text = "error: one or more fields not filled in...";      break;
                case 2:  output.Text = "error: passwords do not match. Try again...";     break;
                case 3:  output.Text = "error: password does not meatch requirements..."; break;
                case 4:  output.Text = "error: username or password is incorrect...";     break;
                case 5:  output.Text = "error: username already exists...";               break;
                default: output.Text = "error: somethig went wrong. Try again...";        break;
            }
        }

        private void Password_Check(string pass, TextBox p_control, string conf, TextBox c_control)
        {
            if (!pass.Equals(conf))                                                 // if the passwords are NOT the same
            {
                p_control.BackColor = error_background;
                c_control.BackColor = error_background;

                Print_Error(2, lbl_register_error);
                error = 1;

                return;
            }

            if(pass.Length < 16)                                                    // if the password is too short
            {
                p_control.BackColor = error_background;
                c_control.BackColor = error_background;

                Print_Error(3, lbl_register_error);
                error = 1;

                return;
            }

            if (!Regex.IsMatch(pass, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d-_@$!%*?&#|]{16,}$")) // regex matching
            {
                p_control.BackColor = error_background;
                c_control.BackColor = error_background;

                Print_Error(3, lbl_register_error);
                error = 1;

                return;
            }
        }
    }
}