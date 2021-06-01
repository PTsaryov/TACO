using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOSystem.BLL;
using TACOSystem.BLL.Employee;
using TACOSystem.BLL.Security;

namespace TACOWebApp.Task
{
    public partial class CreateArea : System.Web.UI.Page
    {
        /// <summary>
        /// <para>
        /// This method will check the role and load the page appropriately.
        /// </para>
        /// Created By: Pavel Tsariyov
        /// Created On: 
        /// Modified By: Anton Drantiev 
        /// Modified On: April 22, 2019
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();

            if (Request.IsAuthenticated && (User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName && SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin"))
            {
                if (!IsPostBack)
                    CheckboxExpired.Checked = false;
            }
            else
            {
                all_content.Visible = false;
            }
        }

        /// <summary>
        /// <para>
        /// This method will lookup the selected area from the drop down list.
        /// </para>
        /// Created By: Pavel Tsaryov
        /// Created On: 
        /// Modified By: Anton Drantiev 
        /// Modified On: April 22,2019
        /// </summary>
        protected void ButtonAreaLookup_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            if (!CheckboxExpired.Checked)
            {
                ButtonAreaUpdate.Text = "Update";
            }
            // Get the name from the textbox
            var areaId = DropDownArea.SelectedValue;

            if (DropDownArea.SelectedIndex == 0)
            {
                MessageUserControl.ShowInfo("Please select an area to search");                
            }
            else
            {
                // Bring controller first
                var controller = new AreaController();
                var areaInfo = controller.AreaById(int.Parse(areaId));

                if (areaInfo == null)
                {
                    throw new Exception("Area does not exist or could not be found.");
                }
                else
                {
                    ButtonAreaUpdate.Visible = true;
                    MessageUserControl.Visible = false;
                    //unpack controller
                    TextBoxAreaId.Text = areaInfo.AreaId.ToString();
                    DropDownDepartment.SelectedValue = areaInfo.DepartmentId.ToString();
                    TextBoxAreaName.Text = areaInfo.AreaName;
                    TextBoxAreaDescription.Text = areaInfo.AreaDescription;
                    TextBoxStartDate.Text = areaInfo.StartDate.ToLongDateString();
                    TextBoxExpiryDate.Text = areaInfo.ExpiryDate.ToLongDateString();

                    ButtonAreaUpdate.Visible = true;
                    ButtonAreaCreate.Visible = false;
                    if (CheckboxExpired.Checked == false)
                    {
                        ButtonRemove.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// <para>
        /// This method will create a new area after checking all the required fields.
        /// </para>
        /// Created By: Pavel Tsaryov
        /// Created On: 
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        protected void ButtonAreaCreate_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                AreaDetails newArea = new AreaDetails();
                if (TextBoxAreaName.Text == "")
                {
                    throw new Exception("Area name cannot be empty.");
                }
                else if (TextBoxAreaDescription.Text == "")
                {
                    throw new Exception("Area description cannot be empty.");
                }
                else if (TextBoxStartDate.Text == "")
                {
                    throw new Exception("Start date cannot be empty.");
                }
                else if (TextBoxExpiryDate.Text == "")
                {
                    throw new Exception("Expiry date cannot be empty.");
                }
                else
                {
                    DateTime convertedDate;

                    newArea.AreaName = TextBoxAreaName.Text;
                    newArea.AreaDescription = TextBoxAreaDescription.Text;

                    if (DateTime.TryParse(TextBoxStartDate.Text, out convertedDate))
                    {
                        newArea.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                    }
                    else
                    {
                        throw new Exception("Start Date format error. Try using the following format: DD/MM/YYYY");
                    }
                    if (DateTime.TryParse(TextBoxExpiryDate.Text, out convertedDate))
                    {
                        newArea.ExpiryDate = Convert.ToDateTime(TextBoxExpiryDate.Text);
                    }
                    else
                    {
                        throw new Exception("Expiry Date format error. Try using the following format: DD/MM/YYYY");
                    }

                    //compare today's date to startdate.. startdate cannot be before today's date
                    int dateCompareResultStart = DateTime.Compare(newArea.StartDate, DateTime.Today);
                    if (dateCompareResultStart < 0) //if start date is earlier than todays date
                    {
                        throw new Exception("Start Date cannot be earlier than today's date. Please select another date.");
                    }
                    else
                    {
                        int dateCompareResultExpiry = DateTime.Compare(newArea.ExpiryDate, DateTime.Today);
                        if (dateCompareResultExpiry < 0) //if expiry date is earlier than todays date
                        {
                            throw new Exception("Expiry Date cannot be earlier than today's date. Please select another date.");
                        }
                        else
                        {
                            //compare expiry date to startdate.. expiry cannot be before start date
                            int dateCompareResult = DateTime.Compare(newArea.ExpiryDate, newArea.StartDate);
                            if (dateCompareResult < 0) //if expiry date is earlier than start date
                            {
                                throw new Exception("Expiry Date cannot be earlier than the Start Date. Please select another date.");
                            }
                            else if (dateCompareResult == 0)//if dates are the same
                            {
                                throw new Exception("Expiry Date is set to the same day as the Start Date. Please select another date.");
                            }
                            else //if expirydate is later than the start date
                            {
                                newArea.DepartmentId = int.Parse(DropDownDepartment.SelectedItem.Value);

                                var employeeController = new EmployeeController();

                                newArea.CreatedBy = employeeController.GetEmployeeIdByEmployeeNumber(); //retrieves id for CURRENTLY LOGGED IN USER
                                newArea.CreatedOn = DateTime.Today;
                            }
                        }
                    }
                }

                var controller = new AreaController();
                controller.CreateNewArea(newArea);

                CheckboxExpired.Checked = false;
                DropDownArea.Items.Clear();
                DropDownArea.DataBind();
                DropDownArea.Items.Insert(0, new ListItem("Select an Area", ""));
                DropDownArea.SelectedIndex = -1;
                Clear();
            }, "Success", TextBoxAreaName.Text + " Area created");

        }


        /// <summary>
        /// <para>
        /// This method will clear all the fields.
        /// </para>
        /// Created By: Pavel Tsaryov
        /// Created On: 
        /// Modified By: Anton Drantiev 
        /// Modified On: April 22,2019
        /// </summary>
        protected void ButtonAreaCancel_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;
            CheckboxExpired.Checked = false;
            // Enable 
            DropDownDepartment.Enabled = true;
            TextBoxAreaName.Enabled = true;
            TextBoxAreaDescription.Enabled = true;
            TextBoxStartDate.Enabled = true;
            TextBoxExpiryDate.Enabled = true;
            Clear();
        }

        /// <summary>
        /// <para>
        /// This method will update the selected area from the dropdown after checking all the required fields.
        /// </para>
        /// Created By: Pavel Tsaryov
        /// Created On: 
        /// Modified By: Anton Drantiev 
        /// Modified On: April 22,2019
        /// </summary>
        protected void ButtonAreaUpdate_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                if (CheckboxExpired.Checked)
                {
                    // Get security
                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    int userId = SecurityController.FindAccount(windowsAccountName).AccountID;
                    string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;

                    // Activate
                    AreaController controller = new AreaController();
                    controller.ActivateArea(userId, role, int.Parse(TextBoxAreaId.Text));

                    CheckboxExpired.Checked = false;
                    DropDownArea.Items.Clear();
                    DropDownArea.DataBind();
                    DropDownArea.Items.Insert(0, new ListItem("Select an Area", ""));
                    DropDownArea.SelectedIndex = -1;

                    // Enable 
                    DropDownDepartment.Enabled = true;
                    TextBoxAreaName.Enabled = true;
                    TextBoxAreaDescription.Enabled = true;
                    TextBoxStartDate.Enabled = true;
                    TextBoxExpiryDate.Enabled = true;


                    // Clear
                    DropDownArea.SelectedIndex = -1;
                    TextBoxAreaId.Text = "";
                    DropDownDepartment.SelectedIndex = -1;
                    TextBoxAreaName.Text = "";
                    TextBoxAreaDescription.Text = "";
                    TextBoxStartDate.Text = "";
                    TextBoxExpiryDate.Text = "";
                   


                }
                else
                {
                    AreaDetails updatedArea = new AreaDetails();
                    if (TextBoxAreaName.Text == "")
                    {
                        throw new Exception("Area name cannot be empty.");
                    }
                    else if (TextBoxAreaDescription.Text == "")
                    {
                        throw new Exception("Area description cannot be empty.");
                    }
                    else if (TextBoxStartDate.Text == "")
                    {
                        throw new Exception("Start date cannot be empty.");
                    }
                    else if (TextBoxExpiryDate.Text == "")
                    {
                        throw new Exception("Expiry date cannot be empty.");
                    }
                    else
                    {
                        DateTime convertedDate;

                        if (DateTime.TryParse(TextBoxStartDate.Text, out convertedDate))
                        {
                            updatedArea.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                        }
                        else
                        {
                            throw new Exception("Start Date format error. Try using the following format: DD/MM/YYYY");
                        }
                        if (DateTime.TryParse(TextBoxExpiryDate.Text, out convertedDate))
                        {
                            updatedArea.ExpiryDate = Convert.ToDateTime(TextBoxExpiryDate.Text);
                        }
                        else
                        {
                            throw new Exception("Expiry Date format error. Try using the following format: DD/MM/YYYY");
                        }

                        int dateCompareResultExpiry = DateTime.Compare(updatedArea.ExpiryDate, DateTime.Today);
                        if (dateCompareResultExpiry < 0) //if expiry date is earlier than todays date
                        {
                            throw new Exception("Expiry Date cannot be earlier than today's date. Please select another date.");
                        }
                        else
                        {
                            //compare expiry date to startdate.. expiry cannot be before start date
                            int dateCompareResult = DateTime.Compare(updatedArea.ExpiryDate, updatedArea.StartDate);
                            if (dateCompareResult < 0) //if expiry date is earlier than start date
                            {
                                throw new Exception("Expiry Date cannot be earlier than the Start Date. Please select another date.");
                            }
                            else if (dateCompareResult == 0)//if dates are the same
                            {
                                throw new Exception("Expiry Date is set to the same day as the Start Date. Please select another date.");
                            }
                            else //if expirydate is later than the start date
                            {
                                updatedArea.AreaId = int.Parse(TextBoxAreaId.Text);
                                updatedArea.DepartmentId = int.Parse(DropDownDepartment.SelectedItem.Value);
                                updatedArea.AreaName = TextBoxAreaName.Text;
                                updatedArea.AreaDescription = TextBoxAreaDescription.Text;
                                updatedArea.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                                updatedArea.ExpiryDate = Convert.ToDateTime(TextBoxExpiryDate.Text);

                                var employeeController = new EmployeeController();

                                var windowsAccountName = User.Identity.Name.Split('\\').Last();

                                updatedArea.ModifiedBy = SecurityController.FindAccount(windowsAccountName).AccountID; //retrieves id for CURRENTLY LOGGED IN USER 
                                updatedArea.ModifiedOn = DateTime.Today;

                                var controller = new AreaController();
                                controller.UpdateArea(updatedArea);
                            }
                        }
                    }

                    CheckboxExpired.Checked = false;
                    DropDownArea.Items.Clear();
                    DropDownArea.DataBind();
                    DropDownArea.Items.Insert(0, new ListItem("Select an Area", ""));
                    DropDownArea.SelectedIndex = -1;
                }

            }, "Success", TextBoxAreaName.Text + " has been updated.");
         
        }


        /// <summary>
        /// <para>
        /// This method will deactivate the selected area from the dropdown.
        /// </para>
        /// Created By: Pavel Tsaryov
        /// Created On: 
        /// Modified By: Prince Selhi
        /// Modified On: April 22,2019 
        /// </summary>
        protected void ButtonRemove_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                AreaDetails removedArea = new AreaDetails();
                if (TextBoxAreaName.Text == "")
                {
                    throw new Exception("Area name cannot be empty.");
                }
                else if (TextBoxAreaDescription.Text == "")
                {
                    throw new Exception("Area description cannot be empty.");
                }
                else if (TextBoxStartDate.Text == "")
                {
                    throw new Exception("Start date cannot be empty.");
                }
                else if (TextBoxExpiryDate.Text == "")
                {
                    throw new Exception("Expiry date cannot be empty.");
                }
                else
                {
                    DateTime convertedDate;

                    if (DateTime.TryParse(TextBoxStartDate.Text, out convertedDate))
                    {
                        removedArea.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                    }
                    else
                    {
                        throw new Exception("Start Date format error. Try using the following format: DD/MM/YYYY");
                    }
                    if (DateTime.TryParse(TextBoxExpiryDate.Text, out convertedDate))
                    {
                        removedArea.ExpiryDate = Convert.ToDateTime(TextBoxExpiryDate.Text);
                    }
                    else
                    {
                        throw new Exception("Expiry Date format error. Try using the following format: DD/MM/YYYY");
                    }

                    removedArea.AreaId = int.Parse(TextBoxAreaId.Text);
                    removedArea.DepartmentId = int.Parse(DropDownDepartment.SelectedItem.Value);
                    removedArea.AreaName = TextBoxAreaName.Text;
                    removedArea.AreaDescription = TextBoxAreaDescription.Text;
                    removedArea.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                    removedArea.ExpiryDate = DateTime.Today;

                    var employeeController = new EmployeeController();

                    removedArea.ModifiedBy = employeeController.GetEmployeeIdByEmployeeNumber(); //retrieves id for CURRENTLY LOGGED IN USER
                    removedArea.ModifiedOn = DateTime.Today;

                }

                var controller = new AreaController();
                controller.UpdateArea(removedArea);

                CheckboxExpired.Checked = false;
                DropDownArea.Items.Clear();
                DropDownArea.DataBind();
                DropDownArea.Items.Insert(0, new ListItem("Select an Area", ""));
                DropDownArea.SelectedIndex = -1;

                // Clear
                DropDownArea.SelectedIndex = -1;
                TextBoxAreaId.Text = "";
                DropDownDepartment.SelectedIndex = -1;
                TextBoxAreaName.Text = "";
                TextBoxAreaDescription.Text = "";
                TextBoxStartDate.Text = "";
                TextBoxExpiryDate.Text = "";

            }, "Success", TextBoxAreaName.Text + " has been removed / expired.");

            
        }

        /// <summary>
        /// <para>
        /// This method will check if expired check box is check and display the page accordingly.
        /// </para>
        /// Created By: 
        /// Created On: 
        /// Modified By: Anton Drantiev 
        /// Modified On: April 22,2019 
        /// </summary>
        protected void CheckboxExpired_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckboxExpired.Checked)
            {
                MessageUserControl.Visible = false;
                DropDownArea.Items.Clear();
                ODSArea.SelectMethod = "ExpiredAreaList";

                DropDownArea.Items.Insert(0, new ListItem("Select an Area", ""));
                DropDownArea.SelectedIndex = 0;
                DropDownArea.DataBind();

                ButtonAreaUpdate.Visible = false;
                ButtonAreaUpdate.Text = "Activate";
                // Disable 
                DropDownDepartment.Enabled = false;
                TextBoxAreaName.Enabled = false;
                TextBoxAreaDescription.Enabled = false;
                TextBoxStartDate.Enabled = false;
                TextBoxExpiryDate.Enabled = false;
                ButtonAreaCreate.Visible = false;
                ButtonRemove.Visible = false;
                
            }
            else
            {
                MessageUserControl.Visible = false;
                DropDownArea.Items.Clear();
                ODSArea.SelectMethod = "AreaList";

                DropDownArea.Items.Insert(0, new ListItem("Select an Area", ""));
                DropDownArea.SelectedIndex = 0;
                DropDownArea.DataBind();

                ButtonAreaUpdate.Text = "Update";
                // Enable 
                DropDownDepartment.Enabled = true;
                TextBoxAreaName.Enabled = true;
                TextBoxAreaDescription.Enabled = true;
                TextBoxStartDate.Enabled = true;
                TextBoxExpiryDate.Enabled = true;
            }
        }

        
        public void Clear()
        {
            DropDownArea.SelectedIndex = -1;
            TextBoxAreaId.Text = "";
            DropDownDepartment.SelectedIndex = -1;
            TextBoxAreaName.Text = "";
            TextBoxAreaDescription.Text = "";
            TextBoxStartDate.Text = "";
            TextBoxExpiryDate.Text = "";

            ButtonAreaUpdate.Visible = false;
            ButtonRemove.Visible = false;
            Response.Redirect(Request.RawUrl);
            ButtonAreaCreate.Visible = true;
        }
    }
}