using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearnDAL
{
    public struct VideoTitlePair
    {
        public string Video { get; set; }
        public string Title { get; set; }

        public VideoTitlePair(string vid, string titl)
        {
            this.Title = titl;
            this.Video = vid;
        }
    }

    public enum SuggestionType
    {
        NewCourse = 1,
        NewLessons,
        Other
    }
    public static class CourseSuggestionDAL
    {
        
        /// <summary>
        /// מחזיר את כל ההצעות
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllSuggestions()
        {
            return OleDbHelper.Fill("SELECT * FROM CourseSuggestions", "CourseSuggestions").Tables[0];
        }

        /// <summary>
        /// מחזיר את כל ההצעות שאושרו/לא אושרו
        /// </summary>
        /// <param name="approved"></param>
        /// <returns></returns>
        public static DataTable GetAllSuggestions(bool approved)
        {
            return OleDbHelper.Fill("SELECT * FROM CourseSuggestions WHERE Approved=" + (approved ? 1 : 0).ToString(), "CourseSuggestions").Tables[0];
        }

        /// <summary>
        /// הצעה חדשה
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="subject"></param>
        /// <param name="cat"></param>
        /// <param name="desc"></param>
        /// <param name="videos"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int NewSuggestion(int teacherId, string subject, CategoryDAL.Categories cat, string desc, VideoTitlePair[] videos, SuggestionType type)
        {
            OleDbHelper.DoQuery(string.Format("INSERT INTO CourseSuggestions(TeacherID,Subject,Category,Description,VideoSamples,DateSuggested,SuggestionType) VALUES({0},'{1}',{2},'{3}','{4}','{5}',{6})", 
                teacherId, subject, (int)cat, desc, ParseVideoTitles(videos), DateTime.Now.ToShortDateString(),(int)type));

            return int.Parse(OleDbHelper.Fill("SELECT MAX(Key) FROM CourseSuggestions", "CourseSuggestions").Tables[0].Rows[0][0].ToString());

        }

        /// <summary>
        /// מאשר הצעה
        /// </summary>
        /// <param name="suggestionId"></param>
        public static void ApproveSuggestion(int suggestionId)
        {
            OleDbHelper.DoQuery("UPDATE CourseSuggestions SET Approved=1 WHERE Key=" + suggestionId);
        }

        /// <summary>
        /// ממיר מערך של VideoTitlePair 
        /// למחזרוזת שמאוחסנת במסד
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static string ParseVideoTitles(VideoTitlePair[] arr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in arr)
            {
                sb.Append("({"+item.Title+"}:{"+item.Video+"})");
            }

            return sb.ToString();
        }

    }
}
