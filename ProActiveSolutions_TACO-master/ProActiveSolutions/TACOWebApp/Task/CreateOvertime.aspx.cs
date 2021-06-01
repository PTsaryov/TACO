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
    public partial class CreateOvertime : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();
            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName && SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin") { }
            else { all_content.Visible = false; }

            if (!IsPostBack)
            {
                CheckboxOvertimeExpired.Checked = false;
            }
        }

        protected void ButtonOvertimeLookup_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;
            if (!CheckboxOvertimeExpired.Checked)
            {
                ButtonUpdateOvertimeCode.Text = "Update";
            }

            if (DropdownOvertimeLookup.SelectedIndex == 0)
            {
                MessageUserControl.Visible = true;
                MessageUserControl.ShowInfo("Please select an overtime code.");
            }
            else
            {
                var controller = new OvertimeController();
                var overtimeinfo = controller.GetOvertimeInformation(int.Parse(DropdownOvertimeLookup.SelectedValue));

                TextBoxOvertimeId.Text = overtimeinfo.OvertimeId.ToString();
                TextBoxOvertimeCode.Text = overtimeinfo.OvertimeCode;
                TextBoxDescription.Text = overtimeinfo.OvertimeDescription;
                PickerColor.Text = overtimeinfo.Color.Trim(new Char[] { '#' });
                DropdownlistUnits.SelectedValue = overtimeinfo.Units;
                CheckboxOvertimeCodeDeactivate.Checked = overtimeinfo.OvertimeDeactivated;

                ButtonUpdateOvertimeCode.Visible = true;
                ButtonCreateOvertimeCode.Visible = false;
                if (CheckboxOvertimeExpired.Checked == false)
                {
                    ButtonDeleteOvertimeCode.Visible = true;
                }
            }
        }

        protected void CheckboxOvertimeExpired_CheckedChanged(object sender, EventArgs e)
        {
            ButtonClearOvertimeCode_Click(sender, e);
            if (CheckboxOvertimeExpired.Checked == true)
            {
                DropdownOvertimeLookup.Items.Clear();
                OvertimeListODS.SelectMethod = "DeactivatedOvertimeList";

                DropdownOvertimeLookup.Items.Insert(0, new ListItem("Choose an overtime code", ""));
                DropdownOvertimeLookup.SelectedIndex = 0;
                DropdownOvertimeLookup.DataBind();

                ButtonUpdateOvertimeCode.Text = "Activate";
                //Disable the fields
                TextBoxOvertimeCode.Enabled = false;
                TextBoxDescription.Enabled = false;
                PickerColor.Enabled = false;
                DropdownlistUnits.Enabled = false;
                CheckboxOvertimeCodeDeactivate.Enabled = false;
                ButtonCreateOvertimeCode.Visible = false;
                

            }
            else
            {
                DropdownOvertimeLookup.Items.Clear();
                OvertimeListODS.SelectMethod = "OvertimeList";

                DropdownOvertimeLookup.Items.Insert(0, new ListItem("Choose an overtime code", ""));
                DropdownOvertimeLookup.SelectedIndex = 0;
                DropdownOvertimeLookup.DataBind();

                ButtonUpdateOvertimeCode.Text = "Update";
                //Enable the fields
                TextBoxOvertimeCode.Enabled = true;
                TextBoxDescription.Enabled = true;
                PickerColor.Enabled = true;
                DropdownlistUnits.Enabled = true;
                CheckboxOvertimeCodeDeactivate.Enabled = true;
                ButtonCreateOvertimeCode.Visible = true;
            }
        }

        protected void ButtonCreateOvertimeCode_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;

            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;

                OvertimeInformation newOvertimeCode = new OvertimeInformation();

                if (TextBoxOvertimeCode.Text == "" || TextBoxOvertimeCode.Text.Length > 10)
                {
                    throw new Exception("Overtime code cannot be empty and must be less than 10 characters");
                }
                if (TextBoxDescription.Text == "" || TextBoxDescription.Text.Length > 250)
                {
                    throw new Exception("Overtime description cannot be empty and must be less than 250 characters");
                }

                if (CheckboxOvertimeCodeDeactivate.Checked == true)
                {
                    throw new Exception("Overtime code must be active");
                }
                if (PickerColor.Text.Length > 6)
                {
                    throw new Exception("Invalid color");
                }
                else
                {
                    newOvertimeCode.OvertimeCode = TextBoxOvertimeCode.Text;
                    newOvertimeCode.OvertimeDescription = TextBoxDescription.Text;
                    newOvertimeCode.Units = DropdownlistUnits.SelectedValue.ToString();
                    newOvertimeCode.Color = '#' + PickerColor.Text;
                }
                var controller = new OvertimeController();
                controller.CreateOvertimeCode(newOvertimeCode, userId);

                TextBoxOvertimeId.Text = "";
                TextBoxOvertimeCode.Text = "";
                TextBoxDescription.Text = "";
                PickerColor.Text = "";
                DropdownlistUnits.SelectedIndex = 0;
                CheckboxOvertimeExpired.Checked = false;
                DropdownOvertimeLookup.Items.Clear();
                DropdownOvertimeLookup.DataBind();
                DropdownOvertimeLookup.Items.Insert(0, new ListItem("Choose an overtime code", ""));
                DropdownOvertimeLookup.SelectedIndex = -1;


            }, "Success", "Overtime code has been created.");
        }

        protected void ButtonUpdateOvertimeCode_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                MessageUserControl.Visible = true;
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;
                string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
                //For Activation of deactivated
                if (CheckboxOvertimeExpired.Checked)
                {
                    OvertimeController controller = new OvertimeController();
                    controller.ActivateOvertimeCode(userId, role, int.Parse(TextBoxOvertimeId.Text));

                    CheckboxOvertimeExpired.Checked = false;
                    DropdownOvertimeLookup.Items.Clear();
                    DropdownOvertimeLookup.DataBind();
                    DropdownOvertimeLookup.Items.Insert(0, new ListItem("Choose an overtime code", ""));
                    DropdownOvertimeLookup.SelectedIndex = -1;
                    ButtonClearOvertimeCode_Click(sender, e);
                    MessageUserControl.Visible = true;

                    //Enable the fields
                    TextBoxOvertimeCode.Enabled = true;
                    TextBoxDescription.Enabled = true;
                    PickerColor.Enabled = true;
                    DropdownlistUnits.Enabled = true;
                    CheckboxOvertimeCodeDeactivate.Enabled = true;
                    ButtonCreateOvertimeCode.Visible = true;
                }
                else
                {

                    OvertimeInformation updatedOvertimeCodeInfo = new OvertimeInformation();
                    if (TextBoxOvertimeCode.Text == "" || TextBoxOvertimeCode.Text.Length > 10)
                    {
                        throw new Exception("Overtime code cannot be empty and must be less than 10 characters");
                    }
                    if (TextBoxDescription.Text == "" || TextBoxOvertimeCode.Text.Length > 250)
                    {
                        throw new Exception("Overtime description cannot be empty and must be less than 250 characters");
                    }                                        
                    if (PickerColor.Text.Length > 6)
                    {
                        throw new Exception("Invalid color");
                    }
                    else
                    {
                        updatedOvertimeCodeInfo.OvertimeId = int.Parse(TextBoxOvertimeId.Text);
                        updatedOvertimeCodeInfo.OvertimeCode = TextBoxOvertimeCode.Text;
                        updatedOvertimeCodeInfo.OvertimeDescription = TextBoxDescription.Text;
                        updatedOvertimeCodeInfo.Units = DropdownlistUnits.SelectedValue.ToString();
                        updatedOvertimeCodeInfo.Color = '#' + PickerColor.Text;
                        updatedOvertimeCodeInfo.OvertimeDeactivated = CheckboxOvertimeCodeDeactivate.Checked;
                    }
                    var controller = new OvertimeController();
                    controller.UpdateOvertimeCode(updatedOvertimeCodeInfo, userId);

                    CheckboxOvertimeExpired.Checked = false;
                    DropdownOvertimeLookup.Items.Clear();
                    DropdownOvertimeLookup.DataBind();
                    DropdownOvertimeLookup.Items.Insert(0, new ListItem("Choose an overtime code", ""));
                    DropdownOvertimeLookup.SelectedIndex = -1;
                    ButtonClearOvertimeCode_Click(sender, e);
                    MessageUserControl.Visible = true;
                }


            }, "Success", "Overtime code has been updated.");
        }

        protected void ButtonDeleteOvertimeCode_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;

                OvertimeInformation deletedOvertimeCodeInfo = new OvertimeInformation();

                if (TextBoxOvertimeId.Text == null)
                {
                    throw new Exception("Please select an overtime code to deactivate");
                }
                else
                {
                    deletedOvertimeCodeInfo.OvertimeId = int.Parse(TextBoxOvertimeId.Text);
                }
                var controller = new OvertimeController();
                controller.DeleteOvertimeCode(deletedOvertimeCodeInfo.OvertimeId, userId);

                DropdownOvertimeLookup.Items.Clear();
                DropdownOvertimeLookup.DataBind();
                DropdownOvertimeLookup.Items.Insert(0, new ListItem("Choose an overtime code", ""));
                DropdownOvertimeLookup.SelectedIndex = 0;

                ButtonClearOvertimeCode_Click(sender, e);
                MessageUserControl.Visible = true;

            }, "Success", "Overtime code has been deactivated.");
        }

        protected void ButtonClearOvertimeCode_Click(object sender, EventArgs e)
        {
            DropdownOvertimeLookup.SelectedIndex = 0;
            TextBoxOvertimeId.Text = "";
            TextBoxOvertimeCode.Text = "";
            TextBoxDescription.Text = "";
            PickerColor.Text = "";
            DropdownlistUnits.SelectedIndex = 0;
            CheckboxOvertimeCodeDeactivate.Checked = false;

            ButtonUpdateOvertimeCode.Visible = false;
            ButtonCreateOvertimeCode.Visible = true;
            ButtonDeleteOvertimeCode.Visible = false;
            MessageUserControl.Visible = false;
        }
    }
}