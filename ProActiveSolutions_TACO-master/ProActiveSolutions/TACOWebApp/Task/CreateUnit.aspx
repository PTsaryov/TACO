<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateUnit.aspx.cs" Inherits="TACOWebApp.Task.CreateUnit" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id='all_content' runat="server">
        <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />
        <div class="row col-md-12 info">
            <h1>Unit</h1>
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
                    <asp:TextBox ID="TextBoxUnitId" runat="server" Visible="false"></asp:TextBox>

                    <asp:Label ID="UnitLookup" runat="server" AssociatedControlID="DropDownUnit">Find Unit</asp:Label>
                    <asp:DropDownList ID="DropDownUnit" runat="server" DataSourceID="UnitListODS" DataTextField="UnitName"
                        DataValueField="UnitId" AppendDataBoundItems="true">
                        <asp:ListItem Value="0">Choose a Unit</asp:ListItem>
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DropDownUnit" PromptText="Type to search a unit"></ajaxToolkit:ListSearchExtender>
                   
                    <br />

                      <div class="button-checkbox-container" style="margin-left: 44%;">
                            <div class="checkbox-container">
                                <asp:Label ID="LabelCheckbox" runat="server" AssociatedControlID="CheckboxExpired">Show expired items</asp:Label>
                                 <asp:CheckBox ID="CheckboxExpired" runat="server" AutoPostBack="true" OnCheckedChanged="CheckboxExpired_CheckedChanged" />
                            </div>

                              <asp:Button ID="ButtonLookupUnit" runat="server" Text="Search" OnClick="ButtonLookupUnit_Click" class="btn btnLookup" Style="margin-bottom: 1rem;" />
                        </div>
                    
                    <asp:Label ID="LabelUnitName" runat="server" AssociatedControlID="TextBoxUnitName">Unit Name<span class="required">*</span></asp:Label>
                    <asp:TextBox ID="TextBoxUnitName" runat="server" placeholder="Unit Name"></asp:TextBox>

                    

                    <asp:Label ID="LabelArea" runat="server" AssociatedControlID="DropDownAreaList">Area<span class="required">*</span></asp:Label>
                    <asp:DropDownList ID="DropDownAreaList" runat="server" DataSourceID="AreaListODS" DataTextField="AreaName" DataValueField="AreaId" AppendDataBoundItems="true"
                        CssClass="form-control" Style="display: inline-block;">
                        <asp:ListItem Value="0">Select an Area</asp:ListItem>
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropDownAreaList" PromptText="Type to search an area"></ajaxToolkit:ListSearchExtender>

                    <asp:Label for="TextBoxStartDate" runat="server" AssociatedControlID="TextBoxStartDate">Start Date<span class="required">*</span></asp:Label>
                    <asp:TextBox ID="TextBoxStartDate" runat="server" CssClass="form-control" Style="display: inline-block;" AutoComplete="off"
                        placeholder="Choose a Date or MM-DD-YYYY"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                        PopupButtonID="ButtonStartDate" PopupPosition="TopRight"
                        TargetControlID="TextBoxStartDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>

                    <asp:Label for="TextBoxExpiryDate" runat="server" AssociatedControlID="TextBoxExpiryDate">Expiry Date</asp:Label>
                    <asp:TextBox ID="TextBoxExpiryDate" runat="server" CssClass="form-control" Style="display: inline-block;" AutoComplete="off"
                        placeholder="Choose a Date or MM-DD-YYYY"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                        PopupButtonID="ButtonExpiryDate" PopupPosition="TopRight"
                        TargetControlID="TextBoxExpiryDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>

                </fieldset>
            </div>

            <div class="col-md-6">
             
                <fieldset>

                   
                    <asp:Label ID="LabelUnitDescription" runat="server" AssociatedControlID="TextBoxUnitDescription">Description<span class="required">*</span></asp:Label>
                    <asp:TextBox ID="TextBoxUnitDescription" runat="server" TextMode="MultiLine" Height="150px" Wrap="true" placeholder="Description" ></asp:TextBox>
                </fieldset>
            </div>
        </div>
        </div>
        
        <div class="row">
            <div class="col-md-12 text-right">
                
                <asp:Button ID="ButtonCreateUnit" runat="server" Text="Create" class="btn btnCreate" OnClick="ButtonCreateUnit_Click" Visible="true" />
                <asp:Button ID="ButtonUpdateUnit" runat="server" Text="Update" class="btn btnCreate" OnClick="ButtonUpdateUnit_Click" Visible="false" />
                <asp:Button ID="ButtonDeleteUnit" runat="server" Text="Deactivate" class="btn btnCancel" OnClick="ButtonDeleteUnit_Click" Visible="false" />
                <asp:Button ID="ButtonCancelUnit" runat="server" Text="Cancel" class="btn btnCancel" OnClick="ButtonCancelUnit_Click" />
            </div>
        </div>

        <asp:ObjectDataSource ID="AreaListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="AreaList" TypeName="TACOSystem.BLL.UnitController"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="UnitListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="UnitList" TypeName="TACOSystem.BLL.UnitController"></asp:ObjectDataSource>

        <script src="/Scripts/bootwrap-freecode.js"></script>
    </div>
</asp:Content>
