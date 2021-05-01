using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cumulative_Project_1.Models;
using MySql.Data.MySqlClient;
using System.Web.Http.Cors;


namespace Cumulative_Project_1.Controllers

{
    public class CourseDataController : ApiController
    {

        //create instance of class as an object to allow access to MySQL Databse
        private SchoolDbContext SchoolDb = new SchoolDbContext();

        /// <summary>
        /// Gets associated courses given an input of Teacher Id
        /// <param name="id">teacherid</param>
        /// </summary>
        [HttpGet]
        [Route("api/CourseData/ListCourses/{teacherid}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public IEnumerable<Course> ListCourses(int id)
        {
            // instance of connection
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            // access open method
            Conn.Open();

            // establishing a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            // command object property SQL Query
            cmd.CommandText = "Select teachers.teacherid, classcode, classname from teachers left join classes ON classes.teacherid = teachers.teacherid where classes.teacherid=@id";

            // adding parameters for security
            cmd.Parameters.AddWithValue("id", id);
            cmd.Prepare();

            // result of SQL Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // create an empty list of Course names
            List<Course> Courses = new List<Course> { };

            while (ResultSet.Read())
            {
                // instantiating a new Course object
                Course NewCourse = new Course();

                //access column info
                int TeacherId = (int)ResultSet["teacherid"];
                string ClassName = ResultSet["classname"].ToString();
                string ClassCode = ResultSet["classcode"].ToString();


                //set properties of objects
                NewCourse.TeacherId = TeacherId;
                NewCourse.ClassCode = ClassCode;
                NewCourse.ClassName = ClassName;


                // adding courses to empty string
                Courses.Add(NewCourse);

            }
            // closing the connection to database
            Conn.Close();

            // return teacher information
            return Courses;
        }
    }
}
