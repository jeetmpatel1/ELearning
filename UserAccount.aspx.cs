using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Elearning
{
    public partial class UserAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
             * (UC003) Display User Account
             * 1a: User not logged in.
                .1: Redirect user to UserLogin.aspx.
             */

            // case 1a.1
            if (Session.Count == 0)
            {
                Response.Redirect("~/UserLogin.aspx");
            }
            else
            {
                // CHeck user changed password just now, If QueryString in the session is set, show the message for vlaid Passwod change.
                if (Request.QueryString.HasKeys() && Request.QueryString["pwdChange"].Equals("true") )
                {
                    lblUpdateSuccess.Text = "Password Changed Successfully.";
                    
                }

                if (Page.IsPostBack)
                {
                    lblUpdateSuccess.Text = "";
                }

                // Retrieve User name and show welcome message
                string user_real_name = (string)Session["RealName"];
                lblWelcome.Text = "Welcome " + user_real_name;

                // Retrieve User Roles and show buttons accordingly
                int user_role_id = Int32.Parse(Session["RoleID"].ToString());

                // If User is Student, set visitbility to false for tutor's view componenets.
                if (user_role_id == 1) 
                {
                    lblTutorChangePassword.Visible = false;
                    lblUserAccount.Text = "Student Account";
                }

                // If User is Tutor, set visitbility to true for tutor's view componenets.
                if (user_role_id == 2) 
                {
                    btnUpdateTutorCourse.Visible = true;
                    btnUserDetails.Text = "Tutor Details";
                    lblUserAccount.Text = "Tutor Account";
                    btnUserDetails.PostBackUrl = "~/TutorDetails.aspx";
                }
            }
        }
    }
}