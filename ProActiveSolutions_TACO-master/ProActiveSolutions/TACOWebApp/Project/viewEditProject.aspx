<%@ Page Title="View/Edit Project" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewEditProject.aspx.cs" Inherits="TACOWebApp.Project.viewEditProject" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        
    <link href="/Content/bootwrap-freecode.css" rel="stylesheet" />
    
    <div id='all_content' runat="server">

    <div class="row col-md-12 info">
    <h1>Edit / View Project</h1>
        <p class="annotation"><span class="required">*</span> are required fields.</p>
    </div>

    <div class="row">
        <div class="col-md-12" style="margin-top:20px;">
            <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
        </div>        
    </div>
    <div class="container-two">
            
    <div class="col-md-5" runat="server" id="CheckboxExpired">
           
                <asp:CheckBox ID="CheckboxExpireds" runat="server"  Text="Show expired Projects" AutoPostBack="true" OnCheckedChanged="CheckboxExpired_CheckedChanged" />
              <br />
                <br />
        </div>

    <div class="row col-md-12 ">
        <div class="col-md-12">



        <asp:ListView  ID="ListProject" runat="server" OnItemCommand="ProjectDetails" DataSourceID="ProjectDataSource" ItemType="TACOData.Entities.POCOs.ProjectInformation" DataKeyNames="ProjectID">        
        <EmptyDataTemplate>
            <table runat="server">
                <tr>
                    <td><h2>None of the Project is available.</h2></td>
                </tr>
            </table>
        </EmptyDataTemplate>
     
        <ItemTemplate>
            <tr style="background-color: #E4F6FC; color: #333333;" >
                <td style="text-align:center;">
                     <asp:HiddenField Value='<%# Eval("ProjectID") %>' runat="server" ID="LableProjectId" />
                    <asp:Label CssClass="namebox" Text='<%# Eval("ProjectName") %>' runat="server" ID="LabelProjectName" /></td>
                <td style="text-align:center;">
                    <asp:Label CssClass="nameboxfield" Text='<%# Eval("StartDate", "{0:dddd, MMMM dd, yyyy}") %>' runat="server" ID="LableStartDate" /></td>
                <td style="text-align:center;">
                    <asp:Label CssClass="nameboxfield" Text='<%# Eval("EndDate", "{0:dddd, MMMM dd, yyyy}") %>' runat="server" ID="LabelEndDate" /></td>
                <td style="text-align:center;">
                    <asp:Label CssClass="nameboxfield" Text='<%# Eval("CategoryName") %>' runat="server" ID="LabelCategory" /></td> 
                <td style="text-align:center;">  
                    <asp:LinkButton  class="btn btnCreate" ID="ButtonViewProject" CommandName="ProjectDetails" CommandArgument="<%# Item.ProjectId %>" runat="server">View Project</asp:LinkButton> </td>
                <td style="text-align:center;">
                    <asp:LinkButton  class="btn btnCreate" ID="ButtonViewAllocation" CommandName="AllocationDetails" CommandArgument="<%# Item.ProjectId %>" runat="server">Allocation</asp:LinkButton> </td>     
           </tr>
        </ItemTemplate>

        <LayoutTemplate>
            <table runat="server" style="background-color: #FFFFFF; margin-bottom:0; width:100%">
                <tr runat="server">
                    <td runat="server">
                        <table  class="table table-responsive table-bordered" runat="server" id="itemPlaceholderContainer" style="margin:0;" >
                            <tr runat="server" style="background-color: #98C0CE; color: #333333;">
                                
                                <th style="text-align:center;" runat="server">Project Name</th>
                                <th style="text-align:center;" runat="server">Start Date</th>
                                <th style="text-align:center;" runat="server">End Date</th> 
                                <th style="text-align:center;" runat="server">Category</th>
                                <th style="text-align:center;" runat="server">View Project</th>     
                                <th style="text-align:center;" runat="server">Allocation</th> 
                            </tr>
                            <tr runat="server" id="itemPlaceholder"></tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" style="text-align: center; background-color: #98C0CE; font-family: Verdana, Arial, Helvetica, sans-serif; padding:5px; color: #333333">
                        <asp:DataPager runat="server" ID="DataPager1">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                <asp:NumericPagerField></asp:NumericPagerField>
                                <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                            </Fields>
                        </asp:DataPager>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>   
        </asp:ListView>
        </div>
                
</div>


    <div runat="server" id="ExpiredProjects" visible="false">
        <asp:ListView  ID="ListView2" runat="server" OnItemCommand="ProjectDetails" DataSourceID="ExpiredProjectDataSource" ItemType="TACOData.Entities.POCOs.ProjectInformation" DataKeyNames="ProjectID">        
        <EmptyDataTemplate>
            <table runat="server" style="width:100%">
                <tr>
                    <td><h2 style="font-weight:bold; text-align:center;">None of the projects are available.</h2></td>
                </tr>
          </table>
        </EmptyDataTemplate>
     
        <ItemTemplate>
            <tr style="background-color: #E4F6FC; color: #333333;" >
                <td style="text-align:center;">
                    <asp:HiddenField Value='<%# Eval("ProjectID") %>' runat="server" ID="LableProjectId" />
                    <asp:Label CssClass="namebox" Text='<%# Eval("ProjectName") %>' runat="server" ID="LabelProjectName" /></td>
                <td style="text-align:center;">
                    <asp:Label CssClass="nameboxfield" Text='<%# Eval("StartDate", "{0:dddd, MMMM dd, yyyy}") %>' runat="server" ID="LableStartDate" /></td>
                <td style="text-align:center;">
                    <asp:Label CssClass="nameboxfield" Text='<%# Eval("EndDate", "{0:dddd, MMMM dd, yyyy}") %>' runat="server" ID="LabelEndDate" /></td>
                <td style="text-align:center;">
                    <asp:Label CssClass="nameboxfield" Text='<%# Eval("CategoryName") %>' runat="server" ID="LabelCategory" /></td> 
                <td style="text-align:center;">  
                    <asp:LinkButton class="btn btnCreate" ID="ButtonViewProject" CommandName="ProjectDetails" CommandArgument="<%# Item.ProjectId %>" runat="server">View Project</asp:LinkButton></td>                   
           </tr>
        </ItemTemplate>

        <LayoutTemplate>
            <table runat="server" style="background-color: #FFFFFF; margin-bottom:0; width:100%">
                <tr runat="server">
                    <td runat="server">
                        <table  class="table table-responsive table-bordered" runat="server" id="itemPlaceholderContainer" style="margin-bottom:0;" >
                            <tr runat="server" style="background-color: #98C0CE; color: #333333;">
                                
                                <th style="text-align:center;" runat="server">Project Name</th>
                                <th style="text-align:center;" runat="server">Start Date</th>
                                <th style="text-align:center;" runat="server">End Date</th> 
                                <th style="text-align:center;" runat="server">Category</th>
                                <th style="text-align:center;" runat="server">View Project</th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder"></tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" style="text-align:center; background-color: #98C0CE; font-family: Verdana, Arial, Helvetica, sans-serif; padding:5px; margin-bottom:0; color: #333333; ">
                        <asp:DataPager runat="server" ID="DataPager1">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                <asp:NumericPagerField></asp:NumericPagerField>
                                <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                            </Fields>
                        </asp:DataPager>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>   
        </asp:ListView>
    </div>

    
    <div runat="server" id="FormEdit">
    <div class="col-md-12">

    <div class="col-md-6">
        <fieldset>
                <br />
               <asp:Label ID="LabelProjectId" runat="server" Visible="False" AssociatedControlID="TextBoxProjectId"> Project Id</asp:Label>
               <asp:TextBox ID="TextBoxProjectId" runat="server" Visible="False"></asp:TextBox>     
                                 
                <asp:Label ID="LabelProjectName" runat="server" AssociatedControlID="TextBoxProjectName"> Project Name <span class="required">*</span></asp:Label>
                <asp:TextBox ID="TextBoxProjectName" runat="server" Placeholder ="Tech SOlution"></asp:TextBox>

                <asp:Label ID="LabelStartDate" runat="server" AssociatedControlID="TextBoxStartDate"> Start Date <span class="required">*</span></asp:Label>
                <asp:TextBox ID="TextBoxStartDate" runat="server" placeholder="Choose a Date"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="ButtonStartDate" PopupPosition="TopRight"
                        TargetControlID="TextBoxStartDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>
                                
                <asp:Label ID="LabelEndDate" runat="server" AssociatedControlID="TextBoxEndDate">End Date</asp:Label>
                <asp:TextBox ID="TextBoxEndDate" runat="server" placeholder="Choose a Date"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="ButtonEndDate" PopupPosition="TopRight"
                        TargetControlID="TextBoxEndDate" Format="dddd, MMMM dd, yyyy"></ajaxToolkit:CalendarExtender>
                
                <asp:Label ID="LabelLookupCategory" runat="server" AssociatedControlID="DropDownCategory" >Category <span class="required">*</span></asp:Label>
                <asp:DropDownList ID="DropDownCategory" runat="server" DataTextField="CategoryName" DataSourceID="CategorylistDataSource" DataValueField="CategoryId" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Choose a Category</asp:ListItem>
                </asp:DropDownList>
            

                   <asp:Label ID="LabelEmployees" runat="server" AssociatedControlID="selectEmployees">Employees</asp:Label>
                                <button id="selectEmployees" runat="server" type="button" class="btn btn-primary" data-toggle="modal" data-target="#ModalAddEmployee">
                       Select Employees</button>

                    <asp:LinkButton runat="server" ID="ExistingEmployees" Text="Remove Employees" class="btn btn-primary" OnClick="ExistingEmployees_Click"></asp:LinkButton>
                   <br />
                   <br />

                   <div class="modal fade" id="ModalAddEmployee" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                       <div class="modal-dialog modal-dialog-centered" role="document">
                           <div class="modal-content">
                               <div class="modal-header">
                                   <h2 class="modal-title" id="exampleModalLabel">Add Employees</h2>
                                   <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                       <span aria-hidden="true">&times;</span>
                                   </button>
                               </div>
                               <div class="modal-body">
                                   <form>

                                       <asp:ListView ID="ListView1" runat="server" DataSourceID="NotInProjectEmployee" ItemType="TACOData.Entities.POCOs.ProjectInformation" DataKeyNames="ProjectID">

                                           <ItemTemplate>
                                               <tr style="background-color: #E4F6FC; color: #333333;" >
                                                   <td style="text-align:center;">
                                                       <asp:CheckBox ID="CheckBoxSelectEmployee" runat="server"></asp:CheckBox></td>
                                                   <td style="text-align:center;">
                                                       <asp:Label CssClass="namebox" Text='<%# Eval("EmployeeId") %>' runat="server" ID="LableEmployeeId" /></td>
                                                   <td style="text-align:center;">
                                                       <asp:Label CssClass="namebox" Text='<%# Eval("FullName") %>' runat="server" ID="LabelFullName" /></td>
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
                <asp:TextBox ID="TextBoxProjectDescription" runat="server" Height="120px" TextMode="MultiLine" Wrap="true"></asp:TextBox>
                
                <asp:Label ID="LabelPriority" runat="server" AssociatedControlID="RadioButtonListPriority">Priority <span class="required">*</span></asp:Label>
                <asp:RadioButtonList ID="RadioButtonListPriority" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" OnSelectedIndexChanged="RadioButtonListPriority_SelectedIndexChanged">
                    <asp:ListItem Text="Low" Value="Low"></asp:ListItem>
                    <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>
                    <asp:ListItem Text="High" Value="High"></asp:ListItem>
                </asp:RadioButtonList>


            <asp:Label ID="LabelColorPicker" runat="server" AssociatedControlID="TextBoxPickerColor">Color</asp:Label>
                    <asp:TextBox ID="TextBoxPickerColor" runat="server"></asp:TextBox>
                    <ajaxToolkit:ColorPickerExtender ID="ColorPickerExtender1" runat="server" PopupButtonID="ColorPickerbutton" PopupPosition="TopRight" TargetControlID="TextBoxPickerColor" />
          
             
            </fieldset>    
        <div class="row">          
              <div class="col-md-12 text-right" style="margin-top:3rem;">
                <asp:Button ID="ButtonActivateProject" runat="server" Text="Activate" Visible="false" CssClass="btn btnCreate" OnClick="ButtonActivateProject_Click" />
                <asp:Button ID="ButtonUpdateProject" runat="server"  Text="Update" CssClass="btn btnCreate" OnClick="ButtonUpdateProject_Click" />
                <asp:Button ID="ButtonCancelProject" runat="server"  Text="Cancel" CssClass="btn btnTerminate" OnClick="ButtonCancelProject_Click" />
                <asp:Button ID="ButtonTerminateProject" runat="server" Text="Terminate" CssClass="btn btnTerminate" OnClick="ButtonTerminateProject_Click" />                
              </div>
        </div>
        </div> 
        </div>            
       </div>
    <div runat="server" id="EmptyEMployee">
          <table runat="server" style="width:100%">
                <tr>
                    <td><h2 style="font-weight:bold; text-align:center">None of the Employee(s) are assigned to this project.</h2></td>
                </tr>
          </table>
     </div>

    <div runat="server" id="Existingemployee" visible="false">
        
        <h2>Remove Employee</h2>

        <asp:ListView ID="ListExistingEmployees" runat="server" DataSourceID="ExistingEmployeeDataSource" ItemType="TACOData.Entities.POCOs.ProjectInformation" DataKeyNames="ProjectID">
            <EmptyDataTemplate>
            <table runat="server" style="width:100%">
                <tr>
                    <td><h2 style="font-weight:bold; text-align:center">None of the Employee(s) are assigned to this project.</h2></td>
                </tr>
          </table>
        </EmptyDataTemplate>

                        <ItemTemplate>
                            <tr style="background-color: #E4F6FC; color: #333333;" >
                                <td style="text-align:center;">
                                    <asp:Checkbox CssClass="namebox" ID="CheckBoxSelectEmployee" runat="server"></asp:CheckBox></td>                                
                                <td style="text-align:center;">
                                    <asp:Label CssClass="namebox" Text='<%# Eval("FullName") %>' runat="server" ID="LabelFullName" /></td>                               
                                <td style="text-align:center;">
                                    <asp:Label CssClass="namebox" Text='<%# Eval("ProjectName") %>' runat="server" ID="LabelProjectName" />

                                    <asp:HiddenField Value='<%# Eval("ProjectId") %>' runat="server" ID="LabelProjectId" />
                                    <asp:HiddenField Value='<%# Eval("EmployeeId") %>' runat="server" ID="LableEmployeeId" />                                
                                    <asp:HiddenField Value='<%# Eval("ProjectTeamId") %>' runat="server" ID="LableProjectTeamId" /></td>
                            </tr>
                         </ItemTemplate>
                        <LayoutTemplate>
                             <table runat="server" style=" background-color: #FFFFFF; width:100%; ">
                                        <tr runat="server">
                                        <td runat="server">
                                          <table  class="table table-responsive table-bordered" runat="server" id="itemPlaceholderContainer" style="margin-bottom:0;" >
                                             <tr runat="server" style="background-color: #98C0CE; color: #333333;">
                                                <th style="text-align:center;" runat="server">Select to remove</th>                                                
                                                <th style="text-align:center;" runat="server">Employee Name</th>
                                                <th style="text-align:center;" runat="server">Project Name</th>
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

        <div class="col-md-12 text-right">
            <br />
            <asp:Button ID="ButtonSaveRemoveEmployee" runat="server"  Text="Save" CssClass="btn btnCreate" OnClick="ButtonSaveRemoveEmployee_Click"  />
            <asp:Button ID="ButtonCancelRemoveEmployee" runat="server"  Text="Cancel" CssClass="btn btnCancel" OnClick="ButtonCancelRemoveEmployee_Click"  />
        </div>
    </div>

        <div runat="server" id="AllocationForm">
           <div class="col-md-12">
                
           <asp:Label runat="server" ID="SideProjectId"></asp:Label>
  <fieldset>
   <div class="row">
       <asp:Label ID="Label1" runat="server" Visible="False" AssociatedControlID="TextBox1"> Project Id</asp:Label>
          <asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox>       
       
        <div class="form-group form-inline">
            <p style="color: red;">
            &nbsp;&nbsp;&nbsp;&nbsp;
             <asp:Label ID="LabelProName" CssClass="labelbox" runat="server" > Project: </asp:Label>
             <asp:Label ID="ProName" CssClass="labelbox" runat="server" text="Project"></asp:Label>  
       &nbsp;&nbsp;&nbsp;&nbsp;
       <asp:DropDownList ID="DropDownYear" runat="server" class="form-control" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="DropDownYear_SelectedIndexChanged"></asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;
            Remember to save before changing year!</p>
        </div>
      

       <table class="table-bordered" style="background-color: #E4F6FC; width:100%; padding:0 3rem;">
       <tr class="headerRow" style="background-color: #98C0CE; color: #000; font-weight: bold; text-align: center;">
          <td> &nbsp;</td>
         <td></td>
                <td class="header" style="text-align:center; padding:1rem"> <asp:Label runat="server" ID="MM1"  AssociatedControlID="MM1">January</asp:Label> </td>
                <td class="header" style="text-align:center"> <asp:Label runat="server" ID="MM2"  AssociatedControlID="MM2">February</asp:Label> </td>
                <td class="header" style="text-align:center"> <asp:Label runat="server" ID="MM3"  AssociatedControlID="MM3">March</asp:Label> </td>
                <td class="header" style="text-align:center"> <asp:Label runat="server" ID="MM4"  AssociatedControlID="MM4">April</asp:Label> </td>
                <td class="header" style="text-align:center"> <asp:Label runat="server" ID="MM5"  AssociatedControlID="MM5">May</asp:Label> </td>
                <td class="header" style="text-align:center"> <asp:Label runat="server" ID="MM6"  AssociatedControlID="MM6">June</asp:Label> </td>
                <td class="header" style="text-align:center"> <asp:Label runat="server" ID="MM7"  AssociatedControlID="MM7">July</asp:Label> </td>
                <td class="header" style="text-align:center"> <asp:Label runat="server" ID="MM8"  AssociatedControlID="MM8">August</asp:Label> </td>
                <td class="header" style="text-align:center"> <asp:Label runat="server" ID="MM9"  AssociatedControlID="MM9">September</asp:Label> </td>
                <td class="header" style="text-align:center"> <asp:Label runat="server" ID="MM10" AssociatedControlID="MM10">October</asp:Label> </td>
                <td class="header" style="text-align:center"> <asp:Label runat="server" ID="MM11" AssociatedControlID="MM11" >November</asp:Label> </td>
                <td class="header" style="text-align:center"> <asp:Label runat="server" ID="MM12" AssociatedControlID="MM12" >December</asp:Label> </td>
       </tr>
        <asp:Repeater ID="RepeaterEmployees" runat="server">
          <ItemTemplate>
               <tr>
                <td >
                    <asp:HiddenField ID="HiddenFieldEmployeeId" runat="server" Value='<%# Bind("EmployeeId") %>'></asp:HiddenField>                
                    <asp:Label CssClass="namebox" ID="LabelEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                </td>
                <td>
                   <asp:HiddenField ID="ProjectId" runat="server" Value='<%# Bind("ProjectId") %>'></asp:HiddenField>
                   <asp:HiddenField ID="LProjectTeamId" runat="server" Value='<%# Bind("ProjectTeamId") %>'></asp:HiddenField>
                </td>
                 
                 <td style="text-align:center" class="sup"><asp:TextBox ID="M1" style="border:0.5px solid #ccc; border-radius:2px; text-align:center" CssClass="contentTextBox" Text='<%# Bind("January") %>' Textmode="Number" min="0" max="31" runat="server"></asp:TextBox></td>
                 <td style="text-align:center" class="sup"><asp:TextBox ID="M2" style="border:0.5px solid #ccc; border-radius:2px; text-align:center" CssClass="contentTextBox" Text='<%# Bind("February") %>' Textmode="Number" min="0" max="29" runat="server"></asp:TextBox></td>
                 <td style="text-align:center" class="sup"><asp:TextBox ID="M3" style="border:0.5px solid #ccc; border-radius:2px; text-align:center" CssClass="contentTextBox" Text='<%# Bind("March") %>' Textmode="Number" min="0"  max="31" runat="server"></asp:TextBox></td>
                 <td style="text-align:center" class="sup"><asp:TextBox ID="M4" style="border:0.5px solid #ccc; border-radius:2px; text-align:center" CssClass="contentTextBox" Text='<%# Bind("April") %>' Textmode="Number" min="0"  max="30" runat="server"></asp:TextBox></td>
                 <td style="text-align:center" class="sup"><asp:TextBox ID="M5" style="border:0.5px solid #ccc; border-radius:2px; text-align:center" CssClass="contentTextBox" Text='<%# Bind("May") %>' Textmode="Number" min="0" max="31" runat="server"></asp:TextBox></td>
                 <td style="text-align:center" class="sup"><asp:TextBox ID="M6" style="border:0.5px solid #ccc; border-radius:2px; text-align:center" CssClass="contentTextBox" Text='<%# Bind("June") %>' Textmode="Number" min="0" max="30" runat="server"></asp:TextBox></td>
                 <td style="text-align:center" class="sup"><asp:TextBox ID="M7" style="border:0.5px solid #ccc; border-radius:2px; text-align:center" CssClass="contentTextBox" Text='<%# Bind("July") %>' Textmode="Number" min="0" max="31" runat="server"></asp:TextBox></td>
                 <td style="text-align:center" class="sup"><asp:TextBox ID="M8" style="border:0.5px solid #ccc; border-radius:2px; text-align:center" CssClass="contentTextBox" Text='<%# Bind("August") %>' Textmode="Number" min="0" max="31" runat="server"></asp:TextBox></td>
                 <td style="text-align:center" class="sup"><asp:TextBox ID="M9" style="border:0.5px solid #ccc; border-radius:2px; text-align:center" CssClass="contentTextBox" Text='<%# Bind("September") %>' Textmode="Number" min="0"  max="30" runat="server"></asp:TextBox></td>
                 <td style="text-align:center" class="sup"><asp:TextBox ID="M10" style="border:0.5px solid #ccc; border-radius:2px; text-align:center" CssClass="contentTextBox" Text='<%# Bind("October") %>' Textmode="Number" min="0"  max="31" runat="server"></asp:TextBox></td>
                 <td style="text-align:center" class="sup"><asp:TextBox ID="M11" style="border:0.5px solid #ccc; border-radius:2px; text-align:center" CssClass="contentTextBox" Text='<%# Bind("November") %>' Textmode="Number" min="0"  max="30" runat="server"></asp:TextBox></td>
                 <td style="text-align:center" class="sup"><asp:TextBox ID="M12" style="border:0.5px solid #ccc; border-radius:2px; text-align:center" CssClass="contentTextBox" Text='<%# Bind("December") %>' Textmode="Number" min="0"  max="31" runat="server"></asp:TextBox></td>    
             </tr>
          </ItemTemplate>
         </asp:Repeater>
       </table>
   </div> 
  </fieldset>
         </div>
            <div class="row">
      <div class="col-md-12 text-right">
          <br />
        <asp:Button ID="ButtonSaveAllocation" runat="server"  Text="Save" CssClass="btn btnCreate" OnClick="ButtonSaveAllocation_Click" />
        <asp:Button ID="ButtonCancelAllocation" runat="server"  Text="Cancel" CssClass="btn btnCancel" OnClick="ButtonCancelAllocation_Click" />
      </div>
  </div>
</div>
        </div>
    
    <script src="/Scripts/bootwrap-freecode.js"></script>

    <asp:ObjectDataSource ID="ProjectDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ProjectLists" TypeName="TACOSystem.BLL.ProjectController"></asp:ObjectDataSource>

    <asp:ObjectDataSource ID="ExpiredProjectDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ProjectListsExpired" TypeName="TACOSystem.BLL.ProjectController"></asp:ObjectDataSource>

    <asp:ObjectDataSource ID="CategorylistDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="CategoryList"
        TypeName="TACOSystem.BLL.CategoryController"></asp:ObjectDataSource>

    <asp:ObjectDataSource ID="NotInProjectEmployee" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="NotInProjectEmployeeLists" TypeName="TACOSystem.BLL.ProjectController">
        <SelectParameters>
            <asp:ControlParameter ControlID="TextBoxProjectId" DefaultValue="1" Name="ProjectId" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="ExistingEmployeeDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ExistingEmployeeLists" TypeName="TACOSystem.BLL.ProjectController">
        <SelectParameters>
            <asp:ControlParameter ControlID="TextBoxProjectId" DefaultValue="1" Name="ProjectId" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>

        </div>

    <style>.contentTextBox { height:25px; width: 50px;}</style> 
    
    <style>.namebox {white-space:nowrap;
  overflow:hidden; padding: 2rem; color:black; font-weight: bold;
  text-overflow:ellipsis;}</style> 

    <style>.labelbox {white-space:nowrap;
  overflow:hidden; color:black; font-weight: bold;
  text-overflow:ellipsis;}</style>


    <style>.nameboxfield {white-space:nowrap;
  overflow:hidden; padding: 1rem; color:black;
  text-overflow:ellipsis;}</style> 

<style>.sup { padding: 1rem;}</style> 

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
                url: "ViewEditProject.aspx/getEmployeeIdFromFront",
                type: 'POST',
                     //traditional: true,
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
