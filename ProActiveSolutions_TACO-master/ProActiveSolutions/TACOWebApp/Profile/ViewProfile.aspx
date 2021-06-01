<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewProfile.aspx.cs" Inherits="TACOWebApp.Profile.createProfile" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />

    <div id="all_content" runat="server">
        <div class="row col-md-12 info">
        <h2>Your Profile</h2>
       <p class="annotation">To update information, contact Team Lead or Admin.</p>
    </div>

     <div class="row user-control">
         <div class="col-md-12" id="MessageUserControl" style="margin-top:20px;">
             <uc1:MessageUserControl runat="server" ID="MessageUserControl1" />
         </div>
    </div>
<div class="container-two">
      <div class="row">
        <div class="col-md-6">
            <fieldset>
                 <br />
                <br />
                 <asp:TextBox ID="TextBoxEmployeeId" runat="server" placeholder="AAA123456" Visible="false" ></asp:TextBox>
                
                <asp:Label ID="LabelEmployeeNumber" runat="server" AssociatedControlID="TextBoxEmployeeNumber" >Employee Number <span class="required">*</span></asp:Label>
                <asp:TextBox ID="TextBoxEmployeeNumber" runat="server" ToolTip="Employee Number Format: AAA00000" Placeholder ="AAA000000" ReadOnly="true"></asp:TextBox>
                
                <asp:Label ID="LabelFirstName" runat="server"  AssociatedControlID="TextBoxFirstName">First Name <span class="required">*</span></asp:Label>
                <asp:TextBox ID="TextBoxFirstName" runat="server" Placeholder ="Jane" ReadOnly="true"></asp:TextBox>
                
                <asp:Label ID="LabelLastName" runat="server" AssociatedControlID="TextBoxlastName"> Last Name <span class="required">*</span></asp:Label>
                <asp:TextBox ID="TextBoxLastName" runat="server" Placeholder ="Doe" ReadOnly="true"></asp:TextBox>
                 
                <asp:ObjectDataSource ID="ObjectDataSourcePosition" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="PositionDetails" TypeName="TACOSystem.BLL.PositionController"></asp:ObjectDataSource>
                <asp:Label ID="LabelPosition" runat="server"  AssociatedControlID="DropdownPosition">Position <span class="required">*</span></asp:Label>
                <asp:DropDownList ID="DropdownPosition" runat="server"  DataSourceID="ObjectDataSourcePosition" DataTextField="PositionName" DataValueField="PositionId" AppendDataBoundItems="true" Enabled="False">
                     <asp:ListItem Value="0">Select a position</asp:ListItem>
                </asp:DropDownList>
                
                <asp:Label ID="LabelBirthDate" runat="server"  AssociatedControlID="TextBoxBirthDate">Birth Date <span class="required">*</span></asp:Label>
                <asp:TextBox ID="TextBoxBirthDate" runat="server" ReadOnly="true"></asp:TextBox>
                  <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                    PopupButtonID="ButtonStartDate" PopupPosition="TopRight"
                    TargetControlID="TextBoxBirthDate" Format="dddd, MMMM dd, yyyy" ></ajaxToolkit:CalendarExtender>

                <asp:Label ID="LabelHireDate" runat="server"  AssociatedControlID="TextBoxHireDate">Hire Date <span class="required">*</span></asp:Label>
                <asp:TextBox ID="TextBoxHireDate" runat="server" ReadOnly="true"></asp:TextBox>
                 <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                    PopupButtonID="ButtonStartDate" PopupPosition="TopRight"
                    TargetControlID="TextBoxHireDate"  Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>

            </fieldset>
        </div>

        <div class="col-md-6">
            <br />
            <fieldset>
                 <asp:ObjectDataSource ID="ObjectDataSourceSecurityRole" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="EmployeeSecurityRolesList" TypeName="TACOSystem.BLL.Employee.EmployeeController"></asp:ObjectDataSource>
                <asp:Label ID="LabelSecurityRole" runat="server" AssociatedControlID="DropdownSecurityRole">Security Role <span class="required">*</span></asp:Label>
                <asp:DropDownList ID="DropdownSecurityRole" runat="server" AppendDataBoundItems="true" DataSourceID="ObjectDataSourceSecurityRole" DataTextField="RoleDescription" DataValueField="SecurityRoleId" Enabled="False">
                    <asp:ListItem Value="0">Select a security role</asp:ListItem>
                </asp:DropDownList>

                <asp:ObjectDataSource ID="ObjectDataSourceSchedule" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ScheduleList" TypeName="TACOSystem.BLL.ScheduleTypeController"></asp:ObjectDataSource>
                <asp:Label ID="LabelScheduleType" runat="server" AssociatedControlID="DropdownScheduleType">Schedule Type <span class="required">*</span></asp:Label>
                <asp:DropDownList ID="DropdownScheduleType" runat="server"  DataSourceID="ObjectDataSourceSchedule" DataTextField="ScheduleName" DataValueField="ScheduleId" AppendDataBoundItems="true" Enabled="False">
                     <asp:ListItem Value="0">Select a schedule</asp:ListItem>
                </asp:DropDownList>

                <asp:Label ID="LabelPhone" runat="server"  AssociatedControlID="TextBoxPhone">Phone Number <span class="required">*</span></asp:Label>
                <asp:TextBox ID="TextBoxPhone" runat="server" TextMode="Phone" Placeholder ="(123) 456-7890" ReadOnly="true"></asp:TextBox>

                <asp:Label ID="LabelEmail" runat="server" AssociatedControlID="TextBoxEmail">Email <span class="required">*</span></asp:Label>
                <asp:TextBox ID="TextBoxEmail" runat="server" TextMode="Email" Placeholder ="janedoe@wcb.ca" ReadOnly="true"></asp:TextBox>

                 <asp:Label ID="LabelComputerName" runat="server" AssociatedControlID="TextBoxComputerName">Computer Name <span class="required">*</span></asp:Label>
                <asp:TextBox ID="TextBoxComputerName"  Placeholder ="AA000" runat="server" Enabled="False"></asp:TextBox>

                 <asp:Label ID="LabelStation" runat="server" AssociatedControlID="TextBoxStation">Station <span class="required">*</span></asp:Label>
                <asp:TextBox ID="TextBoxStation"  Placeholder ="B13" runat="server" ReadOnly="true"></asp:TextBox>

                <asp:Label ID="LabelEmergencyContact" runat="server" Text="Emergency Contact" AssociatedControlID="TextBoxEmergencyContact"></asp:Label>
                <asp:TextBox ID="TextBoxEmergencyContact"  Placeholder ="John Doe" runat="server" ReadOnly="true"></asp:TextBox>

                <asp:Label ID="LabelEmergencyContactPhone" runat="server" Text="Emergency Contact Number" AssociatedControlID="TextBoxEmergencyContactNumber"></asp:Label>
                <asp:TextBox ID="TextBoxEmergencyContactNumber" Placeholder ="(123) 456-7890" runat="server" ReadOnly="true"></asp:TextBox>
            </fieldset>
        </div>

    </div>
    </div>
</div>
  
     
    <script src="/Scripts/bootwrap-freecode.js"></script>

</asp:Content>
