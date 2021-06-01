<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateTeam.aspx.cs" Inherits="TACOWebApp.Task.CreateTeam" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />

    <div id='all_content' runat="server">
        <div class="row col-md-12 info">
            <h2>Create/Edit Team</h2>
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
                    <asp:ObjectDataSource ID="ODSTeam" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="TeamDetails" TypeName="TACOSystem.BLL.TeamController"></asp:ObjectDataSource>
                    <asp:Label ID="LabelLookupTeam" runat="server" AssociatedControlID="DropDownTeam">Find Team</asp:Label>
                    <asp:DropDownList ID="DropDownTeam" runat="server" AppendDataBoundItems="true" DataSourceID="ODSTeam" DataTextField="TeamName" DataValueField="TeamId">
                        <asp:ListItem Value="0">Select a Team</asp:ListItem>
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropDownTeam" PromptText="Type to search a team"></ajaxToolkit:ListSearchExtender>
                    <div class="button-checkbox-container" style="margin-left: 44%;">
                            <div class="checkbox-container">
                                <asp:Label ID="LabelCheckbox" runat="server" AssociatedControlID="CheckboxExpired">Show expired items</asp:Label>
                                <asp:CheckBox ID="CheckboxExpired" runat="server" AutoPostBack="true" OnCheckedChanged="CheckboxExpired_CheckedChanged" />
                            </div>

                              <asp:Button ID="ButtonLookup" runat="server" Text="Search" OnClick="ButtonLookup_Click" CssClass="btn btnLookup" Style=" margin-bottom: 1rem;" />
                        </div>                    
              
                    <asp:TextBox ID="TextBoxTeamId" runat="server" Visible="false"></asp:TextBox>

                    <asp:Label ID="LabelTeamName" runat="server" AssociatedControlID="TextBoxTeamName">Team Name <span class="required">*</span></asp:Label>
                    <asp:TextBox ID="TextBoxTeamName" runat="server" placeholder="Team A"></asp:TextBox>

                    
                    <asp:Label ID="LabelStartDate" runat="server" AssociatedControlID="TextBoxStartDate">Start Date <span class="required">*</span></asp:Label>
                    <asp:TextBox ID="TextBoxStartDate" runat="server" Format="dddd, MMMM dd, yyyy" placeholder="Choose a Date or MM-DD-YYYY"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                        PopupButtonID="ButtonStartDate" PopupPosition="TopRight"
                        TargetControlID="TextBoxStartDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>

                    <asp:Label ID="LabelExpiryDate" runat="server" AssociatedControlID="TextBoxExpiryDate">Expiry Date</asp:Label>
                    <asp:TextBox ID="TextBoxExpiryDate" runat="server" Format="dddd, MMMM dd, yyyy" placeholder="Choose a Date or MM-DD-YYYY"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                        PopupButtonID="ButtonExpiryDate" PopupPosition="TopRight"
                        TargetControlID="TextBoxExpiryDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>

                    <asp:ObjectDataSource ID="ODSUnit" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="UnitList" TypeName="TACOSystem.BLL.UnitController"></asp:ObjectDataSource>
                    <asp:Label ID="LabelDropDownUnit" runat="server" AssociatedControlID="DropDownUnit">Unit <span class="required">*</span></asp:Label>
                    <asp:DropDownList ID="DropDownUnit" runat="server" DataSourceID="ODSUnit" DataTextField="UnitName" DataValueField="UnitId" AppendDataBoundItems="true">
                        <asp:ListItem Value="">Select a unit</asp:ListItem>
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DropDownUnit" PromptText="Type to search a unit"></ajaxToolkit:ListSearchExtender>

                    <%--used to capture data to use in at the bottom of the page--%>
                    <asp:TextBox ID="TextBoxCreatedBy" runat="server" ReadOnly="true" BackColor="LightGray" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="TextBoxCreatedOn" runat="server" ReadOnly="true" BackColor="LightGray" Format="dddd, MMMM dd, yyyy" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="TextBoxModifiedBy" runat="server" ReadOnly="true" BackColor="LightGray" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="TextBoxModifiedOn" runat="server" ReadOnly="true" BackColor="LightGray" Format="dddd, MMMM dd, yyyy" Visible="false"></asp:TextBox>

                </fieldset>
            </div>

            <div class="col-md-6">
             
                <fieldset>
     
                    <asp:Label ID="LabelDescription" runat="server" AssociatedControlID="TextBoxDescription">Description <span class="required">*</span></asp:Label>
                    <asp:TextBox ID="TextBoxDescription" runat="server" TextMode="MultiLine" Height="150px" Wrap="true" placeholder="Description"></asp:TextBox>

                </fieldset>
            </div>
        </div>
        </div>

        <div class="row">
            <div class="col-md-12 text-right">
                <asp:Button ID="ButtonCreate" runat="server" OnClick="ButtonCreate_Click" Text="Create" Visible="true" CssClass="btn btnCreate" />
                <asp:Button ID="ButtonUpdate" runat="server" OnClick="ButtonUpdate_Click" Text="Update" Visible="false" CssClass="btn btnCreate" />
                <asp:Button ID="ButtonDeactivate" runat="server" OnClick="ButtonDeactivate_Click" Text="Deactivate" Visible="false" CssClass="btn btnTerminate" />                
                <asp:Button ID="ButtonActivateTeam" runat="server" Text="Activate" Visible="false" CssClass="btn btnCreate" OnClick="ButtonActivateTeam_Click" />
                <asp:Button ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Click" Text="Cancel" CssClass="btn btnCancel" />
            </div>
        </div>
        <script src="/Scripts/bootwrap-freecode.js"></script>
    </div>
</asp:Content>
