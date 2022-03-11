using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eLearnBL;
namespace eLearnWeb
{
    public partial class SuggestCourse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["login"] == null)
                Response.Redirect("/Homepage.aspx");

            if (Request.Params["ajax"] != null)
            {
                if (Require(new string[] { Request.Params["subject"], Request.Params["titles"], Request.Params["videos"],
                Request.Params["desc"], Request.Params["category"], Request.Params["type"]}))
                {
                    string[] titles = Request.Params["titles"].Split(';');
                    string[] vids = Request.Params["videos"].Split(';');
                    eLearnDAL.VideoTitlePair[] arr = new eLearnDAL.VideoTitlePair[titles.Length - 1];

                    for (int i = 0; i < titles.Length - 1; i++)
                    {
                        arr[i] = new eLearnDAL.VideoTitlePair(vids[i], titles[i]);
                    }

                    CourseSuggestion cs = new CourseSuggestion((Session["login"] as eLearnBL.User).UserID, Request.Params["subject"], Request.Params["desc"],
                        arr, (eLearnDAL.CategoryDAL.Categories)int.Parse(Request.Params["category"]), (eLearnDAL.SuggestionType)(int.Parse(Request.Params["type"])));

                    Response.Write("ok");
                    Response.End();
                }
                else
                {
                    Response.Write("bad");
                    Response.End();

                }
            }
        }

        private bool Require(string[] arr)
        {
            return !arr.Any(a => a == null);
        }
        protected void subBtn_Click(object sender, EventArgs e)
        {
            ////if (Titles.Text.Split(';').Length != Videos.Text.Split(';').Length)
            ////{
            ////    Response.Write("<script> alert('Video and Titles are not valid. Please make sure you have the same amount of Videos and corresponding Titles.') </script>");
            ////    Response.End();
            ////    return;
            ////}

            ////string[] titles = Titles.Text.Split(';');
            ////string[] vids = Videos.Text.Split(';');

            //eLearnDAL.VideoTitlePair[] arr = new eLearnDAL.VideoTitlePair[vids.Length];
            //for (int i = 0; i < vids.Length; i++)
            //{
            //    arr[i] = new eLearnDAL.VideoTitlePair(vids[i], titles[i]);
            //}

            //CourseSuggestion cs = new CourseSuggestion((Session["login"] as eLearnBL.User).UserID, Subject.Text, Description.Text,
            //    arr, (eLearnDAL.CategoryDAL.Categories)int.Parse(Category.SelectedValue), (eLearnDAL.SuggestionType)(int.Parse(SuggType.SelectedValue)));

            //Response.Write("<script> alert('Suggestion sent.'); setTimeout(function(){ window.location.replace('/Homepage.aspx');},1500); </script>");

            //Response.End();
        }
    }
}