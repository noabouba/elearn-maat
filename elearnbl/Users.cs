using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLearnDAL;
using System.Data;
namespace eLearnBL
{
    public enum Role
    {
        Student,
        Teacher,
        Admin
    }


    public class Users
    {
        #region Properties
        private List<User> users;

        public List<User> Collection
        {
            get { return users; }
            private set { users = value; }
        }
        #endregion

        /// <summary>
        /// Gets all users in DB
        /// </summary>
        public Users()
        {
            this.users = new List<User>();
            DataTable dt = UserDAL.GetAllUsers();
            foreach (DataRow dr in dt.Rows)
                this.users.Add(new User(dr));
        }

        /// <summary>
        /// Get all users by course
        /// </summary>
        /// <param name="lessonId"></param>
        public Users(int courseId)
        {
            this.users = new List<User>();
            DataTable dt = CourseDAL.GetStudents(courseId);
            foreach (DataRow dr in dt.Rows)
            {
                this.users.Add(new User(dr));
            }
        }

        /// <summary>
        /// Gets all users that their Field are Value
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public Users(string field, object value)
        {
            DataTable dt = UserDAL.FindUserByValue(value, field);
            this.users = new List<User>();
            foreach (DataRow row in dt.Rows)
            {
                this.users.Add(new User(row));
            }
        }
        /// <summary>
        /// Get all users by role
        /// </summary>
        /// <param name="rl"></param>
        public Users(Role rl)
        {
            this.users = new List<User>();
            DataTable dt = UserDAL.GetUsersByRole((UserDAL.Role)((int)rl));
            foreach (DataRow dr in dt.Rows)
            {
                this.users.Add(new User(dr));
            }

        }
        




    }
}
