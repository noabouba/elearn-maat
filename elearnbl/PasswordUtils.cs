using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using eLearnDAL;
using System.Data;

namespace eLearnBL
{
    public static class PasswordUtils
    {
        private static readonly string salt = "1kPO==";

        /// <summary>
        /// Create hash
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Hash(string src)
        {
            MD5 md = MD5.Create();

            byte[] hash = md.ComputeHash(Encoding.UTF8.GetBytes(src + PasswordUtils.salt));

            StringBuilder sb = new StringBuilder();

            foreach (byte x in hash)
                sb.Append(x.ToString("x2"));


            return sb.ToString();
        }


        /// <summary>
        /// Verify a clean string to a hash
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool Verify(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = Hash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks whether an email/password combination matches any user in the database, and returns an object of the user
        /// or null if it does not exist
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static User IsUser(string email, string password)
        {
            DataTable dt = UserDAL.FindUserByValue(email, "Email");
            if (dt == null || dt.Rows.Count == 0)
                return null;

            if (Verify(password, dt.Rows[0]["Password"].ToString()))
                return new User(dt.Rows[0]);
            return null;
        }

        public static bool CanRegister(string email)
        {
            DataTable dt = UserDAL.FindUserByValue(email, "Email");

            if (dt == null)
                return true;

            return false;
        }
    }
}
