<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhoneVerification.aspx.cs" Inherits="PhoneVerification" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Softerware Registration</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">
    <script src="//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/js/bootstrap.min.js"></script>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
    <!------ Include the above in your HEAD tag ---------->
    <link href="assests/Registration.css" rel="stylesheet" />
    <style>
        /* Remove the navbar's default margin-bottom and rounded borders */
        .navbar {
            margin-bottom: 0;
            border-radius: 0;
        }

        /* Set height of the grid so .sidenav can be 100% (adjust as needed) */
        .row.content {
            height: 450px;
        }

        /* Set gray background color and 100% height */
        .sidenav {
            padding-top: 20px;
            /*background-color: #f1f1f1;*/
            height: 100%;
        }

        /* Set black background color, white text and some padding */
        footer {
            /*background-color: #555;*/
            color: white;
            padding: 15px;
        }

        /* On small screens, set height to 'auto' for sidenav and grid */
        @media screen and (max-width: 767px) {
            .sidenav {
                height: auto;
                padding: 15px;
            }

            .row.content {
                height: auto;
            }
        }
    </style>
</head>
<body>

    <nav class="navbar navbar-inverse">
        <div class="container-fluid">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>

            </div>

        </div>
    </nav>
  
    <div class="container-fluid text-center">
        <div class="row content">
            <div class="col-sm-2 sidenav">
            </div>
            <div class="col-sm-8 text-left">
                <h1>
                    <center>Congratulation.</center>
                </h1>
                <h4>
                    <center>Your Email Address and Phone No. are now Confirmed. And now you can Login by<br />
          <br />
          <br />
          <h5>
          UserName: <asp:Label ID="lblUN" runat="server"></asp:Label>
          <br />
          Password: <asp:Label ID="lblPWD" runat="server"></asp:Label>
          </h5> 
           and enjoy our company services..
                        <br/>
                       <h43> *we will send your Client Code to your email ID</h43>
          </center>
                    <br />
                    <br />
                </h4>
                <hr>
                <h5>
                    <center><form id="form1" runat="server"><asp:Button ID="btnLogin" OnClick="btnLogin_Click" runat="server" Text="Login" class="btn btn-lg btn-info" /></form> </center>
                </h5>

            </div>
            <div class="col-sm-2 sidenav">
            </div>
        </div>
    </div>

<footer class="container-fluid text-center">
 
</footer>

</body>
</html>
<script>
 $("#btnLogin").bind("click", function () {    
        window.location.href = 'LogIn.aspx';
 });
    </script>
