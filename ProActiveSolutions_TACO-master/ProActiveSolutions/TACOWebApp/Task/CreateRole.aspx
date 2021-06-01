<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateRole.aspx.cs" Inherits="TACOWebApp.Task.CreateRole" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id='all_content' runat="server">
        <div class="row col-md-12 info">
            <h2>Security Roles</h2>
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
                        <asp:Label ID="LabelSecurityRoleLookup" runat="server" AssociatedControlID="DropdownRoleLookup">Find Security Role</asp:Label>
                        <asp:DropDownList ID="DropdownRoleLookup" runat="server" AppendDataBoundItems="true" DataSourceID="SecurityRoleListODS" DataTextField="SecurityRole" DataValueField="SecurityRoleId">
                            <asp:ListItem Value="0">Choose a Security Role</asp:ListItem>
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropdownRoleLookup" PromptText="Type to search a role"></ajaxToolkit:ListSearchExtender>
                        <asp:Button ID="ButtonSecurityRoleLookup" runat="server" Text="Search" OnClick="ButtonSecurityRoleLookup_Click" CssClass="btn btnLookup" Style="margin-left: 60%; margin-bottom: 1rem;" />
                        <br />

                        <asp:TextBox ID="TextBoxSecurityRoleId" runat="server" Visible="false"></asp:TextBox>

                        <asp:Label ID="LabelSecurityRole" runat="server" AssociatedControlID="TextBoxSecurityRole">SecurityRole <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxSecurityRole" runat="server" Placeholder="Role Name"></asp:TextBox>

                       

                    </fieldset>
                </div>
                <div class="col-md-6">
                    <fieldset>
                         <asp:Label ID="LabelDescription" runat="server" AssociatedControlID="TextBoxDescription">Description <span class="required">*</span></asp:Label>
                        <asp:TextBox ID="TextBoxDescription" runat="server" TextMode="MultiLine" Height="150px" Wrap="true" Placeholder="Description"></asp:TextBox>
                    </fieldset>
                    
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 text-right" style="margin-top:5%;">
                    <asp:Button ID="ButtonCreateSecurityRole" runat="server" Text="Create" OnClick="ButtonCreateSecurityRole_Click" Visible="true" CssClass="btn btnCreate" />
                    <asp:Button ID="ButtonUpdateSecurityRole" runat="server" Text="Update" OnClick="ButtonUpdateSecurityRole_Click" Visible="false" CssClass="btn btnTerminate" />
                    <asp:Button ID="ButtonClearSecurityRole" runat="server" Text="Cancel" OnClick="ButtonClearSecurityRole_Click" CssClass="btn btnCancel" />
                </div>
            </div>
        </div>

        <script src="/Scripts/bootwrap-freecode.js"></script>

        <asp:ObjectDataSource ID="SecurityRoleListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="RoleList" TypeName="TACOSystem.BLL.Security.RoleController"></asp:ObjectDataSource>
    </div>
</asp:Content>
