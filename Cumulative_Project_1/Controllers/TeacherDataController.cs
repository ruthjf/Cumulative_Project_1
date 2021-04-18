using Cumulative_Project_1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;

namespace Cumulative_Project_1.Controllers
{
    public class TeacherDataController : ApiController
    {
        //create instance of class as an object to allow access to MySQL Databse
        private SchoolDbContext SchoolDb = new SchoolDbContext();

        // controller will access teachers and classes tables from schooldb database
        /// <summary>
        /// Returns a list of Teachers in the database matching the search key/search key range
        /// </summary>
        /// <returns>
        /// A list of Teacher objects
        /// </returns>
        /// GET api/TeacherData/ListTeachers/{SearchKey}/{SearchKeyR}
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}/{SearchKeyR?}")]
        public List<Teacher> ListTeachers(string SearchKey = null, int? SearchKeyR = null)
        {
            // ensure access search key
            Debug.WriteLine("Searching for a key of ");
            Debug.WriteLine(SearchKey);
            Debug.WriteLine("Searching for a key range of ");
            Debug.WriteLine(SearchKeyR);

            // instance of connection
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            // access open method
            Conn.Open();

            // establishing a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            // command object property SQL Query
            // search matches teacher first name, last name via SearchKey and salary via RangeSearch
            cmd.CommandText = "Select * from teachers left join classes ON classes.classid = teachers.teacherid where lower(teacherfname) like lower(@searchkey) or lower(teacherlname) like lower(@searchkey) or concat(teacherfname, ' ', teacherlname) like lower(@searchkey) or salary = @rangesearch";

            // adding parameters for security and defining @searchkey and @rangesearch
            cmd.Parameters.AddWithValue("@searchkey", "%" + SearchKey + "%");
            cmd.Parameters.AddWithValue("@rangesearch", SearchKeyR);
            cmd.Prepare();

            // result of SQL Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // create an empty list of Teacher names
            List<Teacher> Teachers = new List<Teacher> { };

            while (ResultSet.Read())
            {
                // instantiating a new Teacher object
                Teacher NewTeacher = new Teacher();
                Course NewCourse = new Course();

                //set properties of objects
                NewTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                NewTeacher.TeacherFname = ResultSet["teacherfname"].ToString();
                NewTeacher.TeacherLname = ResultSet["teacherlname"].ToString();
                NewTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
                NewTeacher.HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                NewTeacher.TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);

                NewCourse.className = Convert.ToString(ResultSet["classname"]);
                NewCourse.classCode = Convert.ToString(ResultSet["classcode"]);

                // adding teacher information to empty string
                Teachers.Add(NewTeacher);

            }
            // closing the connection to database
            Conn.Close();

            // return teacher information
            return Teachers;

        }

        /// <summary>
        /// Returns a teacher's information by id
        /// </summary>
        /// <returns>A teacher's information</returns>
        /// <example>
        /// GET api/TeacherData/ShowTeacher/{id}
        /// </example>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{teacherid}")]
        public Teacher FindTeacher(int teacherid)
        {
            // instance of connection
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            // access open method
            Conn.Open();

            // establishing a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            // command object property SQL Query
            cmd.CommandText = "Select * from teachers join classes ON classes.classid = teachers.teacherid where teachers.teacherid= " + teacherid;

            // result of SQL Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // create a variable for one teacher
            Teacher SelectedTeacher = new Teacher();

            while (ResultSet.Read())
            {

                //set properties of object
                SelectedTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                SelectedTeacher.TeacherFname = ResultSet["teacherfname"].ToString();
                SelectedTeacher.TeacherLname = ResultSet["teacherlname"].ToString();
                SelectedTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
                SelectedTeacher.HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                SelectedTeacher.TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);
                // SelectedTeacher.ClassName = Convert.ToString(ResultSet["classname"]);
                // SelectedTeacher.ClassCode = Convert.ToString(ResultSet["classcode"]);

            }
            // closing the connection to database
            Conn.Close();

            // return teacher information
            return SelectedTeacher;
        }


        /// <summary>
        /// Deletes a teacher from the database
        /// </summary>
        /// <param name="id">Teacher ID</param>
        /// <example>POST: /api/TeacherData/DeleteTeacher/3</example>
        
        [HttpPost]
        [Route("api/TeacherData/DeleteTeacher/{id}")]
        public void DeleteTeacher(int id)
        {
           // instance of connection
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            // access open method
            Conn.Open();

            // establishing a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL Query
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //execute statements other than select
            cmd.ExecuteNonQuery();

            //close connection
            Conn.Close();
        }


        /// <summary>
        /// Adds a teacher to the database
        /// </summary>
        /// <param name="NewTeacher">New Teacher Object</param>
        /// <example>POST:/api/TeacherData/AddTeacher/ </example>
       
        [HttpPost]
        public void AddTeacher([FromBody]Teacher NewTeacher)
        {
            // instance of connection
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            // access open method
            Conn.Open();

            // establishing a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL Query
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherFname, @TeacherLname, @EmployeeNumber, @HireDate, @TeacherSalary)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@TeacherSalary", NewTeacher.TeacherSalary);
            cmd.Prepare();

            //execute statements other than select
            cmd.ExecuteNonQuery();

            //close connection
            Conn.Close();
        }

    }
}
        
        
