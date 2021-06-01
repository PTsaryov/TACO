using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOSystem.BLL;
using TACOSystem.BLL.Security;

namespace TACOWebApp.Task
{
    public partial class CreateItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();
            if (Request.IsAuthenticated && (User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName && SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin"))
            {
            }
            else
            {
                all_content.Visible = false;
            }
            TextBoxDepartmentId.ReadOnly = true;
            MessageUserControl.Visible = true;
            TextBoxStartDate.Enabled = true;            
        }

        protected void ButtonLookupDepartment_Click(object sender, EventArgs e)
        {
            
            if (DropDownDepartment.SelectedIndex == 0)
            {
                MessageUserControl.ShowInfo("Please select department first to lookup");
                TextBoxDepartmentId.Text = "";
                TextBoxDepartmentName.Text = "";
                TextBoxDepartmentDescription.Text = "";
                TextBoxExpiryDate.Text = "";
                TextBoxStartDate.Text = "";
                ButtonUpdateDepartment.Visible = false;
                ButtonDeleteDepartment.Visible = false;
                ButtonCreateDepartment.Visible = true;
            }
            else 
            {                
                var controller = new DepartmentController();
                var departmentinfo = controller.GetDepartmentInformation(int.Parse(DropDownDepartment.SelectedValue));

                TextBoxDepartmentId.Text = departmentinfo.DepartmentId.ToString();
                TextBoxDepartmentName.Text = departmentinfo.DepartmentName;
                TextBoxDepartmentDescription.Text = departmentinfo.DepartmentDescription;
                TextBoxStartDate.Text = departmentinfo.StartDate.ToLongDateString();
                TextBoxExpiryDate.Text = departmentinfo.ExpiryDate.ToLongDateString();

                TextBoxStartDate.Enabled = false;                

                if (CheckboxExpired.Checked == true)
                {
                    ButtonUpdateDepartment.Visible = false;
                    ButtonDeleteDepartment.Visible = false;
                    ButtonActivateDepartment.Visible = true;
                    TextBoxDepartmentName.Enabled = false;
                    TextBoxDepartmentDescription.Enabled = false;
                    TextBoxExpiryDate.Enabled = false;
                }
                else
                {
                    ButtonUpdateDepartment.Visible = true;
                    ButtonDeleteDepartment.Visible = true;
                }
                                                
                ButtonCreateDepartment.Visible = false;                
                MessageUserControl.Visible = false;
            }           
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            TextBoxDepartmentId.Text = "";
            TextBoxDepartmentName.Text = "";
            TextBoxDepartmentDescription.Text = "";
            TextBoxExpiryDate.Text = "";
            TextBoxStartDate.Text = "";
            DropDownDepartment.SelectedIndex = 0;

            ButtonCreateDepartment.Visible = true;
            ButtonUpdateDepartment.Visible = false;
            ButtonDeleteDepartment.Visible = false;
            MessageUserControl.Visible = false;
            Response.Redirect(Request.RawUrl);
        }


        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            TextBoxStartDate.Enabled = false;
            MessageUserControl.TryRun(() =>
            {
                DepartmentInformation updatedDepartmentinfo = new DepartmentInformation();
                if (TextBoxDepartmentName.Text == null)
                {
                    throw new Exception(" Department name cannot be empty.");
                }

                else if (TextBoxDepartmentDescription.Text == null)
                {
                    throw new Exception(" Department description cannot be empty.");
                }

                else if (TextBoxDepartmentDescription.Text.Length > 250)
                {
                    throw new Exception(" Department description cannot be longer than 250 words.");
                }

                else
                {
                    updatedDepartmentinfo.DepartmentId = int.Parse(TextBoxDepartmentId.Text);
                    updatedDepartmentinfo.DepartmentName = TextBoxDepartmentName.Text;
                    updatedDepartmentinfo.DepartmentDescription = TextBoxDepartmentDescription.Text;
                    updatedDepartmentinfo.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                    updatedDepartmentinfo.ExpiryDate = Convert.ToDateTime(TextBoxExpiryDate.Text);

                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    updatedDepartmentinfo.ModifiedBy = SecurityController.FindAccount(windowsAccountName).AccountID;
                }
                var controller = new DepartmentController();
                controller.UpdateDepartment(updatedDepartmentinfo);

                DropDownDepartment.Items.Clear();
                DropDownDepartment.DataBind();
                DropDownDepartment.Items.Insert(0, new ListItem("Choose a Departrment", ""));
                DropDownDepartment.SelectedIndex = -1;
            }, "Success", "Department : " + TextBoxDepartmentName.Text + " has been updated.");            
        }


        protected void ButtonCreateDepartment_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                DepartmentInformation newDepartment = new DepartmentInformation();
                DropDownDepartment.SelectedIndex = 0;
                if (TextBoxDepartmentName.Text == "")
                {
                    throw new Exception("Department name cannot be empty.");
                }      
                
                else if (TextBoxDepartmentDescription.Text == "")
                {
                    throw new Exception("Department Description cannot be empty.");
                } 
                else
                {
                    DateTime convertedDate;
                    newDepartment.DepartmentName = TextBoxDepartmentName.Text;
                    newDepartment.DepartmentDescription = TextBoxDepartmentDescription.Text;

                    if (DateTime.TryParse(TextBoxStartDate.Text, out convertedDate))
                    {
                        newDepartment.StartDate = convertedDate;
                    }
                    else
                    {
                        throw new Exception("Please input Start Date in MM-DD-YYYY format (example: 04-26-2019) or choose from Date Picker.");
                    }
                    if (DateTime.TryParse(TextBoxExpiryDate.Text, out convertedDate))
                    {
                        newDepartment.ExpiryDate = convertedDate;
                    }
                    else if (DateTime.TryParse("December 31, 9999", out convertedDate))
                    {
                        newDepartment.ExpiryDate = convertedDate;
                    }
                    
                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    newDepartment.CreatedBy = SecurityController.FindAccount(windowsAccountName).AccountID;
                }

                var controller = new DepartmentController();
                controller.CreateDepartment(newDepartment);

                TextBoxDepartmentId.Text = "";
                TextBoxDepartmentName.Text = "";
                TextBoxDepartmentDescription.Text = "";
                TextBoxExpiryDate.Text = "";
                TextBoxStartDate.Text = "";

                DropDownDepartment.Items.Clear();
                DropDownDepartment.DataBind();
                DropDownDepartment.Items.Insert(0, new ListItem("Choose a Departrment", ""));
                DropDownDepartment.SelectedIndex = -1;

            }, "Success", "Department : " +TextBoxDepartmentName.Text+ " has been created.");           

        }

        protected void ButtonDeleteDepartment_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                DepartmentInformation deletedepartmentinfo = new DepartmentInformation();
                if (TextBoxDepartmentId.Text == null)
                {
                    throw new Exception("Please select a department to Deactivate");
                }
                else
                {
                    deletedepartmentinfo.DepartmentId = int.Parse(TextBoxDepartmentId.Text);
                    deletedepartmentinfo.ExpiryDate = DateTime.Today;

                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    deletedepartmentinfo.ModifiedBy = SecurityController.FindAccount(windowsAccountName).AccountID;

                    TextBoxDepartmentId.Text = "";
                    TextBoxDepartmentName.Text = "";
                    TextBoxDepartmentDescription.Text = "";
                    TextBoxExpiryDate.Text = "";
                    TextBoxStartDate.Text = "";                                        
                }
                
                var controller = new DepartmentController();
                controller.DeleteDepartment(deletedepartmentinfo);
                DropDownDepartment.Items.Clear();
                DropDownDepartment.DataBind();
                DropDownDepartment.Items.Insert(0, new ListItem("Choose a Departrment", ""));
                DropDownDepartment.SelectedIndex = -1;
            }, "Success", "Department : " +TextBoxDepartmentName.Text+ " has been Deactivated.");

            ButtonCreateDepartment.Visible = true;
            ButtonUpdateDepartment.Visible = false;
            ButtonDeleteDepartment.Visible = false;            
        }


        protected void CheckboxExpired_CheckedChanged(object sender, EventArgs e)
        {
            TextBoxDepartmentId.Text = "";
            TextBoxDepartmentName.Text = "";
            TextBoxDepartmentDescription.Text = "";
            TextBoxExpiryDate.Text = "";
            TextBoxStartDate.Text = "";
            DropDownDepartment.SelectedIndex = 0;

            ButtonCreateDepartment.Visible = true;
            ButtonUpdateDepartment.Visible = false;
            ButtonDeleteDepartment.Visible = false;
            MessageUserControl.Visible = false;

            if (CheckboxExpired.Checked == true)
            {
                DropDownDepartment.Items.Clear();
                DepartmentlistDataSource.SelectMethod = "DepartmentListExpired";

                DropDownDepartment.Items.Insert(0, new ListItem("Choose a Department", ""));
                DropDownDepartment.SelectedIndex = 0;
                DropDownDepartment.DataBind();
                ButtonUpdateDepartment.Visible = false;
                ButtonDeleteDepartment.Visible = false;
                ButtonCreateDepartment.Visible = false;
            }
            else
            {

                DropDownDepartment.Items.Clear();
                DepartmentlistDataSource.SelectMethod = "DepartmentList";

                DropDownDepartment.Items.Insert(0, new ListItem("Choose a Department", ""));
                DropDownDepartment.SelectedIndex = 0;
                DropDownDepartment.DataBind();

                ButtonActivateDepartment.Visible = false;
                TextBoxDepartmentName.Enabled = true;
                TextBoxDepartmentDescription.Enabled = true;
                TextBoxExpiryDate.Enabled = true;
                TextBoxStartDate.Enabled = true;
            }
        }

        protected void ButtonActivateDepartment_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                DepartmentInformation activateddepartmentinfo = new DepartmentInformation();
                if (TextBoxDepartmentId.Text == null)
                {
                    throw new Exception("Please select a department to Activate");
                }
                else
                {
                    activateddepartmentinfo.DepartmentId = int.Parse(TextBoxDepartmentId.Text);
                    activateddepartmentinfo.ExpiryDate = DateTime.Parse("December 31, 9999");
                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    activateddepartmentinfo.ModifiedBy = SecurityController.FindAccount(windowsAccountName).AccountID;

                    TextBoxDepartmentId.Text = "";
                    TextBoxDepartmentName.Text = "";
                    TextBoxDepartmentDescription.Text = "";
                    TextBoxExpiryDate.Text = "";
                    TextBoxStartDate.Text = "";
                }

                var controller = new DepartmentController();
                controller.ActivateDepartment(activateddepartmentinfo);
                DropDownDepartment.Items.Clear();
                DropDownDepartment.DataBind();
                DropDownDepartment.Items.Insert(0, new ListItem("Choose a Departrment", ""));
                DropDownDepartment.SelectedIndex = -1;

                CheckboxExpired.Checked = false;
                ButtonActivateDepartment.Visible = false;

            }, "Success", "Department : " + TextBoxDepartmentName.Text + " has been activated.");

            ButtonCreateDepartment.Visible = true;
            ButtonUpdateDepartment.Visible = false;
            ButtonDeleteDepartment.Visible = false;
            TextBoxDepartmentName.Enabled = true;
            TextBoxDepartmentDescription.Enabled = true;
            TextBoxExpiryDate.Enabled = true;
        }

    }
}