using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOSystem.BLL;
using TACOSystem.BLL.Security;

namespace TACOWebApp.Project
{
    public partial class viewEditProject : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();
            if (Request.IsAuthenticated && (User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName && (SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin") || SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "TeamLead") || SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "TeamAdmin")
            {
                //you can create a generic lable, to have the role you are currently in displayed for you
                //but in production it should be empty if you are not planing displain the role or 
                //employeeName/ID
                //Label1.Text = "You're the Boss " + User.Identity.Name.Split('\\').Last() + " your role is: " + SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
            }
            else
            {
                all_content.Visible = false;
            }
            
            FormEdit.Visible = false;
            AllocationForm.Visible = false;
            EmptyEMployee.Visible = false;
            SideProjectId.Visible = false;
            TextBoxStartDate.Enabled = false;
        }

        protected void RadioButtonListPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Session["Priority"] = RadioButtonListPriority.SelectedValue.ToString();
        }

        [WebMethod]
        public static void getEmployeeIdFromFront(string eventsJson)
        {
            ProjectInformation updatedProjectinfo = new ProjectInformation();
            ProjectController controller = new ProjectController();

            List<int> list = new List<int>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();

            List<int> ids = json_serializer.Deserialize<List<int>>(eventsJson);

            controller.UpdateProject(updatedProjectinfo, ids);

        }

        protected void ProjectDetails(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ProjectDetails":

                    int ProjectId = int.Parse(e.CommandArgument.ToString());

                    Session["Data"] = ProjectId;

                    ListProject.Visible = false;
                    FormEdit.Visible = true;
                    AllocationForm.Visible = false;
                    TextBoxStartDate.Enabled = false;
                    var controller = new ProjectController();
                    var projectinfo = controller.GetProjectInformation(ProjectId);

                    ProjectController ProjectIdForEmployee = new ProjectController();
                    var abc = controller.NotInProjectEmployeeLists(ProjectId);

                    if (projectinfo.EndDate < DateTime.Now)
                    {
                        TextBoxProjectId.Text = projectinfo.ProjectId.ToString();
                        TextBoxProjectName.Text = projectinfo.ProjectName;
                        TextBoxProjectDescription.Text = projectinfo.ProjectDescription;
                        TextBoxStartDate.Text = projectinfo.StartDate.ToLongDateString();
                        TextBoxEndDate.Text = projectinfo.EndDate.ToLongDateString();
                        // DropDownCategory.SelectedIndex = projectinfo.CategoryId;
                        RadioButtonListPriority.SelectedValue = projectinfo.Priority;
                        TextBoxPickerColor.Text = projectinfo.Color;
                        CheckboxExpired.Visible = false;
                        selectEmployees.Visible = false;
                        TextBoxProjectName.Enabled = false;
                        TextBoxProjectDescription.Enabled = false;
                        TextBoxStartDate.Enabled = false;
                        TextBoxEndDate.Enabled = false;
                        RadioButtonListPriority.Enabled = false;
                        TextBoxPickerColor.Enabled = false;
                        DropDownCategory.Enabled = false;
                        ExistingEmployees.Visible = false;
                        LabelEmployees.Visible = false;
                        ButtonTerminateProject.Visible = false;
                        ButtonUpdateProject.Visible = false;
                        ButtonActivateProject.Visible = true;
                        ExpiredProjects.Visible = false;

                    }

                    else
                    {
                        TextBoxProjectId.Text = projectinfo.ProjectId.ToString();
                        TextBoxProjectName.Text = projectinfo.ProjectName;
                        TextBoxProjectDescription.Text = projectinfo.ProjectDescription;
                        TextBoxStartDate.Text = projectinfo.StartDate.ToLongDateString();
                        TextBoxEndDate.Text = projectinfo.EndDate.ToLongDateString();
                        // DropDownCategory.SelectedIndex = projectinfo.CategoryId;
                        RadioButtonListPriority.SelectedValue = projectinfo.Priority;
                        TextBoxPickerColor.Text = projectinfo.Color;
                        CheckboxExpired.Visible = false;
                    }

                    break;

                case "AllocationDetails":

                    int ProId = int.Parse(e.CommandArgument.ToString());
                    Session["Data"] = ProId;
                    var controllerName = new ProjectController();
                    var ProjectNames = controllerName.GetProjectInformation(ProId);
                    
                    ProjectController pController = new ProjectController();
                    var list = pController.assignedEmployeesList(ProId, ProjectNames.StartDate.Year);

                    SideProjectId.Text = ProId.ToString();

                    ProName.Text = ProjectNames.ProjectName;
                    CheckboxExpired.Visible = false;
                    RepeaterEmployees.DataSource = list;
                    if (list.Count <= 0)
                    {
                        AllocationForm.Visible = false;
                        EmptyEMployee.Visible = true;
                        ListProject.Visible = false;
                        FormEdit.Visible = false;
                    }

                    else
                    {
                        RepeaterEmployees.DataBind();
                        {
                            int StartYear = ProjectNames.StartDate.Year;
                            int yearIncreament = ProjectNames.EndDate.Year - ProjectNames.StartDate.Year;

                            for (int i = StartYear; i <= StartYear + yearIncreament; i++)
                            {
                                ListItem listYear = new ListItem(i.ToString());
                                DropDownYear.Items.Add(listYear);
                            }
                        }
                        AllocationForm.Visible = true;
                        ListProject.Visible = false;
                        FormEdit.Visible = false;
                        MessageUserControl.Visible = false;
                    }
                    break;
            }
        }
        
        protected void ButtonUpdateProject_Click(object sender, EventArgs e)
        {
            ListProject.Visible = false;
            FormEdit.Visible = true;
            AllocationForm.Visible = false;

            TextBoxStartDate.Enabled = false;
            MessageUserControl.TryRun(() =>
            {
                ProjectInformation updatedProjectinfo = new ProjectInformation();
                if (TextBoxProjectName.Text == null)
                {
                    throw new Exception(" project name cannot be empty.");
                }

                else if (TextBoxProjectDescription.Text == null)
                {
                    throw new Exception(" project description cannot be empty.");
                }

                else if (TextBoxProjectDescription.Text.Length > 250)
                {
                    throw new Exception(" project description cannot be longer than 250 words.");
                }

                else
                {
                    updatedProjectinfo.ProjectId = int.Parse(TextBoxProjectId.Text);
                    updatedProjectinfo.ProjectName = TextBoxProjectName.Text;
                    updatedProjectinfo.ProjectDescription = TextBoxProjectDescription.Text;
                    updatedProjectinfo.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                    updatedProjectinfo.EndDate = Convert.ToDateTime(TextBoxEndDate.Text);
                    updatedProjectinfo.Priority = RadioButtonListPriority.SelectedValue.ToString();
                    //updatedProjectinfo.CategoryId = DropDownCategory.SelectedIndex;
                    updatedProjectinfo.ProjectColor = '#' + TextBoxPickerColor.Text;
                }
                var controller = new ProjectController();
                controller.UpdateProject(updatedProjectinfo);

            }, "Success", "Project has been updated.");
        }
        
        protected void ButtonSaveAllocation_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                List<ProjectTeamInformation> AllocatedDaysList = new List<ProjectTeamInformation>();
                ProjectTeamInformation AllocatedDays = new ProjectTeamInformation();

                AllocatedDays.AllocatedYear = int.Parse(DropDownYear.SelectedValue);

                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                AllocatedDays.CreatedBy = SecurityController.FindAccount(windowsAccountName).AccountID;

                foreach (RepeaterItem item in RepeaterEmployees.Items)
                {
                    var ProjectTeamId = item.FindControl("LProjectTeamId") as HiddenField;
                    var findM1 = item.FindControl("M1") as TextBox;
                    var findM2 = item.FindControl("M2") as TextBox;
                    var findM3 = item.FindControl("M3") as TextBox;
                    var findM4 = item.FindControl("M4") as TextBox;
                    var findM5 = item.FindControl("M5") as TextBox;
                    var findM6 = item.FindControl("M6") as TextBox;
                    var findM7 = item.FindControl("M7") as TextBox;
                    var findM8 = item.FindControl("M8") as TextBox;
                    var findM9 = item.FindControl("M9") as TextBox;
                    var findM10 = item.FindControl("M10") as TextBox;
                    var findM11 = item.FindControl("M11") as TextBox;
                    var findM12 = item.FindControl("M12") as TextBox;

                    AllocatedDays.ProjectTeamId = int.Parse(ProjectTeamId.Value);
                    AllocatedDays.January = int.Parse(findM1.Text);
                    AllocatedDays.February = int.Parse(findM2.Text);
                    AllocatedDays.March = int.Parse(findM3.Text);
                    AllocatedDays.April = int.Parse(findM4.Text);
                    AllocatedDays.May = int.Parse(findM5.Text);
                    AllocatedDays.June = int.Parse(findM6.Text);
                    AllocatedDays.July = int.Parse(findM7.Text);
                    AllocatedDays.August = int.Parse(findM8.Text);
                    AllocatedDays.September = int.Parse(findM9.Text);
                    AllocatedDays.October = int.Parse(findM10.Text);
                    AllocatedDays.November = int.Parse(findM11.Text);
                    AllocatedDays.December = int.Parse(findM12.Text);

                    AllocatedDaysList.Add(AllocatedDays);
                    var controller = new ProjectController();

                    controller.SaveAllocation(AllocatedDaysList);

                    AllocationForm.Visible = true;
                    ListProject.Visible = false;
                    FormEdit.Visible = false;
                    MessageUserControl.Visible = true;
                }

            }, "Success", "Allocation has been updated.");
        }
        
        protected void ButtonCancelAllocation_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void ButtonCancelProject_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void DropDownYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            AllocationForm.Visible = true;
            ListProject.Visible = false;
            FormEdit.Visible = false;
            MessageUserControl.Visible = false;
            
            int year = int.Parse(DropDownYear.SelectedValue);
            var controller = new ProjectController();
            var ProjectYear = controller.assignedEmployeesList(int.Parse(DropDownYear.SelectedValue), year);
            
            var controllerName = new ProjectController();
            var ProjectNames = controllerName.GetProjectInformation(int.Parse(SideProjectId.Text));

            ProjectController pController = new ProjectController();
            var list = pController.assignedEmployeesList(int.Parse(SideProjectId.Text), year);
            
            ProName.Text = ProjectNames.ProjectName;

            RepeaterEmployees.DataSource = list;
            if (list.Count <= 0)
            {
                AllocationForm.Visible = false;
                EmptyEMployee.Visible = true;
                ListProject.Visible = false;
                FormEdit.Visible = false;
            }

            else
            {
                RepeaterEmployees.DataBind();
            }
        }

        protected void ExistingEmployees_Click(object sender, EventArgs e)
        {
            Existingemployee.Visible = true;

            var existingemployee = new ProjectController();
            var ProjectNames = existingemployee.ExistingEmployeeLists(int.Parse(TextBoxProjectId.Text));

        }

        protected void ButtonSaveRemoveEmployee_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                List<ProjectTeamInformation> AssignedEmployeesList = new List<ProjectTeamInformation>();
                ProjectTeamInformation AssignedEmployees = new ProjectTeamInformation();
                
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                AssignedEmployees.ModifiedBy = SecurityController.FindAccount(windowsAccountName).AccountID;

                foreach (ListViewDataItem item in ListExistingEmployees.Items)
                {
                    if ((item.FindControl("CheckBoxSelectEmployee") as CheckBox).Checked == true)
                    {
                        var ProjectTeamId = item.FindControl("LableProjectTeamId") as HiddenField;
                        
                        AssignedEmployees.ProjectTeamId = int.Parse(ProjectTeamId.Value);

                        AssignedEmployeesList.Add(AssignedEmployees);

                        var controller = new ProjectController();
                        controller.UpdateAssignedEmployeeList(AssignedEmployeesList);

                        ListProject.Visible = false;
                        FormEdit.Visible = true;
                        AllocationForm.Visible = false;
                        Existingemployee.Visible = false;
                    }
                }

            }, "Success", "Employee List has been updated.");
        }

        protected void ButtonCancelRemoveEmployee_Click(object sender, EventArgs e)
        {
            ListProject.Visible = false;
            FormEdit.Visible = true;
            AllocationForm.Visible = false;
            Existingemployee.Visible = false;
        }

        protected void ButtonTerminateProject_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                ProjectInformation DeletedProjectInfo = new ProjectInformation();
                if (TextBoxProjectId.Text == null)
                {
                    throw new Exception("Please select a Project to terminate");
                }
                else
                {
                    DeletedProjectInfo.ProjectId = int.Parse(TextBoxProjectId.Text);
                    DeletedProjectInfo.EndDate = DateTime.Today;

                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    DeletedProjectInfo.ModifiedBy = SecurityController.FindAccount(windowsAccountName).AccountID;

                }

                var controller = new ProjectController();
                controller.TerminateProject(DeletedProjectInfo);
                Response.Redirect(Request.RawUrl);


            }, "Success", "Project : " + TextBoxProjectName.Text + " has been terminated.");
            
        }

        protected void ButtonActivateProject_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                ProjectInformation ActivatedProjectInfo = new ProjectInformation();
                if (TextBoxProjectId.Text == null)
                {
                    throw new Exception("Please select a Project to Activate");
                }
                
                else
                {
                    ActivatedProjectInfo.ProjectId = int.Parse(TextBoxProjectId.Text);
                    ActivatedProjectInfo.EndDate = DateTime.Today.AddMonths(1);
                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    ActivatedProjectInfo.ModifiedBy = SecurityController.FindAccount(windowsAccountName).AccountID;

                    var controller = new ProjectController();
                    controller.ActivateProject(ActivatedProjectInfo);
                    Response.Redirect(Request.RawUrl);
                }
            }, "Success", "Project : " + TextBoxProjectName.Text + " has been activated.");
        }
          

        protected void CheckboxExpired_CheckedChanged(object sender, EventArgs e)
        {
            if(CheckboxExpireds.Checked == true)
            {
                ExpiredProjects.Visible = true;
                ListProject.Visible = false;
            }

            else
            {
                ExpiredProjects.Visible = false;
                ListProject.Visible = true;
                Response.Redirect(Request.RawUrl);
            }

        }
    }
}