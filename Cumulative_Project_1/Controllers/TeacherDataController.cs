using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cumulative_Project_1.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

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
        [Route("api/TeacherData/ListTeachers/{SearchKey}/{SearchKeyR}")]
        public List<Teacher> ListTeachers(string SearchKey, int SearchKeyR)
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
            cmd.CommandText = "Select * from teachers join classes ON classes.classid = teachers.teacherid where teacherfname like @searchkey or teacherlname like @searchkey and salary = @rangesearch";

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

                //set properties of objects
                NewTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                NewTeacher.TeacherName = ResultSet["teacherfname"] + " " + ResultSet["teacherlname"];
                NewTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
                NewTeacher.HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                NewTeacher.TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);
                NewTeacher.ClassName = Convert.ToString(ResultSet["classname"]);
                NewTeacher.ClassCode = Convert.ToString(ResultSet["classcode"]);

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
                SelectedTeacher.TeacherName = ResultSet["teacherfname"] + " " + ResultSet["teacherlname"];
                SelectedTeacher.EmployeeNumber = ResultSet["employeenumber"].ToString();
                SelectedTeacher.HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                SelectedTeacher.TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);
                SelectedTeacher.ClassName = Convert.ToString(ResultSet["classname"]);
                SelectedTeacher.ClassCode = Convert.ToString(ResultSet["classcode"]);

            }
            // closing the connection to database
            Conn.Close();

            // return teacher information
            return SelectedTeacher;
        }
    }
}
        
        
