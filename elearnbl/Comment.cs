using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using eLearnDAL;
namespace eLearnBL
{


    public class Comment
    {
        #region Properties


        private int lessonID;
        /// <summary>
        /// The connected lessonID
        /// </summary>
        public int LessonID
        {
            get { return lessonID; }
        }

        private int parentID;
        /// <summary>
        /// Parent comment key
        /// </summary>
        public int ParentID
        {
            get { return parentID; }
        }

        private int userID;
        /// <summary>
        /// Publisher's ID
        /// </summary>
        public int PublisherID
        {

            get { return userID; }
        }

        private string message;
        /// <summary>
        /// Comment's content
        /// </summary>
        public string Message
        {
            get { return message; }
        }


        private DateTime date;
        /// <summary>
        /// Publish Date
        /// </summary>
        public DateTime Date
        {
            get { return date; }
        }

        private int commentID;
        /// <summary>
        /// Comment key
        /// </summary>
        public int CommentID
        {
            get { return commentID; }
        }


        private User publisher;
        public User Publisher
        {
            get
            {
                if (this.publisher == null)
                    this.publisher = new User(this.userID);

                return publisher;
            }
        }

        #endregion

        /// <summary>
        /// Create comment by DataRow from table
        /// </summary>
        /// <param name="dr"></param>
        public Comment(DataRow dr)
        {
            this.lessonID = int.Parse(dr["LessonKey"].ToString());
            this.parentID = int.Parse(dr["ParentKey"].ToString());
            this.commentID = int.Parse(dr["CommentKey"].ToString());
            this.userID = int.Parse(dr["UserKey"].ToString());
            this.message = dr["Body"].ToString();
            this.date = DateTime.Parse(dr["UploadDate"].ToString());
        }

        /// <summary>
        /// Creates a new comment (root or child, depends on parent key)
        /// </summary>
        /// <param name="lesson">The comment's lesson</param>
        /// <param name="parent">Parent id: -1 for a root.</param>
        /// <param name="userId">Publisher's id</param>
        /// <param name="body">The body of the comment</param>
        public Comment(int lesson, int parent, int userId, string body)
        {
            if (parent < 0)
            {
                CommentsDAL.CreateNewCommentTree(lesson, userId, body);

            }
            else
            {
                CommentsDAL.AddChildComment(parent, userId, body);
            }

            this.lessonID = lesson;
            this.parentID = parent;
            this.userID = userId;
            this.message = body;
            this.date = DateTime.Now.Date;

        }

        /// <summary>
        /// Removes this comment and all it's children
        /// </summary>
        public void RemoveComment()
        {
            CommentsDAL.DeleteComment(this.CommentID);
        }


    }
}
