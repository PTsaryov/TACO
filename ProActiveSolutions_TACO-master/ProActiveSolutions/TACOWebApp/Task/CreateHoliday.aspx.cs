using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOSystem.BLL.Employee;
using TACOSystem.BLL;
using TACOSystem.BLL.Security;

namespace TACOWebApp.Task
{
    public partial class CreateHoliday : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();

            if (Request.IsAuthenticated && (User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName && SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin"))
            {
                all_content.Visible = true;

                if (!IsPostBack)
                {
                    CheckboxExpired.Checked = false;
                }
            }
            else
            {
                all_content.Visible = false;
            }
        }

        protected void ButtonHolidayLookup_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            if (DropDownHoliday.SelectedIndex == 0)
            {
                MessageUserControl.ShowInfo("Please select a holiday to search");
                Clear();
            }
            else
            {
                MessageUserControl.Visible = false;
                //get the id from the ddl
                var holidayId = DropDownHoliday.SelectedValue;

                if (holidayId != string.Empty)
                {
                    //bring controller first
                    var controller = new HolidayController();
                    var holidayInfo = controller.HolidayById(int.Parse(holidayId));

                    //unpack controller
                    TextBoxHolidayId.Text = holidayInfo.HolidayId.ToString();
                    TextBoxHolidayName.Text = holidayInfo.HolidayName;
                    TextBoxDate.Text = holidayInfo.Date.ToLongDateString();                   
                    CheckBoxHolidayDeactivated.Checked = holidayInfo.HolidayDeactivated;

                    
                    ButtonHolidayUpdate.Visible = true;
                    ButtonHolidayCreate.Visible = false;
                    if (CheckboxExpired.Checked == true)
                    {
                        ButtonHolidayUpdate.Visible = false;
                        ButtonHolidayDeactivate.Visible = false;
                        ButtonActivateHoliday.Visible = true;
                        TextBoxHolidayName.Enabled = false;
                        TextBoxDate.Enabled = false;
                        CheckBoxHolidayDeactivated.Enabled = false;
                    }
                    else
                    {
                        ButtonHolidayUpdate.Visible = true;
                        ButtonHolidayDeactivate.Visible = true;
                    }
                }
                else
                {
                    throw new Exception("Holiday does not exist");
                }
            }
        }

        protected void ButtonHolidayCreate_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                HolidayInformation newHoliday = new HolidayInformation();

                if (TextBoxHolidayName.Text == "")
                {
                    throw new Exception("Holiday name cannot be empty.");
                }
                
                else if (CheckBoxHolidayDeactivated.Checked == true)
                {
                    throw new Exception("Holiday must be active. (Cannot have deactivated checked).");
                }
                else
                {
                    DateTime convertedDate;
                    
                    newHoliday.HolidayName = TextBoxHolidayName.Text;

                    if (DateTime.TryParse(TextBoxDate.Text, out convertedDate))
                    {
                        newHoliday.Date = Convert.ToDateTime(TextBoxDate.Text);
                    }
                    else
                    {
                        throw new Exception("Date format error. Try using the following format: DD/MM/YYYY");
                    }

                    newHoliday.HolidayDeactivated = false;

                    var employeeController = new EmployeeController();

                    newHoliday.CreatedBy = employeeController.GetEmployeeIdByEmployeeNumber(); //retrieves id for CURRENTLY LOGGED IN USER
                    newHoliday.CreatedOn = DateTime.Today;
                }

                var controller = new HolidayController();
                controller.CreateNewHoliday(newHoliday);

                CheckboxExpired.Checked = false;
                DropDownHoliday.Items.Clear();
                DropDownHoliday.DataBind();
                DropDownHoliday.Items.Insert(0, new ListItem("Select a Holiday", ""));
                DropDownHoliday.SelectedIndex = -1;

            }, "Success", TextBoxHolidayName.Text + " Holiday created");

            Clear();
        }

        protected void ButtonHolidayUpdate_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                HolidayInformation updatedHoliday = new HolidayInformation();

                if (TextBoxHolidayName.Text == "")
                {
                    throw new Exception("Holiday name cannot be empty.");
                }
                else if (TextBoxDate.Text == "")
                {
                    throw new Exception("Date cannot be empty.");
                }
                else
                {
                    DateTime convertedDate;

                    updatedHoliday.HolidayId = int.Parse(TextBoxHolidayId.Text);
                    updatedHoliday.HolidayName = TextBoxHolidayName.Text;

                    if (DateTime.TryParse(TextBoxDate.Text, out convertedDate))
                    {
                        updatedHoliday.Date = convertedDate;
                    }
                    else
                    {
                        throw new Exception("Date format error. Try using the following format: DD/MM/YYYY");
                    }

                    updatedHoliday.HolidayDeactivated = CheckBoxHolidayDeactivated.Checked;
                    
                    var employeeController = new EmployeeController();

                    updatedHoliday.ModifiedBy = employeeController.GetEmployeeIdByEmployeeNumber(); //retrieves id for CURRENTLY LOGGED IN USER
                    updatedHoliday.ModifiedOn = DateTime.Today;
                    
                }

                var controller = new HolidayController();
                controller.UpdateHoliday(updatedHoliday);

                CheckboxExpired.Checked = false;
                DropDownHoliday.Items.Clear();
                DropDownHoliday.DataBind();
                DropDownHoliday.Items.Insert(0, new ListItem("Select a Holiday", ""));
                DropDownHoliday.SelectedIndex = -1;

            }, "Success", TextBoxHolidayName.Text + " Holiday updated");
            Clear();
        }

        protected void ButtonRemove_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                HolidayInformation deactivatedHoliday = new HolidayInformation();
                if (TextBoxHolidayName.Text == "")
                {
                    throw new Exception("Holiday name cannot be empty.");
                }                
                else if (TextBoxDate.Text == "")
                {
                    throw new Exception("Date cannot be empty.");
                }
                else
                {
                    DateTime convertedDate;

                    deactivatedHoliday.HolidayId = int.Parse(TextBoxHolidayId.Text);
                    deactivatedHoliday.HolidayName = TextBoxHolidayName.Text;

                    if (DateTime.TryParse(TextBoxDate.Text, out convertedDate))
                    {
                        deactivatedHoliday.Date = convertedDate;
                    }
                    else
                    {
                        throw new Exception("Date format error. Try using the following format: DD/MM/YYYY");
                    }

                    deactivatedHoliday.HolidayDeactivated = true;
                    
                    var employeeController = new EmployeeController();

                    deactivatedHoliday.ModifiedBy = employeeController.GetEmployeeIdByEmployeeNumber(); //retrieves id for CURRENTLY LOGGED IN USER
                    deactivatedHoliday.ModifiedOn = DateTime.Today;
                    

                }

                var controller = new HolidayController();
                controller.UpdateHoliday(deactivatedHoliday);

                CheckboxExpired.Checked = false;
                DropDownHoliday.Items.Clear();
                DropDownHoliday.DataBind();
                DropDownHoliday.Items.Insert(0, new ListItem("Select a Holiday", ""));
                DropDownHoliday.SelectedIndex = -1;

            }, "Success", TextBoxHolidayName.Text + " Holiday removed / deactivated");
            Clear();
        }

        protected void CheckboxExpired_CheckedChanged(object sender, EventArgs e)
        {
            Clear();

            if (CheckboxExpired.Checked == true)
            {
                MessageUserControl.Visible = false;
                DropDownHoliday.Items.Clear();
                ODSHoliday.SelectMethod = "ExpiredHolidayList";

                DropDownHoliday.Items.Insert(0, new ListItem("Select a Holiday", ""));
                DropDownHoliday.SelectedIndex = 0;
                DropDownHoliday.DataBind();

                ButtonHolidayUpdate.Visible = false;
                ButtonHolidayCreate.Visible = false;
            }
            else
            {
                MessageUserControl.Visible = false;
                DropDownHoliday.Items.Clear();
                ODSHoliday.SelectMethod = "HolidayList";

                DropDownHoliday.Items.Insert(0, new ListItem("Select a Holiday", ""));
                DropDownHoliday.SelectedIndex = 0;
                DropDownHoliday.DataBind();

                ButtonActivateHoliday.Visible = false;
                TextBoxHolidayName.Enabled = true;
                TextBoxDate.Enabled = true;
                CheckBoxHolidayDeactivated.Enabled = true;
            }
        }

        protected void ButtonHolidayCancel_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;
            Clear();
            Response.Redirect(Request.RawUrl);
        }

        public void Clear()
        {
            TextBoxHolidayId.Text = "";
            TextBoxHolidayName.Text = "";
            TextBoxDate.Text = "";
            TextBoxCreatedBy.Text = "";
            TextBoxCreatedOn.Text = "";
            TextBoxModifiedBy.Text = "";
            TextBoxModifiedOn.Text = "";
            CheckBoxHolidayDeactivated.Checked = false;

            ButtonHolidayUpdate.Visible = false;
            ButtonHolidayDeactivate.Visible = false;
            ButtonHolidayCreate.Visible = true;
        }

        protected void ButtonActivateHoliday_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                HolidayInformation activatedholidayinfo = new HolidayInformation();
                if (TextBoxHolidayId.Text == null)
                {
                    throw new Exception("Please select a holiday to Activate");
                }
                else
                {
                    activatedholidayinfo.HolidayId = int.Parse(TextBoxHolidayId.Text);                    
                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    activatedholidayinfo.ModifiedBy = SecurityController.FindAccount(windowsAccountName).AccountID;

                    TextBoxHolidayId.Text = "";
                    TextBoxHolidayName.Text = "";
                    TextBoxDate.Text = "";
                }

                var controller = new HolidayController();
                controller.ActivateHoliday(activatedholidayinfo);
                DropDownHoliday.Items.Clear();
                DropDownHoliday.DataBind();
                DropDownHoliday.Items.Insert(0, new ListItem("Choose a Departrment", ""));
                DropDownHoliday.SelectedIndex = -1;

                CheckboxExpired.Checked = false;
                ButtonActivateHoliday.Visible = false;

            }, "Success", "Holiday : " + TextBoxHolidayName.Text + " has been activated.");

            ButtonHolidayCreate.Visible = true;
            ButtonHolidayUpdate.Visible = false;
            ButtonHolidayDeactivate.Visible = false;
            TextBoxHolidayName.Enabled = true;
            TextBoxDate.Enabled = true;
            CheckBoxHolidayDeactivated.Enabled = true;
            CheckBoxHolidayDeactivated.Checked = false;
        }
    }
}