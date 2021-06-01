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
    public partial class CreateProject : System.Web.UI.Page
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
        }
        [WebMethod]
        public static void getEmployeeIdFromFront(string eventsJson)
        {
            ProjectInformation newProject = new ProjectInformation();
            ProjectController controller = new ProjectController();

            List<int> list = new List<int>();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();

            List<int> ids = json_serializer.Deserialize<List<int>>(eventsJson);

            controller.Createproject(newProject,ids);
            
        }

        protected void RadioButtonListPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Session["Priority"] = RadioButtonListPriority.SelectedValue.ToString();
        }

        protected void ButtonCreateProject_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                ProjectInformation newProject = new ProjectInformation();
                if (TextBoxProjectName.Text == null || TextBoxProjectName.Text == "")
                {
                    throw new Exception("Project name cannot be empty.");
                }

                else if (TextBoxProjectDescription.Text == null || TextBoxProjectDescription.Text == "")
                {
                    throw new Exception("Project Description cannot be empty.");
                }

                else if (DropDownCategory.SelectedIndex == 0)
                {
                    throw new Exception("Please Select a Category.");
                }

                else if (TextBoxPickerColor.Text == null || TextBoxPickerColor.Text == "")
                {
                    throw new Exception("Please Select a Color or type the appropriate Code for desired color like #000000.");
                }


                else
                {
                    DateTime convertedDate;
                    newProject.ProjectName = TextBoxProjectName.Text;
                    newProject.ProjectDescription = TextBoxProjectDescription.Text;
                    newProject.CategoryId = int.Parse(DropDownCategory.SelectedValue);
                    newProject.Priority = RadioButtonListPriority.SelectedValue.ToString();
                    newProject.ProjectColor = '#'+TextBoxPickerColor.Text;
                                        
                    if (DateTime.TryParse(TextBoxStartDate.Text, out convertedDate))
                    {
                        newProject.StartDate = convertedDate;
                    }
                    else
                    {
                        throw new Exception("Please input Start Date");
                    }

                    if (DateTime.TryParse(TextBoxEndDate.Text, out convertedDate))
                    {
                        newProject.EndDate = convertedDate;
                    }
                    else if (DateTime.TryParse("December 31, 9999", out convertedDate))
                    {
                        newProject.EndDate = convertedDate;
                    }
                }

                var controller = new ProjectController();
                controller.Createproject(newProject);
                                
                TextBoxProjectName.Text = "";
                TextBoxProjectDescription.Text = "";
                TextBoxEndDate.Text = "";
                TextBoxStartDate.Text = "";
                DropDownCategory.SelectedIndex = 0;
                TextBoxPickerColor.Text = "";
                RadioButtonListPriority.SelectedIndex = -1;
            }, "Success", "Project : " + TextBoxProjectName.Text + " has been created.");
        }

        protected void ButtonCancelProject_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}