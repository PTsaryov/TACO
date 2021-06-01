<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateEntitlement.aspx.cs" Inherits="TACOWebApp.Task.UpdateEntitlement" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id='all_content' runat="server">
        <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />
        <div class="row col-md-12 info">
            <h1>Update Entitlements</h1>
        </div>
        <div class="row">
            <div class="col-md-12" style="margin-top:20px;">
                <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
            </div>
        </div>

        <div class="container-two">
            <div>
                <div class="form-group form-inline">
                    <asp:Label ID="LabelAttendanceLookup" runat="server" AssociatedControlID="DropdownAttendanceLookup">Select:</asp:Label>
                    <asp:DropDownList ID="DropdownAttendanceLookup" class="form-control" runat="server" DataSourceID="AttendanceCodeODS" DataTextField="AttendanceCode" DataValueField="AttendanceId"
                        OnSelectedIndexChanged="DropdownAttendanceLookup_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true">
                        <asp:ListItem Value="0">Choose an Attendance Code</asp:ListItem>
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropdownAttendanceLookup" PromptText="Type to search an attendance code"></ajaxToolkit:ListSearchExtender>
                </div>


                <asp:GridView ID="GridViewEntitlement" runat="server" EmptyDataText="No Employees" PageSize="15" CssClass="table-bordered entitlement-gridview" AllowPaging="True"
                    OnPageIndexChanging="EntitlementGrid_PageIndexChanging" AutoGenerateColumns="False" ShowFooter="True" Style="background-color: #E4F6FC; width: 100%;">

                    <Columns>
                        <asp:TemplateField HeaderText="AttendanceEntitlementId" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxId" runat="server" Text='<%# Bind("AttendanceEntitlementId") %>' Enabled="false"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EmployeeId" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxEmployeeId" runat="server" Text='<%# Bind("EmployeeId") %>' Enabled="false"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" SortExpression="FullName">
                            <FooterTemplate>
                                <asp:DropDownList ID="DropDownName" runat="server" DataSourceID="EmployeeNameODS" DataTextField="FullName" DataValueField="EmployeeId"
                                    AppendDataBoundItems="true" class="form-control">
                                    <asp:ListItem Value="-1">Choose an Employee</asp:ListItem>
                                </asp:DropDownList>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("FullName") %>' Style="font-weight: bold;"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attendance Code" SortExpression="AttendanceCode">
                            <FooterTemplate>
                                <asp:DropDownList ID="DropDownAttendance" runat="server" DataSourceID="AttendanceCodeODS"
                                    DataTextField="AttendanceCode" class="form-control" DataValueField="AttendanceId" AppendDataBoundItems="true">
                                    <asp:ListItem Value="-1">Choose an Attendance Code</asp:ListItem>
                                </asp:DropDownList>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("AttendanceCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Units" HeaderText="Units" SortExpression="Units" />
                        <asp:BoundField DataField="TotalTime" HeaderText="Current Total" SortExpression="TotalTime" />

                        <asp:TemplateField HeaderText="Amount to add">
                            <FooterTemplate>
                                <div style="display: flex;">
                                    <asp:TextBox ID="TextBoxNewAmount" runat="server" CssClass="form-control" Style="width: 80%;"></asp:TextBox>
                                    <asp:Button ID="ButtonAddEntitlement" runat="server" CommandName="Insert" Style="margin-left: 1rem;" OnClick="ButtonAddEntitlement_Click" Text="Add" class="btn btnCreate" />
                                </div>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxUpdateAmount" runat="server" CssClass="form-control" Textmode="Number" min="-50" max="1000"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <PagerStyle HorizontalAlign="Center" CssClass="grid-pager" />
                </asp:GridView>
            </div>
            <div class="row">
                <div class="col-md-12 text-right" style="margin-top: 3rem;">
                    <asp:Button ID="ButtonSubmitUpdates" runat="server" Text="Submit" class="btn btnCreate" OnClick="ButtonSubmitUpdates_Click" />
                    <asp:Button ID="ButtonCancelUpdates" runat="server" Text="Cancel" class="btn btnCancel" OnClick="ButtonCancelUpdates_Click" />
                </div>

            </div>

        </div>

    </div>
    <asp:ObjectDataSource ID="AttendanceCodeODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="AttendanceList" TypeName="TACOSystem.BLL.Attendance.AttendanceController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="EmployeeNameODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ActiveEmployeeList" TypeName="TACOSystem.BLL.Attendance.AttendanceEntitlementController"></asp:ObjectDataSource>
</asp:Content>
