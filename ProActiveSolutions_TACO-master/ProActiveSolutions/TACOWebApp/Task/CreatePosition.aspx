<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreatePosition.aspx.cs" Inherits="TACOWebApp.Task.CreatePosition" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <div id='all_content' runat="server">
        <div class="row col-md-12 info">
            <h2>Position</h2>
            <p class="annotation"><span class="required">*</span> required fields.</p>

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
                        <asp:Label ID="LabelPositionLookup" runat="server" AssociatedControlID="DropdownPositionLookup">Find:</asp:Label>
                        <asp:DropDownList ID="DropdownPositionLookup" runat="server" AppendDataBoundItems="true" DataSourceID="ObjectDataSourcePosition" DataTextField="PositionName" DataValueField="PositionId">
                            <asp:ListItem Value="0">Choose a Position</asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropdownPositionLookup" PromptText="Type to search a position"></ajaxToolkit:ListSearchExtender>

                        <div class="button-checkbox-container" style="margin-left: 44%;">
                            <div class="checkbox-container">
                                <asp:Label ID="LabelCheckbox" runat="server" AssociatedControlID="CheckboxPositionExpired">Show expired items</asp:Label>
                                <asp:CheckBox ID="CheckboxPositionExpired" runat="server" AutoPostBack="true" OnCheckedChanged="CheckboxPositionExpired_CheckedChanged" />
                            </div>

                            <asp:Button ID="ButtonPositionLookup" runat="server" Text="Search" OnClick="ButtonPositionLookup_Click" CssClass="btn btnLookup" Style="margin-bottom: 1rem;" />
                        </div>


                        <asp:TextBox ID="TextBoxPositionId" runat="server" Visible="false"></asp:TextBox>

                        <asp:Label ID="LabelPositionName" runat="server" AssociatedControlID="TextBoxPositionName">Position Name<span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxPositionName" runat="server" Placeholder="OT"></asp:TextBox>


                        <asp:Label ID="LabelPostionDeactivate" runat="server" AssociatedControlID="CheckboxPositionDeactivate">Deactivated</asp:Label>
                        <asp:CheckBox ID="CheckboxPositionDeactivate" runat="server" Style="margin-left: 1rem;" />

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
                <asp:Button ID="ButtonCreatePosition" runat="server" Text="Create" OnClick="ButtonCreatePosition_Click" Visible="true" CssClass="btn btnCreate" />
                <asp:Button ID="ButtonUpdatePosition" runat="server" Text="Update" OnClick="ButtonUpdatePosition_Click" Visible="false" CssClass="btn btnCreate" />
                <asp:Button ID="ButtonDeletePosition" runat="server" Text="Deactivate" OnClick="ButtonDeletePosition_Click" Visible="false" CssClass="btn btnTerminate" />
                <asp:Button ID="ButtonClearPosition" runat="server" Text="Cancel" OnClick="ButtonClearPosition_Click" CssClass="btn btnCancel" />
            </div>
        </div>
        <script src="/Scripts/bootwrap-freecode.js"></script>

          <asp:ObjectDataSource ID="ObjectDataSourcePosition" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="PositionDetails" TypeName="TACOSystem.BLL.PositionController"></asp:ObjectDataSource>
    </div>
</asp:Content>
