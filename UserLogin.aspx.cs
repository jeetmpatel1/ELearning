using Elearning.App_Code;
using System;

namespace Elearning
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        // Nothing needs to be done upon page loading of UserLogin.aspx
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            /*
             * (UC001) Login User
             * 3a: Entered username length is less than 6 or greater than 20 characters.
                .1: Output appropriate error message.
               3b: Entered password length is less than 5 characters .
                .1: Output appropriate error message.
               4a: Database query returns no rows - details must be incorrect.
                .1: Output appropriate error message.
               4b: Catch database error.
                .1: Output appropriate error message.
             */
            // case 3a.1
            if (txtUsername.Text.Length < 6 || txtUsername.Text.Length > 20)
            {
                lblError.Text = "Username length should be between 6 to 20 characters including both.";
            }

            // case 3b.1
            else if (txtPassword.Text.Length < 5)
            {
                lblError.Text = "Passsword length should be greater than or equal to 5 characters.";
            }
            else
            {
                try
                {
                    UserTable userTable = new UserTable();
                    userTable.user_name = txtUsername.Text;
                    userTable.user_password = txtPassword.Text;
                    if (userTable.isValidUser())
                    {
                        Response.Redirect("~/UserAccount.aspx");
                    }
                    else
                    {
                        // case 4a.1
                        lblError.Text = "Login details are incorrect.Username/password is not matching.";
                    }
                }
                catch
                {
                    // case 4b.1
                    lblError.Text = "Unable to connect to the database.Database error.Please contact administrator.";
                }
            }
        }
    }
}