using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eLearnBL;

namespace eLearnWeb
{
    public partial class Profile : System.Web.UI.Page
    {
        public struct CoursePercentPair
        {
            public Course course;
            public int percent;
            public CoursePercentPair(Course c, int pe)
            {
                this.course = c;
                this.percent = pe;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            int id;
            eLearnBL.User profile;
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out id))
            {
                try
                {
                    profile = new User(id);
                }
                catch
                {
                    return; // could not find user
                }

                Name.Text = profile.FirstName + " " + profile.LastName;
                Name.Font.Size = 36;
                Role.Text = profile.Role.ToString();
                Role.Font.Size = 18;



                //TOOD: Course Progress Bar????
                CourseRep.DataSource = profile.GetCourses().Collection.Select(a => new CoursePercentPair(a, a.GetUserProgress(profile))).ToList();
                CourseRep.DataBind();
            }
            else
                Response.Redirect("/Homepage.aspx");


        }
    }
}