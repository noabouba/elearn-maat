using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using eLearnDAL;
namespace eLearnBL
{

    public class CommentTree
    {
        #region Properties

        private List<Comment> children;
        public List<Comment> Children
        {
            get { return children; }
        }

        private Comment self;
        public Comment Head
        {
            get { return self; }
        }
        #endregion


        /// <summary>
        /// Creates new comment tree and assigns head
        /// </summary>
        /// <param name="head"></param>
        public CommentTree(Comment head)
        {
            /// If we got a comment, it was *already created in the database!!* 

            this.children = new List<Comment>();
            this.self = head;
        }

        /// <summary>
        /// Get tree by Head comment Id
        /// </summary>
        /// <param name="commentId"></param>
        public CommentTree(int commentId)
        {
            DataRow selfDt = CommentsDAL.GetComment(commentId).Rows[0];
            DataTable childDt = CommentsDAL.GetAllChildren(commentId);

            this.self = new Comment(selfDt);

            this.children = new List<Comment>();

            foreach (DataRow row in childDt.Rows)
            {
                this.children.Add(new Comment(row));
            }
        }


        /// <summary>
        /// Add a child comment to tree
        /// </summary>
        /// <param name="cmt"></param>
        public void AddChildComment(Comment cmt)
        {
            /// If we got a comment, it was *already created in the database!!* 
            
            this.children.Add(cmt);
        }

        /// <summary>
        /// Deletes the tree and all of it's comments
        /// </summary>
        public void DeleteTree()
        {
            self.RemoveComment();
        }

       




    }
}
