<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateScheduleType.aspx.cs" Inherits="TACOWebApp.Task.CreateScheduleType" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id='all_content' runat="server">
        <div class="row col-md-12 info">
            <h2>Schedule Type</h2>
            <p class="annotation"><span class="required">*</span> are required fields.</p>

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
                        <asp:Label ID="LabelLookup" runat="server" AssociatedControlID="DropDownScheduleLookup">Find Schedule</asp:Label>
                        <asp:DropDownList ID="DropDownScheduleLookup" runat="server" DataSourceID="ScheduleListODS" DataTextField="ScheduleName" DataValueField="ScheduleId" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">Choose a Schedule</asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropDownScheduleLookup" PromptText="Type to search a schedule"></ajaxToolkit:ListSearchExtender>

                          <div class="button-checkbox-container" style="margin-left: 44%;">
                            <div class="checkbox-container">
                                <asp:Label ID="LabelCheckbox" runat="server" AssociatedControlID="CheckboxScheduleExpired">Show expired items</asp:Label>
                                  <asp:CheckBox ID="CheckboxScheduleExpired" runat="server"  AutoPostBack="true" OnCheckedChanged="CheckboxScheduleExpired_CheckedChanged" />
                            </div>

                            <asp:Button ID="ButtonLookup" runat="server" Text="Search" OnClick="ButtonLookup_Click" CssClass="btn btnLookup" Style=" margin-bottom: 1rem;" />
                        </div>
                    
                      
                        <asp:TextBox ID="TextBoxScheduleId" runat="server" Visible="false"></asp:TextBox>

                        <asp:Label ID="LabelScheduleName" runat="server" AssociatedControlID="TextBoxScheduleName">Schedule Name <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxScheduleName" runat="server" Placeholder="Schedule Name"></asp:TextBox>

                       
                        <asp:Label ID="LabelScheduleTime" runat="server" AssociatedControlID="TextBoxScheduleTime">Schedule Time (in minutes) <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxScheduleTime" runat="server" Placeholder="480" Textmode="Number" min="0" max="1440" Width="320px"></asp:TextBox>

                        <asp:Label ID="LabelScheduleDeactivate" runat="server" AssociatedControlID="CheckboxScheduleDeactivate">Deactivated</asp:Label>
                        <asp:CheckBox ID="CheckboxScheduleDeactivate" runat="server" style="margin-left:1rem;"/>

                    </fieldset>
                </div>
                <div class="col-md-6">
                    <br />
                    <fieldset>
                        <asp:Label ID="LabelDescription" runat="server" AssociatedControlID="TextBoxDescription">Description <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxDescription" runat="server" TextMode="MultiLine" Height="150px" Wrap="true" Placeholder="Description"></asp:TextBox>

                    </fieldset>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 text-right">
                <asp:Button ID="ButtonCreateScheduleType" runat="server" Text="Create" OnClick="ButtonCreateScheduleType_Click" Visible="true" CssClass="btn btnCreate" />
                <asp:Button ID="ButtonUpdate" runat="server" Text="Update" OnClick="ButtonUpdate_Click" Visible="false" CssClass="btn btnCreate" />
                <asp:Button ID="ButtonDeactivate" runat="server" Text="Deactivate" OnClick="ButtonDeactivate_Click" Visible="false" CssClass="btn btnTerminate" />
                <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" OnClick="ButtonCancel_Click" CssClass="btn btnCancel" />
            </div>
        </div>
        <script src="/Scripts/bootwrap-freecode.js"></script>

        <asp:ObjectDataSource ID="ScheduleListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ScheduleList" TypeName="TACOSystem.BLL.ScheduleTypeController"></asp:ObjectDataSource>
    </div>
</asp:Content>
