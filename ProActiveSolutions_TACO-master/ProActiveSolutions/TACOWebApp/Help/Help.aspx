<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="TACOWebApp.Help.Help" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />
    <div id="all_content" runat="server">
        <div class="row col-md-12 info">
        <h2>Help Desk</h2>
        <p class="annotation">Questions? We have the answers.</p>
    </div>

    <div class="row">
        <div class="col-md-12" id="MessageUserControl">
            <uc1:MessageUserControl runat="server" ID="MessageUserControl1" />
        </div>
    </div>
    <div class="container-two">
        <div id="accordion" class="panel-group">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne">How to view or update your profile information?</a>
                    </h4>
                </div>
                <div id="collapseOne" class="panel-collapse collapse">
                    <div class="panel-body">
                        <div class="col-md-12">
                            <p style="font-weight:bold;">To view your profile:</p>
                            <ol>
                                <li>Hover your mouse over 'Main' and click on 'Profile' in the dropdown menu.</li>
                            </ol>
                            <p  style="font-weight:bold;">To update your profile:</p>
                            <ol>
                                <li>Contact your team lead or admin.</li>
                            </ol>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">How to fill up a timesheet?</a>
                    </h4>
                </div>
                <div id="collapseTwo" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <div class="col-md-12">
                            <ol>
                                <li>Click on the WCB logo. It should lead you to the timesheet page.</li>
                                <li>Using the arrows on the top left corner navigate to the desired week. Note that you can only go to the next week.</li>
                                <li>Drag and drop the projects you have worked on the day in the calendar. Adjust the time by dragging the edges. You can also adjust the time by click on the project that is on the calendar.</li>
                                <li>If you need to have the time you worked as overtime or banked time, simply drag overtime or banked time in the calendar. A request will be sent automatically to your team lead once you click 'Save'.</li>
                                <li>Note that if you need to change something in a timesheet that is in the past week, you need to contact your team lead or admin. </li>
                            </ol>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseThree">How to file a day off?</a>
                    </h4>
                </div>
                <div id="collapseThree" class="panel-collapse collapse">
                    <div class="panel-body">
                        <ol>
                            <li>Click on 'Scheduler' in the menu.</li>
                            <li>Choose the month and year from the dropdowns at the top of the page and click 'Submit'.</li>
                            <li>The appropriate calendar will load up in the screen.</li>
                            <li>Click on the day that you want your day off. Choose a reason from your list and click 'Next'. Fill out the dates and click 'Save changes'. The system will display a success alert if the day off has been booked.</li>
                        </ol>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseFour">How to know if your request was approved or denied?</a>
                    </h4>
                </div>
                <div id="collapseFour" class="panel-collapse collapse">
                    <div class="panel-body">
                        <ol>
                            <li>Hover your mouse over 'Main' and click on 'Requests' in the dropdown menu. </li>
                        </ol>
                    </div>
                </div>
            </div>

        </div>
    </div>
    </div>
    

    <script src="/Scripts/bootwrap-freecode.js"></script>

</asp:Content>
