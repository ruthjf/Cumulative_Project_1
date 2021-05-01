using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Cumulative_Project_1.Models
{
    public class Teacher
    {
        // fields defining a teacher
        public int TeacherId;
        public string TeacherFname;
        public string TeacherLname;
        public string EmployeeNumber;
        public DateTime HireDate;
        public decimal TeacherSalary;

        public bool IsValid()
        {
            bool valid = true;

            if (TeacherFname == null || TeacherLname == null || EmployeeNumber == null)
            {
                valid = false;
            } else
            {
                Regex enumber = new Regex(@"/^\w\d{4}$/");
                if (!enumber.IsMatch(EmployeeNumber)) valid = false;
            }

            Debug.WriteLine("Teacher Model validity is: " + valid);
            return valid;
        }

        // parameter-less constructor function
        public Teacher() { }

    }
}