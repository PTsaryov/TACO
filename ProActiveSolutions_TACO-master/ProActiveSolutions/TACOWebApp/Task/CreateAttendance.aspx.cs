using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOSystem.BLL.Attendance;
using TACOSystem.BLL.Security;

namespace TACOWebApp.Task
{
    public partial class CreateAttendance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();


            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName && SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin") { }
            else { all_content.Visible = false; }

            if (!IsPostBack)
            {
                CheckboxAttendanceExpired.Checked = false;
            }
        }

        protected void ButtonAttendanceLookup_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;
            if (!CheckboxAttendanceExpired.Checked)
            {
                ButtonUpdateAttendanceCode.Text = "Update";
            }

            if (DropdownAttendanceLookup.SelectedIndex == 0)
            {
                MessageUserControl.Visible = true;
                MessageUserControl.ShowInfo("Please select an attendance code.");
            }
            else
            {
                var controller = new AttendanceController();
                var attendanceinfo = controller.GetAttendanceInformation(int.Parse(DropdownAttendanceLookup.SelectedValue));

                TextBoxAttendanceId.Text = attendanceinfo.AttendanceId.ToString();
                TextBoxAttendanceCode.Text = attendanceinfo.AttendanceCode;
                TextBoxDescription.Text = attendanceinfo.AttendanceDescription;
                DropdownlistUnits.SelectedValue = attendanceinfo.Units;
                CheckboxAttendanceCodeDeactivate.Checked = attendanceinfo.AttendanceDeactivated;

                ButtonUpdateAttendanceCode.Visible = true;
                ButtonCreateAttendanceCode.Visible = false;
                if (CheckboxAttendanceExpired.Checked == false)
                {
                    ButtonDeleteAttendanceCode.Visible = true;
                }

            }
        }

        protected void CheckboxAttendanceExpired_CheckedChanged(object sender, EventArgs e)
        {
            ButtonClearAttendanceCode_Click(sender, e);
            if (CheckboxAttendanceExpired.Checked == true)
            {
                DropdownAttendanceLookup.Items.Clear();
                AttendanceListODS.SelectMethod = "DeactivatedAttendanceList";

                DropdownAttendanceLookup.Items.Insert(0, new ListItem("Choose an attendance code", ""));
                DropdownAttendanceLookup.SelectedIndex = 0;
                DropdownAttendanceLookup.DataBind();

                ButtonUpdateAttendanceCode.Text = "Activate";

                //Disable fields
                TextBoxAttendanceCode.Enabled = false;
                TextBoxDescription.Enabled = false;
                DropdownlistUnits.Enabled = false;
                CheckboxAttendanceCodeDeactivate.Enabled = false;
                ButtonCreateAttendanceCode.Visible = false;


            }
            else
            {
                DropdownAttendanceLookup.Items.Clear();
                AttendanceListODS.SelectMethod = "AttendanceList";

                DropdownAttendanceLookup.Items.Insert(0, new ListItem("Choose an attendance code", ""));
                DropdownAttendanceLookup.SelectedIndex = 0;
                DropdownAttendanceLookup.DataBind();

                ButtonUpdateAttendanceCode.Text = "Update";

                //Enable fields
                TextBoxAttendanceCode.Enabled = true;
                TextBoxDescription.Enabled = true;
                DropdownlistUnits.Enabled = true;
                CheckboxAttendanceCodeDeactivate.Enabled = true;
                ButtonCreateAttendanceCode.Visible = true;
            }
        }

        protected void ButtonCreateAttendanceCode_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;

            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;

                AttendanceInformation newAttendanceCode = new AttendanceInformation();

                if (TextBoxAttendanceCode.Text == "" || TextBoxAttendanceCode.Text.Length > 10)
                {
                    throw new Exception("Attendance code cannot be empty and must be 10 characters or less.");
                }
                if (TextBoxDescription.Text == "" || TextBoxDescription.Text.Length > 250)
                {
                    throw new Exception("Attendance description cannot be empty and must be 250 characters or less");
                }

                if (CheckboxAttendanceCodeDeactivate.Checked == true)
                {
                    throw new Exception("Attendance code must be active");
                }
                else
                {
                    newAttendanceCode.AttendanceCode = TextBoxAttendanceCode.Text;
                    newAttendanceCode.AttendanceDescription = TextBoxDescription.Text;
                    newAttendanceCode.Units = DropdownlistUnits.SelectedValue.ToString();
                }
                var controller = new AttendanceController();
                controller.CreateAttendanceCode(newAttendanceCode, userId);

                TextBoxAttendanceId.Text = "";
                TextBoxAttendanceCode.Text = "";
                TextBoxDescription.Text = "";
                DropdownlistUnits.SelectedIndex = 0;
                CheckboxAttendanceExpired.Checked = false;
                DropdownAttendanceLookup.Items.Clear();
                DropdownAttendanceLookup.DataBind();
                DropdownAttendanceLookup.Items.Insert(0, new ListItem("Choose an attendance code", ""));
                DropdownAttendanceLookup.SelectedIndex = -1;


            }, "Success", "Attendance code has been created.");
        }

        protected void ButtonUpdateAttendanceCode_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                MessageUserControl.Visible = true;
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;
                string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
                //For Activation of deactivated
                if (CheckboxAttendanceExpired.Checked)
                {
                    AttendanceController controller = new AttendanceController();
                    controller.ActivateAttendanceCode(userId, role, int.Parse(TextBoxAttendanceId.Text));

                    CheckboxAttendanceExpired.Checked = false;
                    DropdownAttendanceLookup.Items.Clear();
                    DropdownAttendanceLookup.DataBind();
                    DropdownAttendanceLookup.Items.Insert(0, new ListItem("Choose an attendance code", ""));
                    DropdownAttendanceLookup.SelectedIndex = -1;
                    ButtonClearAttendanceCode_Click(sender, e);
                    MessageUserControl.Visible = true;

                    //Enable fields
                    TextBoxAttendanceCode.Enabled = true;
                    TextBoxDescription.Enabled = true;
                    DropdownlistUnits.Enabled = true;
                    CheckboxAttendanceCodeDeactivate.Enabled = true;
                    ButtonCreateAttendanceCode.Visible = true;
                }
                else
                {
                    AttendanceInformation updatedAttendanceCodeInfo = new AttendanceInformation();
                    if (TextBoxAttendanceCode.Text == "" || TextBoxAttendanceCode.Text.Length > 10)
                    {
                        throw new Exception("Attendance code cannot be empty and must be 10 characters or less.");
                    }
                    if (TextBoxDescription.Text == "" || TextBoxDescription.Text.Length > 250)
                    {
                        throw new Exception("Attendance description cannot be empty and must be 250 characters or less");
                    }

                    else
                    {
                        updatedAttendanceCodeInfo.AttendanceId = int.Parse(TextBoxAttendanceId.Text);
                        updatedAttendanceCodeInfo.AttendanceCode = TextBoxAttendanceCode.Text;
                        updatedAttendanceCodeInfo.AttendanceDescription = TextBoxDescription.Text;
                        updatedAttendanceCodeInfo.Units = DropdownlistUnits.SelectedValue.ToString();
                        updatedAttendanceCodeInfo.AttendanceDeactivated = CheckboxAttendanceCodeDeactivate.Checked;
                    }
                    var controller = new AttendanceController();
                    controller.UpdateAttendanceCode(updatedAttendanceCodeInfo, userId);

                    CheckboxAttendanceExpired.Checked = false;
                    DropdownAttendanceLookup.Items.Clear();
                    DropdownAttendanceLookup.DataBind();
                    DropdownAttendanceLookup.Items.Insert(0, new ListItem("Choose an attendance code", ""));
                    DropdownAttendanceLookup.SelectedIndex = -1;
                    ButtonClearAttendanceCode_Click(sender, e);
                    MessageUserControl.Visible = true;
                }

            }, "Success", "Attendance code has been updated.");
        }

        protected void ButtonDeleteAttendanceCode_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;

                AttendanceInformation deletedAttendanceCodeInfo = new AttendanceInformation();

                if (TextBoxAttendanceId.Text == null)
                {
                    throw new Exception("Please select an attendance code to deactivate");
                }
                else
                {
                    deletedAttendanceCodeInfo.AttendanceId = int.Parse(TextBoxAttendanceId.Text);
                }
                var controller = new AttendanceController();
                controller.DeleteAttendanceCode(deletedAttendanceCodeInfo.AttendanceId, userId);

                DropdownAttendanceLookup.Items.Clear();
                DropdownAttendanceLookup.DataBind();
                DropdownAttendanceLookup.Items.Insert(0, new ListItem("Choose an attendance code", ""));
                DropdownAttendanceLookup.SelectedIndex = 0;

                ButtonClearAttendanceCode_Click(sender, e);
                MessageUserControl.Visible = true;

            }, "Success", "Attendance code has been deactivated.");
        }

        protected void ButtonClearAttendanceCode_Click(object sender, EventArgs e)
        {
            DropdownAttendanceLookup.SelectedIndex = 0;
            TextBoxAttendanceId.Text = "";
            TextBoxAttendanceCode.Text = "";
            TextBoxDescription.Text = "";
            DropdownlistUnits.SelectedIndex = 0;
            CheckboxAttendanceCodeDeactivate.Checked = false;

            TextBoxAttendanceId.Enabled = true;
            TextBoxAttendanceCode.Enabled = true;
            TextBoxDescription.Enabled = true;
            DropdownlistUnits.Enabled = true;
            ButtonUpdateAttendanceCode.Visible = false;
            ButtonCreateAttendanceCode.Visible = true;
            ButtonDeleteAttendanceCode.Visible = false;
            MessageUserControl.Visible = false;
        }
    }
}