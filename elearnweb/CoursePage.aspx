<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CoursePage.aspx.cs" Inherits="eLearnWeb.CoursePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="CourseScript.js"></script>
    <link href="lesson_style.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row well">
        <div class="row">
            <div class="col-sm-offset-2">
                <asp:Label runat="server" ID="CourseTitle" CssClass="text-center head-main"></asp:Label>
                <br />
                <br />
            </div>
        </div>

        <div class="row">

            <div class="col-md-3 ">

                <asp:Repeater ID="lessonRepeater" runat="server">
                    <HeaderTemplate>
                        <div class="list-group">
                            <div class="list-group-item active">Lessons</div>
                    </HeaderTemplate>

                    <FooterTemplate>
                        </div>
                    </FooterTemplate>

                    <ItemTemplate>
                        <a href="#" class="lesson-list-item list-group-item"
                            lessonid="<%# (Container.DataItem as eLearnBL.Lesson).LessonID %>"
                            videoid="<%# GetVideoID(Container.DataItem as eLearnBL.Lesson) %>"
                            percent="<%# (Container.DataItem as eLearnBL.Lesson).GetUserProgress((Session["login"] as eLearnBL.User)) %>"
                            onload="colorLesson(this)">

                            <%# (Container.DataItem as eLearnBL.Lesson).Title %>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div id="comment-panel" disabled="true" class="col-md-8 comment">
                <textarea class="form-control"></textarea>
                <br />
                <input onclick="newComment($(this).siblings('textarea'))" type="button" value="Post comment" class="btn btn-success" />
                <input onclick="$(this).parent().hide()" type="button" value="Cancel" class="btn btn-danger" />
                <br />

            </div>
            <div id="rightpane" class="col-md-8">
                <div id="player">
                </div>
                <div class="col-md-8">
                    <input onclick="commentPanel(this)" type="button" class="btn btn-primary btn-lg" value="Write a comment" />
                </div>
                <div id="comments" class="col-md-10">
                </div>
            </div>

            <!--
            Hidden repeater starts here
            -->
            <style>
                .ay {
                    max-height: 350px;
                    overflow-y: auto;
                }
            </style>
            <asp:Repeater ID="parentRep" runat="server">

                <HeaderTemplate>
                    <div class="ay">
                </HeaderTemplate>

                <FooterTemplate>
                </FooterTemplate>

                <ItemTemplate>
                    <!-- Item starts here -->
                    <ul class="tree">
                        <li class="comment parent" commentid="<%# (Container.DataItem as eLearnBL.CommentTree).Head.CommentID %>"><%# (Container.DataItem as eLearnBL.CommentTree).Head.Message %>
                            <br />
                            <span class="comment-info">
                                <a href="/Profile.aspx?id=<%# (Container.DataItem as eLearnBL.CommentTree).Head.PublisherID %>">
                                    <%# (Container.DataItem as eLearnBL.CommentTree).Head.Publisher.FullName %>
                                </a>
                                , <%# (Container.DataItem as eLearnBL.CommentTree).Head.Date.ToShortDateString() %>
                                <a href="#" onclick="collapse(this)" class="child-collapse">Show child comments</a>
                                <% if (Session["login"] != null)
                                    { %>
                                <a href="#" onclick="commentPanel(this)" class="button-reply pull-right">Reply to comment</a>

                                <%} %>
                            </span>
                        </li>
                        <asp:Repeater ID="childRep" runat="server" DataSource="<%# (Container.DataItem as eLearnBL.CommentTree).Children %>">
                            <HeaderTemplate>
                                <li></li>
                                <ul class="children">
                            </HeaderTemplate>
                            <FooterTemplate></ul> </FooterTemplate>
                            <ItemTemplate>
                                <li class="comment child">
                                    <%# (Container.DataItem as eLearnBL.Comment).Message %>
                                    <br />
                                    <span class="comment-info">

                                        <a href="/Profile.aspx?id=<%# (Container.DataItem as eLearnBL.Comment).PublisherID %>">
                                            <%# (Container.DataItem as eLearnBL.Comment).Publisher.FullName %>
                                        </a>
                                        , <%# (Container.DataItem as eLearnBL.Comment).Date.ToShortDateString() %>
                                    </span>
                                </li>

                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </ItemTemplate>
            </asp:Repeater>
            <!-- Hidden repeater ends here 
            -->
        </div>

    </div>
</asp:Content>
