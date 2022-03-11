using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLearnDAL;
using System.Data;
using System.Text.RegularExpressions;

namespace eLearnBL
{
    public class CourseSuggestion
    {
        #region Properties
        private int key;

        public int Key
        {
            get { return key; }
        }

        private int teacherId;

        public int TeacherId
        {
            get { return teacherId; }
        }

        private string subject;

        public string Subject
        {
            get { return subject; }
        }

        private CategoryDAL.Categories cat;

        public CategoryDAL.Categories Category
        {
            get { return cat; }
        }

        private string desc;

        public string Description
        {
            get { return desc; }
        }

        private VideoTitlePair[] videos;
        public VideoTitlePair[] Videos
        {
            get { return videos; }
        }

        private DateTime date;

        public DateTime SuggestionDate
        {
            get { return date; }
        }

        private bool approved;

        public bool IsApproved
        {
            get { return approved; }
        }

        private SuggestionType type;

        public SuggestionType Type
        {
            get { return type; }
        }

        #endregion

        public CourseSuggestion(DataRow dr)
        {
            this.key = int.Parse(dr["Key"].ToString());
            this.teacherId = int.Parse(dr["TeacherID"].ToString());
            this.subject = dr["Subject"].ToString();
            this.cat = (CategoryDAL.Categories)int.Parse(dr["Category"].ToString());
            this.desc = dr["Description"].ToString();
            this.videos = PairsFromString(dr["VideoSamples"].ToString());
            this.date = DateTime.Parse(dr["DateSuggested"].ToString());
            this.approved = dr["Approved"].ToString().ToLower() == "true";
            this.type = (SuggestionType)dr["SuggestionType"];
        }


        public CourseSuggestion(int teacherId, string subject, string desc, VideoTitlePair[] videos, CategoryDAL.Categories cat, SuggestionType type)
        {
            this.key = CourseSuggestionDAL.NewSuggestion(teacherId, subject, cat, desc, videos, type);
            this.teacherId = teacherId;
            this.subject = subject;
            this.cat = cat;
            this.desc = desc;
            this.videos = videos;
            this.date = DateTime.Now.Date;
            this.approved = false;
            this.type = type;
        }

        /// <summary>
        /// Approves this suggestion and returns the new course (if any)
        /// </summary>
        /// <returns></returns>
        public Course Approve()
        {

            CourseSuggestionDAL.ApproveSuggestion(this.key);
            Course cr;
            if (this.type == SuggestionType.NewCourse)
            {
                cr = new Course(new User(this.teacherId), this.subject, this.cat);
                foreach (var vid in videos)
                {
                    cr.AddLesson(new Lesson(DateTime.Now, vid.Video, cr.CourseID, vid.Title));
                }
            }
            else
                cr = new Courses().Collection.First(a => a.Subject.ToLower() == this.subject.ToLower());

            return cr;

        }

        /// <summary>
        /// Returns a list of all suggestions
        /// </summary>
        /// <returns></returns>
        public static List<CourseSuggestion> GetAllSuggestions()
        {
            DataTable dt = CourseSuggestionDAL.GetAllSuggestions();
            List<CourseSuggestion> cr = new List<CourseSuggestion>();

            foreach (DataRow row in dt.Rows)
            {
                cr.Add(new CourseSuggestion(row));
            }

            return cr;
        }

        /// <summary>
        /// Returns a list of all suggestions that have/have not been approved
        /// </summary>
        /// <param name="approved"></param>
        /// <returns></returns>
        public static List<CourseSuggestion> GetAllSuggestions(bool approved)
        {
            DataTable dt = CourseSuggestionDAL.GetAllSuggestions(approved);
            List<CourseSuggestion> cr = new List<CourseSuggestion>();

            foreach (DataRow row in dt.Rows)
            {
                cr.Add(new CourseSuggestion(row));
            }

            return cr;
        }

        /// <summary>
        /// Creates an array of VideoTitlePair from a string that was stored in the database
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        private static VideoTitlePair[] PairsFromString(string st)
        {
            MatchCollection all = Regex.Matches(st, @"\(\{.*?\}\)");
            VideoTitlePair[] arr = new VideoTitlePair[all.Count];
            int i = 0;
            foreach (Match item in all)
            {
                Match mt = Regex.Match(item.Value, @"{(?'title'.*?)}:{(?'video'.*?)}");
                arr[i] = new VideoTitlePair(mt.Groups["video"].Value, mt.Groups["title"].Value);
                i++;
            }

            return arr;
        }
    }
}
