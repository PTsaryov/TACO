<%@ Page Title="Create Project" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateProject.aspx.cs" Inherits="TACOWebApp.Project.CreateProject" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />

    <div id='all_content' runat="server">
        <div class="row col-md-12 info">
            <h1>Create Project</h1>
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
                    <asp:Label ID="LabelProjectId" runat="server" Visible="False" AssociatedControlID="TextBoxProjectId"> Project Id</asp:Label>
                    <asp:TextBox ID="TextBoxProjectId" runat="server" Visible="False"></asp:TextBox>

                    <asp:Label ID="LabelProjectName" runat="server" AssociatedControlID="TextBoxProjectName"> Project Name <span class="required">*</span></asp:Label>
                    <asp:TextBox ID="TextBoxProjectName" runat="server" Placeholder="Tech SOlution"></asp:TextBox>

                    <asp:Label ID="LabelStartDate" runat="server" AssociatedControlID="TextBoxStartDate"> Start Date <span class="required">*</span></asp:Label>
                    <asp:TextBox ID="TextBoxStartDate" runat="server" placeholder="Choose a Date"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="ButtonStartDate" PopupPosition="Right"
                        TargetControlID="TextBoxStartDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>

                    <asp:Label ID="LabelEndDate" runat="server" AssociatedControlID="TextBoxEndDate">End Date</asp:Label>
                    <asp:TextBox ID="TextBoxEndDate" runat="server" placeholder="Choose a Date"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="ButtonEndDate" PopupPosition="Right"
                        TargetControlID="TextBoxEndDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>

                    <asp:Label ID="LabelLookupCategory" runat="server" AssociatedControlID="DropDownCategory">Category <span class="required">*</span></asp:Label>
                    <asp:DropDownList ID="DropDownCategory" runat="server" DataTextField="CategoryName" DataSourceID="CategorylistDataSource" DataValueField="CategoryId" AppendDataBoundItems="true">
                        <asp:ListItem Value="0">Choose a Category</asp:ListItem>
                    </asp:DropDownList>
                    

                    <asp:Label ID="LabelEmployees" runat="server" AssociatedControlID="selectEmployees">Employees</asp:Label>

                    <button id="selectEmployees" runat="server" type="button" class="btn btn-primary" data-toggle="modal" data-target="#ModalAddEmployee">
                        Select Employees
                    </button>
                    <br />
                    <br />

                    <div class="modal fade" id="ModalAddEmployee" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h2 class="modal-title" id="exampleModalLabel">Add Employees</h2>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <form>

                                         <asp:ListView ID="ListView1" runat="server" DataSourceID="EmployeeDataSource" ItemType="TACOData.Entities.POCOs.ProjectInformation" DataKeyNames="ProjectID">

                                           <ItemTemplate>
                                               <tr style="background-color: #E4F6FC; color: #333333;" >
                                                   <td style="text-align:center;">
                                                       <asp:CheckBox ID="CheckBoxSelectEmployee" runat="server"></asp:CheckBox></td>
                                                   <td style="text-align:center;">
                                                       <asp:Label CssClass="namebox" Text='<%# Eval("EmployeeId") %>' runat="server" ID="LableEmployeeId" /></td>
                                                   <td style="text-align:center;">
                                                       <asp:Label CssClass="namebox" Text='<%# Eval("FullName") %>' runat="server" ID="LabelFullName" /></td>
                                                   <td style="text-align:center;">
                                                        <asp:Label CssClass="nameboxfield" Text='<%# Eval("Position") %>' runat="server" ID="LablePositionName" /></td>
                                                   <td style="text-align:center;">
                                                        <asp:Label CssClass="nameboxfield" Text='<%# Eval("DepartmentName") %>' runat="server" ID="LabelDepartmentName" /></td>
                                                   <td style="text-align:center;">
                                                        <asp:Label CssClass="nameboxfield" Text='<%# Eval("TeamName") %>' runat="server" ID="LabelTeamName" /></td>
                                               </tr>
                                           </ItemTemplate>

                                           <LayoutTemplate>
                                               <table runat="server" style="background-color: #FFFFFF; margin-bottom:0; width:100%">
                                                   <tr runat="server">
                                                       <td runat="server">
                                                           <table  class="table table-responsive table-bordered" runat="server" id="itemPlaceholderContainer" style="margin-bottom:0;" >
                                                               <tr runat="server" style="background-color: #98C0CE; color: #333333;">
                                                                   <th style="text-align:center;" runat="server"></th>
                                                                   <th style="text-align:center;" runat="server">Id</th>
                                                                   <th style="text-align:center;" runat="server">Full Name</th>
                                                                   <th style="text-align:center;" runat="server">Position</th>
                                                                   <th style="text-align:center;" runat="server">Department</th>
                                                                   <th style="text-align:center;" runat="server">Team</th>
                                                               </tr>
                                                              <tr runat="server" id="itemPlaceholder"></tr>
                                                           </table>
                                                       </td>
                                                   </tr>
                                                   <tr runat="server">
                                                      <td runat="server" style="text-align: center; background-color: #5D7B9D; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF"></td>
                                                   </tr>
                                               </table>
                                           </LayoutTemplate>

                                       </asp:ListView>
                                    </form>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                    <button type="button" class="btn btn-primary" id="saveEmployee">Save</button>
                                </div>
                            </div>
                        </div>
                    </div>

                       </fieldset>

                    </div>

                    <div class="col-md-6">
                        <fieldset>

                           <br />

                    <asp:Label ID="LabelProjectDescription" runat="server" AssociatedControlID="TextBoxProjectDescription">Description <span class="required">*</span></asp:Label>
                    <asp:TextBox ID="TextBoxProjectDescription" TextMode="MultiLine" Height="120px" Wrap="true" Placeholder ="Tech project needs to completed soon." runat="server"></asp:TextBox>

                    <asp:Label ID="LabelPriority" runat="server" AssociatedControlID="RadioButtonListPriority">Priority <span class="required">*</span></asp:Label>
                    <asp:RadioButtonList ID="RadioButtonListPriority" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" OnSelectedIndexChanged="RadioButtonListPriority_SelectedIndexChanged">
                        <asp:ListItem Text="Low" Value="Low"></asp:ListItem>
                        <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>
                        <asp:ListItem Text="High" Value="High"></asp:ListItem>
                    </asp:RadioButtonList>

                    <asp:Label ID="LabelColorPicker" runat="server" AssociatedControlID="TextBoxPickerColor">Color</asp:Label>
                    <asp:TextBox ID="TextBoxPickerColor" Placeholder="Select a Color" runat="server"></asp:TextBox>
                    <ajaxToolkit:ColorPickerExtender ID="ColorPickerExtender1" runat="server" PopupButtonID="ColorPickerbutton" PopupPosition="TopRight" TargetControlID="TextBoxPickerColor" />
                                   
                </fieldset>
            </div>
        </div>
          </div>
            <div class="row">              
                <div class="col-md-12 text-right"> 
                    <asp:Button ID="ButtonCreateProject" runat="server" Text="Create" CssClass="btn btnCreate" OnClick="ButtonCreateProject_Click" />
                    <asp:Button ID="ButtonCancelProject" runat="server" Text="Cancel" CssClass="btn btnCancel" OnClick="ButtonCancelProject_Click" />
                </div>
            </div>
    </div>

    <asp:ObjectDataSource ID="CategorylistDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="CategoryList"
        TypeName="TACOSystem.BLL.CategoryController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="EmployeeDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="EmployeeLists" TypeName="TACOSystem.BLL.ProjectController"></asp:ObjectDataSource>

    <script src="/Scripts/bootwrap-freecode.js"></script>
    
    <style>.namebox {white-space:nowrap;
  overflow:hidden; padding: 2rem; color:black; font-weight: bold;
  text-overflow:ellipsis;}</style>

    <style>.nameboxfield {white-space:nowrap;
  overflow:hidden; padding: 1rem; color:black;
  text-overflow:ellipsis;}</style> 

    <script>

        $('#saveEmployee').click(function () {
            console.log('you are inside');
            var employeeIdSelected = [];
            $('table td').each(function (index) {
                var id = this.id;

                if ($(this).children().is(':checkbox')) {
                    if ($(this).children().prop('checked') == true) {
                        var value = $(this).next();                        
                        console.log(value[0].innerText);
                        employeeIdSelected.push(parseInt(value[0].innerText))
                        console.log('done'); 
                        $('#ModalAddEmployee').modal('hide');
                    }
                }
            });
            
            var jsonData = JSON.stringify(employeeIdSelected)
            console.log(jsonData);
             $.ajax(
            {
                url: "CreateProject.aspx/getEmployeeIdFromFront",
                type: 'POST',
                     traditional: true,
                     data: JSON.stringify({ eventsJson: jsonData }
                ),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                },
                error: function (xhr) {
                    alert(JSON.stringify(xhr));
                }

            });
        });

    </script>
</asp:Content>

