<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SuggestCourse.aspx.cs" Inherits="eLearnWeb.SuggestCourse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(document).ready(function () {
            $("#add").click(function () {
                newRow();

            });

            $("#remove").click(function () {
                deleteRow();
            });

            $("[name=submit]").click(function () {
                var subject = $("[name=subject]").val();
                var x = wrapUp();
                var titles = x[0];
                var videos = x[1];
                var description = $("[name=description]").val();
                var cat = $("[name=category]").val();
                var type = $("[name=type]").val();

                // ajax

                $.ajax({
                    url: "/SuggestCourse.aspx",
                    method: "POST",
                    success: function (data) {
                        if (data == "ok") {
                            alert("Suggestion sent.")
                        }
                        else {
                            alert("Could not send suggestion.")
                        }
                    },
                    data: {
                        ajax :1,
                        subject: subject,
                        titles: titles,
                        videos: videos,
                        desc: description,
                        category: cat,
                        type: type
                    }
                });
            });


            function newRow() {
                $("#list").append(`<tr class="pair">
  		            <td class='title-box'>
    	            <input type="text" class="form-control"/>
  		            </td>
  		            <td class='video-box'>
    	            <input type="text" class="form-control"/>
  		            </td>
			</tr>`);
            }


            function deleteRow() {
                if ($(".pair").length > 1) {
                    $("#list tr:last-child").remove();
                }
            }

            function wrapUp() {
                var titles = "";
                var vids = "";
                var rows = $(".pair");
                for (var x = 0; x < rows.length; x++) {
                    titles += $(rows[x]).find('.title-box > input').val() + ";";
                    vids += $(rows[x]).find('.video-box > input').val() + ";";
                }
                return [titles, vids];

            }

        });

    </script>
    <style>
        .pair > td {
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="well">
        <div class="form-group row">
            <ul style="list-style-type: none" class="col-md-4 col-sm-offset-1">
                <li>Subject
                <input type="text" class="form-control" name="subject" placeholder="Subject" />
                    <br />
                </li>

                <li>
                    <%--Youtube Videos (Links seperated by ';')
                    <asp:TextBox CssClass="form-control" ID="Videos" placeholder="Videos" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="SuggestValidation" CssClass="text-danger" ErrorMessage="Field is required" ControlToValidate="Videos" runat="server" />
                    <br />
                </li>
                <li>Corresponding Titles(Seperated by ';')
                    <asp:TextBox CssClass="form-control" ID="Titles" placeholder="Titles" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ValidationGroup="SuggestValidation" CssClass="text-danger" ErrorMessage="Field is required" ControlToValidate="Titles" runat="server" />
                    <br />--%>
                    <div>
                        <table id='list'>
                            <th>Title</th>
                            <th>Video</th>
                            <tr class="pair">
                                <td class='title-box'>
                                    <input type="text" class="form-control" />
                                </td>
                                <td class='video-box'>
                                    <input type="text" class="form-control" />
                                </td>
                            </tr>
                        </table>
                        <div class="col-sm-offset-2">
                            <input type="button" class="btn btn-primary" value="Add lesson" id="add" />
                            <input type="button" class="btn btn-danger" value="Remove" id="remove" />
                        </div>

                    </div>
                    <br />

                </li>
                <li>Describe the suggestion
                <textarea class="form-control" name="description" placeholder="Description"></textarea>
                    <br />
                </li>

                <li>
                    <select name="category" class="form-control">
                        <option value="1">Science</option>
                        <option value="2">Math</option>
                        <option value="3">Art</option>
                        <option value="4">Programming</option>
                    </select>

                    <%--  --%>

                    <br />
                </li>
                <li>
                    <select name="type" class="form-control">
                        <option value="1">New Course</option>
                        <option value="2">New Lessons</option>
                        <option value="3">Other </option>
                    </select>
                    <%--  --%>
                </li>
                <li>
                    <br />
                    <input type="button" name="submit" value="Send Suggestion" class="btn btn-default" />

                </li>
            </ul>
        </div>
    </div>
</asp:Content>
