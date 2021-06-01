<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookDaysOff.aspx.cs" Inherits="TACOWebApp.Schedule.BookDaysOff" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />
    <div id="all_content" runat="server">
        <div class="row info">
            <div class="col-12">
                <h2>Book Days Off</h2>
                <p class="annotation">Have a break, book a day off!</p>
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
                    <div class="form-group form-inline">
                        <asp:Label ID="LabelYear" runat="server" AssociatedControlID="DropdownYears">Year <span class="required">*</span></asp:Label>
                        <asp:DropDownList ID="DropdownYears" runat="server" class="form-control" AppendDataBoundItems="true" AutoPostBack="True"></asp:DropDownList>

                        <asp:Label ID="LabelMonth" runat="server" AssociatedControlID="DropdownMonths">Month <span class="required">*</span></asp:Label>
                        <asp:DropDownList ID="DropdownMonths" class="form-control" runat="server" AppendDataBoundItems="true"></asp:DropDownList>

                        <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" OnClick="ButtonSubmit_Click" Visible="true" CssClass="btn btnCreate" />
                    </div>


                </div>
            </div>

            <div id="DivCalendar" class="row" runat="server" visible="false">
                <div class="col-md-12">
                    <asp:Repeater ID="RepeaterEmployeeDaysOff" runat="server" ItemType="TACOData.Entities.POCOs.DaysOff.DayOffInformation" OnItemCreated="RepeaterEmployeeDaysOff_ItemCreated" OnItemDataBound="RepeaterEmployeeDaysOff_ItemDataBound">
                        <HeaderTemplate>
                            <table id="TableSchedule" class="table-bordered days-off-calendar" style="background-color: #E4F6FC;">
                                <tr style="background-color: #98C0CE; font-size: large; color: #000; font-weight: bold; text-align: center;">
                                    <td></td>

                                    <asp:Repeater ID="RepeaterDaysOfMonth" runat="server" ItemType="TACOData.Entities.POCOs.MonthDayYear">
                                        <ItemTemplate>
                                            <td style="padding: 0.5rem;">
                                                <asp:HiddenField ID="HiddenMonth" runat="server" Value="<%#Item.MonthNumber %>"></asp:HiddenField>
                                                <asp:Label ID="LabelDay" runat="server" Text="<%#Item.DayNumber %>"> </asp:Label>
                                                <asp:HiddenField ID="HiddenYear" runat="server" Value="<%#Item.YearNumber%>"></asp:HiddenField>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>


                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="padding: 0.5rem; font-weight: bold;">
                                    <asp:HiddenField ID="HiddenFieldEmployeeId" runat="server" Value="<%#Item.EmployeeId %>" />
                                    <asp:HiddenField ID="HiddenFieldTimesheetId" runat="server" Value="<%# Item.TimesheetId%>" />
                                    <asp:Label ID="LabelLastName" runat="server" Text="<%#Item.LastName%>"></asp:Label>
                                    <asp:Label ID="LabelFirstName" runat="server" Text="<%#Item.FirstName%>"></asp:Label>
                                </td>
                                <asp:Repeater ID="RepeaterDaysOff" runat="server" DataSource="<%#Item.BookedDaysPerMonth.Flag%>" ItemType="System.Collections.Generic.KeyValuePair`2[System.Int32,System.String]">
                                    <ItemTemplate>
                                        <td>
                                            <!--begin TD-->

                                            <asp:Button ID="ButtonLaunchModal" data-id="launchModal" runat="server" CssClass="btn btn-primary" data-toggle="modal" OnClientClick="return false;" Text="<%#Item.Value%>" data-button="<%#Item.Key%>" Enabled="true" Style="width: 4.7rem; padding: 0.5rem;" />

                                            <!-- Modal -->
                                            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                                <div class="modal-dialog" role="document">
                                                    <div id="Absence-Modal" class="modal-content">
                                                        <div class="modal-header">
                                                            <h5 class="modal-title" id="exampleModalLabel">Choose reason for day off:</h5>
                                                            <div id="errors"></div>
                                                        </div>
                                                        <div class="modal-body" id="pop-up">
                                                            <div id="radio-buttons-container">
                                                            </div>
                                                            <div id="form-container" class="hidden">
                                                            </div>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <button id="Close" type="button" class="btn btnCancel" data-dismiss="modal">Close</button>
                                                            <button id="Save" type="button" class="btn btnCreate">Save changes</button>
                                                            <button id="Next" type="button" class="btn btnCreate hidden">Next</button>
                                                            <button id="Book" type="button" class="btn btnCreate hidden">Book day off</button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                        <!--end TD-->
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table> <%--endTable--%>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>

    </div>

    <script src="/Scripts/bootwrap-freecode.js"></script>


</asp:Content>
