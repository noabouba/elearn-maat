using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using eLearnBL;
using System.Web.UI.WebControls;
using System.Drawing;

namespace eLearnWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["err"] != null)
            {
                errLbl.Text = Request.QueryString["err"].ToString();
                errLbl.Font.Size =24;
            }
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            string refer;
          
            if (Request.QueryString["ref"] != null)
            {
                refer = Server.UrlEncode(Request.QueryString["ref"]);
            }
            else
                refer = "/Homepage.aspx";

            eLearnBL.User user = PasswordUtils.IsUser(Email.Text, Password.Text);
            if (user != null)
            {
                Session["login"] = user;
                Response.Redirect(refer);
            }
            else
            {
                errLbl.Text = "Invalid user or password.";
                errLbl.ForeColor = Color.Red;
            }
        }


    }
}