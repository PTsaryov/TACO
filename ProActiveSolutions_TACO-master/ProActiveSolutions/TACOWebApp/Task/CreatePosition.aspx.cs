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
    public partial class CreatePosition : System.Web.UI.Page
    {
        /// <summary>
        /// <para>
        /// This method will check the role and load the page according to the signed in role.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: April 22,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();
            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName && SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin") { }
            else { all_content.Visible = false; }
            if (!IsPostBack)
            {
                CheckboxPositionExpired.Checked = false;
            }
        }
        /// <summary>
        /// <para>
        /// This method will check if expired check box is check and display the expired view if checked,
        /// or unexpired unchecked.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: April 22,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        protected void CheckboxPositionExpired_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckboxPositionExpired.Checked)
            {
                DropdownPositionLookup.Items.Clear();
                ObjectDataSourcePosition.SelectMethod = "DeactivatedPositions";

                DropdownPositionLookup.Items.Insert(0, new ListItem("Choose a position", ""));
                DropdownPositionLookup.SelectedIndex = 0;
                DropdownPositionLookup.DataBind();

                ButtonUpdatePosition.Text = "Activate";
                //Disable the fields
                TextBoxPositionName.Enabled = false;
                TextBoxDescription.Enabled = false;
                CheckboxPositionDeactivate.Enabled = false;
                ButtonCreatePosition.Visible = false;
            }
            else
            {
                DropdownPositionLookup.Items.Clear();
                ObjectDataSourcePosition.SelectMethod = "PositionDetails";

                DropdownPositionLookup.Items.Insert(0, new ListItem("Choose a position", ""));
                DropdownPositionLookup.SelectedIndex = 0;
                DropdownPositionLookup.DataBind();

                ButtonUpdatePosition.Visible = false;
                ButtonUpdatePosition.Text = "Update";
                //Enable the fields
                TextBoxPositionName.Enabled = true;
                TextBoxDescription.Enabled = true;
                CheckboxPositionDeactivate.Enabled = true;
                ButtonCreatePosition.Visible = true;

                ButtonClearPosition_Click(sender, e);
            }
        }

        /// <summary>
        /// <para>
        /// This method will lookup the selected position from the drop down list.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: April 22,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        protected void ButtonPositionLookup_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;

            if (!CheckboxPositionExpired.Checked)
            {
                ButtonUpdatePosition.Text = "Update";
            }
            if (DropdownPositionLookup.SelectedIndex == 0)
            {
                MessageUserControl.Visible = true;
                MessageUserControl.ShowInfo("Please select a position.");
            }
            else
            {

                PositionController controller = new PositionController();
                PositionInformation position = controller.GetPosition(int.Parse(DropdownPositionLookup.SelectedValue));

                TextBoxPositionId.Text = position.PositionId.ToString();
                TextBoxPositionName.Text = position.PositionName;
                TextBoxDescription.Text = position.PositionDescription;
                ButtonUpdatePosition.Visible = true;
                ButtonCreatePosition.Visible = false;
                if (!CheckboxPositionExpired.Checked)
                {
                    ButtonDeletePosition.Visible = true;
                }
            }

        }

        /// <summary>
        /// <para>
        /// This method will create a new position after checking all the required fields.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: April 22,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        protected void ButtonCreatePosition_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;

            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
                if (TextBoxPositionName.Text == "" || TextBoxPositionName.Text.Length > 100)
                {
                    throw new Exception("Position name cannot be empty and must be less than 100 characters");
                }
                if (TextBoxDescription.Text == "" || TextBoxDescription.Text.Length > 250)
                {
                    throw new Exception("Position description cannot be empty and must be less than 250 characters");
                }

                if (CheckboxPositionDeactivate.Checked)
                {
                    throw new Exception("Position must be active");
                }
                else
                {
                    PositionInformation newPosition = new PositionInformation()
                    {
                        PositionName = TextBoxPositionName.Text,
                        PositionDescription = TextBoxDescription.Text
                    };
                    PositionController controller = new PositionController();
                    controller.CreatePosition(newPosition, employeeId);
                    ButtonClearPosition_Click(sender, e);
                    DropdownPositionLookup.Items.Clear();
                    DropdownPositionLookup.DataBind();
                    DropdownPositionLookup.Items.Insert(0, new ListItem("Choose an overtime code", ""));
                    DropdownPositionLookup.SelectedIndex = -1;
                    MessageUserControl.Visible = true;

                }

            }, "Success", "Position: " + TextBoxPositionName.Text + " has been created.");
        }

        /// <summary>
        /// <para>
        /// This method will update the selected position from the dropdown after checking all the required fields.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: April 22,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        protected void ButtonUpdatePosition_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                MessageUserControl.Visible = true;
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
                string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
                PositionController controller = new PositionController();
                //For Activation of deactivated
                if (CheckboxPositionExpired.Checked)
                {

                    controller.ActivatePosition(employeeId, role, int.Parse(TextBoxPositionId.Text));

                    CheckboxPositionExpired.Checked = false;
                    DropdownPositionLookup.Items.Clear();
                    DropdownPositionLookup.DataBind();
                    DropdownPositionLookup.Items.Insert(0, new ListItem("Choose a position", ""));
                    DropdownPositionLookup.SelectedIndex = -1;
                    ButtonClearPosition_Click(sender, e);
                    MessageUserControl.Visible = true;

                    ////Enable the fields
                    TextBoxPositionName.Enabled = true;
                    TextBoxDescription.Enabled = true;
                    CheckboxPositionDeactivate.Enabled = true;
                    ButtonCreatePosition.Visible = true;
                }
                else
                {
                    if (TextBoxPositionName.Text == "" || TextBoxPositionName.Text.Length > 100)
                    {
                        throw new Exception("Position name cannot be empty and must be less than 100 characters");
                    }
                    if (TextBoxDescription.Text == "" || TextBoxDescription.Text.Length > 250)
                    {
                        throw new Exception("Position description cannot be empty and must be less than 250 characters");
                    }
                    else
                    {
                        PositionInformation updatedPosition = new PositionInformation()
                        {
                            PositionId = int.Parse(TextBoxPositionId.Text),
                            PositionName = TextBoxPositionName.Text,
                            PositionDescription = TextBoxDescription.Text
                        };

                        controller.UpdatePosition(updatedPosition, employeeId, CheckboxPositionDeactivate.Checked);

                        ButtonClearPosition_Click(sender, e);
                        DropdownPositionLookup.Items.Clear();
                        DropdownPositionLookup.DataBind();
                        DropdownPositionLookup.Items.Insert(0, new ListItem("Choose an overtime code", ""));
                        DropdownPositionLookup.SelectedIndex = -1;
                        MessageUserControl.Visible = true;
                    }
                }
            }, "Success", "Position: " + TextBoxPositionName.Text + " has been updated.");
        }

        /// <summary>
        /// <para>
        /// This method will deactivate the selected position from the dropdown.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: April 22,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        protected void ButtonDeletePosition_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
                string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
                PositionController controller = new PositionController();
                controller.DeactivatePosition(employeeId, role, int.Parse(TextBoxPositionId.Text));

                DropdownPositionLookup.Items.Clear();
                DropdownPositionLookup.DataBind();
                DropdownPositionLookup.Items.Insert(0, new ListItem("Choose a position", ""));
                DropdownPositionLookup.SelectedIndex = 0;

                ButtonClearPosition_Click(sender, e);
                MessageUserControl.Visible = true;

            }, "Success", "Position: " + TextBoxPositionName.Text + " has been deactivated.");
        }

        /// <summary>
        /// <para>
        /// This method will clear all the fields.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: April 22,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        protected void ButtonClearPosition_Click(object sender, EventArgs e)
        {
            TextBoxPositionName.Text = "";
            TextBoxPositionId.Text = "";
            TextBoxDescription.Text = "";
            DropdownPositionLookup.SelectedIndex = 0;
            CheckboxPositionDeactivate.Checked = false;
            CheckboxPositionExpired.Checked = false;

            ButtonUpdatePosition.Visible = false;
            ButtonCreatePosition.Visible = true;
            ButtonDeletePosition.Visible = false;
            MessageUserControl.Visible = false;

            ////Enable the fields
            TextBoxPositionName.Enabled = true;
            TextBoxDescription.Enabled = true;
            CheckboxPositionDeactivate.Enabled = true;

        }
    }
}