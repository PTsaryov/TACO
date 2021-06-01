<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateOvertime.aspx.cs" Inherits="TACOWebApp.Task.CreateOvertime" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id='all_content' runat="server">
        <div class="row col-md-12 info">
            <h2>Overtime Codes</h2>
            <p class="annotation"><span class="required">*</span> are required fields.</p>

        </div>
        <div class="row">
            <div class="col-md-12" style="margin-top: 20px;">
                <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
            </div>
        </div>
        <div class="container-two">
            <div class="row">
                <div class="col-md-6">
                    <fieldset>
                        <br />
                        <asp:Label ID="LabelOvertimeLookup" runat="server" AssociatedControlID="DropdownOvertimeLookup">Find Overtime Code</asp:Label>
                        <asp:DropDownList ID="DropdownOvertimeLookup" runat="server" AppendDataBoundItems="true" DataSourceID="OvertimeListODS" DataTextField="OvertimeCode" DataValueField="OvertimeId">
                            <asp:ListItem Value="0">Choose an Overtime Code</asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropdownOvertimeLookup" PromptText="Type to search an overtime code"></ajaxToolkit:ListSearchExtender>

                        <div class="button-checkbox-container" style="margin-left: 44%;">
                            <div class="checkbox-container">
                                <asp:Label ID="LabelCheckbox" runat="server" AssociatedControlID="CheckboxOvertimeExpired">Show expired items</asp:Label>
                                <asp:CheckBox ID="CheckboxOvertimeExpired" runat="server" AutoPostBack="true" OnCheckedChanged="CheckboxOvertimeExpired_CheckedChanged" />
                            </div>

                            <asp:Button ID="ButtonOvertimeLookup" runat="server" Text="Search" OnClick="ButtonOvertimeLookup_Click" CssClass="btn btnLookup" Style="margin-bottom: 1rem;" />
                        </div>


                        <asp:TextBox ID="TextBoxOvertimeId" runat="server" Visible="false"></asp:TextBox>

                        <asp:Label ID="LabelOvertimeCode" runat="server" AssociatedControlID="TextBoxOvertimeCode">Overtime Code <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxOvertimeCode" runat="server" Placeholder="OT"></asp:TextBox>



                        <asp:Label ID="LabelUnits" runat="server" AssociatedControlID="DropdownlistUnits">Units<span class="required">*</span></asp:Label>
                        <asp:DropDownList ID="DropdownlistUnits" runat="server">
                            <asp:ListItem Value="hours">Hours</asp:ListItem>
                            <asp:ListItem Value="days">Days</asp:ListItem>
                        </asp:DropDownList>

                        <asp:Label ID="LabelColorPicker" runat="server" AssociatedControlID="PickerColor">Color</asp:Label>
                        <asp:TextBox ID="PickerColor" runat="server" Placeholder="FF0000"></asp:TextBox>
                        <ajaxToolkit:ColorPickerExtender ID="ColorPickerExtender1" runat="server" PopupButtonID="ColorPickerbutton" TargetControlID="PickerColor" />

                        <asp:Label ID="LabelOvertimeDeactivate" runat="server" AssociatedControlID="CheckboxOvertimeCodeDeactivate">Deactivated</asp:Label>
                        <asp:CheckBox ID="CheckboxOvertimeCodeDeactivate" runat="server" Style="margin-left: 1rem;" />

                    </fieldset>
                </div>
                <div class="col-md-6">

                    <fieldset>
                        <asp:Label ID="LabelDescription" runat="server" AssociatedControlID="TextBoxDescription">Description <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxDescription" runat="server" Placeholder="Description" Height="150px" TextMode="MultiLine" Wrap="true"></asp:TextBox>
                    </fieldset>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 text-right">
                <asp:Button ID="ButtonCreateOvertimeCode" runat="server" Text="Create" OnClick="ButtonCreateOvertimeCode_Click" Visible="true" CssClass="btn btnCreate" />
                <asp:Button ID="ButtonUpdateOvertimeCode" runat="server" Text="Update" OnClick="ButtonUpdateOvertimeCode_Click" Visible="false" CssClass="btn btnCreate" />
                <asp:Button ID="ButtonDeleteOvertimeCode" runat="server" Text="Deactivate" OnClick="ButtonDeleteOvertimeCode_Click" Visible="false" CssClass="btn btnTerminate" />
                <asp:Button ID="ButtonClearOvertimeCode" runat="server" Text="Cancel" OnClick="ButtonClearOvertimeCode_Click" CssClass="btn btnCancel" />
            </div>
        </div>
        <script src="/Scripts/bootwrap-freecode.js"></script>

        <asp:ObjectDataSource ID="OvertimeListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="OvertimeList" TypeName="TACOSystem.BLL.OvertimeController"></asp:ObjectDataSource>
    </div>
</asp:Content>
