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
    public partial class CreateUnit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();


            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName && SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin") { }
            else { all_content.Visible = false; }

            if (!IsPostBack)
            {
                CheckboxExpired.Checked = false;
            }
        }
        protected void ButtonCancelUnit_Click(object sender, EventArgs e)
        {
            TextBoxUnitId.Text = "";
            TextBoxUnitName.Text = "";
            DropDownAreaList.SelectedIndex = 0;
            TextBoxUnitDescription.Text = "";
            TextBoxStartDate.Text = "";
            TextBoxExpiryDate.Text = "";
            DropDownUnit.SelectedIndex = 0;

            ButtonDeleteUnit.Visible = false;
            ButtonUpdateUnit.Visible = false;
            ButtonCreateUnit.Visible = true;
            MessageUserControl.Visible = false;

        }

        protected void ButtonLookupUnit_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;
            if (!CheckboxExpired.Checked)
            {
                ButtonUpdateUnit.Text = "Update";
            }
            if (DropDownUnit.SelectedIndex == 0)
            {
                MessageUserControl.Visible = true;
                MessageUserControl.ShowInfo("Please select a unit.");
            }
            else
            {
                var controller = new UnitController();
                var unitinfo = controller.GetUnitInformation(int.Parse(DropDownUnit.SelectedValue));

                TextBoxUnitId.Text = unitinfo.UnitId.ToString();
                DropDownAreaList.SelectedValue = unitinfo.AreaId.ToString();
                TextBoxUnitName.Text = unitinfo.UnitName;
                TextBoxUnitDescription.Text = unitinfo.UnitDescription;
                TextBoxStartDate.Text = unitinfo.StartDate.ToLongDateString();
                TextBoxExpiryDate.Text = unitinfo.ExpiryDate.ToLongDateString();
                ButtonUpdateUnit.Visible = true;
                ButtonCreateUnit.Visible = false;

                if (CheckboxExpired.Checked == false)
                {
                    ButtonDeleteUnit.Visible = true;
                }

            }
        }

        protected void ButtonCreateUnit_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;

                UnitInformation newUnit = new UnitInformation();
                if (TextBoxUnitName.Text == "" || TextBoxUnitName.Text.Length > 100)
                {
                    throw new Exception("Unit name cannot be empty and must be less than 100 characters.");
                }

                else if (TextBoxUnitDescription.Text == "" || TextBoxUnitDescription.Text.Length > 250)
                {
                    throw new Exception("Unit Description cannot be empty and must be less than 250 characters.");
                }
                else if (DropDownAreaList.SelectedIndex == 0)
                {
                    throw new Exception("Please choose an area.");
                }
                else
                {
                    DateTime convertedDate;
                    newUnit.UnitName = TextBoxUnitName.Text;
                    newUnit.UnitDescription = TextBoxUnitDescription.Text;
                    newUnit.AreaId = int.Parse(DropDownAreaList.SelectedValue);

                    if (DateTime.TryParse(TextBoxStartDate.Text, out convertedDate))
                    {
                        newUnit.StartDate = convertedDate;
                    }
                    else
                    {
                        throw new Exception("Please choose a Start Date");
                    }

                    if (DateTime.TryParse(TextBoxExpiryDate.Text, out convertedDate))
                    {
                        newUnit.ExpiryDate = convertedDate;
                    }
                    else if (DateTime.TryParse("December 31, 9999", out convertedDate))
                    {
                        newUnit.ExpiryDate = convertedDate;
                    }
                    if (newUnit.ExpiryDate < newUnit.StartDate)
                    {
                        throw new Exception("Expiry date must not be before the start date");
                    }
                }

                var controller = new UnitController();
                controller.CreateUnit(newUnit, userId);

                TextBoxUnitId.Text = "";
                TextBoxUnitName.Text = "";
                TextBoxUnitDescription.Text = "";
                DropDownAreaList.SelectedIndex = -1;
                TextBoxExpiryDate.Text = "";
                TextBoxStartDate.Text = "";
                DropDownUnit.SelectedIndex = -1;
                MessageUserControl.Visible = true;

                CheckboxExpired.Checked = false;
                DropDownUnit.Items.Clear();
                DropDownUnit.DataBind();
                DropDownUnit.Items.Insert(0, new ListItem("Choose a Unit", ""));
                DropDownUnit.SelectedIndex = -1;

            }, "Success", "Unit : " + TextBoxUnitName.Text + " has been created.");
        }

        protected void ButtonUpdateUnit_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;
                string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
                //For Activation of deactivated
                if (CheckboxExpired.Checked)
                {
                    UnitController controller = new UnitController();
                    controller.ActivateUnit(userId, role, int.Parse(TextBoxUnitId.Text));

                    CheckboxExpired.Checked = false;
                    DropDownUnit.Items.Clear();
                    DropDownUnit.DataBind();
                    DropDownUnit.Items.Insert(0, new ListItem("Choose a Unit", ""));
                    DropDownUnit.SelectedIndex = int.Parse(TextBoxUnitId.Text) - 1;
                    ButtonCancelUnit_Click(sender, e);
                    MessageUserControl.Visible = true;
                    //Enable fields
                    TextBoxUnitName.Enabled = true;
                    TextBoxUnitDescription.Enabled = true;
                    DropDownAreaList.Enabled = true;
                    TextBoxExpiryDate.Enabled = true;
                    TextBoxStartDate.Enabled = true;
                    ButtonCreateUnit.Visible = true;
                }
                else
                {
                    UnitInformation updatedUnitInfo = new UnitInformation();
                    if (TextBoxUnitName.Text == null || TextBoxUnitName.Text.Length > 100)
                    {
                        throw new Exception("Unit name cannot be empty and must be less than 100 characters.");
                    }

                    //check if number entered in Unit name field
                    int number;
                    if (int.TryParse(TextBoxUnitName.Text, out number))
                    {
                        throw new Exception("Unit name cannot be a Number.");
                    }

                    if (int.TryParse(TextBoxUnitDescription.Text, out number))
                    {
                        throw new Exception("Unit description cannot be a Number.");
                    }

                    else if (TextBoxUnitDescription.Text == null || TextBoxUnitDescription.Text.Length > 250)
                    {
                        throw new Exception("Unit description cannot be empty and must be less than 250 characters.");
                    }
                    else if (DropDownAreaList.SelectedIndex == 0)
                    {
                        throw new Exception("Please choose an area.");
                    }
                    else
                    {
                        updatedUnitInfo.UnitId = int.Parse(TextBoxUnitId.Text);
                        updatedUnitInfo.UnitName = TextBoxUnitName.Text;
                        updatedUnitInfo.UnitDescription = TextBoxUnitDescription.Text;
                        updatedUnitInfo.AreaId = int.Parse(DropDownAreaList.SelectedValue);
                        updatedUnitInfo.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                        updatedUnitInfo.ExpiryDate = Convert.ToDateTime(TextBoxExpiryDate.Text);
                    }
                    var controller = new UnitController();
                    controller.UpdateUnit(updatedUnitInfo, userId);

                    CheckboxExpired.Checked = false;
                    DropDownUnit.Items.Clear();
                    DropDownUnit.DataBind();
                    DropDownUnit.Items.Insert(0, new ListItem("Choose a Unit", ""));
                    DropDownUnit.SelectedIndex = int.Parse(TextBoxUnitId.Text) - 1;
                    ButtonCancelUnit_Click(sender, e);

                    MessageUserControl.Visible = true;
                }


            }, "Success", "Unit has been updated.");
        }

        protected void CheckboxExpired_CheckedChanged(object sender, EventArgs e)
        {
            ButtonCancelUnit_Click(sender, e);

            if (CheckboxExpired.Checked == true)
            {
                DropDownUnit.Items.Clear();
                UnitListODS.SelectMethod = "UnitListExpired";

                DropDownUnit.Items.Insert(0, new ListItem("Choose a Unit", ""));
                DropDownUnit.SelectedIndex = 0;
                DropDownUnit.DataBind();
                ButtonUpdateUnit.Text = "Activate";

                //Disable fields
                TextBoxUnitName.Enabled = false;
                TextBoxUnitDescription.Enabled = false;
                DropDownAreaList.Enabled = false;
                TextBoxExpiryDate.Enabled = false;
                TextBoxStartDate.Enabled = false;
                ButtonCreateUnit.Visible = false;
            }
            else
            {

                DropDownUnit.Items.Clear();
                UnitListODS.SelectMethod = "UnitList";

                DropDownUnit.Items.Insert(0, new ListItem("Choose a Unit", ""));
                DropDownUnit.SelectedIndex = 0;
                DropDownUnit.DataBind();
                ButtonUpdateUnit.Text = "Update";

                //Enable fields
                TextBoxUnitName.Enabled = true;
                TextBoxUnitDescription.Enabled = true;
                DropDownAreaList.Enabled = true;
                TextBoxExpiryDate.Enabled = true;
                TextBoxStartDate.Enabled = true;
                ButtonCreateUnit.Visible = true;
            }
        }

        protected void ButtonDeleteUnit_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;

                UnitInformation unitInfo = new UnitInformation();
                if (TextBoxUnitName.Text == null)
                {
                    throw new Exception("Unit name cannot be empty.");
                }

                else if (TextBoxUnitDescription.Text == null)
                {
                    throw new Exception("Unit description cannot be empty.");
                }
                else if (DropDownAreaList.SelectedIndex == 0)
                {
                    throw new Exception("Please choose an area.");
                }
                else
                {
                    unitInfo.UnitId = int.Parse(TextBoxUnitId.Text);

                }
                var controller = new UnitController();
                controller.DeleteUnit(unitInfo.UnitId, userId);

                DropDownUnit.Items.Clear();
                DropDownUnit.DataBind();
                DropDownUnit.Items.Insert(0, new ListItem("Choose a Unit", ""));
                DropDownUnit.SelectedIndex = 0;

                ButtonCancelUnit_Click(sender, e);

                ButtonDeleteUnit.Visible = false;
                MessageUserControl.Visible = true;
            }, "Success", "Unit has been deleted.");
        }
    }
}