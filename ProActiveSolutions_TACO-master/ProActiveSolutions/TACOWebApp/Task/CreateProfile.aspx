<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateProfile.aspx.cs" Inherits="TACOWebApp.Task.CreateProfile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />
    <div id="all_content" runat="server">
        <div class="row  info">
            <div class="col-md-12">
                  <h2>Create/Edit Profile</h2>
            <p class="annotation"><span class="required">*</span> are required fields.</p>
            </div>
          
        </div>

        <div class="row">
            <div class="col-md-12" style=" margin-top:20px;">
                <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
            </div>
        </div>
        <div class="container-two">
            <div class="row">
                <div class="col-md-6">
                    <fieldset>
                        <br />
                        <asp:Label ID="LabelLookup" runat="server" AssociatedControlID="TextBoxLookup">Find Employee</asp:Label>
                        <asp:TextBox ID="TextBoxLookup" placeholder="Jane Doe" runat="server" runat="server"></asp:TextBox>
                        <asp:Button ID="ButtonLookup" runat="server" Text="Search" OnClick="ButtonLookup_Click" CssClass="btn btnLookup" Style="margin-left: 62%; margin-bottom: 1rem;" />
                        <br />

                        <asp:TextBox ID="TextBoxEmployeeId" runat="server" placeholder="AAA123456" Visible="false"></asp:TextBox>

                        <asp:Label ID="LabelEmployeeNumber" runat="server" AssociatedControlID="TextBoxEmployeeNumber">Employee Number <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxEmployeeNumber" runat="server" ToolTip="Employee Number Format: AAA00000" Placeholder="AAA000000"></asp:TextBox>

                        <asp:Label ID="LabelFirstName" runat="server" AssociatedControlID="TextBoxFirstName">First Name <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxFirstName" runat="server" Placeholder="Jane"></asp:TextBox>

                        <asp:Label ID="LabelLastName" runat="server" AssociatedControlID="TextBoxlastName"> Last Name <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxLastName" runat="server" Placeholder="Doe"></asp:TextBox>

                        <asp:ObjectDataSource ID="ObjectDataSourcePosition" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="PositionDetails" TypeName="TACOSystem.BLL.PositionController"></asp:ObjectDataSource>
                        <asp:Label ID="LabelPosition" runat="server" AssociatedControlID="DropdownPosition">Position <span class="required">*</span></asp:Label>
                        <asp:DropDownList ID="DropdownPosition" runat="server" DataSourceID="ObjectDataSourcePosition" DataTextField="PositionName" DataValueField="PositionId" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">Select a position</asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DropdownPosition" PromptText="Type to search a position"></ajaxToolkit:ListSearchExtender>

                        <asp:Label ID="LabelBirthDate" runat="server" AssociatedControlID="TextBoxBirthDate">Birth Date <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxBirthDate" runat="server" placeholder="Choose a Date or MM-DD-YYYY"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                            PopupButtonID="ButtonStartDate" PopupPosition="TopRight"
                            TargetControlID="TextBoxBirthDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>

                        <asp:Label ID="LabelHireDate" runat="server" AssociatedControlID="TextBoxHireDate">Hire Date <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxHireDate" runat="server" placeholder="Choose a Date or MM-DD-YYYY"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                            PopupButtonID="ButtonStartDate" PopupPosition="TopRight"
                            TargetControlID="TextBoxHireDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>

                        <asp:Label ID="LabelTeamMember" runat="server" AssociatedControlID="DropdownTeamMember">Team <span class="required">*</span></asp:Label>
                        <asp:DropDownList ID="DropdownTeamMember" runat="server" DataTextField="Value" DataValueField="Key" CssClass="form-control"></asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropdownTeamMember" PromptText="Type to search a team"></ajaxToolkit:ListSearchExtender>

                    </fieldset>
                </div>

                <div class="col-md-6">
                    <br />
                    <fieldset>

                        <asp:ObjectDataSource ID="ObjectDataSourceSecurityRole" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="EmployeeSecurityRolesList" TypeName="TACOSystem.BLL.Employee.EmployeeController"></asp:ObjectDataSource>
                        <asp:Label ID="LabelSecurityRole" runat="server" AssociatedControlID="DropdownSecurityRole">Security Role <span class="required">*</span></asp:Label>
                        <asp:DropDownList ID="DropdownSecurityRole" runat="server" AppendDataBoundItems="true" DataSourceID="ObjectDataSourceSecurityRole" DataTextField="RoleDescription" DataValueField="SecurityRoleId">
                            <asp:ListItem Value="0">Select a security role</asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DropdownSecurityRole" PromptText="Type to search a role"></ajaxToolkit:ListSearchExtender>

                        <asp:ObjectDataSource ID="ObjectDataSourceSchedule" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ScheduleList" TypeName="TACOSystem.BLL.ScheduleTypeController"></asp:ObjectDataSource>
                        <asp:Label ID="LabelScheduleType" runat="server" AssociatedControlID="DropdownScheduleType">Schedule Type <span class="required">*</span></asp:Label>
                        <asp:DropDownList ID="DropdownScheduleType" runat="server" DataSourceID="ObjectDataSourceSchedule" DataTextField="ScheduleName" DataValueField="ScheduleId" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">Select a schedule</asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DropdownScheduleType" PromptText="Type to search a schedule type"></ajaxToolkit:ListSearchExtender>

                        <asp:Label ID="LabelPhone" runat="server" AssociatedControlID="TextBoxPhone">Phone Number <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxPhone" runat="server" TextMode="Phone" Placeholder="(123)456-7890"></asp:TextBox>

                        <asp:Label ID="LabelEmail" runat="server" AssociatedControlID="TextBoxEmail">Email <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxEmail" runat="server" TextMode="Email" Placeholder="janedoe@wcb.ca"></asp:TextBox>

                        <asp:Label ID="LabelComputerName" runat="server" AssociatedControlID="TextBoxComputerName">Computer Name <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxComputerName" Placeholder="AA000" runat="server"></asp:TextBox>

                        <asp:Label ID="LabelStation" runat="server" AssociatedControlID="TextBoxStation">Station <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxStation" Placeholder="B13" runat="server"></asp:TextBox>

                        <asp:Label ID="LabelEmergencyContact" runat="server" Text="Emergency Contact" AssociatedControlID="TextBoxEmergencyContact"></asp:Label>
                        <asp:TextBox ID="TextBoxEmergencyContact" Placeholder="John Doe" runat="server"></asp:TextBox>

                        <asp:Label ID="LabelEmergencyContactPhone" runat="server" Text="Emergency Contact Number" AssociatedControlID="TextBoxEmergencyContactNumber"></asp:Label>
                        <asp:TextBox ID="TextBoxEmergencyContactNumber" Placeholder="(123) 456-7890" runat="server"></asp:TextBox>
                    </fieldset>
                </div>

            </div>
            <div class="row">
                <div class="col-md-6">
                </div>
                <div class="col-md-6 text-right">

                    <asp:Button ID="ButtonCreateProfile" runat="server" Text="Create" OnClick="ButtonCreateProfile_Click" Visible="true" CssClass="btn btnCreate" />
                    <asp:Button ID="ButtonUpdate" runat="server" Text="Update" OnClick="ButtonUpdate_Click" Visible="false" CssClass="btn btnCreate" />
                    <asp:Button ID="ButtonTerminate" runat="server" Text="Terminate" OnClick="ButtonTerminate_Click" Visible="false" CssClass="btn btnTerminate" />
                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" OnClick="ButtonCancel_Click" CssClass="btn btnCancel" />
                </div>

            </div>
        </div>

    </div>

    <script src="/Scripts/bootwrap-freecode.js"></script>

</asp:Content>
