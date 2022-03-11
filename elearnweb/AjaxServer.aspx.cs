using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eLearnBL;
namespace eLearnWeb
{
    public partial class AjaxServer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["ajax"] != null)
            {
                if (Request.Params["command"] != null)
                {
                    switch (Request.Params["command"])
                    {
                        case "login":
                            if (Request.Params["user"] != null && Request.Params["password"] != null && Session["login"] == null)
                            {
                                User us = PasswordUtils.IsUser(Request.Params["user"], Request.Params["password"]);
                                if (us != null)
                                {
                                    Session["login"] = us;
                                    Response.Write("ok");
                                    Response.End();
                                    return;
                                }

                            }
                            Response.Write("bad");
                            Response.End();
                            break;

                        case "isLogged":
                            Response.Write(Session["login"] != null);
                            Response.End();
                            break;

                        case "logout":
                            if (Session["login"] != null)
                            {
                                Session["login"] = null;
                                Response.Write("ok");
                                Response.End();
                            }
                            else {
                                Response.Write("bad");
                                Response.End();
                            }
                            break;
                    }
                }
            }
            Response.Write("<p class='ajax-error'> Could not understand given request :( </p>");
            Response.End();

        }
    }
}