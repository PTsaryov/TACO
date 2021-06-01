<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateHoliday.aspx.cs" Inherits="TACOWebApp.Task.CreateHoliday" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />
    <div id='all_content' runat="server">
        <div class="row col-md-12 info">
            <h2>Create Holiday</h2>
            <p class="annotation"><span class="required">*</span> are required fields.</p>
          
        </div>
        <div class="row">
            <div class="col-12" style="margin-top:20px;">
                  <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
            </div>
        </div>
        <div class="container-two">
            <div class="row">
                <div class="col-md-6">
                    <fieldset>
                        <br />
                        <asp:ObjectDataSource ID="ODSHoliday" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="HolidayList" TypeName="TACOSystem.BLL.HolidayController"></asp:ObjectDataSource>
                        <asp:Label ID="LabelHolidayLookup" runat="server" Text="Find" AssociatedControlID="DropDownHoliday"></asp:Label>
                        <asp:DropDownList ID="DropDownHoliday" runat="server" AppendDataBoundItems="true" DataSourceID="ODSHoliday" DataTextField="HolidayName" DataValueField="HolidayId">
                            <asp:ListItem Value="0">Select a Holiday</asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropDownHoliday" PromptText="Type to search a holiday"></ajaxToolkit:ListSearchExtender>

                         <div class="button-checkbox-container" style="margin-left:44%;">
                            <div class="checkbox-container">
                                <asp:Label ID="LabelCheckbox" runat="server" AssociatedControlID="CheckboxExpired">Show expired items</asp:Label>
                               <asp:CheckBox ID="CheckboxExpired" runat="server" AutoPostBack="true"  OnCheckedChanged="CheckboxExpired_CheckedChanged" />
                            </div>
                        
                         <asp:Button ID="ButtonHolidayLookup" runat="server" Text="Search" OnClick="ButtonHolidayLookup_Click" CssClass="btn btnLookup" Style=" margin-bottom: 1rem;" />
                        </div>

                        <asp:TextBox ID="TextBoxHolidayId" runat="server" Visible="false"></asp:TextBox>

                        <asp:Label ID="LabelHolidayName" runat="server" Text="Holiday Name" AssociatedControlID="TextBoxHolidayName"></asp:Label>
                        <asp:TextBox ID="TextBoxHolidayName" runat="server"></asp:TextBox>

                        <asp:Label ID="LabelDate" runat="server" Text="Date" AssociatedControlID="TextBoxDate"></asp:Label>
                        <asp:TextBox ID="TextBoxDate" runat="server" Format="dddd, MMMM dd, yyyy" placeholder="Choose a Date or MM-DD-YYYY"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                            PopupButtonID="ButtonDate" PopupPosition="TopRight"
                            TargetControlID="TextBoxDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>

                        <asp:Label ID="LabelHolidayDeactivated" runat="server" Text="Deactivated" AssociatedControlID="CheckBoxHolidayDeactivated"></asp:Label>
                        <asp:CheckBox ID="CheckBoxHolidayDeactivated" runat="server" Style="margin-left: 1rem;"  />

                        <%--used to capture data to use in at the bottom of the page--%>
                        <asp:TextBox ID="TextBoxCreatedBy" runat="server" ReadOnly="true" BackColor="LightGray" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="TextBoxCreatedOn" runat="server" ReadOnly="true" BackColor="LightGray" Format="dddd, MMMM dd, yyyy" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="TextBoxModifiedBy" runat="server" ReadOnly="true" BackColor="LightGray" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="TextBoxModifiedOn" runat="server" ReadOnly="true" BackColor="LightGray" Format="dddd, MMMM dd, yyyy" Visible="false"></asp:TextBox>
                    </fieldset>
                </div>

                <div class="col-md-6">
                    <br />
                    <fieldset>
                       
                    </fieldset>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 text-right">
                <asp:Button ID="ButtonHolidayCreate" runat="server" Text="Create" Visible="true" CssClass="btn btnCreate" OnClick="ButtonHolidayCreate_Click" />
                <asp:Button ID="ButtonHolidayUpdate" runat="server" Text="Update" Visible="false" CssClass="btn btnCreate" OnClick="ButtonHolidayUpdate_Click" />
                <asp:Button ID="ButtonHolidayDeactivate" runat="server" Text="Deactivate" CssClass="btn btnTerminate" Visible="false" OnClick="ButtonRemove_Click" />                
                <asp:Button ID="ButtonActivateHoliday" runat="server" Text="Activate" CssClass="btn btnCreate" Visible="false" OnClick="ButtonActivateHoliday_Click" />
                <asp:Button ID="ButtonHolidayCancel" runat="server" Text="Cancel" CssClass="btn btnCancel" OnClick="ButtonHolidayCancel_Click" />
            </div>
        </div>
        <script src="/Scripts/bootwrap-freecode.js"></script>
    </div>
</asp:Content>
