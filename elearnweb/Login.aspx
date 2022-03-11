<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="eLearnWeb.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row well">

        <div class="col-md-6">

            <div class="form-group">
                <asp:TextBox ID="Email" CssClass="form-control" placeholder="Email" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:TextBox ID="Password" CssClass="form-control" placeholder="Password" TextMode="Password" runat="server" />
            </div>
            <asp:Button ID="loginBtn" CssClass="btn btn-primary" runat="server" Text="Login" OnClick="Login_Click" />
            <br />
            <div style="margin:5px; padding: 5px;" class="bg-info">Haven't signed up yet? Click here:<input style="margin: 5px; margin-left: 15px" type="button" class="btn btn-success" value="Register" onclick="window.location = '/Register.aspx';" /></div>

            <asp:Label runat="server" CssClass="bg-danger text-danger" ID="errLbl"></asp:Label>
        </div>
    </div>
</asp:Content>
