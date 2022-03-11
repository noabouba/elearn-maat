using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLearnDAL;
using System.Data;
namespace eLearnBL
{
    public class Course
    {

        #region Properties

        private User teacher;
        public User Teacher
        {
            get{return teacher;}
            set
            {
                CourseDAL.UpdateCourse(this.courseId, value.UserID, "TeacherKey");
            }
        }
        private List<Lesson> lessons;
        
        public Lesson[] Lessons
        {
            
            get
            {
                var s = new Lesson[lessons.Count];
                lessons.CopyTo(s);
                return s;
            }

        }

        private List<Course> preCourses;

        public List<Course> PreCourses
        {
            get
            {
                if (this.preCourses != null)
                    return this.preCourses;
                FetchPrePostCourses();
                return this.preCourses;
            }
            
        }

        private List<Course> postCourses;

        public List<Course> PostCourses
        {
            get {
                if(this.postCourses !=null)
                    return this.postCourses;
                FetchPrePostCourses();
                return this.postCourses;
            }
        }
        

        private string courseSubject;

        public string Subject
        {
            get { return courseSubject; }
            set
            {
                CourseDAL.UpdateCourse(this.courseId, value, "Subject");   
            }
        }

        private int courseId;

        public int CourseID
        {
            get { return courseId; }
            
        }

        private CategoryDAL.Categories category;

        public CategoryDAL.Categories Category
        {
            get { return category; }
        }
        
        #endregion

        /// <summary>
        /// Gets Previous and Post Recommended courses
        /// </summary>
        private void FetchPrePostCourses()
        {
            try
            {
                this.preCourses = new List<Course>();
                this.postCourses = new List<Course>();
                DataTable crss = CourseDAL.GetPreCourses(this.courseId);

                foreach (DataRow row in crss.Rows)
                {
                    this.preCourses.Add(new Course(int.Parse(row["PreCourse"].ToString())));
                }

                crss = CourseDAL.GetPostCourses(this.courseId);

                foreach (DataRow row in crss.Rows)
                {
                    this.postCourses.Add(new Course(int.Parse(row["PostCourse"].ToString())));
                }

            }
            catch (Exception e)
            {
                
                throw e;
            }
            
        }
        
        /// <summary>
        /// Creates a NEW Course
        /// </summary>
        /// <param name="teacher"></param>
        /// <param name="subject"></param>
        /// <param name="cat"></param>
        public Course(User teacher, string subject,CategoryDAL.Categories cat)
        {
            int id = CourseDAL.AddCourse(teacher.UserID, subject, cat);
            this.courseId = id;
            this.teacher = teacher;
            this.courseSubject = subject;
            this.category = cat;
            this.lessons = new List<Lesson>();
        }

        /// <summary>
        /// Loads Course from DB
        /// </summary>
        /// <param name="dr"></param>
        public Course(DataRow dr)
        {
            this.lessons = new List<Lesson>();
            this.courseId = int.Parse(dr["Key"].ToString());
            this.teacher = new User(int.Parse(dr["TeacherKey"].ToString()));
            this.courseSubject = dr["Subject"].ToString();
            this.category = (CategoryDAL.Categories)int.Parse(dr["Category"].ToString());
            DataTable td = CourseDAL.GetLessons(this.courseId);
           
            foreach (DataRow lessonrow in td.Rows)
                this.lessons.Add(new Lesson(lessonrow));
            
        }
        
        public Course(int id):this(CourseDAL.GetCourse(id).Rows[0])
        {}

        /// <summary>
        /// Add a Post course 
        /// </summary>
        /// <param name="cr"></param>
        public void AddPostCourse(Course cr)
        {
            CourseDAL.AddPostCourse(this.courseId, cr.courseId);
        }

        /// <summary>
        /// Add a Pre course
        /// </summary>
        /// <param name="cr"></param>
        public void AddPreCourse(Course cr)
        {
            CourseDAL.AddPostCourse(cr.courseId, this.courseId);
        }

        /// <summary>
        /// Returns the participating users in course
        /// </summary>
        /// <returns></returns>
        public Users GetUsers()
        {
            return new Users(this.courseId);
        }

        /// <summary>
        /// Calculates user's whole course progress by lesson's progress (percentage)
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public int GetUserProgress(User usr)
        {
            try
            {

                DataTable dt = CourseDAL.GetUserLessons(this.courseId, usr.UserID);
                if (dt.Rows.Count <= 0)
                    return 0;

                int fullVal = this.lessons.Count * 100;
                int count = 0;
                foreach (DataRow row in dt.Rows)
                {
                    count += new Lesson(row).GetUserProgress(usr);
                }

                return 100 *  count / fullVal;

            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Adds a lesson to this course
        /// </summary>
        /// <param name="ls"></param>
        public void AddLesson(Lesson ls)
        {
            if (ls.CourseKey == this.courseId)
            {
                LessonDAL.AddLesson(this.courseId, ls.Title, ls.VideoPath);
                this.lessons.Add(ls);
            }
        }
        
       /// <summary>
       /// Deletes an existing lesson from this course
       /// </summary>
       /// <param name="ls"></param>
        public void RemoveLesson(Lesson ls)
        {
            if (this.lessons.Contains(ls))
            {
                LessonDAL.RemoveLesson(ls.LessonID);
                this.lessons.Remove(ls);
            }
            else
                throw new Exception("Could not remove lesson from course (It doesnt exist)");
        }
    }
}
