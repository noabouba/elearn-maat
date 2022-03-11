<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="eLearnWeb.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row well ">
       <style>
           .register{
               list-style-type:none;
           }
           li > span{
               color:red;
           }
       </style>
        <ul class="register col-md-4 col-sm-offset-1">
            <li>
                First name
                <asp:TextBox CssClass="form-control"  ID="FirstName" placeholder="First name" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="RegisterValidation" ErrorMessage="Field is required" ControlToValidate="FirstName" runat="server" />
            </li>

            <li>
                Last name
                <asp:TextBox CssClass="form-control"  ID="LastName" placeholder="Last name" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="RegisterValidation" ErrorMessage="Field is required" ControlToValidate="LastName" runat="server" />
            </li>
            <li>
                Email 
                <asp:TextBox CssClass="form-control"  TextMode="Email" ID="Email" placeholder="Email Address" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="RegisterValidation" ErrorMessage="Field is required" ControlToValidate="Email" runat="server" />
                <asp:CustomValidator ErrorMessage="Email already exists." OnServerValidate="Email_ServerValidate" ValidationGroup="RegisterValidation"  ControlToValidate="Email" runat="server" />
                <asp:RegularExpressionValidator ValidationGroup="RegisterValidation" ErrorMessage="Invalid Email address" ControlToValidate="Email" ValidationExpression="^(\S+@\w+\.\w+)" runat="server" />
            </li>
            <li>
                Password
                <asp:TextBox CssClass="form-control"  TextMode="Password" type="password" ID="Password" placeholder="Password" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="RegisterValidation" ErrorMessage="Field is required" ControlToValidate="Password" runat="server" />
                <asp:RegularExpressionValidator ValidationGroup="RegisterValidation" ErrorMessage="Minimum password length is 6"  ControlToValidate="Password" runat="server" ValidationExpression="^(\S{6,})" />
            </li>
            <li>
                Birth Date <span class="text-muted">(DD/MM/YYYY)</span>
                <asp:TextBox CssClass="form-control"  TextMode="Date" ID="Bday" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="RegisterValidation" ErrorMessage="Field is required" ControlToValidate="Bday" runat="server" />
                <asp:CompareValidator ValidationGroup="RegisterValidation" Type="Date" Operator="DataTypeCheck" ErrorMessage="Date is invalid" ControlToValidate="Bday" runat="server" />
                <asp:CustomValidator ValidationGroup="RegisterValidation" ErrorMessage="Date is invalid" ControlToValidate="Bday" OnServerValidate="Unnamed_ServerValidate" runat="server" />
            </li>
            <li>
                <asp:DropDownList CssClass="form-control"  ID="Role" runat="server">
                    <asp:ListItem Selected="True" Value="0">Student</asp:ListItem>
                    <asp:ListItem Value="1">Teacher</asp:ListItem>
                </asp:DropDownList>
                <%--  --%>
            </li>
            <li>
                <br />
                <asp:Button ID="regBtn" CssClass="btn btn-default" ValidationGroup="RegisterValidation" runat="server" Text="Register" OnClick="regBtn_Click" />
            </li>
        </ul>

    </div>
</asp:Content>
