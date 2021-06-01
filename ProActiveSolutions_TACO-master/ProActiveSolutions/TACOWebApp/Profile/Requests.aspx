<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Requests.aspx.cs" Inherits="TACOWebApp.Profile.Requests" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id='all_content' runat="server">
        <div class="row info">
            <div class="col-12">
                <h2>Overtime Requests</h2>
                <p class="annotation"></p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12" style="margin-top: 20px;">
                <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
            </div>
        </div>
        <div class="container-two">
            <div class="row">
                <div class="col-12">
                    <asp:GridView ID="GridViewRequest" runat="server" AutoGenerateColumns="False" EmptyDataText="No Overtime Requests"
                        AllowPaging="True" PageSize="15" OnRowDataBound="RequestGrid_RowDataBound" GridLines="None" OnPageIndexChanging="RequestGrid_PageIndexChanging" class="table-bordered employee-request-gridview" Style="background-color: #E4F6FC;">
                        <Columns>
                            <asp:BoundField DataField="RequestId" HeaderText="RequestId" SortExpression="RequestId" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="EmployeeId" HeaderText="EmployeeId" SortExpression="EmployeeId" Visible="false"></asp:BoundField>
                            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:MMM dd, yyyy}"></asp:BoundField>
                            <asp:TemplateField HeaderText="Time">
                                <ItemTemplate>
                                    <asp:Label ID="LabelTotalTime" runat="server" Text='<%# Eval("TotalTime")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="OvertimeDescription" HeaderText="Type" SortExpression="OvertimeId"></asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>
                            <asp:BoundField DataField="Comment" HeaderText="Comments" SortExpression="Comment"></asp:BoundField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                    </asp:GridView>
                    <br />


                </div>

            </div>
            <div class="row">
                <div class="col-md-12 text-right" style="margin-top: 3rem;">
                    <asp:Button ID="closeRequest" runat="server" Text="Close" class="btn btnCancel" OnClick="closeRequest_Click"></asp:Button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
