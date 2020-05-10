using Elearning.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Elearning
{
    public partial class UpdatePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            /*
             * (UC006) Update User Password
             * 1a: User not logged in.
                .1: Redirect user to UserLogin.aspx.
             */
            if (Session.Count == 0)
            {
                Response.Redirect("~/UserLogin.aspx");
            }
        }

        protected void btnUpdatePassword_Click(object sender, EventArgs e)
        {

            /*
             * (UC006) Update User Password
             *  4a: Entered current password length is less than 6 characters.
                 .1: Output appropriate error message.
                4b: Entered new password length is less than 6 characters.
                 .1: Output appropriate error message.
                4c: Entered new password confirmation not equal to new password input.
                 .1: Output appropriate error message.
                5a: Inputted current password does not match that in database for this user.
                 .1: Output appropriate error message.
                6a: Catch database error.
                 .1: Output appropriate error message.
             */
            try
            {
                // case 4a.1
                if (txtCurrentPassword.Text.Length < 6)
                {
                    lblError.Text = "Current Passsword length should be greater than or equal to 5 characters";
                }

                // case 4b.1
                else if (txtNewPassword.Text.Length < 6)
                {
                    lblError.Text = "New Passsword length should be greater than or equal to 5 characters";
                }

                // case 4c.1
                else if (!txtConfirmPassword.Text.Equals(txtNewPassword.Text))
                {
                    lblError.Text = "Password and Confirm Password fields do not match.";
                }

                else
                {
                    UserTable userObject = new UserTable();
                    userObject.user_id = Int32.Parse(Session["UserID"].ToString());

                    // get the actual password for the user from database.
                    string pwdFromDatabase = userObject.getUserPlainTextPasswordFromId();

                    // find out hte hashed string of password entered by user.
                    string hashOfPwdEnteredByUser = hashPassword(txtCurrentPassword.Text);
                    
                    // If database password equals to plain text of User entered password or Database password is equal to hashed password
                    if (pwdFromDatabase.Equals(txtCurrentPassword.Text) || pwdFromDatabase.Equals(hashOfPwdEnteredByUser))
                    {
                        // hash the new password by user
                        hashOfPwdEnteredByUser = hashPassword(txtNewPassword.Text);
                        
                        // set the password to new hashed passport.
                        userObject.user_password = hashOfPwdEnteredByUser;

                        // update user's password
                        if (userObject.updateUserPlainTextPasswordFromId())
                        {
                            System.Threading.Thread.Sleep(4000);
                            Response.Redirect("~/UserAccount.aspx?pwdChange=true");
                        }
                        else
                        {
                            lblError.Text = "Unable to Update the password. Please contact administrator";
                        }
                    }
                    else
                    {
                        // case 5a.1
                        lblError.Text = "Your Current Password is Incorrect. Please type the correct password.";
                    }
                }
            }
            catch
            {
                // case 6a.1
                lblError.Text = "Unable to connect to the database.Database error.Please contact administrator.";
            }
        }

        static string hashPassword(string password)
        {
            using (MD5CryptoServiceProvider md5Digest = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8Encoding = new UTF8Encoding();
                byte[] data = md5Digest.ComputeHash(utf8Encoding.GetBytes(password));
                return Convert.ToBase64String(data);
            }
        }
    }
    
}