<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogIn.aspx.cs" Inherits="LogIn" %>

<link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">
<script src="//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/js/bootstrap.min.js"></script>
<script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
<!------ Include the above in your HEAD tag ---------->
<link href="assests/Registration.css" rel="stylesheet" />
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <title>Software Registration</title>
</head>
<body>
    <div class="container">
        <h1 class="well">Sign In</h1>
        <div class="col-lg-12 well">
            <div class="row">
                <form id="Form2" runat="server">
                    <div class="col-sm-12">
                        <div class="row">
                            <div class="col-sm-6 form-group">
                                <label>UserType</label>
                                <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control" DataSourceID="dsUserType" DataTextField="UserType" DataValueField="UserTypeID"></asp:DropDownList>
                                <asp:SqlDataSource ID="dsUserType" runat="server" ConnectionString='<%$ ConnectionStrings:UserDetails %>' SelectCommandType="StoredProcedure" SelectCommand="ListUserType"></asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6 form-group">
                                <label>User Name</label>
                                <input type="text" id="txtUsername" runat="server" placeholder="UserName" class="form-control">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6 form-group">
                                <label>Password</label>
                                <input type="password" id="txtPwd" runat="server" placeholder="Password" class="form-control">
                            </div>
                        </div>
                        <button type="button" id="btnSubmit" runat="server" class="btn btn-lg btn-info">Sign In</button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</body>
</html>


<script src="lib/jquery.min.js"></script>

<script>

    $(function () {
      
    });


    $("#btnSubmit").bind("click", function () {
        debugger;
        var UN = document.getElementById("<%=this.txtUsername.ClientID%>").value;
        var PWD = document.getElementById("<%=this.txtPwd.ClientID%>").value;
        var UT = document.getElementById("<%=this.ddlUserType.ClientID%>").value;
                $.ajax({
                    type: "POST",
                    url: "LogIn.aspx/Login",
                    data: '{UN: ' + JSON.stringify(UN) + ',PWD: ' + JSON.stringify(PWD) + ',UT: ' + JSON.stringify(UT) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {                      
                        var msg = response.d                       
                        if (msg != '0')
                            window.location.href = 'Dashboard.aspx';
                        else {
                            if (UT == '1') {
                                alert("Wrong Username or Password");
                            }
                            else {
                                alert("No More Node or Check Your Username and Password");
                            }
                        }
                    },
                    failure: function (response) {
                        swal(response.d);
                    },
                    error: function (response) {
                        swal(response.d);
                    }
                });
            //}
        });


</script>