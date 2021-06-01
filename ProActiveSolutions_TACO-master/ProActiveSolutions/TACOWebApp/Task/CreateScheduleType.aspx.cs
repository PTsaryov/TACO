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
    public partial class CreateScheduleType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();


            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName && SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin") { }
            else { all_content.Visible = false; }

            if (!IsPostBack)
            {
                CheckboxScheduleExpired.Checked = false;
            }
        }

        protected void ButtonCreateScheduleType_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;

                ScheduleInformation newSchedule = new ScheduleInformation();
                int time = 0;
                bool result = int.TryParse(TextBoxScheduleTime.Text, out time);
                if (TextBoxScheduleName.Text == "" || TextBoxScheduleName.Text.Length > 100)
                {
                    throw new Exception("Schedule name cannot be empty and must be less than 100 characters");
                }
                if (TextBoxDescription.Text == "" || TextBoxDescription.Text.Length > 250)
                {
                    throw new Exception("Schedule description cannot be empty and must be less than 250 characters");
                }
                if (time <= 0 || result == false)
                {
                    throw new Exception("Schedule must have a time in minutes");
                }
                if (CheckboxScheduleDeactivate.Checked == true)
                {
                    throw new Exception("Schedule must be active");
                }
                else
                {                    
                    newSchedule.ScheduleName = TextBoxScheduleName.Text;
                    newSchedule.ScheduleDescription = TextBoxDescription.Text;
                    newSchedule.ScheduleTime = int.Parse(TextBoxScheduleTime.Text);
                }
                var controller = new ScheduleTypeController();
                controller.CreateSchedule(newSchedule, userId);

                TextBoxScheduleId.Text = "";
                TextBoxScheduleName.Text = "";
                TextBoxDescription.Text = "";
                TextBoxScheduleTime.Text = "";
                CheckboxScheduleExpired.Checked = false;
                DropDownScheduleLookup.Items.Clear();
                DropDownScheduleLookup.DataBind();
                DropDownScheduleLookup.Items.Insert(0, new ListItem("Choose a Schedule", ""));
                DropDownScheduleLookup.SelectedIndex = -1;


            }, "Success", "Schedule has been created.");
        }

        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;
                string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
                MessageUserControl.Visible = true;
                //For Activation of deactivated
                if (CheckboxScheduleExpired.Checked)
                {
                    ScheduleTypeController controller = new ScheduleTypeController();
                    controller.ActivateScheduleType(userId, role, int.Parse(TextBoxScheduleId.Text));

                    CheckboxScheduleExpired.Checked = false;
                    DropDownScheduleLookup.Items.Clear();
                    DropDownScheduleLookup.DataBind();
                    DropDownScheduleLookup.Items.Insert(0, new ListItem("Choose a Schedule", ""));
                    DropDownScheduleLookup.SelectedIndex = -1;
                    ButtonCancel_Click(sender, e);
                    MessageUserControl.Visible = true;

                    //Enable fields
                    TextBoxScheduleName.Enabled = true;
                    TextBoxDescription.Enabled = true;
                    TextBoxScheduleTime.Enabled = true;
                    CheckboxScheduleDeactivate.Enabled = true;
                    ButtonCreateScheduleType.Visible = true;
                }
                else
                {
                    ScheduleInformation updatedScheduleInfo = new ScheduleInformation();
                    if (TextBoxScheduleName.Text == "" || TextBoxScheduleName.Text.Length > 100)
                    {
                        throw new Exception("Schedule name cannot be empty and must be less than 100 characters");
                    }
                    if (TextBoxDescription.Text == "" || TextBoxDescription.Text.Length > 250)
                    {
                        throw new Exception("Schedule description cannot be empty and must be less than 250 characters");
                    }
                    if (TextBoxScheduleTime.Text == "")
                    {
                        throw new Exception("Schedule must have a time in minutes");
                    }
                    else
                    {
                        updatedScheduleInfo.ScheduleId = int.Parse(TextBoxScheduleId.Text);
                        updatedScheduleInfo.ScheduleName = TextBoxScheduleName.Text;
                        updatedScheduleInfo.ScheduleDescription = TextBoxDescription.Text;
                        updatedScheduleInfo.ScheduleTime = int.Parse(TextBoxScheduleTime.Text);
                        updatedScheduleInfo.ScheduleDeactivated = CheckboxScheduleDeactivate.Checked;
                    }
                    var controller = new ScheduleTypeController();
                    controller.UpdateSchedule(updatedScheduleInfo, userId);

                    CheckboxScheduleExpired.Checked = false;
                    DropDownScheduleLookup.Items.Clear();
                    DropDownScheduleLookup.DataBind();
                    DropDownScheduleLookup.Items.Insert(0, new ListItem("Choose a Schedule", ""));
                    DropDownScheduleLookup.SelectedIndex = -1;
                    ButtonCancel_Click(sender, e);
                    MessageUserControl.Visible = true;
                }

                
            }, "Success", "Schedule has been updated.");
        }

        protected void ButtonDeactivate_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;

                ScheduleInformation deletedScheduleInfo = new ScheduleInformation();
                
                if (TextBoxScheduleId.Text == null)
                {
                    throw new Exception("Please select a schedule to deactivate");
                }
                else
                {
                    deletedScheduleInfo.ScheduleId = int.Parse(TextBoxScheduleId.Text);
                }
                var controller = new ScheduleTypeController();
                controller.DeleteSchedule(deletedScheduleInfo.ScheduleId, userId);

                DropDownScheduleLookup.Items.Clear();
                DropDownScheduleLookup.DataBind();
                DropDownScheduleLookup.Items.Insert(0, new ListItem("Choose a Schedule", ""));
                DropDownScheduleLookup.SelectedIndex = 0;

                ButtonCancel_Click(sender, e);
                MessageUserControl.Visible = true;

            }, "Success", "Schedule has been deactivated.");
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            DropDownScheduleLookup.SelectedIndex = 0;
            TextBoxScheduleId.Text = "";
            TextBoxScheduleName.Text = "";
            TextBoxDescription.Text = "";
            TextBoxScheduleTime.Text = "";
            CheckboxScheduleDeactivate.Checked = false;

            TextBoxScheduleId.Enabled = true;
            CheckboxScheduleDeactivate.Enabled = true;
            TextBoxScheduleName.Enabled = true;
            TextBoxDescription.Enabled = true;
            TextBoxScheduleTime.Enabled = true;
            ButtonUpdate.Visible = false;
            ButtonCreateScheduleType.Visible = true;
            ButtonDeactivate.Visible = false;
            MessageUserControl.Visible = false;


        }

        protected void ButtonLookup_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;
            if (!CheckboxScheduleExpired.Checked)
            {
                ButtonUpdate.Text = "Update";
            }
            if (DropDownScheduleLookup.SelectedIndex == 0)
            {
                MessageUserControl.Visible = true;
                MessageUserControl.ShowInfo("Please select a schedule.");
            }
            else
            {
                var controller = new ScheduleTypeController();
                var scheduleInfo = controller.GetScheduleInformation(int.Parse(DropDownScheduleLookup.SelectedValue));

                TextBoxScheduleId.Text = scheduleInfo.ScheduleId.ToString();
                TextBoxScheduleName.Text = scheduleInfo.ScheduleName;
                TextBoxDescription.Text = scheduleInfo.ScheduleDescription;
                TextBoxScheduleTime.Text = scheduleInfo.ScheduleTime.ToString();
                CheckboxScheduleDeactivate.Checked = scheduleInfo.ScheduleDeactivated;

                ButtonUpdate.Visible = true;
                ButtonCreateScheduleType.Visible = false;
                if(CheckboxScheduleDeactivate.Checked == false)
                {
                    ButtonDeactivate.Visible = true;
                }
                
            }
        }

        protected void CheckboxScheduleExpired_CheckedChanged(object sender, EventArgs e)
        {
            ButtonCancel_Click(sender, e);

            if (CheckboxScheduleExpired.Checked == true)
            {
                DropDownScheduleLookup.Items.Clear();
                ScheduleListODS.SelectMethod = "ExpiredScheduleList";
                
                DropDownScheduleLookup.Items.Insert(0, new ListItem("Choose a Schedule", ""));
                DropDownScheduleLookup.SelectedIndex = 0;
                DropDownScheduleLookup.DataBind();

                ButtonUpdate.Text = "Activate";
                //Disable fields
                TextBoxScheduleName.Enabled = false;
                TextBoxDescription.Enabled = false;
                TextBoxScheduleTime.Enabled = false;
                CheckboxScheduleDeactivate.Enabled = false;
                ButtonCreateScheduleType.Visible = false;

            }
            else
            {
                DropDownScheduleLookup.Items.Clear();
                ScheduleListODS.SelectMethod = "ScheduleList";
                
                DropDownScheduleLookup.Items.Insert(0, new ListItem("Choose a Schedule", ""));
                DropDownScheduleLookup.SelectedIndex = 0;
                DropDownScheduleLookup.DataBind();

                ButtonUpdate.Text = "Update";
                //Enable fields
                TextBoxScheduleName.Enabled = true;
                TextBoxDescription.Enabled = true;
                TextBoxScheduleTime.Enabled = true;
                CheckboxScheduleDeactivate.Enabled = true;
                ButtonCreateScheduleType.Visible = true;

            }

        }
    }
}