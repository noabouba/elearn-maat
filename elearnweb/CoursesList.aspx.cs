using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using eLearnBL;
using System.IO;


namespace eLearnWeb
{
    public partial class CoursesList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Courses cr;
            List<Course> ls;

            if (!IsPostBack)
            {
                searchRep.DataSource = Enum.GetNames(typeof(eLearnDAL.CategoryDAL.Categories));
                searchRep.DataBind();
            }
            if (Session["courses"] == null)
            {
                cr = new Courses();
                Session["courses"] = cr;
            }

            cr = Session["courses"] as Courses;

            if (Request.QueryString["cat"] != null)
                try
                {
                    
                    eLearnDAL.CategoryDAL.Categories cat = (eLearnDAL.CategoryDAL.Categories)int.Parse(Request.QueryString["cat"]);
                    ls = cr.Collection.FindAll(a => a.Category == cat).ToList();
                }
                catch
                {
                    ls = cr.Collection;
                }
            else
                ls = cr.Collection;

            TableCell tc;
            foreach (Course course in ls)
            {
                TableRow tr = new TableRow();
                tc = new TableCell();
                tc.CssClass = "course-table-cell";
                tr.HorizontalAlign = HorizontalAlign.Justify;
                Image a = new Image();

                try
                {
                    if (File.Exists("/Images/" + course.CourseID + ".jpg"))
                        a.ImageUrl = "/Images/" + course.CourseID + ".jpg";
                    else throw new Exception();

                }
                catch
                {
                    a.ImageUrl = "/Images/def.jpg";
                }

                a.CssClass = "img-circle";
                a.ImageAlign = ImageAlign.Left;
                a.Width = 150;
                a.Height = 150;
                tc.Controls.Add(a);
                tr.Controls.Add(tc);


                tc = new TableCell();
                tc.CssClass = "course-table-cell";

                HyperLink header = new HyperLink();
                header.NavigateUrl = "/CoursePage.aspx?id=" + course.CourseID;
                header.Text = course.Subject + "<br/>";
                tc.Controls.Add(header);
                header.Font.Bold = true;

                Label tttt = new Label();
                tttt.Text = "Teacher: ";
                tc.Controls.Add(tttt);

                HyperLink lbl = new HyperLink();
                lbl.Text = course.Teacher.FirstName + " " + course.Teacher.LastName;
                lbl.NavigateUrl = "/Profile.aspx?id=" + course.Teacher.UserID;
                tc.Controls.Add(lbl);

                tr.Controls.Add(tc);

                var post = course.PostCourses;
                var pre = course.PreCourses;

                tc = new TableCell();
                tc.CssClass = "course-table-cell text-center";

                if (pre.Count > 0)
                {
                    Label ll = new Label();
                    ll.Text = "Recommended pre-courses:";
                    ll.Style["font-weight"] = "bold";
                    ll.Style["font-size"] = "12px";
                    tc.Controls.Add(ll);
                    BulletedList bl = new BulletedList();
                    bl.Style["font-size"] = "12px";
                    bl.Items.AddRange(pre.Select(x => new ListItem()
                    {
                        Value = "'/CoursePage.aspx?id=" + x.CourseID,
                        Text = x.Subject

                    }).ToArray());
                    bl.Style["list-style-type"] = "none";
                    bl.DisplayMode = BulletedListDisplayMode.HyperLink;

                    tc.Controls.Add(bl);
                }
                if (post.Count > 0)
                {
                    Label ll = new Label();
                    ll.Text = "Recommended post-courses:";
                    ll.Style["font-weight"] = "bold";
                    ll.Style["font-size"] = "12px";

                    tc.Controls.Add(ll);
                    BulletedList bl = new BulletedList();
                    bl.Style["font-size"] = "12px";
                    bl.Items.AddRange(post.Select(x => new ListItem()
                    {
                        Value = "/CoursePage.aspx?id=" + x.CourseID,
                        Text = x.Subject
                    }).ToArray());

                    bl.Style["list-style-type"] = "none";
                    bl.DisplayMode = BulletedListDisplayMode.HyperLink;
                    tc.Controls.Add(bl);
                }

                tr.Controls.Add(tc);


                tc = new TableCell();
                tc.CssClass = "course-table-cell text-center";
                Button enrollBt = new Button();
                enrollBt.Text = "Enroll";
                enrollBt.Enabled = Session["login"] != null;
                enrollBt.CssClass = "btn btn-default";
                enrollBt.Click += (s, r) =>
                    {
                        if (enrollBt.Enabled)
                        {
                            User ss = (Session["login"] as User);
                            if (!ss.GetCourses().Collection.Contains(course))
                                ss.AddCourse(course.CourseID);
                        }
                    };

                if (enrollBt.Enabled)
                {
                    User us = (Session["login"] as User);
                    if (us.GetCourses().Collection.Any(b => b.Subject == course.Subject))
                    {

                        enrollBt.Enabled = false;
                        enrollBt.CssClass = "btn btn-success";
                    }
                }

                tc.Controls.Add(enrollBt);
                tr.Controls.Add(tc);
                CourseTable.Rows.Add(tr);
            }



        }
        public int GetCatId(string cat)
        {
            var x = (int)(Enum.Parse(typeof(eLearnDAL.CategoryDAL.Categories), cat));
            return x;
        }

    }
}