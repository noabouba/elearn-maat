using eLearnDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearnBL
{
    public class CommentTrees
    {
        private List<CommentTree> trees;

        public List<CommentTree> Collection
        {
            get { return trees; }
            set { trees = value; }
        }

        public CommentTrees(Lesson less)
        {
            DataTable dt = CommentsDAL.GetAllHeads(less.LessonID);

            this.trees = new List<CommentTree>();

            foreach (DataRow row in dt.Rows)
                this.trees.Add(new CommentTree(int.Parse(row[0].ToString())));
        }
        /// <summary>
        /// Sorts the trees by date
        /// </summary>
        /// <param name="ascending"></param>
        public void SortTrees(bool ascending = true)
        {
            if (ascending)
            {
                this.trees.Sort((a, b) => -1 * a.Head.Date.CompareTo(b.Head.Date));
            }
            else
            {
                this.trees.Sort((a, b) => a.Head.Date.CompareTo(b.Head.Date));
            }
        }

    }
}
