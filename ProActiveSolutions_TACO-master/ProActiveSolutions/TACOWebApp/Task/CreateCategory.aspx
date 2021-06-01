<%@ Page Title="Category" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateCategory.aspx.cs" Inherits="TACOWebApp.Task.CreateCategory" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />

    <div id='all_content' runat="server">
        <div class="row col-md-12 info">
            <h2>Category</h2>
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
                        <asp:Label ID="LabelLookup" runat="server" AssociatedControlID="DropDownCategory">Find Category</asp:Label>
                        <asp:DropDownList ID="DropDownCategory" runat="server" DataSourceID="CategorylistDataSource" DataTextField="CategoryName" DataValueField="CategoryId" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">Choose a Category</asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropDownCategory" PromptText="Type to search a category"></ajaxToolkit:ListSearchExtender>


                          <div class="button-checkbox-container" style="margin-left:44%;">
                            <div class="checkbox-container">
                                <asp:Label ID="LabelCheckbox" runat="server" AssociatedControlID="CheckboxExpired">Show expired items</asp:Label>
                                    <asp:CheckBox ID="CheckboxExpired" runat="server" AutoPostBack="true" OnCheckedChanged="CheckboxExpired_CheckedChanged" />
                            </div>
                        <asp:Button ID="ButtonLookup" runat="server" OnClick="ButtonLookupCategory_Click" Text="Search" CssClass="btn btnLookup" style="margin-bottom:1rem;" />
                    </div>
                        <asp:Label ID="LabelCategoryId" runat="server" Visible="false" AssociatedControlID="TextBoxCategorytId"> Category Id</asp:Label>
                        <asp:TextBox ID="TextBoxCategoryId" runat="server" Visible="false"></asp:TextBox>

                        <asp:Label ID="LabelCategoryName" runat="server" AssociatedControlID="TextBoxCategoryName"> Category Name <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxCategoryName" runat="server" Placeholder="Technical"></asp:TextBox>

                       
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
                    <asp:Label ID="LabelCategoryDescription" runat="server" AssociatedControlID="TextBoxCategoryDescription"> Description <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxCategoryDescription" runat="server" TextMode="MultiLine" Height="150px" Wrap="true" Placeholder="Description"></asp:TextBox>
                    </fieldset>
                </div>
            </div>

            <div class="row">

                <div class="col-md-12 text-right" style="margin-top:5%;">
                    <asp:Button ID="ButtonCreateCategory" runat="server" Text="Create" OnClick="ButtonCreateCategory_Click" CssClass="btn btnCreate" />
                    <asp:Button ID="ButtonUpdateCategory" runat="server" Text="Update" OnClick="ButtonUpdateCategory_Click" Visible="false" CssClass="btn btnCreate" />                    
                    <asp:Button ID="ButtonDeleteCategory" runat="server" Text="Deactivate" Visible="false" OnClick="ButtonDeleteCategory_Click" CssClass="btn btnTerminate" />
                    <asp:Button ID="ButtonActivateCategory" runat="server" Text="Activate" Visible="false" OnClick="ButtonActivateCategory_Click" CssClass="btn btnCreate" />
                    <asp:Button ID="ButtonCancel" runat="server" OnClick="ButtonCancelCategory_Click" Text="Cancel" CssClass="btn btnTerminate" />
                </div>
            </div>
        </div>

    </div>



    <asp:ObjectDataSource ID="CategorylistDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="CategoryList"
        TypeName="TACOSystem.BLL.CategoryController"></asp:ObjectDataSource>
    <script src="/Scripts/bootwrap-freecode.js"></script>
</asp:Content>
