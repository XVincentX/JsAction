using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApp.Models
{
    public class Student
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDay { get; set; }
        public int Exams { get; set; }
    }
}