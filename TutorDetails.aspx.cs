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
    public partial class TutorDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            /*
             * (UC005A) Load Tutor Details
             * 1a: User not logged in.
                .1: Redirect user to UserLogin.aspx.
               2a: User not a Tutor.
                .1: Redirect user to UserAccount.aspx.
               4a: Catch database error.
                .1: Output appropriate error message.
               5a: Catch database error.
                .1: Output appropriate error message.
            */
            try
            {

                // case 1a.1
                if (Session.Count == 0)
                {
                    Response.Redirect("~/UserLogin.aspx");
                }

                // case 2a.1
                String RoleId = HttpContext.Current.Session["RoleID"].ToString();
                if (!RoleId.Equals("2"))
                {
                    Response.Redirect("~/UserAccount.aspx");
                }

                // User is tutor.
                //if request is NOT a post back
                if (!Page.IsPostBack)
                {
                    // Create course object 
                    Courses courseObject = new Courses();
                    
                    // get the course id into session parameter
                    courseObject.course_id = int.Parse(HttpContext.Current.Session["CourseID"].ToString());
                    
                    // Query for the course . get all the course names for given tutor
                    DataTable courseName = courseObject.getCourseNamesFromCourseId();

                    if (courseName != null)
                    {
                        // Set the lable for the course on view componenet
                        txtCourse.Text = courseName.Rows[0]["CourseName"].ToString();
                    }
                    else
                    {
                        lblError.Text = "Given Tutor doesn't teach any course";
                    }

                    // get all the students who have enrolled into course set into session variable
                    DataTable allStudentsSubscribedToCourse = courseObject.getOnlyStudentsAndCourses();

                    // Query for the Students. get all the students who are getiting same course.
                    if (allStudentsSubscribedToCourse != null)
                    {
                        lstStudents.DataSource = allStudentsSubscribedToCourse;
                        lstStudents.DataTextField = "RealName";
                        lstStudents.DataValueField = "UserID";
                        lstStudents.DataBind();

                    }
                    else
                    {
                        lblError.Text = "NO Student has taking the course.";
                    }
                }
            }
            catch
            {
                // case 4a.1 4a.2
                lblError.Text = "Unable to connect to the database.Database error.Please contact administrator.";
            }
        }

        protected void btnRemoveStudent_Click(object sender, EventArgs e)
        {
            /*
             * (UC005B) Remove Student
             *  1a: No item selected in ListBox.
                  .1: Output appropriate error message.
                2a: Catch database error.
                  .1: Output appropriate error message.
            */
            try
            {
                // case 1a.1
                if (lstStudents.SelectedItem.Text == null)
                {
                    lblError.Text = "No Student is selected. Please select a Student to remove.";
                }
                else
                {
                    // create user object
                    UserTable currentUserToBeRemoved = new UserTable();
                    currentUserToBeRemoved.user_id = Int32.Parse(lstStudents.SelectedValue);
                    
                    // delete the given user
                    DataTable userRemoveDBResult = currentUserToBeRemoved.deleteUserByUserId();
                    if (userRemoveDBResult != null)
                    {
                        lblSuccess.Text = "Student Successfully Removed";
                        
                        // Remove the selected student from View componenet
                        lstStudents.Items.RemoveAt(lstStudents.SelectedIndex);
                    }
                    else
                    {
                        lblError.Text = "Unable to remove student";
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