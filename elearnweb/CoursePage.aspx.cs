using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eLearnBL;
namespace eLearnWeb
{
    public partial class CoursePage : System.Web.UI.Page
    {
        private static Course cr;
        protected void Page_Load(object sender, EventArgs e)
        {
            parentRep.Visible = false;
            if (cr != null && Request.Params["ajax"] != null)
            {
                NavigateAjax();
                Response.Write("<p class='ajax-error'> Could not understand given request :(</p>");
                Response.End();
            }
            if (!IsPostBack)
            {
                lessonRepeater.Controls.Clear();
                if (Request.QueryString["id"] != null)
                {
                    if (cr == null || cr.CourseID.ToString() != Request.QueryString["id"])
                    {
                        int crId;
                        if (int.TryParse(Request.QueryString["id"], out crId))
                        {
                            cr = new Course(crId);
                        }
                    }

                    lessonRepeater.DataSource = cr.Lessons;
                    lessonRepeater.DataBind();
                }

            }

            CourseTitle.Text = cr.Subject;

        }

        private int min_percent = 70;
        public string WasLessonDone(Lesson les)
        {
            if (Session["login"] != null)
            {
                if (les.GetUserProgress(Session["login"] as eLearnBL.User) >= min_percent)
                {
                    return "list-group-item-success";
                }
            }
            return "";
        }

        public string GetVideoID(Lesson les)
        {
            Uri myUri = new Uri(les.VideoPath);
            return HttpUtility.ParseQueryString(myUri.Query).Get("v");
        }


        public void NavigateAjax()
        {
            int lessonId;

            if (Session["login"] == null)
            {
                Response.Write("login;" + "/Login.aspx?err=" + new HtmlString("You have to login in to do that.").ToHtmlString());
                Response.End();
            }

            if (Request.Params["command"] != null && int.TryParse(Request.Params["lesson"], out lessonId))
            {
                if (lessonId > 0)
                {

                    if (cr.Lessons.Any(a => a.LessonID == lessonId))
                    {
                        Lesson ls = cr.Lessons.First(a => a.LessonID == lessonId);

                        switch (Request.Params["command"])
                        {

                            case "load":
                                Response.Write("ok;"+RenderComments(ls));
                                Response.End();
                                break;
                            case "comment":
                                if (Session["login"] != null && Request.Params["content"] != null && Request.Params["parent"] != null)
                                {
                                    int parentId;
                                    if (int.TryParse(Request.Params["parent"], out parentId))
                                    {
                                        if (parentId < 0)
                                        {
                                            CommentTree ct = new CommentTree(new Comment(ls.LessonID, -1,
                                                (Session["login"] as eLearnBL.User).UserID, Request.Params["content"]));

                                        }
                                        else
                                        {
                                            CommentTree ct;
                                            try { ct = new CommentTree(parentId); }
                                            catch { return; }
                                            ct.AddChildComment(new Comment(ls.LessonID, parentId, (Session["login"] as eLearnBL.User).UserID, Request.Params["content"]));

                                        }

                                        Response.Write(RenderComments(ls));
                                        Response.End();
                                    }

                                }
                                break;
                            case "save_progress":
                                double perc;
                                if (double.TryParse(Request.Params["percent"].ToString(), out perc))
                                {
                                    if (perc > 1)
                                        return;
                                    User s = Session["login"] as eLearnBL.User;
                                    if ((int)(perc * 100) > ls.GetUserProgress(s))
                                    {
                                        if (s != null)
                                        {
                                            ls.SetUserProgress(s, (int)(perc * 100));
                                            Response.Write("ok");
                                            Response.End();
                                        }
                                    }

                                    Response.Write("bad");
                                    Response.End();

                                }
                                break;
                        }
                    }
                }
            }

        }

        private void RedirectToLogin()
        {
            Response.Redirect("/Login.aspx?err=" + new HtmlString("You have to login in to do that.") + "&?ref=" + new HtmlString("/CoursePage.aspx?id=" + cr.CourseID));
        }

        private string RenderComments(Lesson ls)
        {
            parentRep.Visible = true;

            CommentTrees useless = new CommentTrees(ls);
            useless.SortTrees();
            parentRep.DataSource = useless.Collection;
            parentRep.DataBind();


            /// render control
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter stWriter = new System.IO.StringWriter(sb);
            System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(stWriter);
            parentRep.RenderControl(htmlWriter);
            parentRep.Visible = false;
            return sb.ToString();

        }
    }
}