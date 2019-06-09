<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SignUpForm.aspx.cs" Inherits="SignUpForm" %>

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
        <h1 class="well">Registration Form</h1>
        <div class="col-lg-12 well">
            <div class="row">
                <form runat="server">
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
                                <input type="text" id="txtUserName" runat="server" placeholder="Enter Your UserName" class="form-control">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6 form-group">
                                <label>First Name</label>
                                <input type="text" id="txtFirstName" runat="server" placeholder="Enter First Name Here.." class="form-control">
                            </div>
                            <div class="col-sm-6 form-group">
                                <label>Last Name</label>
                                <input type="text" id="txtLastName" runat="server" placeholder="Enter Last Name Here.." class="form-control">
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group">
                                <div class="col-sm-6 form-group">
                                    <label>Address</label>
                                    <textarea placeholder="Enter Address Here.." id="txtAddress" runat="server" rows="3" class="form-control"></textarea>
                                </div>
                                <div class="col-sm-6 form-group">
                                    <label>Zip</label>
                                    <input type="text" id="txtPin" runat="server" placeholder="Enter Zip Code Here.." class="form-control">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-4 form-group">
                                <label>City</label>
                                <asp:DropDownList ID="ddlCity" runat="server" class="form-control" DataSourceID="dsCity" DataTextField="CityName" DataValueField="CityID"></asp:DropDownList>
                                <asp:SqlDataSource ID="dsCity" runat="server" ConnectionString='<%$ ConnectionStrings:UserDetails %>' SelectCommandType="StoredProcedure" SelectCommand="ListCity"></asp:SqlDataSource>
                            </div>
                            <div class="col-sm-4 form-group">
                                <label>State</label>
                                <asp:DropDownList ID="ddlState" runat="server" class="form-control" DataSourceID="dsState" DataTextField="StateName" DataValueField="StateID"></asp:DropDownList>
                                <asp:SqlDataSource ID="dsState" runat="server" ConnectionString='<%$ ConnectionStrings:UserDetails %>' SelectCommandType="StoredProcedure" SelectCommand="ListState"></asp:SqlDataSource>
                            </div>
                            <div class="col-sm-4 form-group">
                                <label>Country</label>
                                <asp:DropDownList ID="ddlCountry" runat="server" class="form-control" DataSourceID="dsCountry" DataTextField="CountryName" DataValueField="CountryID"></asp:DropDownList>
                                <asp:SqlDataSource ID="dsCountry" runat="server" ConnectionString='<%$ ConnectionStrings:UserDetails %>' SelectCommandType="StoredProcedure" SelectCommand="ListCountry"></asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6 form-group">
                                <label>Application Name</label>
                                <input type="text" id="txtApplication" runat="server" placeholder="Enter Application Name Here.." class="form-control">
                            </div>
                            <div class="col-sm-6 form-group">
                                <label>Company Name</label>
                                <input type="text" id="txtCompany" runat="server" placeholder="Enter Company Name Here.." class="form-control">
                            </div>
                        </div>
                        <div class="form-group">
                            <label>Phone Number</label>
                            <input type="text" id="txtMobileNo" runat="server" placeholder="Enter Phone Number Here.." class="form-control">
                        </div>
                        <div class="form-group">
                            <label>Email Address</label>
                            <input type="text" id="txtEmail" runat="server" placeholder="Enter Email Address Here.." class="form-control">
                        </div>
                        <div class="form-group">
                            <label>No.of User</label>
                            <asp:DropDownList ID="ddlUserSelection" runat="server" class="form-control">
                                <asp:ListItem Text="1" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label>Password</label>
                            <input type="password" id="txtPwd" runat="server" placeholder="Enter Your Password Here.." class="form-control">
                        </div>
                        <button type="button" id="btnSubmit" runat="server" class="btn btn-lg btn-info">Sign Up</button>
                        <button type="button" id="btnSignin" runat="server" class="btn btn-lg btn-info">Sign In</button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</body>
</html>

<script src="lib/jquery.min.js"></script>

<script>
    //$(function () {

    //});
    $("#btnSignin").bind("click", function () {
        window.location.href = 'LogIn.aspx';
    });

    $("#btnSubmit").bind("click", function () {

        var UserType = document.getElementById("<%=this.ddlUserType.ClientID %>");
        var UserName = document.getElementById("<%=this.txtUserName.ClientID %>");
        var FirstName = document.getElementById("<%=this.txtFirstName.ClientID %>");
        var LastName = document.getElementById("<%=this.txtLastName.ClientID %>");
        var Address = document.getElementById("<%=this.txtAddress.ClientID %>");
        var PinCode = document.getElementById("<%=this.txtPin.ClientID %>");
        var City = document.getElementById("<%=this.ddlCity.ClientID %>");
        var State = document.getElementById("<%=this.ddlState.ClientID %>");
        var Country = document.getElementById("<%=this.ddlCountry.ClientID %>");
        var MobileNo = document.getElementById("<%=this.txtMobileNo.ClientID %>");
        var Email = document.getElementById("<%=this.txtEmail.ClientID %>");
        var CompanyName = document.getElementById("<%=this.txtCompany.ClientID %>");
        var ApplicationName = document.getElementById("<%=this.txtApplication.ClientID %>");
        var UserSelection = document.getElementById("<%=this.ddlUserSelection.ClientID %>");
        var Password = document.getElementById("<%=this.txtPwd.ClientID %>");

        var Emailfilter = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;

        if (UserName.value == "") {
            UserName.focus();
            alert("Enter User Name..");
            return false;
        }
        else if (FirstName.value == "") {
            FirstName.focus();
            alert("Enter First Name..");
            return false;
        }
        else if (LastName.value == "") {
            LastName.focus();
            alert("Enter Last Name..");
            return false;
        }
        else if (Address.value == "") {
            Address.focus();
            alert("Enter Address..");
            return false;
        }
        else if (PinCode.value == "") {
            PinCode.focus();
            alert("Enter PinCode..");
            return false;
        }
        else if (PinCode.value != "" && PinCode.value.length != 6) {
            PinCode.focus();
            alert("Enter Valid PinCode..");
            return false;
        }
        else if (MobileNo.value != "" && MobileNo.value.length != 10) {
            MobileNo.focus();
            alert("Enter valid MobileNo..");
            return false;
        }
        else if (Email.value != "" && Emailfilter.test(Email.value) == false) {
            alert("Enter Valid Email");
            Email.focus();
            return false;
        }
        else if (Email.value == "") {
            Email.focus();
            alert("Enter Email..");
            return false;
        }
        else if (CompanyName.value == "") {
            CompanyName.focus();
            alert("Enter Company Name..");
        }
        else if (ApplicationName.value == "") {
            ApplicationName.focus();
            alert("Enter Application Name..");
            return false;
        }
        else if (Password.value == "") {
            Password.focus();
            alert("Enter Password..");
            return false;
        }
        else {

            $.ajax({
                type: "POST",
                url: "SignUpForm.aspx/CheckHardware",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: onSuccessCheck,
                failure: function (response) {
                    alert("1");
                    //swal(response.d);
                },
                error: function (response) {
                    alert("2");
                    // swal("error");
                }
            });
        }
    });

    function onSuccessCheck(response) {

        var msg = response.d;

        if (msg == false) {

            var UReg = {};
            UReg.UserName = document.getElementById("<%=this.txtUserName.ClientID %>").value;
            UReg.UserType = document.getElementById("<%=this.ddlUserType.ClientID %>").value;
            UReg.FirstName = document.getElementById("<%=this.txtFirstName.ClientID %>").value;
            UReg.LastName = document.getElementById("<%=this.txtLastName.ClientID %>").value;
            UReg.Address = document.getElementById("<%=this.txtAddress.ClientID %>").value;
            UReg.Pincode = document.getElementById("<%=this.txtPin.ClientID %>").value;
            UReg.City = document.getElementById("<%=this.ddlCity.ClientID %>").value;
            UReg.State = document.getElementById("<%=this.ddlState.ClientID %>").value;
            UReg.Country = document.getElementById("<%=this.ddlCountry.ClientID %>").value;
            UReg.MobileNo = document.getElementById("<%=this.txtMobileNo.ClientID %>").value;
            UReg.Email = document.getElementById("<%=this.txtEmail.ClientID %>").value;
            UReg.Comname = document.getElementById("<%=this.txtCompany.ClientID %>").value;
            UReg.Application = document.getElementById("<%=this.txtApplication.ClientID %>").value;
            UReg.UserSeletetion = document.getElementById("<%=this.ddlUserSelection.ClientID %>").value;
            UReg.Pwd = document.getElementById("<%=this.txtPwd.ClientID %>").value;

            $.ajax({
                type: "POST",
                url: "SignUpForm.aspx/ManageUserDetail",
                data: '{UReg: ' + JSON.stringify(UReg) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: alert("Check Your Email to Verify Your Account"),
                failure: function (response) {
                    swal(response.d);
                },
                error: function (response) {
                    swal(response.d);
                }
            });
        }
        else {
            alert("Your System Is Already Register..!")
        }
    }
</script>