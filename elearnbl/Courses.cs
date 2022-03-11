using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLearnDAL;
using System.Data;

namespace eLearnBL
{
    public class Courses
    {
        /// <summary>
        /// Course collection
        /// </summary>
        private List<Course> courses;

        /// <summary>
        /// Course Collection
        /// </summary>
        public List<Course> Collection
        {
            get { return courses; }
            set { courses = value; }
        }

        /// <summary>
        /// Collects all courses
        /// </summary>
        public Courses()
        {
            this.courses = new List<Course>();
            DataTable dt = CourseDAL.GetCourseByName(""); // sqli
            foreach (DataRow course in dt.Rows)
                this.courses.Add(new Course(course));
        }

        /// <summary>
        /// Collects all courses related to a User. Teacher user will give taught courses, and Student user will give courses learnt
        /// </summary>
        /// <param name="id"></param>
        public Courses(User id)
        {
            this.courses = new List<Course>();
            if (id.Role == Role.Teacher)
            {
                DataTable dt = CourseDAL.GetCoursesByTeacher(id.UserID);
                foreach (DataRow course in dt.Rows)
                    this.courses.Add(new Course(course));
            }
            else if (id.Role == Role.Student)
            {
                DataTable dt = CourseDAL.GetUserCourses(id.UserID);
                foreach (DataRow course in dt.Rows)
                    this.courses.Add(new Course(course));

            }
        }


        /// <summary>
        /// Collects courses similiar to name
        /// </summary>
        /// <param name="name"></param>
        public Courses(string name)
        {
            this.courses = new List<Course>();
            DataTable dt = CourseDAL.GetCourseByName(name);
            foreach (DataRow course in dt.Rows)
                this.courses.Add(new Course(course));
        }

        /// <summary>
        /// Collects all courses from Category
        /// </summary>
        /// <param name="category"></param>
        public Courses(CategoryDAL.Categories category)
        {
            this.courses = new List<Course>();
            DataTable dt = CourseDAL.GetCoursesByCategory(category);
            foreach (DataRow course in dt.Rows)
                this.courses.Add(new Course(course));
        }
    }
}
