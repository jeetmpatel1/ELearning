using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web;
#pragma warning disable CS0436

namespace Elearning.App_Code
{
    public class UserTable
    {

        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_password { get; set; }
        public string user_real_name { get; set; }
        public string user_email_address { get; set; }
        public int user_role_id { get; set; }
        public int user_course_id { get; set; }

        private DatabaseConnection database_connection;

        public UserTable()
        {
            database_connection = new DatabaseConnection();
        }

        // method to retrieve and set if user is valid user. it sets the user details in session objects. If user not found, it sends false to caller.
        public bool isValidUser()
        {
            database_connection.addParameter("@user_name", user_name);
            database_connection.addParameter("@user_password", user_password);

            String hashedPassword = hashPassword(user_password);
            database_connection.addParameter("@hashed_password", hashedPassword);

            string sqlQueryTocheckValidUser = "SELECT USERID, REALNAME, ROLEID, COURSEID FROM USERS " +
                            "WHERE (USERNAME=@user_name AND USERPASSWORD=@user_password) OR " +
                            "(USERNAME=@user_name AND USERPASSWORD=@hashed_password)";
            
            DataTable result = database_connection.executeReader(sqlQueryTocheckValidUser);

            if (result.Rows.Count == 1)
            {
                HttpContext.Current.Session["UserID"] = result.Rows[0]["UserID"].ToString();
                HttpContext.Current.Session["RealName"] = result.Rows[0]["RealName"].ToString();
                HttpContext.Current.Session["RoleID"] = result.Rows[0]["RoleID"].ToString();
                HttpContext.Current.Session["CourseID"] = result.Rows[0]["CourseID"].ToString();
                return true;
            }
            return false;

        }

        // This mehtod is used to hash the password with the help of MD5 ( Message Digest algorithm )
        static string hashPassword(string password)
        {
            using (MD5CryptoServiceProvider md5Digest = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8Encoding = new UTF8Encoding();
                byte[] data = md5Digest.ComputeHash(utf8Encoding.GetBytes(password));
                return Convert.ToBase64String(data);
            }
        }

        // Method to check if user name is available to be registered
        public bool isUserNameAvailable()
        {
            database_connection.addParameter("@user_name", user_name);

            string queryTOCheckUserName = "SELECT COUNT(USERNAME) FROM USERS WHERE USERNAME=@user_name";

            int countOfRows = database_connection.executeScalar(queryTOCheckUserName); //result of count

            if (countOfRows == 0)
                return true;
            return false;
        }

        // Method to insert new student into 
        public bool insertNewStudent()
        {
            database_connection.addParameter("@user_name", user_name);
            database_connection.addParameter("@User_Password", user_password);
            database_connection.addParameter("@Real_Name", user_real_name);
            database_connection.addParameter("@Email_Address", user_email_address);
            database_connection.addParameter("@RoleID", 1);
            database_connection.addParameter("@CourseID", user_course_id);

            string queryTOInsertUser = "INSERT INTO USERS (USERNAME, USERPASSWORD, REALNAME, EMAILADDRESS, ROLEID, COURSEID) VALUES (@user_name, @user_Password, @Real_Name, @Email_Address, @RoleID, @CourseID)";

            int updatedRowCount = database_connection.executeNonQuery(queryTOInsertUser);
            if (updatedRowCount > 0)
                return true;
            return false;
        }

        // method to get plain text password from id
        public string getUserPlainTextPasswordFromId()
        {
            database_connection.addParameter("@UserID", user_id);

            string queryToRetrievePasswordFromUserId = "SELECT USERPASSWORD FROM USERS WHERE USERID=@userID";

            DataTable table = database_connection.executeReader(queryToRetrievePasswordFromUserId);

            int rowCount = table.Rows.Count;
            if (rowCount > 0)
            {
                return table.Rows[0]["UserPassword"].ToString();
            }
            return "";
        }

        // method to update plain text password from id
        public bool updateUserPlainTextPasswordFromId()
        {
            database_connection.addParameter("@User_Password", user_password);
            database_connection.addParameter("@UserID", user_id);

            string queryTOUpdatePasswordFromUserId = "UPDATE USERS SET USERPASSWORD=@User_Password WHERE USERID=@UserID";
            int updatedRowsCount = database_connection.executeNonQuery(queryTOUpdatePasswordFromUserId);
            if (updatedRowsCount == 1)
                return true;
            return false;
        }

        // method to get tutor's name from Role iD
        public DataTable getTutorsNameFromCourseId()
        {
            string queryToRetrieveTutorDetails = "SELECT USERID,REALNAME FROM USERS WHERE ROLEID = @role_id AND COURSEID = @course_id";
            database_connection.addParameter("@role_id", 2);
            database_connection.addParameter("@course_id", user_course_id);
            return database_connection.executeReader(queryToRetrieveTutorDetails);
        }

        // method to get tutor's email address
        public string getTutorsEmailAddressFromUserId()
        {
            database_connection.addParameter("@UserID", user_id);
            String queryToRetrieveTutorEmailId = "SELECT EMAILADDRESS FROM USERS WHERE USERID = @UserID";
            DataTable dt = database_connection.executeReader(queryToRetrieveTutorEmailId);
            return dt.Rows[0].ItemArray[0].ToString();
            
        }

        // method to get Course Name from Course Id
        public string getCourseNameUsingCourseID()
        {
            database_connection.addParameter("@CourseID", user_course_id);
            // sql command to get the course name
            string queryToRetrieveCourse = "SELECT COURSENAME FROM COURSES WHERE COURSEID=@CourseID";
            // execute the sql command
            DataTable updatedRowCount = database_connection.executeReader(queryToRetrieveCourse);
            if (updatedRowCount.Rows.Count > 0)
            {
                return updatedRowCount.Rows[0]["CourseName"].ToString();
            }
            else
            {
                return "";
            }

        }

        // Method to update course for user
        public bool updateCourseForStudentByUserIdAndCourseId()
        {
            // set the parametere values
            database_connection.addParameter("@CourseID", user_course_id);
            database_connection.addParameter("@UserID", user_id);
            //sql command to update the course name
            string queryTOUpdateUserCourse = "UPDATE USERS SET COURSEID=@COURSEID WHERE USERID=@UserID";
            //execute the sql command
            return database_connection.executeNonQuery(queryTOUpdateUserCourse) > 0; //i.e. 1 or more rows affected
        }

        // method to delete user
        public DataTable deleteUserByUserId()
        {
            database_connection.addParameter("@UserID", user_id);
            string queryToDeleteUser = "DELETE FROM USERS WHERE USERID=@UserID";
            return database_connection.executeReader(queryToDeleteUser);
        }
    }
}