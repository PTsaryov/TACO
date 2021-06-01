<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateArea.aspx.cs" Inherits="TACOWebApp.Task.CreateArea" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />

    <div id='all_content' runat="server">
        <div class="all_content" runat="server">
            <div class="row col-md-12 info">
                <h2>Area</h2>
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
                            <asp:ObjectDataSource ID="ODSArea" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="AreaList" TypeName="TACOSystem.BLL.AreaController"></asp:ObjectDataSource>
                            <asp:Label ID="LabelAreaLookup" runat="server" AssociatedControlID="DropDownArea">Find<span class="required">*</span></asp:Label>
                            <asp:DropDownList ID="DropDownArea" runat="server" AppendDataBoundItems="true" DataSourceID="ODSArea" DataTextField="AreaName" DataValueField="AreaId">
                                <asp:ListItem Value="0">Select an Area</asp:ListItem>
                            </asp:DropDownList>
                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropDownArea" PromptText="Type to search an area"></ajaxToolkit:ListSearchExtender>
                            <div class="button-checkbox-container" style="margin-left:40%;">
                                <div class="checkbox-container">
                                    <asp:Label ID="LabelCheckbox" runat="server" AssociatedControlID="CheckboxExpired">Show deactivated items</asp:Label>
                                    <asp:CheckBox ID="CheckboxExpired" runat="server" AutoPostBack="true" OnCheckedChanged="CheckboxExpired_CheckedChanged" />
                                </div>
                                <asp:Button ID="ButtonAreaLookup" runat="server" Text="Search" OnClick="ButtonAreaLookup_Click" CssClass="btn btnLookup" Style=" margin-bottom: 1rem;" />
                            </div>


                            <asp:TextBox ID="TextBoxAreaId" runat="server" Visible="false"></asp:TextBox>

                            <asp:Label ID="LabelAreaName" runat="server" AssociatedControlID="TextBoxAreaName">Area Name<span class="required">*</span></asp:Label>
                            <asp:TextBox ID="TextBoxAreaName" Placeholder="Name" runat="server"></asp:TextBox>

                            <asp:Label ID="LabelAreaDescription" runat="server" AssociatedControlID="TextBoxAreaDescription">Area Description<span class="required">*</span></asp:Label>
                            <asp:TextBox ID="TextBoxAreaDescription" runat="server" Height="120px" TextMode="MultiLine" Wrap="true" placeholder="Description"></asp:TextBox>


                        </fieldset>
                    </div>

                    <div class="col-md-6">
                        <br />
                        <fieldset>
                            <asp:Label ID="LabelDepartmentId" runat="server" AssociatedControlID="DropDownDepartment">Department<span class="required">*</span></asp:Label>
                            <asp:ObjectDataSource ID="ODSDepartment" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="DepartmentList" TypeName="TACOSystem.BLL.DepartmentController"></asp:ObjectDataSource>
                            <asp:DropDownList ID="DropDownDepartment" runat="server" DataSourceID="ODSDepartment" DataTextField="DepartmentName" DataValueField="DepartmentId" AppendDataBoundItems="true">
                                <asp:ListItem Value="">Select a department</asp:ListItem>
                            </asp:DropDownList>
                            <ajaxToolkit:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DropDownDepartment" PromptText="Type to search a department"></ajaxToolkit:ListSearchExtender>

                            <asp:Label ID="LabelStartDate" runat="server" AssociatedControlID="TextBoxStartDate">Start Date<span class="required">*</span></asp:Label>
                            <asp:TextBox ID="TextBoxStartDate" runat="server" Format="dddd, MMMM dd, yyyy" placeholder="Choose a Date or MM-DD-YYYY"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                                PopupButtonID="ButtonStarDate" PopupPosition="TopRight"
                                TargetControlID="TextBoxStartDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>

                            <asp:Label ID="LabelExpiryDate" runat="server" AssociatedControlID="TextBoxExpiryDate">Expiry Date</asp:Label>
                            <asp:TextBox ID="TextBoxExpiryDate" runat="server" Format="dddd, MMMM dd, yyyy" placeholder="Choose a Date or MM-DD-YYYY"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                                PopupButtonID="ButtonExpiryDate" PopupPosition="TopRight"
                                TargetControlID="TextBoxExpiryDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>


                            <%--used to capture data to use in at the bottom of the page--%>
                     <%--       <asp:TextBox ID="TextBoxCreatedBy" runat="server" ReadOnly="true" BackColor="LightGray" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="TextBoxCreatedOn" runat="server" ReadOnly="true" BackColor="LightGray" Format="dddd, MMMM dd, yyyy" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="TextBoxModifiedBy" runat="server" ReadOnly="true" BackColor="LightGray" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="TextBoxModifiedOn" runat="server" ReadOnly="true" BackColor="LightGray" Format="dddd, MMMM dd, yyyy" Visible="false"></asp:TextBox>--%>

                        </fieldset>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12 text-right" style="margin-top:5%;">
                        <asp:Button ID="ButtonAreaCreate" runat="server" Text="Create" OnClick="ButtonAreaCreate_Click" Visible="true" CssClass="btn btnCreate" />
                        <asp:Button ID="ButtonAreaUpdate" runat="server" Text="Update" OnClick="ButtonAreaUpdate_Click" Visible="false" CssClass="btn btnCreate" />
                        <asp:Button ID="ButtonRemove" runat="server" Text="Deactivate" CssClass="btn btnTerminate" OnClick="ButtonRemove_Click" Visible="false" />
                        <asp:Button ID="ButtonAreaCancel" runat="server" Text="Cancel" OnClick="ButtonAreaCancel_Click" CssClass="btn btnCancel" />
                    </div>
                </div>

                <script src="/Scripts/bootwrap-freecode.js"></script>
            </div>
        </div>
    </div>

</asp:Content>
