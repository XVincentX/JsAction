using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TestApp.Models;
using System.Net.Http;

namespace TestApp.Controllers
{
    [JsAction.JsAction()]
    public class StudentController : ApiController
    {
        private List<Student> data;

        public StudentController()
        {
            this.data = new List<Student>() 
            {
                new Student(){id=0, Name="Vincenzo", Surname="Chianese", BirthDay=DateTime.Parse("20/05/1989"), Exams=10},
                new Student(){id=1, Name="Fernando", Surname="Alonso", BirthDay=DateTime.Parse("19/07/1981"),Exams=0},
                new Student(){id=2, Name="Bill", Surname="Gates", BirthDay=DateTime.Parse("28/10/1955"), Exams=2}
            };

        }
        public IQueryable<Student> GetStudentList()
        {
            return data.AsQueryable();
        }

        public Student GetById(int id)
        {
            return data.Where(s => s.id == id).First();
        }

        public Student GetByName(string name)
        {
            return data.Where(s => s.Name == name).First();

        }

        public Student GetBySurname(string surname)
        {
            return data.Where(s => s.Surname == surname).First();
        }

        public HttpResponseMessage PostStudent(Student st)
        {
            data.Add(st);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        public HttpResponseMessage DeleteStudent(Student st)
        {
            var elem = data.Where(q => q.id == st.id);
            if (elem.Count() > 0)
            {
                data.Remove(elem.First());
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
        }
    }
}
