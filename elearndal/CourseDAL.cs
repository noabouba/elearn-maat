using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearnDAL
{
    public static class CourseDAL
    {
        /// <summary>
        /// מוסיף קורס חדש
        /// </summary>
        /// <param name="teacherKey"></param>
        /// <param name="courseName"></param>
        /// <param name="category"></param>
        public static int AddCourse(int teacherKey, string courseName, CategoryDAL.Categories category)
        {
            if (!OleDbHelper.IsExist("SELECT * FROM Course WHERE Subject='" + courseName + "'", "Course"))
            {
                OleDbHelper.Fill(String.Format("INSERT INTO Course(Subject,Category,TeacherKey) VALUES('{0}',{1},{2})",
                    courseName, (int)category, teacherKey), "Category");
            }
            return int.Parse(OleDbHelper.Fill("SELECT MAX(Key) FROM Course", "Course").Tables[0].Rows[0][0].ToString());

        }

        /// <summary>
        /// מוסיף קורס המשך
        /// </summary>
        /// <param name="preKey"></param>
        /// <param name="postKey"></param>
        public static void AddPostCourse(int preKey, int postKey)
        {
            if (!OleDbHelper.IsExist("SELECT * FROM CourseCourseCTBL WHERE PreCourse=" + preKey + " AND PostCourse=" + postKey, "CourseCourseCTBL"))
            {
                OleDbHelper.Fill(String.Format("INSERT INTO CourseCourseCTBL(PreCourse,PostCourse) VALUES({0},{1})",
                    preKey, postKey), "CourseCourseCTBL");
            }
        }

        /// <summary>
        /// מחזיר מידע אודות קורס לפי מזהה
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetCourse(int id)
        {
            return OleDbHelper.GetDataSet("SELECT * FROM Course WHERE Key=" + id).Tables[0];
        }

        /// <summary>
        /// מחזיר טבלה משולבת של מידע על קורס
        /// </summary>
        /// <param name="coursekey"></param>
        /// <returns></returns>
        public static DataTable GetCourseCombinedData(int coursekey)
        {
            return OleDbHelper.GetDataSet("SELECT Course.Subject, Category.CategoryName, User.FName, User.LName" +
            " FROM[User] INNER JOIN(Category INNER JOIN Course ON Category.Key = Course.Category) ON User.Key = Course.TeacherKey WHERE Course.Key=" + coursekey).Tables[0];
        }

        /// <summary>
        /// מוסיף תלמיד לקורס
        /// </summary>
        /// <param name="studentKey"></param>
        /// <param name="courseKey"></param>
        public static void AddStudent(int studentKey, int courseKey)
        {
            if (!OleDbHelper.IsExist("SELECT * FROM StudentCourseCTBL WHERE UserKey=" + studentKey + " AND CourseKey=" + courseKey, "StudentCourseCTBL"))
            {
                OleDbHelper.Fill(String.Format("INSERT INTO StudentCourseCTBL(UserKey,CourseKey,LastActivity,RegisterDate) VALUES({0},{1},'{2}','{3}')",
                    studentKey, courseKey, DateTime.Now.Date,DateTime.Now.Date), "StudentCourseCTBL");
            }
        }

        /// <summary>
        /// מוצא קורס לפי ערך בשדה
        /// </summary>
        /// <param name="val"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static DataTable FindCourseByValue(object val, string field)
        {
            DataSet ds = OleDbHelper.Fill("SELECT * FROM [Course] WHERE Key=" + (-1) + "", "Course"); // Getting an empty table, to get Column names
            DataSet ret = null;
            foreach (DataColumn dc in ds.Tables["Course"].Columns)
            {
                if (dc.ColumnName.ToLower() == field.ToLower())
                {
                    if (dc.DataType.Name == "String")
                    {
                        ret = OleDbHelper.Fill(
                          "SELECT * FROM [Course] WHERE " + dc.ColumnName + "='" + val.ToString() + "'",
                      "Course");
                    }
                    else
                    {
                        // val = Convert.ChangeType(val, dc.DataType.GetType());
                        ret = OleDbHelper.Fill(
                           "SELECT * FROM [Course] WHERE " + dc.ColumnName + "='" + val + "'", // Should i have date as string or as DateTime
                       "Course");


                    }
                    if (ret != null)
                    {
                        if (ret.Tables["Course"].Rows.Count > 0)
                            return ret.Tables["Course"];
                        else return null;
                    }

                }
            }
            return null;
        }

        /// <summary>
        /// מסיר תלמיד מקורס
        /// </summary>
        /// <param name="studentKey"></param>
        /// <param name="courseKey"></param>
        public static void RemoveStudent(int studentKey, int courseKey)
        {
            OleDbHelper.DoQuery("DELETE * FROM StudentCourseCTBL WHERE UserKey=" + studentKey + " AND CourseKey=" + courseKey);
        }

        /// <summary>
        /// Returns an array of student's user key for a course
        /// </summary>
        /// <param name="courseKey"></param>
        /// <returns></returns>
        public static int[] GetStudentsKeys(int courseKey)
        {
            DataSet ds = OleDbHelper.Fill("SELECT UserKey FROM StudentCourseCTBL WHERE CourseKey=" + courseKey, "StudentCourseCTBL");
            int[] studs = new int[ds.Tables["StudentCourseCTBL"].Rows.Count];
            for (int i = 0; i < ds.Tables["StudentCourseCTBL"].Rows.Count; i++)
            {
                studs[i] = int.Parse(ds.Tables["StudentCourseCTBL"].Rows[i][0].ToString());
            }
            return studs;
        }

        /// <summary>
        /// מחזיר מידע של כל התלמידים בקורס 
        /// </summary>
        /// <param name="courseKey"></param>
        /// <returns></returns>
        public static DataTable GetStudents(int courseKey)
        {
            return OleDbHelper.Fill("SELECT User.Key,User.FName,User.LName,User.Birthdate,User.Email,User.Role,User.Password FROM [User]" +
                 " INNER JOIN StudentCourseCTBL ON User.Key=StudentCourseCTBL.UserKey WHERE StudentCourseCTBL.CourseKey=" + courseKey, "User").Tables[0];
        }

        /// <summary>
        /// מחזיר את כל השיעורים של קורס מסוים
        /// </summary>
        /// <param name="courseKey"></param>
        /// <returns></returns>
        public static DataTable GetLessons(int courseKey)
        {
            return OleDbHelper.GetDataSet("SELECT * FROM Lesson WHERE CourseKey=" + courseKey).Tables[0];
        }

        /// <summary>
        /// Update course by value and string
        /// </summary>
        /// <param name="courseKey"></param>
        /// <param name="val"></param>
        /// <param name="field"></param>
        public static void UpdateCourse(int courseKey, object val, string field)
        {

            DataSet ds = OleDbHelper.Fill("SELECT * FROM [Course] WHERE Key=" + courseKey, "Course");
            try
            {
                foreach (DataColumn dc in ds.Tables["Course"].Columns)
                {
                    if (dc.ColumnName.ToLower() == field.ToLower())
                    {
                        if (dc.DataType.Name == "String")
                        {
                            OleDbHelper.DoQuery("UPDATE [Course] SET " + dc.ColumnName + "='" + val + "' WHERE Key=" + courseKey);
                        }
                        else
                            OleDbHelper.DoQuery("UPDATE [Course] SET " + dc.ColumnName + "=" + val + " WHERE Key=" + courseKey);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        /// <summary>
        /// מחזיר את כל הקורסים של מורה מסוים
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetCoursesByTeacher(int id)
        {
            return OleDbHelper.Fill("SELECT * FROM Course WHERE TeacherKey=" + id, "Course").Tables[0];
        }
        /// <summary>
        /// מחפש קורס לפי שם
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataTable GetCourseByName(string name)
        {
            return OleDbHelper.Fill("SELECT * FROM Course WHERE Subject LIKE '%" + name + "%'", "Course").Tables[0];
        }
        /// <summary>
        /// מחזיר את כל הקורסים המקדימים של קורס ספציפי
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public static DataTable GetPreCourses(int course)
        {
            return OleDbHelper.Fill("SELECT * FROM CourseCourseCTBL WHERE PostCourse=" + course, "CourseCourseCTBL").Tables[0];
        }

        /// <summary>
        /// מחזיר את כל הקורסים הממשיכים של קורס ספציפי
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public static DataTable GetPostCourses(int course)
        {
            return OleDbHelper.Fill("SELECT * FROM CourseCourseCTBL WHERE PreCourse=" + course, "CourseCourseCTBL").Tables[0];
        }

        /// <summary>
        /// מחזיר את כל הקורסים של תלמיד ספציפי
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static DataTable GetUserCourses(int user)
        {
            return OleDbHelper.GetDataSet("SELECT Course.*,StudentCourseCTBL.RegisterDate FROM Course INNER JOIN StudentCourseCTBL ON Course.Key = StudentCourseCTBL.CourseKey WHERE StudentCourseCTBL.UserKey=" + user).Tables[0];
        }

        /// <summary>
        /// מחזיר את כל השיעורים אותם תלמיד התחיל
        /// </summary>
        /// <param name="course"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static DataTable GetUserLessons(int course, int user)
        {
        //    return OleDbHelper.Fill("SELECT LessonUserCTBL.* FROM(Course INNER JOIN Lesson ON (Course.Key = " + course + "))"+
        //        " INNER JOIN LessonUserCTBL ON (Lesson.Key = LessonUserCTBL.LessonKey WHERE LessonUserCTBL.UserKey=" + user+")"
        //        , "LessonUserCTBL").Tables[0];

          return OleDbHelper.Fill(String.Format("SELECT Lesson.* FROM LessonUserCTBL, Lesson WHERE Lesson.CourseKey={0} AND LessonUserCTBL.LessonKey=Lesson.Key AND LessonUserCTBL.UserKey={1}",
              course, user), "Lesson").Tables[0];
        }

        /// <summary>
        /// מחזיר את כל הקורסים מקטגוריה ספציפית
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public static DataTable GetCoursesByCategory(CategoryDAL.Categories cat)
        {
            return OleDbHelper.Fill("SELECT * FROM Course WHERE Category=" + (int)cat, "Course").Tables[0];
        }

        
    }
}