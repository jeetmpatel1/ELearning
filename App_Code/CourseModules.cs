using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

#pragma warning disable CS0436

namespace Elearning.App_Code
{
    public class CourseModules
    {
        private DatabaseConnection database_connection;
        public int course_id { get; set; }
        public int module_id { get; set; }
        public CourseModules()
        {
            database_connection = new DatabaseConnection();
        }
    }
}