using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLearnDAL;
using System.Data;
namespace eLearnBL
{
    public class Lesson
    {
        #region Properties
        private int lessonId;

        public int LessonID
        {
            get { return lessonId; }
        }

        private DateTime uploadDate;

        public DateTime UploadDate
        {
            get { return uploadDate; }
        }

        private string vidPath;

        public string VideoPath
        {
            get { return vidPath; }
            set
            {
                LessonDAL.UpdateLesson(this.lessonId, value, "VideoLink");
                this.vidPath = value;
            }
        }

        private int courseKey;

        public int CourseKey
        {
            get { return courseKey; }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                try
                {
                    LessonDAL.UpdateLesson(this.lessonId, value, "Title");
                    title = value;
                }
                catch (Exception e) { throw e; }
            }
        }

        #endregion


        /// <summary>
        /// Load Lesson from Database
        /// </summary>
        /// <param name="dr"></param>
        public Lesson(DataRow dr)
        {
            try
            {
                this.lessonId = int.Parse(dr["Key"].ToString());
                this.uploadDate = DateTime.Parse(dr["UploadDate"].ToString());
                this.vidPath = dr["VideoLink"].ToString();
                this.courseKey = int.Parse(dr["CourseKey"].ToString());
                this.title = dr["Title"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Create NEW Lesson
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path"></param>
        /// <param name="courseKey"></param>
        /// <param name="title"></param>
        public Lesson(DateTime dt, string path, int courseKey, string title)
        {
            //try
            //{
            //    this.lessonId = LessonDAL.AddLesson(courseKey, title, path);
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
            this.uploadDate = dt;
            this.vidPath = path;
            this.courseKey = courseKey;
            this.title = title;
        }

        /// <summary>
        /// Get user progress
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public int GetUserProgress(User usr)
        {
            if (usr == null) return 0;
            return LessonDAL.GetUserProgress(this.lessonId, usr.UserID);
        }

        /// <summary>
        /// Update User progress
        /// </summary>
        /// <param name="usr"></param>
        /// <param name="percent"></param>
        public void SetUserProgress(User usr, int percent)
        {
            try
            {
                LessonDAL.SetStudentProgress(this.lessonId, usr.UserID, percent);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get all trees of a lesson
        /// </summary>
        /// <returns></returns>
        public List<CommentTree> GetComments()
        {
            try
            {
                DataTable dt = CommentsDAL.GetAllHeads(this.lessonId);
                List<CommentTree> lst = new List<CommentTree>(dt.Rows.Count);

                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new CommentTree(int.Parse(row["CommentKey"].ToString())));
                }

                return lst;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

