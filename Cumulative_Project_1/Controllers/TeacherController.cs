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
        // GET: Teacher/List/{SearchKey}
        public ActionResult List(string SearchKey = null, int SearchKeyR = 0)
        {
            // debug to ensure searchkey value is being picked up
            Debug.WriteLine("The inputted search key is ");
            Debug.WriteLine(SearchKey);
            Debug.WriteLine("The inputted search key range is ");
            Debug.WriteLine(SearchKeyR);

            // instantiating TeacherDataController to access information
            TeacherDataController Controller = new TeacherDataController();

            // access ListTeachers() method from api controller
            IEnumerable<Teacher> Teachers = Controller.ListTeachers(SearchKey,SearchKeyR);


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
    }
}