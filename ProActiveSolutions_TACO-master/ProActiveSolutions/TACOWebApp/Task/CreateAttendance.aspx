<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateAttendance.aspx.cs" Inherits="TACOWebApp.Task.CreateAttendance" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id='all_content' runat="server">

        <div class="row col-md-12 info">
            <h2>Attendance Codes</h2>
            <p class="annotation"><span class="required">*</span>are required fields.</p>

        </div>
        <div class="row">
            <div class="col-md-12" style="margin-top:20px;">
                <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
            </div>
        </div>
        <div class="container-two">
            <div class="row">

                <div class="col-md-6">

                    <fieldset>
                        <br />
                        <asp:Label ID="LabelAttendanceLookup" runat="server" AssociatedControlID="DropdownAttendanceLookup">Find Attendance Code</asp:Label>
                        <asp:DropDownList ID="DropdownAttendanceLookup" runat="server" DataSourceID="AttendanceListODS" DataTextField="AttendanceCode" DataValueField="AttendanceId" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">Choose an Attendance Code</asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropdownAttendanceLookup" PromptText="Type to search an attendance code"></ajaxToolkit:ListSearchExtender>
                        <div class="button-checkbox-container" style="margin-left:40%;">
                            <div class="checkbox-container">
                                <asp:Label ID="LabelCheckbox" runat="server" AssociatedControlID="CheckboxAttendanceExpired">Show deactivated items</asp:Label>
                                <asp:CheckBox ID="CheckboxAttendanceExpired" runat="server" AutoPostBack="true" OnCheckedChanged="CheckboxAttendanceExpired_CheckedChanged" />
                            </div>

                            <asp:Button ID="ButtonAttendanceLookup" runat="server" Text="Search" OnClick="ButtonAttendanceLookup_Click" CssClass="btn btnLookup" Style="margin-bottom: 1rem;" />
                        </div>

                        <asp:TextBox ID="TextBoxAttendanceId" runat="server" Visible="false"></asp:TextBox>

                        <asp:Label ID="LabelAttendanceCode" runat="server" AssociatedControlID="TextBoxAttendanceCode">Attendance Code <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxAttendanceCode" runat="server" Placeholder="VAC"></asp:TextBox>

                        
                        <asp:Label ID="LabelUnits" runat="server" AssociatedControlID="DropdownlistUnits">Units<span class="required">*</span></asp:Label>
                        <asp:DropDownList ID="DropdownlistUnits" runat="server">
                            <asp:ListItem Value="hours">Hours</asp:ListItem>
                            <asp:ListItem Value="days">Days</asp:ListItem>
                        </asp:DropDownList>

                     

                    </fieldset>
                </div>
                <div class="col-md-6">
                    <fieldset>
                        <asp:Label ID="LabelDescription" runat="server" AssociatedControlID="TextBoxDescription">Description <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxDescription" runat="server" Placeholder="Description" TextMode="MultiLine" Height="150px" Wrap="true" ></asp:TextBox>

                           <asp:Label ID="LabelScheduleDeactivate" runat="server" AssociatedControlID="CheckboxAttendanceCodeDeactivate">Deactivated</asp:Label>
                        <asp:CheckBox ID="CheckboxAttendanceCodeDeactivate" runat="server" Style="margin-left: 1rem;" />

                    </fieldset>

                </div>

            </div>
            <div class="row">
                <div class="col-md-12 text-right" style="margin-top:5%;">
                    <asp:Button ID="ButtonCreateAttendanceCode" runat="server" Text="Create" OnClick="ButtonCreateAttendanceCode_Click" Visible="true" CssClass="btn btnCreate" />
                    <asp:Button ID="ButtonUpdateAttendanceCode" runat="server" Text="Update" OnClick="ButtonUpdateAttendanceCode_Click" Visible="false" CssClass="btn btnCreate" />
                    <asp:Button ID="ButtonDeleteAttendanceCode" runat="server" Text="Deactivate" OnClick="ButtonDeleteAttendanceCode_Click" Visible="false" CssClass="btn btnTerminate" />
                    <asp:Button ID="ButtonClearAttendanceCode" runat="server" Text="Cancel" OnClick="ButtonClearAttendanceCode_Click" CssClass="btn btnCancel" />
                </div>
            </div>
        </div>

        <script src="/Scripts/bootwrap-freecode.js"></script>

        <asp:ObjectDataSource ID="AttendanceListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="AttendanceList" TypeName="TACOSystem.BLL.Attendance.AttendanceController"></asp:ObjectDataSource>
    </div>
</asp:Content>
