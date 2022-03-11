<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="EmailVerification.aspx.cs" Inherits="eLearnWeb.EmailVerification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function start_loading() {
            $(".screen").show(100);
            $(".loading").show();
            $(".loading").animate({ "top": "200" }, 500);
        }
        function stop_loading() {
            $(".screen").hide();
            $(".loading").hide();
            $(".loading").animate({ "top": "-280px" }, 500);
        }
        $(document).ready(function () {
            $(document).ajaxStart(function () {
                start_loading();
            });
            $(document).ajaxStop(function () {
                stop_loading();
            });

            $("#verify").click(function () {
                $.ajax({
                    url: "/EmailVerification.aspx",
                    type: "POST",
                    success: function (data) {
                        if (data != "ok") {
                            alert("Could not verify code.");
                        }else
                        {
                            alert("Success");
                            setTimeout(function () { window.location = "/Homepage.aspx"; }, 1500);
                        }
                    },

                    data: {
                        ajax: 1,
                        command: "verify",
                        code: $("#code").val()
                    }
                });
            });

            $("#resend").click(function () {
                $.ajax({
                    url: "/EmailVerification.aspx",
                    type: "POST",
                    success: function (data) {
                        if (data != "ok") {
                            alert("Could not resend code.");
                        }
                    },

                    data: {
                        ajax: 1,
                        command: "resend"
                    }
                });
            });

        });
    </script>
    <style>
        .screen {
            overflow: hidden;
            display: none;
            width: 100%;
            position: fixed;
            height: 100%;
            top: 0;
            left: 0;
            padding: 0;
            margin: 0;
            background-color: rgba(161, 161, 161, 0.77);
            z-index: 9998;
        }

        .loading {
            display: none;
            z-index: 9999;
            display: block;
            position: absolute;
            top: -280px;
            left: 45%;
            margin-left: -110px;
            margin-top: -140px;
        }

        input.form-control {
            font-size: 38px;
            height: 100px;
            text-align: center;
            width: 250px;
            background-color: white;
            text-transform: uppercase;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="screen">
    </div>

    <div class="row jumbotron">

        <div class=" col-md-6 col-md-offset-3">
            <div class="loading">
                <img width="280" height="220" src="/Images/loading.gif" />
            </div>
            <div class="text-center head-sub-main">
                Enter your verification code
            </div>

            <center>
                    <div class="form-group">
                        <input maxlength="8" id="code" type="text" class="form-control text-capitalize" placeholder="Code" />
                        <br />
                    </div>

                    <div class="form-inline">
                        <button id="verify" class="btn btn-info">Verify</button>
                        <button id="resend" class="btn btn-default">Resend code</button>
                    </div>

            </center>
        </div>

    </div>
</asp:Content>
