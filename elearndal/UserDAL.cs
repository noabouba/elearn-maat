using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearnDAL
{
    public class UserDAL
    {
        public enum Role
        {
            Student ,
            Teacher,
            Admin
        }
        /// <summary>
        /// מחזיר את כל המשתמשים
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllUsers()
        {
            return OleDbHelper.Fill("SELECT * FROM [User]", "User").Tables[0];
        }

        /// <summary>
        /// Return all users by a certain role
        /// </summary>
        /// <param name="rl"></param>
        /// <returns></returns>
        public static DataTable GetUsersByRole(Role rl)
        {
            
            return OleDbHelper.GetDataSet("SELECT * FROM [User] WHERE Role=" + (int)rl).Tables[0];
        }

        /// <summary>
        /// Add user by params.
        /// </summary>
        /// <param name="fname">First name</param>
        /// <param name="lname">Last name</param>
        /// <param name="birth">Date of birth</param>
        /// <param name="email">Email</param>
        /// <param name="role">Role</param>
        public static int AddUser(string fname, string lname, DateTime birth, string email, string password, Role role)
        {
            if (!OleDbHelper.IsExist("SELECT * FROM [User] WHERE [Email]='" + email + "'", "User"))
            {
                //OleDbHelper.Fill(String.Format("INSERT INTO User (FName,LName,Birthdate,[Email],Role,Password) VALUES ('{0}','{1}','{2}','{3}',{4},'{5}')",
                //    fname, lname, birth.ToShortDateString(), email, (int)role,password), "User");
                OleDbHelper.DoQuery(String.Format("INSERT INTO [User](FName,LName,Birthdate,[Email],Role,[Password]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')",
                 fname, lname, birth.ToShortDateString(), email, (int)role, password));
            }
            else
            {
                Console.WriteLine("E-Mail already exists.");
                return -1;
            }
            return int.Parse(OleDbHelper.Fill("SELECT MAX(Key) FROM [User]", "User").Tables[0].Rows[0][0].ToString());

            

        }

        /// <summary>
        /// Delete user by it's key.
        /// </summary>
        /// <param name="userKey">Deleted user key</param>
        public static void RemoveUser(int userKey)
        {
            if (OleDbHelper.IsExist("SELECT * FROM [User] WHERE Key=" + userKey, "User"))
            {
                OleDbHelper.DoQuery("DELETE * FROM [User] WHERE Key=" + userKey);
            }
        }

        /// <summary>
        /// Update user by it's key, and update specified field by value.
        /// </summary>
        /// <param name="userKey">User key</param>
        /// <param name="val">New value</param>
        /// <param name="field">Field name to update</param>
        public static void UpdateUser(int userKey, object val, string field)
        {

            DataSet ds = OleDbHelper.Fill("SELECT * FROM [User] WHERE Key=" + userKey, "User");
            try
            {
                foreach (DataColumn dc in ds.Tables["User"].Columns)
                {
                    if (dc.ColumnName.ToLower() == field.ToLower())
                    {
                        if (dc.DataType.Name == "String")
                        {
                            OleDbHelper.DoQuery("UPDATE [User] SET " + dc.ColumnName + "='" + val + "' WHERE Key=" + userKey);
                        }
                        else
                            OleDbHelper.DoQuery("UPDATE [User] SET " + dc.ColumnName + "=" + val + " WHERE Key=" + userKey);
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
        /// Get user's row by searching object at a certain field
        /// </summary>
        /// <param name="val">Value to search for</param>
        /// <param name="field">At what field to search for it</param>
        /// <returns></returns>
        public static DataTable FindUserByValue(object val, string field)
        {
            DataSet ds = OleDbHelper.Fill("SELECT * FROM [User] WHERE Key=" + (-1) + "", "User"); // Getting an empty table, to get Column names
            DataSet ret = null;
            try
            {
                foreach (DataColumn dc in ds.Tables["User"].Columns)
                {
                    if (dc.ColumnName.ToLower() == field.ToLower())
                    {
                        if (dc.DataType.Name == "String")
                        {
                            ret = OleDbHelper.Fill(
                              "SELECT * FROM [User] WHERE " + dc.ColumnName + "='" + val.ToString() + "'",
                          "User");
                        }
                        else
                        {
                            // val = Convert.ChangeType(val, dc.DataType.GetType());
                            ret = OleDbHelper.Fill(
                               "SELECT * FROM [User] WHERE " + dc.ColumnName + "=" + val, // Should i have date as string or as DateTime
                           "User");


                        }
                        if (ret != null)
                        {
                            if (ret.Tables["User"].Rows.Count > 0)
                                return ret.Tables["User"];
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
