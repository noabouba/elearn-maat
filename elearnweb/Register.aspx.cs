using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eLearnBL;
using System.Text.RegularExpressions;

namespace eLearnWeb
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] != null)
                Response.Redirect("/Homepage.aspx");
        }

        protected void regBtn_Click(object sender, EventArgs e)
        {
            if (PasswordUtils.CanRegister(Email.Text) && Page.IsValid)
            {
                eLearnBL.User newUser = new User(FirstName.Text, LastName.Text, DateTime.Parse(Bday.Text), Email.Text, Role.SelectedValue == "0" ? eLearnBL.Role.Student : eLearnBL.Role.Teacher, Password.Text);
                ScriptManager.RegisterStartupScript(this, GetType(), "hwa", "alert('You have registered.'); window.location = '../Homepage.aspx' ", true);
            }
        }

        protected void Unnamed_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime s = DateTime.Now;
            if (!DateTime.TryParse(args.Value, out s))
            {
                args.IsValid = false;
                
                return;
            }

            if (DateTime.Parse(args.Value) < DateTime.Now.Date && DateTime.Parse(args.Value).Year > 1900)
            {
                args.IsValid = true;
            }
            else
                args.IsValid = false;
        }

        protected void Email_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = PasswordUtils.CanRegister(args.Value);
        }
    }
}