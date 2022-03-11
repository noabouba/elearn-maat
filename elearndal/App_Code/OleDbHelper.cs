using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;

namespace eLearnDAL
{
    class Connect
    {
        public static string GetConnectionString()
        {
            try
            {
                // return @"provider=Microsoft.ACE.OLEDB.12.0; Data source=" + @"..\..\..\..\elearndal\elearndal\App_Data\eLearnDB.accdb";
                return @"provider=Microsoft.ACE.OLEDB.12.0; Data source=" + @"C:\eLearnProj\elearndal\elearndal\App_Data\eLearnDB.accdb";

                //Path.GetFullPath(@"..\..\..\App_Data\eLearnDB.accdb");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
    public class OleDbHelper
    {
        // Disconnected 
        public static DataSet GetDataSet(string strSql)
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(Connect.GetConnectionString());
                OleDbCommand command = new OleDbCommand(strSql, connection);
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        // connected 
        public static object ExecuteScalar(string strSql)// מיועד לפעולות שמחזירות נתון בודד
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(Connect.GetConnectionString());
                OleDbCommand command = new OleDbCommand(strSql, connection);
                connection.Open();
                object obj = command.ExecuteScalar();
                connection.Close();
                return obj;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        // connected 
        static public int ExecuteNonQuery(string strSql)// INSERT UPDATE DELETE מחזיר את מס השורות שהושפעו ע"י הפעולה
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(Connect.GetConnectionString());

                OleDbCommand command = new OleDbCommand(strSql, connection);
                //try
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected;
                //catch  אם יש EXP אין RETURN
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        // Disconnected     
        static public DataSet Fill(string com, string tableName)// שם לוגי לטבלה שתווצר בתוך הDS tableName
        {
            OleDbConnection cn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = cn;
            command.CommandText = com;
            DataSet ds = new DataSet();
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            try
            {
                adapter.Fill(ds, tableName);
            }
            catch (Exception e)
            {

                throw e;
            }
            return ds;

        }
        //פעולה המעדכנת את הדטהבייס בהתאם לדטהסט
        public static void Update(DataSet ds, string com, string name)
        {
            OleDbConnection cn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = cn;
            command.CommandText = com;

            OleDbDataAdapter adapter = new OleDbDataAdapter(command);

            OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
            adapter.InsertCommand = builder.GetInsertCommand();
            adapter.DeleteCommand = builder.GetDeleteCommand();
            adapter.UpdateCommand = builder.GetUpdateCommand();
            try
            {
                cn.Open();
                adapter.Update(ds, name);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cn.Close();
            }
        }
        //Connected
        public static int DoQuery(string sql)
        //הפעולה מקבלת שם מסד נתונים ומחרוזת מחיקה/ הוספה/ עדכון
        //ומבצעת את הפקודה על המסד הפיזי
        {
            try
            {
                OleDbConnection conn = new OleDbConnection(Connect.GetConnectionString());
                conn.Open();
                OleDbCommand com = new OleDbCommand(sql, conn);
                int res = com.ExecuteNonQuery();
                conn.Close();
                return res; //מספר השורות שהושפעו
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///  FULLCONNECTION ב READER עבודה עם  
        ///  מחזיר הפניה לטבלה הפיסית 
        /// </summary>
        /// <returns></returns>
        public static OleDbDataReader GetReader()
        {
            try
            {
                OleDbConnection conn = new OleDbConnection(Connect.GetConnectionString());
                OleDbCommand command = new OleDbCommand("select * from TeachersTbl", conn);

                conn.Open();
                OleDbDataReader reader = command.ExecuteReader();
                return reader;
            }
            catch (Exception e)
            {
                throw e;
            }
            // אלה שישתמשו יעבדו כך
            //while (reader.Read())
            //{
            //    Console.WriteLine(reader[0].ToString());
            //}
            //reader.Close();
        }

        #region Added Methods

        public static bool IsExist(string com, string tableName)
        {
            return OleDbHelper.Fill(com, tableName).Tables[tableName].Rows.Count > 0;
        }

        #endregion


    }
}




