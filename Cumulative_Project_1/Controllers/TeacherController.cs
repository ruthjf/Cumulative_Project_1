using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cumulative_Project_1.Models;
using System.Diagnostics;

namespace Cumulative_Project_1.Controllers
{
    public class TeacherController : Controller
    {
        /// <summary>
        /// Returns a list of teachers from the database
        /// </summary>
        /// <param name="SearchKey">Optional string search key</param>
        /// <returns>A list of teachers from the database or relevant a match for the searchkey</returns>
        // GET: Teacher/List/
        public ActionResult List(string SearchKey = null)
        {
            // debug to ensure searchkey value is being picked up
            Debug.WriteLine("The inputted search key is ");
            Debug.WriteLine(SearchKey);

            // instantiating TeacherDataController to access information
            TeacherDataController Controller = new TeacherDataController();

            // access ListTeachers() method from api controller augmented with search parameters
            IEnumerable<Teacher> Teachers = Controller.ListTeachers(SearchKey);


            return View(Teachers);
        }

        /// <summary>
        /// Returns teacher information with the corresponding TeacherId
        /// </summary>
        /// <param name="id">TeacherId</param>
        /// <returns>Teacher information</returns>
        // GET: Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            //pass id to TeacherData Controller
            // instantiating TeacherDataController to pass information
            TeacherDataController Controller = new TeacherDataController();
            Teacher SelectedTeacher = Controller.FindTeacher(id);

            return View(SelectedTeacher);
        }

        /// <summary>
        /// Reroutes to a page to confirm the deletion of a teacher
        /// </summary>
        /// <param name="id">TeacherId</param>
        /// <returns>Delete confirmation page</returns>
        // GET: Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            //pass id to TeacherData Controller
            // instantiating TeacherDataController to pass information
            TeacherDataController Controller = new TeacherDataController();

            Teacher SelectedTeacher = Controller.FindTeacher(id);

            return View(SelectedTeacher);
        }

        /// <summary>
        /// Reroutes to the List of teachers when delete is confirmed
        /// </summary>
        /// <param name="id">TeacherId</param>
        /// <returns>List of teachers</returns>
        //POST: /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {

            // instantiating TeacherDataController to pass information
            TeacherDataController Controller = new TeacherDataController();

            // execute method
            Controller.DeleteTeacher(id);

            // returns to the list of teachers
            return RedirectToAction("List");
        }


        //GET: /Teacher/New
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// Routes to create new teacher and inputs teacher information
        /// </summary>
        /// <param name="TeacherFname">New teacher first name</param>
        /// <param name="TeacherLname">New teacher last name</param>
        /// <param name="EmployeeNumber">New teacher employee number</param>
        /// <param name="HireDate">New teacher hire date</param>
        /// <param name="TeacherSalary">New teacher salary</param>
        /// <returns>List of teachers with the new teacher</returns>
        //POST: /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, decimal TeacherSalary)
        {
            //identify the inputs from the form

            Debug.WriteLine("This is the Create Method");
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(EmployeeNumber);
            Debug.WriteLine(HireDate);
            Debug.WriteLine(TeacherSalary);

            // teacher object to pass information gathered from form
            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.TeacherSalary = TeacherSalary;

            //access database and add new teacher
            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);

            // returns to list of teachers
            return RedirectToAction("List");
        }



        /// <summary>
        /// Routes to Teacher/Update page and gathers information from the database
        /// </summary>
        /// <param name="id">Teacher ID</param>
        /// <returns>A dynamic Teacher/Update page that provides the current information of the teacher and asks for new user input through a form</returns>
        /// <example>GET: /Teacher/Update/{id}</example>
        [HttpGet]
        public ActionResult Update(int id)
        {

            // this view is expecting a teacher info
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id); //pass teacherid

            return View(SelectedTeacher);
        }



        /// <summary>
        /// POST request containing new information about a teacher in the database. Redirects to Teacher/Show page.
        /// </summary>
        /// <param name="id">Teacher ID</param>
        /// <param name="TeacherFname">Updated teacher first name</param>
        /// <param name="TeacherLname">Updated teacher last name</param>
        /// <param name="EmployeeNumber">Updated teacher first name</param>
        /// <param name="HireDate">Updated hire date</param>
        /// <param name="TeacherSalary">Updated teacher salary</param>
        /// <returns>A webpage that returns the updated information of the teacher</returns>
        /// <example>
        /// POST: /Teacher/Update/1
        /// POST DATA / FORM DATA / REQUEST BODY
        /// {
        /// "TeacherFname":"Alex",
        /// "TeacherLname":"Bennett",
        /// "EmployeeNumber":"T123",
        /// "HireDate":"2001-01-01",
        /// "TeacherSalary":"44.10"
        /// }
        /// </example>
        // POST: /Teacher/Update/{id}
        // binds the teacher data submitted by the user and calls the data access method
        [HttpPost]
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, decimal TeacherSalary)
        {

            Debug.WriteLine("The teacher name is " + TeacherFname);

            Teacher UpdatedTeacherInfo = new Teacher();
            UpdatedTeacherInfo.TeacherFname = TeacherFname;
            UpdatedTeacherInfo.TeacherLname = TeacherLname;
            UpdatedTeacherInfo.EmployeeNumber = EmployeeNumber;
            UpdatedTeacherInfo.HireDate = HireDate;
            UpdatedTeacherInfo.TeacherSalary = TeacherSalary;

            //call the data access logic to update this teacher
            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, UpdatedTeacherInfo);

            // redirect to Show/{id}
            return RedirectToAction("Show/" + id);
        }
    }

}