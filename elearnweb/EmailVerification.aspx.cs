using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eLearnBL;

namespace eLearnWeb
{
    public partial class EmailVerification : System.Web.UI.Page
    {
        private const string subName = "eLearn";
        private static VerifyEmail.VerifyEmail service;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null)
                Response.Redirect("/Homepage.aspx");

            if ((Session["login"] as eLearnBL.User).IsVerified)
                Response.Redirect("/Homepage.aspx");

            if (Request.Params["ajax"] != null)
                NavigateAjax();

            if (!IsPostBack)
            {
                service = new VerifyEmail.VerifyEmail();
                service.Register(subName);
            }
        }

        private void NavigateAjax()
        {
            eLearnBL.User us = Session["login"] as eLearnBL.User;

            if (Request.Params["command"] != null)
            {
                switch (Request.Params["command"].ToLower())
                {
                    case "verify":
                        if (Request.Params["code"] != null)
                        {
                            bool verified = service.VerifyCode(subName, us.Email, Request.Params["code"]);
                            if (verified)
                            {
                                Response.Write("ok");
                                us.IsVerified = true;
                            }
                            else Response.Write("bad");

                            Response.End();
                        }
                        break;

                    case "resend":
                        bool success = service.SendVerification(subName, us.Email);
                        if (success)
                            Response.Write("ok");
                        else Response.Write("bad");
                        Response.End();
                        break;
                }
            }
        }


    }
}