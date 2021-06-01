<%@ Page Title="Department" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateDepartment.aspx.cs" Inherits="TACOWebApp.Task.CreateItem" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />

    <div id='all_content' runat="server">

        <div class="row col-md-12 info">
            <h2>Department</h2>
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
                        <asp:Label ID="LabelLookup" runat="server" AssociatedControlID="DropDownDepartment">Find Department</asp:Label>
                        <asp:DropDownList ID="DropDownDepartment" runat="server" DataSourceID="DepartmentlistDataSource" DataTextField="DepartmentName" DataValueField="DepartmentId" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">Choose a Department</asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropDownDepartment" PromptText="Type to search a department"></ajaxToolkit:ListSearchExtender>
                       
                        <div class="button-checkbox-container" style="margin-left:44%;">
                            <div class="checkbox-container">
                                <asp:Label ID="LabelCheckbox" runat="server" AssociatedControlID="CheckboxExpired">Show expired items</asp:Label>
                                <asp:CheckBox ID="CheckboxExpired" runat="server" AutoPostBack="true"  OnCheckedChanged="CheckboxExpired_CheckedChanged" />
                            </div>
                        
                        <asp:Button ID="ButtonLookup" runat="server" OnClick="ButtonLookupDepartment_Click" Text="Search" CssClass="btn btnLookup" Style="margin-bottom: 1rem;" />
                        </div>
                        <asp:Label ID="LabelDepartmentId" runat="server" Visible="false" AssociatedControlID="TextBoxDepartmentId"> Department Id</asp:Label>
                        <asp:TextBox ID="TextBoxDepartmentId" runat="server" Visible="false"></asp:TextBox>


                        <asp:Label ID="LabelDepartmentName" runat="server" AssociatedControlID="TextBoxDepartmentName"> Department Name <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxDepartmentName" runat="server" Placeholder="Technical"></asp:TextBox>
                      

                        <asp:Label ID="LabelStartDate" runat="server" AssociatedControlID="TextBoxStartDate"> Start Date <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxStartDate" runat="server" placeholder="Choose a Date or MM-DD-YYYY"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                            PopupButtonID="ButtonStartDate" PopupPosition="TopRight"
                            TargetControlID="TextBoxStartDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>
                        <asp:Label ID="LabelExpiryDate" runat="server" AssociatedControlID="TextBoxExpiryDate">Expiry Date</asp:Label>

                        <asp:TextBox ID="TextBoxExpiryDate" runat="server" placeholder="Choose a Date or MM-DD-YYYY"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                            PopupButtonID="ButtonEndDate" PopupPosition="TopRight"
                            TargetControlID="TextBoxExpiryDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>
                    </fieldset>
                </div>
                <div class="col-md-6">
                  
                    <fieldset>

                        <asp:Label ID="LabelDepartmentDescription" runat="server" AssociatedControlID="TextBoxDepartmentDescription"> Description <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxDepartmentDescription" runat="server" TextMode="MultiLine" Height="150px" Wrap="true" Placeholder="Description"></asp:TextBox>


                    </fieldset>
                </div>
            </div>

            <div class="row">

                <div class="col-md-12 text-right" style="margin-top:5%;">
                    <asp:Button ID="ButtonCreateDepartment" runat="server" Text="Create" OnClick="ButtonCreateDepartment_Click" CssClass="btn btnCreate" />
                    <asp:Button ID="ButtonUpdateDepartment" runat="server" Text="Update" OnClick="ButtonUpdate_Click" Visible="false" CssClass="btn btnCreate" />                    
                    <asp:Button ID="ButtonDeleteDepartment" runat="server" Text="Deactivate" Visible="false" OnClick="ButtonDeleteDepartment_Click" CssClass="btn btnTerminate" />
                    <asp:Button ID="ButtonActivateDepartment" runat="server" Text="Activate" Visible="false" OnClick="ButtonActivateDepartment_Click" CssClass="btn btnCreate" />
                    <asp:Button ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Click" Text="Cancel" CssClass="btn btnTerminate" />
                </div>
            </div>
        </div>
    </div>

    <asp:ObjectDataSource ID="DepartmentlistDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="DepartmentList"
        TypeName="TACOSystem.BLL.DepartmentController"></asp:ObjectDataSource>
    <script src="/Scripts/bootwrap-freecode.js"></script>
</asp:Content>
