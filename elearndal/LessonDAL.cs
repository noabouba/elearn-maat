using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearnDAL
{
    public class LessonDAL
    {
        /// <summary>
        /// מוסיף שיעור לקורס ומחזיר את הKEY
        /// </summary>
        /// <param name="courseKey"></param>
        /// <param name="title"></param>
        /// <param name="videoLink"></param>
        public static int AddLesson(int courseKey, string title, string videoLink)
        {
            OleDbHelper.Fill(String.Format("INSERT INTO Lesson(UploadDate,VideoLink,CourseKey,Title) VALUES('{0}','{1}',{2},'{3}')"
                , DateTime.Now.Date.ToShortDateString(), videoLink, courseKey, title), "Lesson");
            return int.Parse(OleDbHelper.Fill("SELECT MAX(Key) FROM Lesson", "Lesson").Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// מחזיר את כל השיעורים מיום מסוים
        /// </summary>
        /// <param name="courseKey"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DataTable GetLessonsByDate(int courseKey, DateTime day)
        {
            return OleDbHelper.Fill("SELECT * FROM Lesson WHERE Cast(UploadDate as datetime)=" + day + " ORDER BY UploadDate DESC", "Lesson").Tables[0];
        }

        /// <summary>
        /// מחזיר את כל השיעורים מתקופת זמן
        /// </summary>
        /// <param name="courseKey"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static DataTable GetLessonsByDate(int courseKey, DateTime from, DateTime to)
        {
            return OleDbHelper.Fill("SELECT * FROM Lesson WHERE Cast(UploadDate as datetime)>=" + from +
                " AND Cast(UploadDate as datetime)<=" + to + " ORDER BY UploadDate DESC", "Lesson").Tables[0];

        }

        /// <summary>
        /// מוחק שיעור
        /// </summary>
        /// <param name="lessId"></param>
        public static void RemoveLesson(int lessId)
        {
            OleDbHelper.DoQuery("DELETE FROM Lesson WHERE Key=" + lessId);
        }

        /// <summary>
        /// מעדכן שיעור לפי שדה וערך
        /// </summary>
        /// <param name="lessonkey"></param>
        /// <param name="val"></param>
        /// <param name="field"></param>
        public static void UpdateLesson(int lessonkey, object val, string field)
        {
            DataSet ds = OleDbHelper.Fill("SELECT * FROM [Lesson] WHERE Key=" + -1, "Lesson");

            foreach (DataColumn dc in ds.Tables["Lesson"].Columns)
            {
                if (dc.ColumnName.ToLower() == field.ToLower())
                {
                    if (dc.DataType.Name == "String")
                    {
                        OleDbHelper.DoQuery("UPDATE [Lesson] SET " + dc.ColumnName + "='" + val + "' WHERE Key=" + lessonkey);
                    }
                    else
                        OleDbHelper.DoQuery("UPDATE [Lesson] SET " + dc.ColumnName + "=" + val + " WHERE Key=" + lessonkey);
                    return;
                }
            }
        }

        /// <summary>
        /// מחזיר את התקדמות תלמיד בשיעור ספציפי
        /// </summary>
        /// <param name="lesson"></param>
        /// <param name="usr"></param>
        /// <returns></returns>
        public static int GetUserProgress(int lesson, int usr)
        {
            try
            {
                return int.Parse(OleDbHelper.Fill("SELECT LessonProgress FROM LessonUserCTBL WHERE UserKey=" + usr + " AND LessonKey=" + lesson, "LessonUserCTBL").Tables[0].Rows[0][0].ToString());
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// מעדכן התקדמות
        /// </summary>
        /// <param name="lessonkey"></param>
        /// <param name="user"></param>
        /// <param name="precent"></param>
        public static void SetStudentProgress(int lessonkey, int user, int precent)
        {
            if (!OleDbHelper.IsExist("SELECT * FROM LessonUserCTBL WHERE UserKey=" + user + " AND LessonKey=" + lessonkey, "LessonUserCTBL"))
            {
                OleDbHelper.DoQuery(string.Format("INSERT INTO LessonUserCTBL(UserKey,LessonKey,LessonProgress) VALUES({0},{1},{2})",
                    user, lessonkey, precent));
            }
            else
            {
                OleDbHelper.DoQuery(string.Format("UPDATE LessonUserCTBL SET LessonProgress=" + precent + " WHERE LessonKey=" + lessonkey + " AND UserKey=" + user));
            }
        }

        /// <summary>
        /// חיפוש שיעור על פי ערך כללי
        /// </summary>
        /// <param name="val"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static DataTable FindLessonByValue(object val, string field)
        {
            DataSet ds = OleDbHelper.Fill("SELECT * FROM [Lesson] WHERE Key=" + (-1) + "", "Lesson"); // Getting an empty table, to get Column names
            DataSet ret = null;
            try
            {
                foreach (DataColumn dc in ds.Tables["Lesson"].Columns)
                {
                    if (dc.ColumnName.ToLower() == field.ToLower())
                    {
                        if (dc.DataType.Name == "String")
                        {
                            ret = OleDbHelper.Fill(
                              "SELECT * FROM [Lesson] WHERE " + dc.ColumnName + "='" + val.ToString() + "'",
                          "Lesson");
                        }
                        else
                        {
                            // val = Convert.ChangeType(val, dc.DataType.GetType());
                            ret = OleDbHelper.Fill(
                               "SELECT * FROM [Lesson] WHERE " + dc.ColumnName + "='" + val + "'", // Should i have date as string or as DateTime
                           "Lesson");


                        }
                        if (ret != null)
                        {
                            if (ret.Tables["Lesson"].Rows.Count > 0)
                                return ret.Tables["Lesson"];
                            else return null;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }



    }
}
