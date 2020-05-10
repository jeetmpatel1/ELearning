using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Elearning.App_Code;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Elearning
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        /*
          * (UC002A) Load Available Courses
          * 2a: Catch database error.
              .1: Output appropriate error message.
         */
            try
            {
                // Checking if request is valid POstBack or not
                if (!IsPostBack)
                {
                    
                    Courses course = new Courses();
                    // Get all the courses from database.
                    DataTable allCourseDataTable = course.getCourses();  
                    
                    // If courses retrieved are not null
                    if (allCourseDataTable != null)
                    {
                        // Set the datasource for componenet in presentation tier.
                        ddlCourses.DataSource = allCourseDataTable;

                        // Set the First Column.
                        ddlCourses.DataValueField = "CourseID";

                        // Set the Second Column.
                        ddlCourses.DataTextField = "CourseName";
                        
                        // Bind the retrieved componenet with its datasource.
                        ddlCourses.DataBind();
                    }
                }
            }
            catch
            {
                // case 2a.1
                lblError.Text = "Unable to load Available Courses. Database Errror. Please contact administrator.";
            }

        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {

            /*
             *  (UC002B) Register Student
             *  2a: Entered username length is less than 6 or greater than 20 characters.
                    .1: Output appropriate error message.
                2b: Entered password length is less than 5 characters.
                    .1: Output appropriate error message.
                2c: Entered password confirmation not equal to password input.
                    .1: Output appropriate error message.
                2d: Entered real name field is empty.
                    .1: Output appropriate error message.
                2e: Entered email field is empty or the email does not contain the string "dmu1.ac.uk".
                    .1: Output appropriate error message.
                3a: Database query returns a record - username must already exist.
                    .1: Output appropriate error message.
                4a: Catch database error.
                    .1: Output appropriate error message. 
            */

            // case 2a.1 
            if (txtUsername.Text.Length < 6 || txtUsername.Text.Length > 20)
            {
                lblError.Text = "Username length should be between 6 to 20 characters including both.";
            }

            // case 2b.1
            else if (txtPassword.Text.Length < 5)
            {
                lblError.Text = "Passsword length should be greater than or equal to 5 characters";
            }

            // case 2c.1
            else if (!txtConfirmPassword.Text.Equals(txtPassword.Text))
            {
                lblError.Text = "Password and Confirm Password fields do not match.";
            }

            // case 2d.1
            else if (txtRealName.Text.Equals(""))
            {
                lblError.Text = "Entered Real Name is empty";
            }

            // case 2e.1
            else if (txtEmailAddress.Text.Equals("") || !txtEmailAddress.Text.Contains("dmu1.ac.uk"))
            {
                lblError.Text = "Entered email field is empty or the email does not contain the string dmu1.ac.uk";
            }
            else
            {
                try
                {
                    // Create a user object.
                    UserTable userToBeRegistered = new UserTable();

                    //set username property, so it  can be used as a parameter for the query
                    userToBeRegistered.user_name = txtUsername.Text;
                    userToBeRegistered.user_email_address = txtEmailAddress.Text;

                    //check if the username exists
                    if (userToBeRegistered.isUserNameAvailable())
                    {
                        // Valid User. set all the required properties to create a user.
                        userToBeRegistered.user_name = txtUsername.Text;
                        userToBeRegistered.user_password = hashPassword(txtPassword.Text);
                        userToBeRegistered.user_real_name = txtRealName.Text;
                        userToBeRegistered.user_email_address = txtEmailAddress.Text;
                        userToBeRegistered.user_course_id = Int32.Parse(ddlCourses.SelectedValue);

                        bool isUserInsertedSuccessfully = userToBeRegistered.insertNewStudent();
                        if (isUserInsertedSuccessfully)
                        {
                            Response.Redirect("~/UserLogin.aspx");
                        }
                    }
                    else
                    {
                        // case 3a.1
                        lblError.Text = "Provided UserName is already taken. Please choose another username.";
                    }
                }
                catch
                {
                    // case 4a.1
                    lblError.Text = "Unable to connect to the database.Database error.Please contact administrator.";
                }

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