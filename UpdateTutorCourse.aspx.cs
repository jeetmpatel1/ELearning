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
    public partial class UpdateTutorCourse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            /*
             * (UC007A) Load Tutor Courses
             * 1a: User not logged in.
                .1: Redirect user to UserLogin.aspx.
               2a: User not a Tutor.
                .1: Redirect user to UserAccount.aspx.
               4a: Catch database error.
                .1: Output appropriate error message.
            */

            // case 1a.1
            if (Session.Count == 0)
            {
                Response.Redirect("~/UserLogin.aspx");
            }

            // case 2a.1
            String roleIdOfCurrentUser = HttpContext.Current.Session["RoleID"].ToString();
            if (!roleIdOfCurrentUser.Equals("2"))
            {
                Response.Redirect("~/UserAccount.aspx");
            }

            try
            {
                //if request is NOT a post back
                if (!Page.IsPostBack)
                {
                    // Create course object and set the valid details
                    Courses courseObject = new Courses();
                    courseObject.course_id = int.Parse(HttpContext.Current.Session["CourseID"].ToString());
                   
                    // Get the course Name and set the text compononet on view side
                    DataTable courseNames = courseObject.getCourseNamesFromCourseId();
                    if (courseNames != null)
                    {
                        txtCourse.Text = courseNames.Rows[0]["CourseName"].ToString();
                    }
                }
            }
            catch
            {
                // case 4a.1
                lblError.Text = "Unable to connect to the database.Database error.Please contact administrator.";
            }

            try
            {
                if (!Page.IsPostBack)
                {
                    Courses courseObject = new Courses();

                    // Get all the courses and bind those to the view componenet.
                    DataTable allCourses = courseObject.getCourses();

                    if (allCourses != null)
                    {
                        // set and bind the properties for view objects. 
                        lstCourses.DataSource = allCourses; 
                        lstCourses.DataValueField = "CourseID";
                        lstCourses.DataTextField = "CourseName";
                        lstCourses.DataBind();
                    }
                }
            }
            catch
            {
                // case 4a.1
                lblError.Text = "Unable to connect to the database.Database error.Please contact administrator.";
            }
        }

        protected void btnUpdateCourse_Click(object sender, EventArgs e)
        {

            /*
             * (UC007B) Update Tutor Course
                1a: No item selected in ListBox.
                  .1: Output appropriate error message.
                2a: Catch database error.
                  .1: Output appropriate error message.
            */
            try
            {
                // if no course selected then throw error into the label.
                // case 1a.1
                if (lstCourses.SelectedIndex == -1)
                {
                    lblError.Text = "No Course Selected. Select a course to Update."; 
                }
                // 
                else
                {
                    // Create the user object and set the parametrs
                    UserTable userToBeUpdated = new UserTable();
                    userToBeUpdated.user_id = int.Parse(HttpContext.Current.Session["UserID"].ToString());
                    userToBeUpdated.user_course_id = int.Parse(lstCourses.SelectedValue);
                    

                    // Update the course into method.
                    if (userToBeUpdated.updateCourseForStudentByUserIdAndCourseId())
                    {
                        string CourseID = (string)Session["CourseID"];
                        System.Threading.Thread.Sleep(4000);
                        txtCourse.Text = userToBeUpdated.getCourseNameUsingCourseID();
                        lblError.Text = "Course Updated Successfully";
                        lblError.ForeColor = System.Drawing.Color.Blue;
                    }
                    else
                    {
                        //exception thrown so display error
                        lblError.Text = "Unable to update a course.";
                    }

                }
            }
            catch
            {
                // case 2a.1
                lblError.Text = "Unable to connect to the database.Database error.Please contact administrator.";
            }
        }
    }
}