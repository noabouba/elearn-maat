using eLearnDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace eLearnBL
{

    public class User
    {

        #region Properties
        private int userID;

        public int UserID
        {
            get { return userID; }
        }

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
        }

        private DateTime birthDate;
        /// <summary>
        /// User's birth date
        /// </summary>
        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                try
                {
                    UserDAL.UpdateUser(this.userID, value.ToShortDateString(), "Birthdate");
                    this.birthDate = value;
                }
                catch
                {
                }
            }

        }

        private string email;
        /// <summary>
        /// User's email address
        /// </summary>
        public string Email
        {
            get { return email; }
            set
            {
                try
                {
                    UserDAL.UpdateUser(this.userID, value, "Email");
                    this.email = value;
                }
                catch { }
            }
        }

        private Role role;
        /// <summary>
        /// User's role
        /// </summary>
        public Role Role
        {
            get { return role; }
            set
            {
                try
                {
                    UserDAL.UpdateUser(this.userID, (int)value, "Role");
                    this.role = value;
                }
                catch
                {
                }
            }
        }

        private string pass;
        /// <summary>
        /// User's Password
        /// </summary>
        public string Password
        {
            get { return pass; }
            set
            {
                try
                {
                    UserDAL.UpdateUser(this.userID, value, "Password");
                    this.pass = value;
                }
                catch
                {
                }
            }
        }
        private bool isVerified;

        public bool IsVerified
        {
            get { return isVerified; }
            set {
                if (value != this.isVerified)
                {
                    try
                    {
                        UserDAL.UpdateUser(this.userID, value ? 1 : 0, "Verified");
                        isVerified = value;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public string FullName
        {
            get { return this.firstName + " " + this.lastName; }
        }
        #endregion

        /// <summary>
        /// Loads User from DataRow
        /// </summary>
        /// <param name="dr"></param>
        public User(DataRow dr)
        {
            this.userID = int.Parse(dr["Key"].ToString());
            this.firstName = dr["FName"].ToString();
            this.lastName = dr["LName"].ToString();
            this.birthDate = DateTime.Parse(dr["Birthdate"].ToString());
            this.email = dr["Email"].ToString();
            this.role = (Role)int.Parse(dr["Role"].ToString());
            this.pass = dr["Password"].ToString();
            this.isVerified = dr["Verified"].ToString().ToLower() == "true";
        }

        /// <summary>
        /// Creates User by his ID
        /// </summary>
        /// <param name="id"></param>
        public User(int id) : this(UserDAL.FindUserByValue(id, "Key").Rows[0])
        { }


        /// <summary>
        /// Creates NEW user
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="dt"></param>
        /// <param name="email"></param>
        /// <param name="rl"></param>
        /// <param name="passw"></param>
        public User(string fname, string lname, DateTime dt, string email, Role rl, string passw)
        {
            int id = UserDAL.AddUser(fname, lname, dt, email, PasswordUtils.Hash(passw), (UserDAL.Role)rl);
            if (id != -1)
            {
                this.userID = id;
                this.firstName = fname;
                this.lastName = lname;
                this.birthDate = dt;
                this.email = email;
                this.role = rl;
                this.pass = passw;
            }
        }

        /// <summary>
        /// Deletes User from Database
        /// </summary>
        public void DeleteUser()
        {
            UserDAL.RemoveUser(this.userID);
        }

        /// <summary>
        /// Adds a Course to User's collection
        /// </summary>
        /// <param name="courseId"></param>
        public void AddCourse(int courseId)
        {
            CourseDAL.AddStudent(this.userID, courseId);
        }

        /// <summary>
        /// Returns user's courses
        /// </summary>
        /// <returns></returns>
        public Courses GetCourses()
        {
            return new Courses(this);
        }

        /// <summary>
        /// ?
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public Course[] GetCourses(out DateTime[] dates)
        {

            DataTable dt = CourseDAL.GetUserCourses(this.userID);
            Course[] ret = new Course[dt.Rows.Count];
            dates = new DateTime[dt.Rows.Count];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = new Course(dt.Rows[i]);
                dates[i] = DateTime.Parse(dt.Rows[i]["RegisterDate"].ToString());
            }

            return ret;
        }


      
        /// <summary>
        /// Produces a scale that rates the student's dedication and work 
        /// </summary>
        /// <returns></returns>
        public float StudentScale()
        {
            float scaleSum = 0.0f;
            DateTime[] dates;
            Course[] studentCourses = GetCourses(out dates);

            const int daysTolerance = 35; //  Each course that was registered to LONGER ago than 'daysTolerance' will be taken into account
            const int percentageTolerance = 85; // A percentage that is bigger than that, would be considered to a finished course
            const float courseFinishWeight = 0.40f; // The weight of the number of courses finished 

            float midSum = 0f;
            int count = 0;
            for (int i = 0; i < dates.Length; i++)
            {
                if ((DateTime.Now - dates[i]).TotalDays >= daysTolerance)
                {
                    if (studentCourses[i].GetUserProgress(this) > percentageTolerance) count++;
                }
            }
            midSum = 10 * (1 - (studentCourses.Length - count) / studentCourses.Length);

            scaleSum += midSum * courseFinishWeight; // Add the finished courses weight


            return scaleSum;
          

        }

    }
}
