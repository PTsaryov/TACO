<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApproveOvertime.aspx.cs" Inherits="TACOWebApp.Profile.approveOvertime" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .commentEdit {
            width: 100%;
        }

        .requestHidden {
            display: none;
        }
    </style>
    <div id='all_content' runat="server">

        <div class="row col-md-12 info">
            <h1>Approve Overtime</h1>
        </div>
        <div class="row">
            <div class="col-md-12" style="margin-top:20px;">
                <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
                <br />
            </div>
        </div>

        <div class="container-two">
            <asp:GridView ID="GridViewOvertime" runat="server" AutoGenerateColumns="False" CssClass="table-bordered overtime-gridview" Style="background-color: #E4F6FC; width: 100%;" CellPadding="10" OnRowDataBound="OvertimeGrid_RowDataBound"
                AllowPaging="True" PageSize="15" EmptyDataText="No Overtime Requests" OnPageIndexChanging="OvertimeGrid_PageIndexChanging">
                <Columns>

                    <asp:BoundField DataField="FullName" HeaderText="Name" SortExpression="FullName" />
                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:MMM dd, yyyy}"></asp:BoundField>
                    <asp:TemplateField HeaderText="Time">
                        <ItemTemplate>
                            <asp:Label ID="LabelTotalTime" runat="server" Text='<%# Eval("TotalTime")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="OvertimeDescription" HeaderText="Type" SortExpression="OvertimeDescription"></asp:BoundField>
                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                        <ItemTemplate>
                            <asp:RadioButtonList ID="RadioButtonStatusItem" CssClass="radiobutton-list-overtime" runat="server" SelectedValue='<%# Bind("Status") %>' RepeatDirection="Horizontal">
                                <asp:ListItem Value="Pending" />
                                <asp:ListItem Text="Approve" Value="Approved" />
                                <asp:ListItem Text="Deny" Value="Denied" />
                            </asp:RadioButtonList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                        <EditItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Comment") %>' ID="TextBoxComment" CssClass="form-control"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Comment") %>' ID="TextBox1" Class="commentEdit" CssClass="form-control"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RequestId" SortExpression="RequestId" ItemStyle-CssClass="requestHidden" HeaderStyle-CssClass="requestHidden" />
                    <asp:BoundField DataField="EmployeeId" SortExpression="EmployeeId" ItemStyle-CssClass="requestHidden" HeaderStyle-CssClass="requestHidden" />
                    <asp:BoundField DataField="OvertimeId" SortExpression="OvertimeId" ItemStyle-CssClass="requestHidden" HeaderStyle-CssClass="requestHidden" />
                </Columns>
                <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
            </asp:GridView>
        </div>
        <div class="row">
            <div class="col-md-12 text-right" style="margin-top: 3rem;">
                <asp:Button ID="ButtonSubmitApproval" runat="server" Text="Submit" class="btn btnCreate" OnClick="ButtonSubmitApproval_Click" />
                <asp:Button ID="ButtonCancelApproval" runat="server" Text="Cancel" class="btn btnCancel" OnClick="ButtonCancelApproval_Click" />
            </div>

        </div>

    </div>

</asp:Content>
