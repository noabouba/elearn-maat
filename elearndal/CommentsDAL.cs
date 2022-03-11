using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearnDAL
{
    public class CommentsDAL
    {
        /// <summary>
        /// הוספת תגובה לעץ קיים
        /// </summary>
        /// <param name="parentkey"></param>
        /// <param name="user"></param>
        /// <param name="body"></param>
        public static void AddChildComment(int parentkey, int user, string body)
        {

            DataSet ds = OleDbHelper.Fill("SELECT LessonKey FROM LessonCommentCTBL WHERE CommentKey=" + parentkey, "LessonCommentCTBL"); // Get lesson key of parent
            try
            {
                OleDbHelper.DoQuery(string.Format("INSERT INTO LessonCommentCTBL(LessonKey,ParentKey,Body,UploadDate,UserKey) VALUES({0},{1},'{2}','{3}',{4})",
                    int.Parse(ds.Tables[0].Rows[0][0].ToString()), parentkey, body, DateTime.Now.ToShortDateString(), user)); // Insert new comment 
            }
            catch
            {

            }
        }
        /// <summary>
        ///  יצירת עץ תגובות חדש
        /// </summary>
        /// <param name="lessonkey"></param>
        /// <param name="user"></param>
        /// <param name="body"></param>
        public static void CreateNewCommentTree(int lessonkey, int user, string body)
        {
            OleDbHelper.DoQuery(string.Format("INSERT INTO LessonCommentCTBL(LessonKey,ParentKey,Body,UploadDate,UserKey) VALUES({0},{1},{2},{3},{4})",
                    lessonkey, -1, "'" + body + "'", "'" + DateTime.Now.ToShortDateString() + "'", user));
        }

        /// <summary>
        /// מחזיר מידע אודות תגובה ספציפית
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetComment(int id)
        {
            return OleDbHelper.GetDataSet("SELECT * FROM LessonCommentCTBL WHERE CommentKey=" + id).Tables[0];
        }
        /// <summary>
        /// רשימה של מפתח תגובה ואבא של התגובה
        /// </summary>
        /// <returns></returns>
        public static DataTable GetChainComment()
        {
            return OleDbHelper.GetDataSet("SELECT LessonCommentCTBL_1.CommentKey, LessonCommentCTBL.ParentKey" +
            " FROM LessonCommentCTBL INNER JOIN LessonCommentCTBL AS LessonCommentCTBL_1 ON LessonCommentCTBL.ParentKey = LessonCommentCTBL_1.CommentKey;").Tables[0];
        }

        /// <summary>
        /// Return all child comments of a parent
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static DataTable GetAllChildren(int parentId)
        {
            return OleDbHelper.GetDataSet("SELECT * FROM LessonCommentCTBL WHERE ParentKey=" + parentId).Tables[0];
        }

        /// <summary>
        /// Get all head comments of all trees in collection
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllHeads()
        {
            return OleDbHelper.GetDataSet("SELECT * FROM LessonCommentCTBL WHERE ParentKey=-1").Tables[0];
        }

        /// <summary>
        /// מחזיר טבלה של כל אב-תגובה במסד
        /// </summary>
        /// <param name="lessonkey"></param>
        /// <returns></returns>
        public static DataTable GetAllHeads(int lessonkey)
        {
            return OleDbHelper.GetDataSet("SELECT * FROM LessonCommentCTBL WHERE ParentKey=-1 AND LessonKey=" + lessonkey).Tables[0];
        }

        /// <summary>
        /// Delete a comment by a given key and all it's children
        /// </summary>
        /// <param name="key"></param>
        public static void DeleteComment(int key)
        {
            OleDbHelper.DoQuery("DELETE * FROM LessonCommentCTBL WHERE CommentKey=" + key);
            OleDbHelper.DoQuery("DELETE * FROM LessonCommentCTBL WHERE ParentKey=" + key);
        }
    }
}
