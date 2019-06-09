<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
    <div class="banner">
        <h2>
            <a href="index.html">Home</a>
            <i class="fa fa-angle-right"></i>
            <span>Dashboard</span>
        </h2>
    </div>
    <div class="grid-form">
        <div class="grid-form1">
            <h3 id="forms-example" class="">Verify Users</h3>
            <form>
                <div class="form-group">
                    <div id="divCircles">
                        <label for="lblUserList">User List</label>
                    </div>
                </div>
                <div class="row" id="divGrid">
                    <div class="col-lg-12">
                        <div class="table-responsive">

                            <asp:GridView ID="gvEvents" CssClass="table table-striped table-bordered table-hover" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="RowNumber" SortExpression="RowNumber" HeaderText="No." ></asp:BoundField>
                                    <asp:BoundField DataField="NodeCode" SortExpression="NodeCode" HeaderText="Node Code" ></asp:BoundField>
                                    <asp:BoundField DataField="ComputerName" SortExpression="ComputerName" HeaderText="Computer Name" ></asp:BoundField>
                                    <asp:BoundField DataField="OSInformation" SortExpression="OSInformation" HeaderText="OS Information" ></asp:BoundField>
                                    <asp:BoundField DataField="MACAddress" SortExpression="MACAddress" HeaderText="MAC Address"></asp:BoundField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>Verify</HeaderTemplate>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                        <HeaderStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Delete</HeaderTemplate>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                        <HeaderStyle  />
                                    </asp:TemplateField>                                   
                                </Columns>
                            </asp:GridView>
                            <asp:HiddenField ID="hdnISEdit" runat="server" />
                            <asp:HiddenField ID="hdnISEdit1" runat="server" />
                            <asp:HiddenField ID="hdnOtherParticipateID" runat="server" />
                            <asp:HiddenField ID="hdnDetailID" runat="server" />
                            <asp:HiddenField ID="hdnAETid" runat="server" />
                            <asp:HiddenField ID="hdnAETTypeid" runat="server" />
                            <div class="Pager">
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </div>
        <asp:HiddenField ID="HiddenField1" runat="server"  />
       
    </div>
    <script>

        $(function () {
           
           // FillMember();
            LoadGrid();
        });
        
        
    
        function save(NodeCode, NodeID) {
           
           
            var Event = {};
            Event.Members = NodeCode;
            Event.NodeID = NodeID;
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/VerifyUser",
                data: '{Events: ' + JSON.stringify(Event) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var msg = response.d;
                    if (msg == true) {
                        alert("Verification Done..!")
                    }
                },
                failure: function (response) {
                    swal(response.d);
                },
                error: function (response) {
                    swal(response.d);
                }
            });
            LoadGrid();
        }

        $("#btnVerify").bind("click", function () {
           
            var Members = "";
            var Event = {};
            $('input[name="Members"]:checked').each(function () {
                Members += $(this).val() + ",";
            });

            Event.Members = Members;

            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/VerifyUser",
                data: '{Events: ' + JSON.stringify(Event) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var msg = response.d;
                    if (msg == true) {
                        alert("Verification Done..!")
                    }                        
                },
                failure: function (response) {
                    swal(response.d);
                },
                error: function (response) {
                    swal(response.d);
                }
            });

        });

        function LoadGrid() {
           
                $.ajax({
                    type: "POST",
                    url: "Dashboard.aspx/LoadGrid",
                    data: '{}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnGridSuccessActivityGrid,
                    failure: function (response) {
                       // swal(response.d);
                    },
                    error: function (response) {
                       // swal(response.d);
                    }
                });           
        }

        var myrowl;
        function OnGridSuccessActivityGrid(response) {

            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var customers = xml.find("EventDetails");
            if (myrowl == null) {
                myrowl = $("[id*=gvEvents] tr:last-child").clone(true);
            }
            $("[id*=gvEvents] tr").not($("[id*=gvEvents] tr:first-child")).remove();

            if (customers.length > 0) {

                $.each(customers, function () {
                    var customer = $(this);
                    var userid = '<%= Session["UserType"].ToString() %>';

                    $("td", myrowl).eq(0).html($(this).find("RowNumber").text());
                    $("td", myrowl).eq(1).html($(this).find("NodeCode").text());
                    $("td", myrowl).eq(2).html($(this).find("ComputerName").text());
                    $("td", myrowl).eq(3).html($(this).find("OSInformation").text());
                    $("td", myrowl).eq(4).html($(this).find("MACAddress").text());
                    if ($(this).find("SystemID").text() == "") {
                        $("td", myrowl).eq(5).html($('<button type="button"  onclick="return save(' + $(this).find("NodeCode").text() + ',' + $(this).find("NodeID").text() + ');"><i class="fa fa-edit"></i></a>'));
                    }
                    else {
                        $("td", myrowl).eq(5).html("Verified");
                    }
                    if (userid == "1") {

                        $("td", myrowl).eq(6).html($('<a class="btn btn-flat ink-reaction btn-default" onclick="return DeleteEvent(' + $(this).find("NodeCode").text() + ',' + $(this).find("NodeID").text() + ');"><i class="fa fa-trash"></i></a>'));
                    }
                    else {
                        $("td", myrowl).eq(6).html($('<a class="btn btn-flat ink-reaction btn-default" disabled="true" onclick="return DeleteEvent(' + $(this).find("NodeCode").text() + ',' + $(this).find("NodeID").text() + ');"><i class="fa fa-trash"></i></a>'));
                    }

                    $("[id*=gvEvents]").append(myrowl);
                    myrowl = $("[id*=gvEvents] tr:last-child").clone(true);
                });

                var pager = xml.find("Pager");

                $(".Pager").ASPSnippets_Pager({
                    ActiveCssClass: "current",
                    PagerCssClass: "pager",
                    PageIndex: parseInt(pager.find("PageIndex").text()),
                    PageSize: parseInt(pager.find("PageSize").text()),
                    RecordCount: parseInt(pager.find("RecordCount").text())
                });


            }
            else {
                var empty_row = myrowl.clone(true);
                $("td:first-child", empty_row).attr("colspan", $("td", myrowl).length);
                $("td:first-child", empty_row).attr("align", "center");
                $("td:first-child", empty_row).html("No records found for the search criteria.");
                $("td", empty_row).not($("td:first-child", empty_row)).remove();
                $("[id*=gvEvents]").append(empty_row);
            }
        };
      
    </script>
</asp:Content>

