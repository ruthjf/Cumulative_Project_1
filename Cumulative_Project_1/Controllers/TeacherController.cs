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
        // GET: Teacher/List/
        public ActionResult List(string SearchKey = null, int? SearchKeyR = null)
        {
            // debug to ensure searchkey value is being picked up
            Debug.WriteLine("The inputted search key is ");
            Debug.WriteLine(SearchKey);
            Debug.WriteLine("The inputted search key range is ");
            Debug.WriteLine(SearchKeyR);

            // instantiating TeacherDataController to access information
            TeacherDataController Controller = new TeacherDataController();

            // access ListTeachers() method from api controller augmented with search parameters
            IEnumerable<Teacher> Teachers = Controller.ListTeachers(SearchKey, SearchKeyR);


            return View(Teachers);
        }

        // GET: Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            //pass id to TeacherData Controller
            // instantiating TeacherDataController to pass information
            TeacherDataController Controller = new TeacherDataController();

            Teacher SelectedTeacher = Controller.FindTeacher(id);

            return View(SelectedTeacher);
        }


        // GET: Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            //pass id to TeacherData Controller
            // instantiating TeacherDataController to pass information
            TeacherDataController Controller = new TeacherDataController();

            Teacher SelectedTeacher = Controller.FindTeacher(id);

            return View(SelectedTeacher);
        }

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

            return RedirectToAction("List");
        }
    }

}