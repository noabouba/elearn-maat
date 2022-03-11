<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CoursesList.aspx.cs" Inherits="eLearnWeb.CoursesList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .course-table-cell > * {
            color: black;
        }

        .course-table-cell > img {
            border: 3px dashed #2db2ff;
            
            padding: 6px;
        }
    </style>
    <div class="well row">
        <div class="panel panel-info" style="margin-left:15%; width:35%">
            <div class="panel-heading">
                Search by category
            </div>
            <div class="panel-body">
                <div class="form-inline">
                    <asp:Repeater ID="searchRep" runat="server">
                        <ItemTemplate>
                            <label class="radio-inline">
                                <input type="radio" name="catRadio" value="<%# GetCatId(Container.DataItem.ToString()) %>" /> <%# (Container.DataItem.ToString()) %>
                            </label>
                        </ItemTemplate>

                    </asp:Repeater>
                    <input style="margin:10px" type="button" onclick="window.location.replace('/CoursesList.aspx?cat='+$('input[name=catRadio]:checked').attr('value'))" class="btn btn-primary" value="Filter" />
                </div>
            </div>
        </div>
        <asp:Table runat="server" ID="CourseTable" CssClass="text-center table-font">

        </asp:Table>
    </div>
</asp:Content>
