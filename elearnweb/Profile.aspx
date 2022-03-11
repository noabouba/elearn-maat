<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="eLearnWeb.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="well col-lg-12 col-md-6 col-xs-6">

        <div id="generalInfo">

            <asp:Label Font-Size="XX-Large" ID="Name" runat="server" />
            <br />
            <asp:Label ForeColor="#999999" Font-Italic="true" Font-Size="Medium" ID="Role" runat="server" />

        </div>
        <div id="profileCourses" >
            <asp:Repeater ID="CourseRep" runat="server">
                <HeaderTemplate><div class="list-group" style="padding:10px; width:40%" >
                    <div class="list-group-item active head-sub-main text-center">Courses</div>
                    </HeaderTemplate>
                <FooterTemplate></div> </FooterTemplate>
                <ItemTemplate>
                    <a href="/CoursePage.aspx?id=<%# ((CoursePercentPair)Container.DataItem).course.CourseID %>" class="list-group-item">
                        <%#  ((CoursePercentPair)Container.DataItem).course.Subject %> 
                        <span class="pull-right text-muted"> <%# ((CoursePercentPair)Container.DataItem).percent %>% Done</span>

                    </a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
