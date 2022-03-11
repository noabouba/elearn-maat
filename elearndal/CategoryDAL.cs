using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearnDAL
{
    public class CategoryDAL
    {
        public enum Categories
        {
            Science=1,
            Math,
            Art,
            Programming
        }

        /// <summary>
        /// Add all categories from the Enum which are not in the Category table already.
        /// </summary>
        public static void AddCategories()
        {
           
            foreach (Categories category in Enum.GetValues(typeof(Categories)))
            {
                if (!OleDbHelper.IsExist("SELECT * FROM Category WHERE CategoryName=" + "'" + category.ToString() + "'", "Category"))
                {
                    OleDbHelper.Fill(String.Format("INSERT INTO Category(CategoryName) VALUES('{0}')", category.ToString()), "Category");
                }
            }
        }
    }
}
