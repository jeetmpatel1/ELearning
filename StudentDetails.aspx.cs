using Elearning.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Elearning
{
    public partial class StudentDetails : System.Web.UI.Page
    {

        //Declare Instance of Middle Class
        Courses courseTakenByCurrentUser = new Courses();
        UserTable currentUser = new UserTable();
        Modules moduleObject = new Modules();


        protected void Page_Load(object sender, EventArgs e)
        {


            /* 
             * (UC008A) Load Student Details
             * 1a: User not logged in.
                .1: Redirect user to UserLogin.aspx.
               2a: User not a Student.
                .1: Redirect user to UserAccount.aspx.
               4a: Catch database error.
                .1: Output appropriate error message.
               5a: Catch database error.
                .1: Output appropriate error message.
            */

            if (!IsPostBack)
            {
                // If User is not logged in, redirect to UserLogin.aspx
                if (Session.Count == 0)
                {
                    Response.Redirect("~/UserLogin.aspx");
                }

                // If User is logged in , check it is valid Student or not. If it is Tutor, redirect to UserAccount.aspx
                String usersRole = HttpContext.Current.Session["RoleID"].ToString();
                if (!usersRole.Equals("1"))
                {
                    Response.Redirect("~/UserAccount.aspx");
                }

                try
                {
                    // retrieve course from middle layer into a DataTable
                    courseTakenByCurrentUser.course_id = int.Parse(HttpContext.Current.Session["CourseID"].ToString());
                    DataTable courseNameResult = courseTakenByCurrentUser.getCourseNamesFromCourseId();

                    currentUser.user_course_id = int.Parse(HttpContext.Current.Session["CourseID"].ToString());
                    currentUser.user_role_id = int.Parse(HttpContext.Current.Session["RoleID"].ToString());
                    
                    // set the course name to the view componenet.
                    if (courseNameResult != null)
                    {
                        //retrieve coursename from the result and update the label.
                        txtCourse.Text = courseNameResult.Rows[0]["CourseName"].ToString();
                    }
                    else
                    {
                        lblError.Text = "Can not find Student's course in the database.";
                    }

                    // Query for the Tutor name and real name.
                    DataTable tutorNameResult = currentUser.getTutorsNameFromCourseId();
                    if (tutorNameResult != null)
                    {
                        // Get and update the view componenets from the result object.
                        lstTutors.DataSource = tutorNameResult;
                        lstTutors.DataTextField = "RealName";
                        lstTutors.DataValueField = "UserID";
                        lstTutors.DataBind();
                    }
                    else
                    {
                        lblError.Text = "Can not find Tutor's details for given course in the database.";
                    }

                    moduleObject.course_id = Int32.Parse(Session["CourseID"].ToString());
                    
                    // bind the modules adata to the result.
                    DataTable allModulesOfCOurse = moduleObject.bindModules();

                    // if result is not null
                    if (allModulesOfCOurse != null)
                    {
                        rptModules.DataSource = allModulesOfCOurse;
                        rptModules.DataBind();
                    }
                }
                catch
                {
                    lblError.Text = "Unable to connect to the database.Database error.Please contact administrator.";
                }
            }
        }

        protected void btnShowEmail_Click(object sender, EventArgs e)
        {

            /*
             * (UC008B) Show Tutor Email Address
             * 1a: No item selected in ListBox.
                .1: Output appropriate error message.
             * 
            */
            // case 1a.1
            if (lstTutors.SelectedItem == null)
            {
                lblError.Text = "No item selected in ListBox.Please select a Tutor to display Email";
                return;
            }
            else
            {
                if (lstTutors.SelectedItem.Text == null)
                {
                    lblError.Text = " ";
                }
                else
                {
                    try
                    {
                        currentUser.user_id = int.Parse(lstTutors.SelectedValue);
                        lblEmail.Text = currentUser.getTutorsEmailAddressFromUserId();
                    }
                    catch
                    {
                        lblError.Text = "Unabel to retrieve Email label from Database. Contact the administrator.";
                    }
                }
            }
        }
    }
}