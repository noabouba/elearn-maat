using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using eLearnDAL;

namespace eLearnBL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetWindowSize(200, 60);

            //Courses crs = new Courses();
            //Course cr = crs.Collection[0];
            //crs.Collection[0].Lessons.Add(new Lesson(DateTime.Now.Date, "https://www.youtube.com/watch?v=x_9lfHjYtVg", cr.CourseID, "Starting C#! Installing VS2010"));
            //crs.Collection[1].Lessons.Add(new Lesson(DateTime.Now.Date, "https://www.youtube.com/watch?v=zUbVMdF_kU4", cr.CourseID, "Changing Forms Properties"));


            Console.ReadKey();
        }


        static void PrintTable(DataTable dt)
        {
            int maxlen = Console.WindowWidth / dt.Columns.Count;
            foreach (DataColumn dc in dt.Columns)
                Console.Write(dc.ColumnName + new string(' ', maxlen - dc.ColumnName.Length - 1) + "|");

            Console.WriteLine(new string('_', maxlen * dt.Columns.Count));

            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Console.Write(dr[i] + new string(' ', maxlen - dr[i].ToString().Length - 1) + "|");
                }
                Console.WriteLine();
            }
        }

        static void PrintObj(object obj)
        {
            var a = obj.GetType().GetProperties();
            int maxlen = Console.WindowWidth / a.Length;
            foreach (var prop in a)
                Console.Write(prop.Name + new string(' ', maxlen - prop.Name.Length - 1) + "|");

            Console.WriteLine(new string('_', maxlen * a.Length));

            foreach (var prop in a)
                Console.Write(prop.GetValue(obj, null).ToString() + new string(' ', maxlen - prop.GetValue(obj).ToString().Length - 1) + "|");

            Console.WriteLine();
        }

        static void PrintObj<T>(List<T> lst)
        {
            var a = lst[0].GetType().GetProperties();
            int maxlen = Console.WindowWidth / a.Length;
            foreach (var prop in a)
                Console.Write(prop.Name + new string(' ', maxlen - prop.Name.Length - 1) + "|");

            Console.WriteLine(new string('_', maxlen * a.Length));
            foreach (var obj in lst)
            {
                foreach (var prop in a)
                    Console.Write(prop.GetValue(obj, null).ToString() + new string(' ', maxlen - prop.GetValue(obj).ToString().Length - 1) + "|");

                Console.WriteLine();
            }
        }
    }
}
