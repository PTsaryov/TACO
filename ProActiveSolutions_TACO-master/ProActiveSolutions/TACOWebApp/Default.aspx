<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TACOWebApp._Default" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .row-buttons p {
            display: inline-block;
            margin-top: 10px;
            margin-right: 10px;
        }

        #calendar, #external-events, #external-events-requests {
            border-radius: 8px;
            border: 1px solid #a9a9a9;
        }

        .container {
            border-radius: 8px;
        }

        .overtime-totals {
            text-align: left;
            margin-left: 35px;
            font-weight: bold;
            padding: 15px;
        }

        .manage-calendars {
            padding: 20px 15px 0 15px;
            margin-top: 20px;
            border-radius: 8px;
            border: 1px solid #a9a9a9;
            background-color: #eee;
        }
    </style>
    <div id="all_content" runat="server">
       
            <div class="col-md-8 calendar-container">
                <div id='calendar'></div>
            </div>

            <div class="col-md-4">
                <div id="manage_calendars" class="manage-calendars" runat="server">
                    <h4>Manage Calendars</h4>
                    <br />
                    <asp:DropDownList ID="DropDownTeamList" runat="server" DataTextField="Value" DataValueField="Key" CssClass="form-control" AutoPostBack="True"  OnSelectedIndexChanged="DropDownTeamList_SelectedIndexChanged">
                    
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DropDownTeamList" PromptText="Type to search a team"></ajaxToolkit:ListSearchExtender>
                    <br />
                    <asp:DropDownList ID="DropdonEmployeeLIst" runat="server" DataTextField="Value" DataValueField="Key" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Value="0">Choose an Employee</asp:ListItem>
                    </asp:DropDownList>
                    <ajaxToolkit:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DropdonEmployeeLIst" PromptText="Type to search an employee"></ajaxToolkit:ListSearchExtender>
                    <br />
                    <p>
                        <a id="fetchEmployee" class="btn btnCreate">Find Calendar</a>
                    </p>
                    <p>
                        <a id="backToMyCalendar" class="btn btnCreate">My Calendar</a>
                    </p>
                </div>
                <div id='external-events'>
                    <h4>Your Projects</h4>
                    <div class="events">
                    </div>
                </div>
                <div id='external-events-requests'>
                    <h4>Over Time / Bank Time</h4>
                    <div class="requests">
                    </div>
                </div>
                <div class="totals-and-buttons text-right">
                    <div class="overtime-totals">
                        <%--will be populated programmatically via Jquery--%>
                    </div>
                    <div class="row-buttons">
                        <p id="save_reminder" style="color: red; font-size: 100%;">Remember to save before leaving this page!</p>
                        <p class="">
                            <a id="save" class="btn btnCreate">Save</a>
                        </p>
                        <p>
                            <a id="cancel" class="btn btnCancel">Cancel</a>
                        </p>
                    </div>
                </div>
            </div>
 
        <%--Editing events popup modal--%>
        <div class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Edit event</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <label class="col-xs-4" for="title">Event title</label>
                                <input type="text" name="title" id="title" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <label class="col-xs-4" for="starts-at">Starts at</label>
                                <input type="text" name="starts_at" id="starts-at" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <label class="col-xs-4" for="ends-at">Ends at</label>
                                <input type="text" name="ends_at" id="ends-at" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-primary" id="save-event">Save changes</button>
                        <button type="button" class="btn btn-danger" id="delete-event">Delete</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </div>

</asp:Content>
