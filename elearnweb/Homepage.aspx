<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="eLearnWeb.Homepage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid head-container" id="home">
        <div class="row text-center">
            <div class="col-md-12">
                <% if (Session["login"] == null)
                    { %>
                <span class="head-main">Participate in professional courses for FREE!</span>
                <h3 class="head-last">Aquire new skills and education for free throught courses taught by REAL proffesionals!
                    <br />
                    Not registered yet? Register now and start learning new stuff everyday for free!
                </h3>
                <a href="Register.aspx" class="btn btn-danger btn-lg head-btn-one">REGISTER</a>
                <%}
                    else {
                %>

                <span class="head-main">Continue your previous courses</span>
                <% if ((Session["login"] as eLearnBL.User).GetCourses().Collection.Count != 0)
                    { %>
                <center>
                <asp:Repeater ID="CourseRep" runat="server">
                    <HeaderTemplate>
                        <div class="list-group" style="padding: 10px; width: 40%">
                    </HeaderTemplate>
                    <FooterTemplate></div> </FooterTemplate>
                    
                    <ItemTemplate>

                        <a href="/CoursePage.aspx?id=<%# (Container.DataItem as eLearnBL.Course).CourseID %>" class="list-group-item"><%# (Container.DataItem as eLearnBL.Course).Subject %></a>
                        
                    </ItemTemplate>
                </asp:Repeater>
                    </center>
                <%} else { %>
                <br />
                <span style="color:#444444" class="head-sub-main">You have no courses... :(</span>
                <%}
                    } %>
            </div>
        </div>
    </div>

</asp:Content>
