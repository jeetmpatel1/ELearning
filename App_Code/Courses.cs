using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
#pragma warning disable CS0436

namespace Elearning.App_Code
{
    public class Courses
    {
        private DatabaseConnection database_connection;

        public int course_id { get; set; }
        public string course_name { get; set; }

        public Courses()
        {
            database_connection = new DatabaseConnection();
        }

        // Method to get all the courses
        public DataTable getCourses()
        {
            string sqlQueryToRetrieveAllCourses = "SELECT * FROM COURSES";
            return database_connection.executeReader(sqlQueryToRetrieveAllCourses);
        }

        // Method to get course names from course id
        public DataTable getCourseNamesFromCourseId()
        {
            string sqlQueryToGetCourseNameFromCourseId = "SELECT COURSENAME FROM COURSES WHERE COURSEID = @course_id";
            database_connection.addParameter("@course_id", course_id);
            return database_connection.executeReader(sqlQueryToGetCourseNameFromCourseId);
        }

        // method to get student details from course id
        public DataTable getOnlyStudentsAndCourses()
        {
            string sqQueryToRetrieveOnlyStudents = "SELECT USERID,REALNAME FROM USERS WHERE ROLEID =  @role_id AND COURSEID = @course_id";
            database_connection.addParameter("@role_id", 1);
            database_connection.addParameter("@course_id", course_id);
            return database_connection.executeReader(sqQueryToRetrieveOnlyStudents);
        }
    }
}