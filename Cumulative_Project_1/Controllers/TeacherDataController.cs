using Cumulative_Project_1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Cors;

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
        /// GET api/TeacherData/ListTeachers/{SearchKey}
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        [EnableCors(origins:"*", methods:"*", headers:"*")]
        public List<Teacher> ListTeachers(string SearchKey = null)
        {
            // ensure access search key
            Debug.WriteLine("Searching for a key of ");
            Debug.WriteLine(SearchKey);

            // instance of connection
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            // access open method
            Conn.Open();

            // establishing a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            // command object property SQL Query
            // search matches teacher first name, last name via SearchKey and salary via RangeSearch
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@searchkey) or lower(teacherlname) like lower(@searchkey) or concat(teacherfname, ' ', teacherlname) like lower(@searchkey)";

            // adding parameters for security and defining @searchkey
            cmd.Parameters.AddWithValue("@searchkey", "%" + SearchKey + "%");
            cmd.Prepare();

            // result of SQL Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // create an empty list of Teacher names
            List<Teacher> Teachers = new List<Teacher> { };

            while (ResultSet.Read())
            {
                // instantiating a new Teacher object
                Teacher NewTeacher = new Teacher();

                //access column info
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal TeacherSalary = (decimal)ResultSet["salary"];


                //set properties of objects
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.TeacherSalary = TeacherSalary;


                // adding teacher information to empty string
                Teachers.Add(NewTeacher);

            }
            // closing the connection to database
            Conn.Close();

            // return teacher information
            return Teachers;

        }

        /// <summary>
        /// Finds a teacher's information through an id
        /// </summary>
        /// <param name="id">Teacher Id</param>
        /// <returns>Teacher object containing matching information about the teacher that has a matching id. If there is not match, it will return a empty teacher object.</returns>
        /// <example>
        /// GET: api/TeacherData/FindTeacher/12 --> {Teacher Object}
        /// </example>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{id}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public Teacher FindTeacher(int id)
        {
            // instance of connection
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            // access open method
            Conn.Open();

            // establishing a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            // command object property SQL Query
            cmd.CommandText = "Select * from teachers where teacherid=@id";
            // parameter for teacherid
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            // result of SQL Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // create a variable for one teacher
            Teacher SelectedTeacher = new Teacher();

            while (ResultSet.Read())
            {

                //access column info
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal TeacherSalary = (decimal)ResultSet["salary"];

                //set properties of object
                SelectedTeacher.TeacherId = TeacherId;
                SelectedTeacher.TeacherFname = TeacherFname;
                SelectedTeacher.TeacherLname = TeacherLname;
                SelectedTeacher.EmployeeNumber = EmployeeNumber;
                SelectedTeacher.HireDate = HireDate;
                SelectedTeacher.TeacherSalary = TeacherSalary;

            }
            // closing the connection to database
            Conn.Close();

            // return teacher information
            return SelectedTeacher;
        }


        /// <summary>
        /// Deletes a teacher from the database with a matching id. Does not maintain relational integrity. Non-deterministic.
        /// </summary>
        /// <param name="id">Teacher ID</param>
        /// <example>POST: /api/TeacherData/DeleteTeacher/3</example>
        
        [HttpPost]
        [Route("api/TeacherData/DeleteTeacher/{id}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
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
        /// Adds a teacher to the database. Non-Deterministic.
        /// </summary>
        /// <param name="NewTeacher">New Teacher Object with fields matching the columns of teacher table in MySQL database. </param>
        /// <example>POST:/api/TeacherData/AddTeacher
        /// POST DATA / FORM DATA / REQUEST BODY
        /// {
        /// "TeacherFname":"Alex",
        /// "TeacherLname":"Bennett",
        /// "EmployeeNumber":"T123",
        /// "HireDate":"2001-01-01",
        /// "TeacherSalary":"44.10"
        /// }
        /// </example>

        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody]Teacher NewTeacher)
        {

            Debug.WriteLine(NewTeacher.TeacherFname);

            // instance of connection
            MySqlConnection Conn = SchoolDb.AccessDatabase();


            // access open method
            Conn.Open();

            // establishing a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL Query
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherFname, @TeacherLname, @EmployeeNumber, @HireDate, @TeacherSalary)";

            //parameterized values
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

        /// <summary>
        /// Updates a teacher on the database. Non-Deterministic.
        /// </summary>
        /// <param name="NewTeacher">New Teacher Object</param>
        /// <param name="TeacherInfo">Teacher object with fields matching the columns of teacher table in MySQL database. </param></param>
        /// <example>POST:/api/TeacherData/UpdateTeacher/11
        /// POST DATA / FORM DATA / REQUEST BODY
        /// {
        /// "TeacherFname":"Alex",
        /// "TeacherLname":"Bennett",
        /// "EmployeeNumber":"T123",
        /// "HireDate":"2001-01-01",
        /// "TeacherSalary":"44.10"
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateTeacher(int id, [FromBody]Teacher TeacherInfo)
        {
            // instance of connection
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            // access open method
            Conn.Open();

            // establishing a new command for database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL Query
            cmd.CommandText = "update teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname, employeenumber= @EmployeeNumber, hiredate=@HireDate, salary=@TeacherSalary where teacherid=@TeacherId";
            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", TeacherInfo.HireDate);
            cmd.Parameters.AddWithValue("@TeacherSalary", TeacherInfo.TeacherSalary);
            cmd.Parameters.AddWithValue("@TeacherId", id);
            cmd.Prepare();

            //execute statements other than select
            cmd.ExecuteNonQuery();

            //close connection
            Conn.Close();


        }
    }
}
        
        
