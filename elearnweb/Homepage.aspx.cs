using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eLearnBL;

namespace eLearnWeb
{
    public partial class Homepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

           if(Session["login"]!=null)
            {
                
                    CourseRep.DataSource = (Session["login"] as eLearnBL.User).GetCourses().Collection;
                    CourseRep.DataBind();
                    
            }
        }
    }
}