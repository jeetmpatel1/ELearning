using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
#pragma warning disable 0436
namespace Elearning.App_Code
{

    public class Modules
    {
        public int module_id { get; set; }
        public int course_id { get; set; }
        public int module_code { get; set; }
        public string module_name { get; set; }

        private DatabaseConnection database_connection;

        public Modules()
        {
            database_connection = new DatabaseConnection();
        }

        // method to bind modules based on course id
        public DataTable bindModules()
        {
            database_connection.addParameter("@CourseID", course_id);
               
            string sqlQueryToGetModuleFromUserId = "SELECT * FROM MODULES WHERE MODULEID IN (SELECT MODULEID FROM COURSEMODULES WHERE COURSEID=@CourseID)";
            
            return database_connection.executeReader(sqlQueryToGetModuleFromUserId);
        }
    }
}