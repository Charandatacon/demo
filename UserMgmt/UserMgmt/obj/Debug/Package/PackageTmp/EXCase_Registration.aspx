﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.master" AutoEventWireup="true" CodeBehind="EXCase_Registration.aspx.cs" Inherits="UserMgmt.EXCase_Registration" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="BodyContent">
    <div role="main">
        <br />
        <div class="">
            <div class="row top_tiles">
                <div class="">
                    <div class="col-md-6 col-sm-6 col-xs-12">
                        <div class="x_panel">
                            <html>
                            <head>
                                <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
                                
                                <script type="text/javascript" src="/common/theme/js/flot/date.js"></script>
                                <title>Hearing</title>
                                   <%--<script language="java
                                       script" type="text/javascript">
                                 $(document).ready(function () {
                                        debugger;
                                       
                                        if ($('#BodyContent_txtDATE').val() == "") {
                                            $('#BodyContent_txtDATE').val($('#BodyContent_txtdob').val());
                                        }
                                 });
</script>--%>
                                <script language="javascript" type="text/javascript">
                                //function Selectdate(e) {
                                //        debugger;
                                //        var todayDate = e.get_selectedDate();
                                //        var dd = todayDate.getDate();
                                //        var mm = todayDate.getMonth() + 1; //January is 0!

                                //        var yyyy = todayDate.getFullYear();
                                //        if (dd < 10) {
                                //            dd = '0' + dd;
                                //        }
                                //        if (mm < 10) {
                                //            mm = '0' + mm;
                                //        }
                                //        todayDate = dd + '-' + mm + '-' + yyyy;
                                     
                                //        $('#BodyContent_txtDATE').val(todayDate);
                                //        $('#BodyContent_txtdob').val(todayDate);
                                //}
                                    function Selectdate(e) {
                                        debugger;
                                    var todayDate = e.get_selectedDate();
                                    var dd = todayDate.getDate();
                                    var mm = todayDate.getMonth() + 1; //January is 0!

                                    var yyyy = todayDate.getFullYear();
                                    if (dd < 10) {
                                        dd = '0' + dd;
                                    }
                                    if (mm < 10) {
                                        mm = '0' + mm;
                                    }
                                    todayDate = dd + '-' + mm + '-' + yyyy;
                                    $('#BodyContent_txtDATE').val(todayDate);
                                    //var date1 = $('#BodyContent_txtDATE').val();
                                    $('#BodyContent_txtdob').val(todayDate);
                                    }
                                    function onlyDotsAndNumbers(txt, event) {
                                        debugger;
                                        var charCode = (event.which) ? event.which : event.keyCode
                                        if (charCode == 46) {
                                            if (txt.value.indexOf(".") < 0)
                                                return true;
                                            else
                                                return false;
                                        }
                                        if (txt.value.indexOf(".") > 0) {
                                            var txtlen = txt.value.length;
                                            var dotpos = txt.value.indexOf(".");
                                            //Change the number here to allow more decimal points than 2
                                            if ((txtlen - dotpos) > 2)
                                                return false;
                                        }
                                        if (charCode > 31 && (charCode < 48 || charCode > 57))
                                            return false;

                                        return true;
                                    }
                                    function phoneValidate() {
                                        debugger;
                                        var mobileN = $('#BodyContent_txtAppellantContactNo').val().length;

                                        if (mobileN != 10) {
                                            alert("Invalid Appellant Mobile No.");
                                            $('#BodyContent_txtAppellantContactNo').val("");
                                            $('#BodyContent_txtAppellantContactNo').focus();
                                        }
                                    }
                                 </script>
                                 <script type="text/javascript">
    function blockSpecialChar(e){
        var k;
        document.all ? k = e.keyCode : k = e.which;
        return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
    }
    function blockAllChar(e) {
        var k;
        document.all ? k = e.keyCode : k = e.which;
        return false;
    }
    function onlyAlphabets(e, t) {
        try {
            if (window.event) {
                var charCode = window.event.keyCode;
            }
            else if (e) {
                var charCode = e.which;
            }
            else { return true; }
            if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 32)
                return true;
            else
                return false;
        }
        catch (err) {
            alert(err.Description);
        }
    }
    </script>
                                <script language="javascript" type="text/javascript">
                                    function validationMsg() {
                                         
                                         if (document.getElementById('<%=ddlCaseType.ClientID%>').value == 'Select') {
                                             alert("Select Case Type");
                                            document.getElementById("<% =ddlCaseType.ClientID%>").focus();
                                            return false;
                                         }
                                          if (document.getElementById('<%=txtAppealCaseno.ClientID%>').value == '') {
                                              alert("Enter Appeal Case no");
                                            document.getElementById("<% =txtAppealCaseno.ClientID%>").focus();
                                            return false;
                                          }
                                         if (document.getElementById('<%=txtAppealCaseRegisterDate.ClientID%>').value == '') {
                                             alert("Select Appeal Case Register Date");
                                            document.getElementById("<% =txtAppealCaseRegisterDate.ClientID%>").focus();
                                            return false;
                                         }
                                         if (document.getElementById('<%=txtAppellantName.ClientID%>').value == '') {
                                             alert("Enter Appellant Name");
                                            document.getElementById("<% =txtAppellantName.ClientID%>").focus();
                                            return false;
                                         }
                                         if (document.getElementById('<%=txtAppellantContactNo.ClientID%>').value == '') {
                                             alert("Enter Appellant Mobile No");
                                            document.getElementById("<% =txtAppellantContactNo.ClientID%>").focus();
                                            return false;
                                         }
                                         if (document.getElementById('<%=txtDateofHearing.ClientID%>').value == '') {
                                             alert("Enter Date of Hearing");
                                            document.getElementById("<% =txtDateofHearing.ClientID%>").focus();
                                            return false;
                                        }
                                       
                                    }
                                </script>
                            </head>
                            <body>
                               <%-- <div>
                                    <ul class="nav nav-tabs">
                                        <li class="active">
                                            <asp:LinkButton runat="server" ID="btnBasicInformation">
                                        <span style="color: #fff; font-size: 14px;">Basic Information</span></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton runat="server" ID="btnEAS" OnClick="btnbtnEAS_Click">
                                        <span style="color: #fff; font-size: 14px;">Excise Articles Seized</span></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton runat="server" ID="btnOtherExisable" OnClick="btnOtherExisable_Click">
                                        <span style="color: #fff; font-size: 14px;">Other Exisable Articles</span></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton runat="server" ID="btnAccusedDetails" OnClick="btnAccusedDetails_Click">
                                        <span style="color: #fff; font-size: 14px;">Accused Details</span></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton runat="server" ID="btnRaidTeam" OnClick="btnRaidTeam_Click">
                                        <span style="color: #fff; font-size: 14px;">Raid Team  </span></asp:LinkButton></li>
                                    </ul>
                                    <br />
                                </div>--%>
                                <a>
                                    <asp:LinkButton runat="server" CssClass="myButton3" ID="ShowRecords" OnClick="ShowRecord_Click" Style="float: right"><i class="fa fa-list"> SHOW RECORD LIST</i></asp:LinkButton></a>
                                <div class="x_title">
                                    <h2>Case Registration at Excise Commissioner Office for Vehicle / Property under Prohibition Act</h2>
                                    <div class="clearfix"></div>
                                </div>

                                <div class="x_content">
                                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                                    <%--<asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                            
                                            
                                              
                                            
                                            <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                                <label class="control-label" style="display: inline"><span style="color: red">*</span>Seizure By :</label>
                                            
                                                <asp:RadioButton ID="rdbExcise" runat="server" GroupName="radio1" AutoPostBack="true" />Excise&nbsp;
                                        <asp:RadioButton ID="rdbPolice" GroupName="radio1" AutoPostBack="true" runat="server"  />Police&nbsp;
                                        
                                     
                                            </div>
                                    <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                                <label class="control-label" style="display: inline"><span style="color: red;width:40%" >*</span>Confiscation Of :</label>&nbsp;
                                                
                                                <asp:RadioButton ID="rdVehicle" runat="server" GroupName="radio" AutoPostBack="true"  />Vehicle 
                                        <asp:RadioButton ID="rdProperty" GroupName="radio" AutoPostBack="true" runat="server"  />Property 
                                        
                                     
                                            </div>
                                            <div class="clearfix"></div>
                                            <p>&nbsp;</p>
                                    <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                                <label class="control-label"><span style="color: red">*</span>Name Of District </label><br />
                                                
                                                <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control" 
                                                    data-toggle="tooltip" data-placement="right" title="Name Of District" 
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                    <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                                <label class="control-label"><span style="color: red">*</span>Thana Name</label><br />
                                                
                                                <asp:DropDownList ID="ddlThana" runat="server" CssClass="form-control"
                                        data-toggle="tooltip" data-placement="right" title="Thana Name" AutoPostBack="true" OnSelectedIndexChanged="ddlThana_SelectedIndexChanged">
                                    </asp:DropDownList>
                                            </div> 
                                    <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                                <label class="control-label"><span style="color: red">*</span>PR / FIR No</label><br />
                                                
                                                <asp:DropDownList ID="ddlPrFirNo" runat="server" CssClass="form-control"  AutoPostBack="true" data-toggle="tooltip" data-placement="right" title="PR / FIR No" OnSelectedIndexChanged="ddlPrFirNo_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                     
                                   
                                    
                                     
                                    <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                                <label class="control-label" style="width:60%"><span style="color:red"></span>Confiscation Order No</label><br />
                                                
                                                <asp:TextBox ID="txtConfiscationOrderNo" ReadOnly CssClass="form-control" runat="server" data-toggle="tooltip" data-placement="right" onkeypress="return blockSpecialChar(event)" MaxLength="100" title="Confiscation Order No"></asp:TextBox>
                                               <asp:HiddenField ID="ConfiscationOrderNo" runat="server" />
                                            </div>
                                    <div class="clearfix"></div>
                                            <p>&nbsp;</p>
                                    <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                                <label class="control-label" style="width:60%"><span style="color:red"></span>Confiscation Order Date</label><br />
                                                <cc1:CalendarExtender runat="server" PopupButtonID="Image1" TargetControlID="txtConfiscationOrderDate" Format="dd-MM-yyyy" ID="CalendarExtender3" ></cc1:CalendarExtender>
                                                <asp:TextBox ID="txtConfiscationOrderDate" ReadOnly  data-toggle="tooltip" data-placement="right" title="Confiscation Order Date " class="form-control validate[required]" onkeypress="return blockAllChar(event)"  AutoComplete="off" runat="server" Font-Size="14px">
                                                </asp:TextBox>
                                                <asp:ImageButton ID="Image1" class="control-label" Visible="false" runat="server" Height="30px" Width="30px" ImageUrl="common/theme/image1/calendar-512.png" />
                                                <asp:HiddenField ID="ConfiscationOrderDate" runat="server" />
                                            </div>
                                     
                                     <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                                <label class="control-label" style="display: inline"><span style="color: red">*</span>Case Type</label>&nbsp; &nbsp;&nbsp;<br />
                                                  <asp:DropDownList ID="ddlCaseType" AutoPostBack="true"  runat="server" CssClass="form-control" 
                                                    data-toggle="tooltip" data-placement="right" title="Case Type">
                                                       <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                                    <asp:ListItem Text="Old" Value="Old"></asp:ListItem>
                                                   </asp:DropDownList>
                                                                             
                                            </div>
                                            
                                    <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                                <label class="control-label" style="width:60%"><span style="color: red">*</span>Appeal / Case No</label><br />
                                                
                                                <asp:TextBox ID="txtAppealCaseno" AutoComplete="off" CssClass="form-control" runat="server" data-toggle="tooltip" data-placement="right" onkeypress="return blockSpecialChar(event)" MaxLength="100" title="Appeal / Case No"></asp:TextBox>
                                          <asp:HiddenField ID="AppealCaseno" runat="server" />
                                            </div>
                                     
                                    <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                                <label class="control-label" style="width:60%"><span style="color: red">*</span>Case Register Date</label><br />
                                                
                                                <cc1:CalendarExtender runat="server" PopupButtonID="Image2" TargetControlID="txtAppealCaseRegisterDate" Format="dd-MM-yyyy" ID="CalendarExtender1" OnClientDateSelectionChanged="Selectdate" ></cc1:CalendarExtender>
                                                <asp:TextBox ID="txtAppealCaseRegisterDate"  data-toggle="tooltip" data-placement="right" title="Case Register Date" class="form-control validate[required]" onkeypress="return blockAllChar(event)"  AutoComplete="off" runat="server" Font-Size="14px">
                                                </asp:TextBox>
                                                <asp:ImageButton ID="Image2" class="control-label" runat="server" Height="30px" Width="30px" ImageUrl="common/theme/image1/calendar-512.png" />
                                                <asp:HiddenField ID="AppealCaseRegisterDate" runat="server" />
                                            </div>
                                     <div class="clearfix"></div>
                                            <p>&nbsp;</p>
                                     <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                                <label class="control-label"><span style="color: red">*</span>Appellant Name</label><br />
                                                
                                         <asp:TextBox ID="txtAppellantName" AutoComplete="off" CssClass="form-control" runat="server" data-toggle="tooltip" data-placement="right" onkeypress="return blockSpecialChar(event);return onlyAlphabets(this,event);"  MaxLength="100" title="Appellant Name"></asp:TextBox>
                                         <asp:HiddenField ID="AppellantName" runat="server" />
                                            </div>

                                    <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                         <label class="control-label" style="width:60%"><span style="color: red">*</span>Appellant Mobile No</label><br />
                                         <asp:TextBox ID="txtAppellantContactNo" AutoComplete="off" onchange="phoneValidate()" CssClass="form-control" runat="server" onkeypress="return onlyDotsAndNumbers(this,event);"  runat="server" data-toggle="tooltip" data-placement="right"  MaxLength="10" title="Appellant Mobile No"></asp:TextBox>
                                          <asp:HiddenField ID="AppellantContactNo" runat="server" />
                                            </div>
                                    <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                                <label class="control-label"><span style="color: red">*</span>Date of Hearing</label><br />
                                                
                                                <cc1:CalendarExtender runat="server" PopupButtonID="Image3" TargetControlID="txtDateofHearing" Format="dd-MM-yyyy" ID="CalendarExtender2" ></cc1:CalendarExtender>
                                                <asp:TextBox ID="txtDateofHearing"  data-toggle="tooltip" data-placement="right" title="Date of Hearing" class="form-control validate[required]" onkeypress="return blockAllChar(event)"  AutoComplete="off" runat="server" Font-Size="14px">
                                                </asp:TextBox>
                                                <asp:ImageButton ID="Image3" class="control-label" runat="server" Height="30px" Width="30px" ImageUrl="common/theme/image1/calendar-512.png" />
                                                <asp:HiddenField ID="DateofHearing" runat="server" />
                                            </div>
                                    <div class="clearfix"></div>
                                            <p>&nbsp;</p>
                                     <div class="x_title">
                                    <h2>DM Case Registery Attached Files</h2>
                                    <div class="clearfix"></div>
                                </div>        
                                    <div class="col-md-11 col-sm-12 col-xs-12 form-inline" >
                                        <asp:GridView ID="grdAdd2" runat="server" AutoGenerateColumns="false"
                                            HeaderStyle-BackColor="#26b8b8" HeaderStyle-ForeColor="#ECF0F1" class="table table-striped responsive-utilities jambo_table">
                                            <Columns>
                                                  <%--<asp:TemplateField HeaderText="Status" ItemStyle-Font-Bold="true" Visible="false" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Visible="true" Text='<%#Eval("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="File Name" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFileName" runat="server" Visible="true" Text='<%#Eval("Doc_name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Document Type" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDocumentType" runat="server" Visible="true" Text='<%#Eval("Doc_Type") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiscriptione" runat="server" Visible="true" Text='<%#Eval("Discription") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FilePath" Visible="false" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFilePath" runat="server" Visible="true" Text='<%#Eval("Doc_path") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="doc_id" Visible="false" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldoc_id" runat="server" Visible="true" Text='<%#Eval("doc_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="doc_id" Visible="false" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbluser_id" runat="server" Visible="true" Text='<%#Eval("user_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action" ItemStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        &nbsp;&nbsp; 
                                                        <asp:ImageButton ID="ImageButton2" Width="30px" Height="20px" CommandArgument='<%#Eval("doc_path") %>' CommandName="Download" ImageUrl="~/img/download.png" runat="server" OnClick="DownloadFile" />
                                                        &nbsp;&nbsp;&nbsp; &nbsp;  
                                                        <asp:ImageButton ID="ImageButton1" Visible="false" CommandArgument='<%#Eval("doc_path") %>' CommandName="Remove" ImageUrl="~/img/delete.gif" runat="server" OnClick="btnRemove_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#26B8B8" ForeColor="#ECF0F1" BorderStyle="Solid" BorderWidth="2px" Height="25px" HorizontalAlign="Center"></HeaderStyle>

                                            <PagerStyle BackColor="#26B8B8" BorderWidth="2px" Height="5px" HorizontalAlign="Right" ForeColor="#ECF0F1" VerticalAlign="Middle" Font-Size="Medium" Font-Bold="True" />

                                            <RowStyle BackColor="Window" BorderStyle="Solid" BorderWidth="2px" Height="25px"></RowStyle>
                                        </asp:GridView>
                                    </div>
                                    <div class="clearfix"></div>
                                            <p>&nbsp;</p>
                                     <div class="x_title">
                                    <h2>DM Case Hearing Attached Files</h2>
                                    <div class="clearfix"></div>
                                </div>        
                                    <div class="col-md-11 col-sm-12 col-xs-12 form-inline" >
                                        <asp:GridView ID="grdAdd3" runat="server" AutoGenerateColumns="false"
                                            HeaderStyle-BackColor="#26b8b8" HeaderStyle-ForeColor="#ECF0F1" class="table table-striped responsive-utilities jambo_table">
                                            <Columns>
                                                  <asp:TemplateField HeaderText="Status" ItemStyle-Font-Bold="true" Visible="false" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Visible="true" Text='<%#Eval("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="File Name" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFileName" runat="server" Visible="true" Text='<%#Eval("Doc_name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Document Type" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDocumentType" runat="server" Visible="true" Text='<%#Eval("Doc_Type") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiscriptione" runat="server" Visible="true" Text='<%#Eval("Discription") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FilePath" Visible="false" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFilePath" runat="server" Visible="true" Text='<%#Eval("Doc_path") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="doc_id" Visible="false" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldoc_id" runat="server" Visible="true" Text='<%#Eval("doc_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="doc_id" Visible="false" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbluser_id" runat="server" Visible="true" Text='<%#Eval("user_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action" ItemStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        &nbsp;&nbsp; 
                                                        <asp:ImageButton ID="ImageButton2" Width="30px" Height="20px" CommandArgument='<%#Eval("doc_path") %>' CommandName="Download" ImageUrl="~/img/download.png" runat="server" OnClick="DownloadFile" />
                                                        &nbsp;&nbsp;&nbsp; &nbsp;  
                                                        <asp:ImageButton ID="ImageButton1" Visible="false" CommandArgument='<%#Eval("doc_path") %>' CommandName="Remove" ImageUrl="~/img/delete.gif" runat="server" OnClick="btnRemove_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#26B8B8" ForeColor="#ECF0F1" BorderStyle="Solid" BorderWidth="2px" Height="25px" HorizontalAlign="Center"></HeaderStyle>

                                            <PagerStyle BackColor="#26B8B8" BorderWidth="2px" Height="5px" HorizontalAlign="Right" ForeColor="#ECF0F1" VerticalAlign="Middle" Font-Size="Medium" Font-Bold="True" />

                                            <RowStyle BackColor="Window" BorderStyle="Solid" BorderWidth="2px" Height="25px"></RowStyle>
                                        </asp:GridView>
                                    </div>
                                     <div class="clearfix"></div>
                                            <p>&nbsp;</p>
                                            
                                             <div id="docs" runat="server">
                                                 <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                            <label class="control-label" style="display: inline" id="Label1" runat="server"><span style="color: red"></span>Document Type</label><br />
                                           
                                            <asp:DropDownList ID="ddlDocumentType" runat="server" CssClass="form-control" 
                                            AutoPostBack="true">
                                                <asp:ListItem Value="Select" Text="Select" Selected="true">                                                    
                                                </asp:ListItem>
                                                <asp:ListItem Value="FIR Document" Text="FIR Document">                                                   
                                                </asp:ListItem>
                                                <asp:ListItem Value="Seizure List" Text="Seizure List">                                                   
                                                                                                   
                                                </asp:ListItem>
                                                <asp:ListItem Value="Chemical Test Report" Text="Chemical Test Report">                                                   
                                                                                                   
                                                </asp:ListItem>
                                                <asp:ListItem Value="Vehicle / Property Owner Document" Text="Vehicle / Property Owner Document">                                                   
                                                                                                   
                                                </asp:ListItem>
                                                <asp:ListItem Value="Driving Licence" Text="Driving Licence">                                                   
                                                                                                   
                                                </asp:ListItem>
                                                <asp:ListItem Value="Vakalatnama" Text="Vakalatnama">                                                   
                                                                                                   
                                                </asp:ListItem>
                                                <asp:ListItem Value="DM Order" Text="DM Order">                                                   
                                                                                                   
                                                </asp:ListItem>
                                                <asp:ListItem Value="Excise Com Order" Text="Excise Com Order">                                                   
                                                                                                   
                                                </asp:ListItem>
                                                <asp:ListItem Value="Others" Text="Others">                                                   
                                                                                                   
                                                </asp:ListItem>
                                                
                                        </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                            <label class="control-label" style="width:60%"><span></span>Attached Documents</label><br />
                                            <asp:FileUpload ID="idupDocument" CssClass="form-control" onchange="validateExtraDocuments();" runat="server" />
                                        </div>
                                        <div class="col-md-3 col-sm-12 col-xs-12 form-inline">
                                            <label class="control-label" style="display: inline" id="dn" runat="server"><span style="color: red"></span>Document Name</label><br />
                                            <asp:TextBox ID="txtDiscription" AutoComplete="off" runat="server" CssClass="form-control" data-toggle="tooltip" data-placement="right" title="Document Name"></asp:TextBox>
                                            <span>
                                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-upload" OnClientClick="javascript:return CheckDiscription()" Text="Upload"  OnClick="UploadFile"  />
                                            </span>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <p>&nbsp;</p>                                          
                                            <div id="dummytable" runat="server" style="height: auto; width: 75%; border: 1px solid gray; margin-left: 10px; margin-right: 10px; background: #f5f6f7;">
                                        <table class="table table-striped responsive-utilities jambo_table" id="membertable">
                                            <thead>
                                                <tr>
                                                    <th>File Name</th>
                                                    <th>Description</th>
                                                    <th>Action</th>
                                                </tr>
                                            </thead>
                                            <tbody id="resourcetable">
                                            </tbody>

                                        </table>
                                    </div>
                                           
                                    <div class="col-md-11 col-sm-12 col-xs-12 form-inline">
                                        <asp:GridView ID="grdAdd" runat="server" AutoGenerateColumns="false"
                                            HeaderStyle-BackColor="#26b8b8" HeaderStyle-ForeColor="#ECF0F1" class="table table-striped responsive-utilities jambo_table">
                                            <Columns>
                                                  <asp:TemplateField HeaderText="Status" ItemStyle-Font-Bold="true" Visible="false" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Visible="true" Text='<%#Eval("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="File Name" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFileName" runat="server" Visible="true" Text='<%#Eval("Doc_name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Document Type" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDocumentType" runat="server" Visible="true" Text='<%#Eval("Doc_Type") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiscriptione" runat="server" Visible="true" Text='<%#Eval("Discription") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FilePath" Visible="false" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFilePath" runat="server" Visible="true" Text='<%#Eval("Doc_path") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="doc_id" Visible="false" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldoc_id" runat="server" Visible="true" Text='<%#Eval("doc_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="doc_id" Visible="false" ItemStyle-Font-Bold="true" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbluser_id" runat="server" Visible="true" Text='<%#Eval("user_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action" ItemStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        &nbsp;&nbsp; 
                                                        <asp:ImageButton ID="ImageButton2" Width="30px" Height="20px" CommandArgument='<%#Eval("doc_path") %>' CommandName="Download" ImageUrl="~/img/download.png" runat="server" OnClick="DownloadFile" />
                                                        &nbsp;&nbsp;&nbsp; &nbsp;  
                                                        <asp:ImageButton ID="ImageButton1" CommandArgument='<%#Eval("doc_path") %>' CommandName="Remove" ImageUrl="~/img/delete.gif" runat="server" OnClick="btnRemove_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#26B8B8" ForeColor="#ECF0F1" BorderStyle="Solid" BorderWidth="2px" Height="25px" HorizontalAlign="Center"></HeaderStyle>

                                            <PagerStyle BackColor="#26B8B8" BorderWidth="2px" Height="5px" HorizontalAlign="Right" ForeColor="#ECF0F1" VerticalAlign="Middle" Font-Size="Medium" Font-Bold="True" />

                                            <RowStyle BackColor="Window" BorderStyle="Solid" BorderWidth="2px" Height="25px"></RowStyle>
                                        </asp:GridView>
                                    </div>
                                     <div class="clearfix"></div>
                                    <p>&nbsp;</p>  
                                            
                                             
                                                <div class="col-md-6 col-sm-12 col-xs-12 ">
                                                   <label class="control-label" style="display: inline; font-size:small"><span style="color: red"></span>Remarks</label>
                                                <asp:TextBox ID="txtRemarks" AutoComplete="off" CssClass="form-control" MaxLength="250" runat="server" Width="90%" data-toggle="tooltip" data-placement="right" title="Remarks" onkeypress="return blockSpecialChar(event)" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        
                                    <div class="clearfix"></div>
                                    <p>&nbsp;</p>
                                    <div class="col-md-12 col-sm-12 col-xs-12 form-inline">
                                        <asp:LinkButton ID="btnSaveasDraft" OnClientClick="javascript:return validationMsg()" runat="server" class="btn btn-info pull-left" OnClick="btnSaveasDraft_Click"> <span aria-hidden="true" ></span>Save</asp:LinkButton>
                                        <asp:LinkButton ID="btnSubmit" runat="server" OnClientClick="javascript:return validationMsg();" CssClass="btn btn-primary" OnClick="btnSubmit_Click" > <span aria-hidden="true"  ></span>Submit</asp:LinkButton>
                                        <%--<asp:LinkButton ID="btnApprove" AutoPostback="false" CssClass="btn btn-primary" OnClientClick="javascript:return validationMsg1()" runat="server" Style="width: 85px;" Text="Approve" />
                                            <asp:LinkButton ID="btnReject" Text="Reject" runat="server" CssClass="btn btn-danger right" OnClientClick="javascript:return validationMsg1()" class="fa fa-cut" />--%>
                                        <asp:LinkButton ID="btnCancel" runat="server" Visible="true"
                                            CssClass="btn btn-danger" OnClick="btnCancel_Click">Cancel <span aria-hidden="true" ></span></asp:LinkButton>
                                    </div>
                                </div>
                            </body>
                            </html>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>