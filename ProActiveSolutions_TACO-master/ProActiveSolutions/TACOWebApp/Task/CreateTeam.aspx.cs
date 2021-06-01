using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOSystem.BLL;
using TACOSystem.BLL.Security;
using TACOSystem.BLL.Employee;

namespace TACOWebApp.Task
{
    public partial class CreateTeam : System.Web.UI.Page
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

        protected void ButtonLookup_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            
            //get the id from the ddl
            var teamId = DropDownTeam.SelectedValue;
            if (DropDownTeam.SelectedIndex == 0)
            {
                MessageUserControl.ShowInfo("Please select a team to search.");
                Clear();
            }
            else
            {
                if (teamId != string.Empty)
                {
                    MessageUserControl.Visible = false;
                    //bring controller first
                    var controller = new TeamController();
                    var teamInfo = controller.TeamById(int.Parse(teamId));

                    //unpack controller
                    TextBoxTeamId.Text = teamInfo.TeamId.ToString();
                    DropDownUnit.SelectedValue = teamInfo.UnitId.ToString();
                    TextBoxTeamName.Text = teamInfo.TeamName;
                    TextBoxDescription.Text = teamInfo.TeamDescription;
                    TextBoxStartDate.Text = teamInfo.StartDate.ToLongDateString();
                    TextBoxExpiryDate.Text = teamInfo.ExpiryDate.ToLongDateString();
                    TextBoxCreatedBy.Text = teamInfo.CreatedBy.ToString();
                    TextBoxCreatedOn.Text = teamInfo.CreatedOn.ToLongDateString();
                    TextBoxModifiedBy.Text = teamInfo.ModifiedBy.ToString();
                    TextBoxModifiedOn.Text = teamInfo.ModifiedOn.ToString();

                    if (CheckboxExpired.Checked == true)
                    {
                        ButtonUpdate.Visible = false;
                        ButtonDeactivate.Visible = false;
                        ButtonActivateTeam.Visible = true;
                        TextBoxTeamName.Enabled = false;
                        TextBoxDescription.Enabled = false;
                        TextBoxExpiryDate.Enabled = false;
                        DropDownUnit.Enabled = false;
                        TextBoxStartDate.Enabled = false;
                        ButtonActivateTeam.Visible = true;
                        ButtonCreate.Visible = false;
                    }
                    else
                    {
                        TextBoxStartDate.Enabled = false;
                        ButtonUpdate.Visible = true;
                        ButtonDeactivate.Visible = true;
                        ButtonCreate.Visible = false;
                    }
                }
                else
                {
                    throw new Exception("Team does not exist or cannot be found.");
                }
            }  
        }

        protected void ButtonCreate_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                TeamInformation newTeam = new TeamInformation();
                if (TextBoxTeamName.Text == "")
                {
                    throw new Exception("Team name cannot be empty.");
                }
                else if (TextBoxDescription.Text == "")
                {
                    throw new Exception("Team description cannot be empty.");
                }
                else if (TextBoxStartDate.Text == "")
                {
                    throw new Exception("Start date cannot be empty.");
                }
                
                else
                {
                    DateTime convertedDate;

                    newTeam.TeamName = TextBoxTeamName.Text;
                    newTeam.TeamDescription = TextBoxDescription.Text;

                    if (DateTime.TryParse(TextBoxStartDate.Text, out convertedDate))
                    {
                        newTeam.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                    }
                    else
                    {
                        throw new Exception("Start Date format error. Try using the following format: DD/MM/YYYY");
                    }
                    if (DateTime.TryParse(TextBoxExpiryDate.Text, out convertedDate))
                    {
                        newTeam.ExpiryDate = Convert.ToDateTime(TextBoxExpiryDate.Text);
                    }

                    else if (DateTime.TryParse("December 31, 9999", out convertedDate))
                    {
                        newTeam.ExpiryDate = convertedDate;
                    }


                    else
                    {
                        throw new Exception("Expiry Date format error. Try using the following format: DD/MM/YYYY");
                    }

                    //compare today's date to startdate.. startdate cannot be before today's date
                    int dateCompareResultStart = DateTime.Compare(newTeam.StartDate, DateTime.Today);
                    if (dateCompareResultStart < 0) //if start date is earlier than todays date
                    {
                        throw new Exception("Start Date cannot be earlier than today's date. Please select another date.");
                    }
                    else
                    {
                        int dateCompareResultExpiry = DateTime.Compare(newTeam.ExpiryDate, DateTime.Today);
                        if (dateCompareResultExpiry < 0) //if expiry date is earlier than todays date
                        {
                            throw new Exception("Expiry Date cannot be earlier than today's date. Please select another date.");
                        }
                        else
                        {
                            //compare expiry date to startdate.. expiry cannot be before start date
                            int dateCompareResult = DateTime.Compare(newTeam.ExpiryDate, newTeam.StartDate);
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
                                newTeam.UnitId = int.Parse(DropDownUnit.SelectedItem.Value);

                                var employeeController = new EmployeeController();

                                newTeam.CreatedBy = employeeController.GetEmployeeIdByEmployeeNumber(); //retrieves id for CURRENTLY LOGGED IN USER  
                                newTeam.CreatedOn = DateTime.Today;                                
                            }
                        }
                    }
                }

                var controller = new TeamController();
                controller.CreateNewTeam(newTeam);

                CheckboxExpired.Checked = false;
                DropDownTeam.Items.Clear();
                DropDownTeam.DataBind();
                DropDownTeam.Items.Insert(0, new ListItem("Select a Team", ""));
                DropDownTeam.SelectedIndex = -1;

            }, "Success", TextBoxTeamName.Text + " Team created");
            Clear();
        }

        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                TeamInformation updatedTeam = new TeamInformation();
                if (TextBoxTeamName.Text == "")
                {
                    throw new Exception("Team name cannot be empty.");
                }
                else if (TextBoxDescription.Text == "")
                {
                    throw new Exception("Team description cannot be empty.");
                }
                else if (TextBoxStartDate.Text == "")
                {
                    throw new Exception("Start date cannot be empty.");
                }
               
                else if (TextBoxCreatedBy.Text == "")
                {
                    throw new Exception("Created by cannot be empty.");
                }
                else if (TextBoxModifiedBy.Text == "")
                {
                    throw new Exception("Modified by cannot be empty.");
                }
                else
                {
                    DateTime convertedDate;

                    if (DateTime.TryParse(TextBoxStartDate.Text, out convertedDate))
                    {
                        updatedTeam.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                    }
                    else
                    {
                        throw new Exception("Start Date format error. Try using the following format: DD/MM/YYYY");
                    }
                    if (DateTime.TryParse(TextBoxExpiryDate.Text, out convertedDate))
                    {
                        updatedTeam.ExpiryDate = Convert.ToDateTime(TextBoxExpiryDate.Text);
                    }

                    else if (DateTime.TryParse("December 31, 9999", out convertedDate))
                    {
                        updatedTeam.ExpiryDate = convertedDate;
                    }

                    else
                    {
                        throw new Exception("Expiry Date format error. Try using the following format: DD/MM/YYYY");
                    }

                    int dateCompareResultExpiry = DateTime.Compare(updatedTeam.ExpiryDate, DateTime.Today);
                    if (dateCompareResultExpiry < 0) //if expiry date is earlier than todays date
                    {
                        throw new Exception("Expiry Date cannot be earlier than today's date. Please select another date.");
                    }
                    else
                    {
                        //compare expiry date to startdate.. expiry cannot be before start date
                        int dateCompareResult = DateTime.Compare(updatedTeam.ExpiryDate, updatedTeam.StartDate);
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
                            updatedTeam.TeamId = int.Parse(TextBoxTeamId.Text);
                            updatedTeam.UnitId = int.Parse(DropDownUnit.SelectedItem.Value);
                            updatedTeam.TeamName = TextBoxTeamName.Text;
                            updatedTeam.TeamDescription = TextBoxDescription.Text;
                            updatedTeam.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                            updatedTeam.ExpiryDate = Convert.ToDateTime(TextBoxExpiryDate.Text);

                            var employeeController = new EmployeeController();

                            updatedTeam.ModifiedBy = employeeController.GetEmployeeIdByEmployeeNumber(); //retrieves id for CURRENTLY LOGGED IN USER 
                            updatedTeam.ModifiedOn = DateTime.Today;
                        }
                    }
                }

                var controller = new TeamController();

                CheckboxExpired.Checked = false;
                DropDownTeam.Items.Clear();
                DropDownTeam.DataBind();
                DropDownTeam.Items.Insert(0, new ListItem("Select a Team", ""));
                DropDownTeam.SelectedIndex = -1;
                TextBoxStartDate.Enabled = true;
                controller.UpdateTeam(updatedTeam);
            }, "Success", TextBoxTeamName.Text + " has been updated.");
            Clear();
        }

        protected void ButtonDeactivate_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                TeamInformation removedTeam = new TeamInformation();
                if (TextBoxTeamName.Text == "")
                {
                    throw new Exception("Team name cannot be empty.");
                }
                else if (TextBoxDescription.Text == "")
                {
                    throw new Exception("Team description cannot be empty.");
                }
                else if (TextBoxStartDate.Text == "")
                {
                    throw new Exception("Start date cannot be empty.");
                }
                else if (TextBoxExpiryDate.Text == "")
                {
                    throw new Exception("Expiry date cannot be empty.");
                }
                else if (TextBoxCreatedBy.Text == "")
                {
                    throw new Exception("Created by cannot be empty.");
                }
                else
                {
                    DateTime convertedDate;

                    if (DateTime.TryParse(TextBoxStartDate.Text, out convertedDate))
                    {
                        removedTeam.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                    }
                    else
                    {
                        throw new Exception("Start Date format error. Try using the following format: DD/MM/YYYY");
                    }
                    if (DateTime.TryParse(TextBoxExpiryDate.Text, out convertedDate))
                    {
                        removedTeam.ExpiryDate = Convert.ToDateTime(TextBoxExpiryDate.Text);
                    }
                    else
                    {
                        throw new Exception("Expiry Date format error. Try using the following format: DD/MM/YYYY");
                    }

                    removedTeam.TeamId = int.Parse(TextBoxTeamId.Text);
                    removedTeam.UnitId = int.Parse(DropDownUnit.SelectedItem.Value);
                    removedTeam.TeamName = TextBoxTeamName.Text;
                    removedTeam.TeamDescription = TextBoxDescription.Text;
                    removedTeam.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                    removedTeam.ExpiryDate = DateTime.Today;
                    removedTeam.CreatedBy = int.Parse(TextBoxCreatedBy.Text);
                    removedTeam.CreatedOn = Convert.ToDateTime(TextBoxCreatedOn.Text);

                    var employeeController = new EmployeeController();

                    removedTeam.ModifiedBy = employeeController.GetEmployeeIdByEmployeeNumber(); //retrieves id for CURRENTLY LOGGED IN USER 
                    removedTeam.ModifiedOn = DateTime.Today;
                }

                var controller = new TeamController();
                controller.UpdateTeam(removedTeam);

                CheckboxExpired.Checked = false;
                DropDownTeam.Items.Clear();
                DropDownTeam.DataBind();
                DropDownTeam.Items.Insert(0, new ListItem("Select a Team", ""));
                DropDownTeam.SelectedIndex = -1;
                TextBoxStartDate.Enabled = true;
            }, "Success", TextBoxTeamName.Text + " has been deactivated / expired.");

            Clear();
        }

        protected void CheckboxExpired_CheckedChanged(object sender, EventArgs e)
        {
            Clear();

            if (CheckboxExpired.Checked == true)
            {
                MessageUserControl.Visible = false;
                DropDownTeam.Items.Clear();
                ODSTeam.SelectMethod = "ExpiredTeamDetails";

                DropDownTeam.Items.Insert(0, new ListItem("Select a Team", ""));
                DropDownTeam.SelectedIndex = 0;
                DropDownTeam.DataBind();

                ButtonCreate.Visible = false;
            
            }
            else
            {
                MessageUserControl.Visible = false;
                DropDownTeam.Items.Clear();
                ODSTeam.SelectMethod = "TeamDetails";
                TextBoxStartDate.Enabled = true;
                DropDownTeam.Items.Insert(0, new ListItem("Select a Team", ""));
                DropDownTeam.SelectedIndex = 0;
                DropDownTeam.DataBind();

                ButtonActivateTeam.Visible = false;
                TextBoxTeamName.Enabled = true;
                TextBoxDescription.Enabled = true;
                TextBoxExpiryDate.Enabled = true;
                TextBoxStartDate.Enabled = true;
                TextBoxDescription.Enabled = true;
                DropDownUnit.Enabled = true;
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;
            Clear();
            Response.Redirect(Request.RawUrl);
        }

        public void Clear()
        {
            TextBoxTeamId.Text = "";
            DropDownUnit.SelectedIndex = -1;
            TextBoxTeamName.Text = "";
            TextBoxDescription.Text = "";
            TextBoxStartDate.Text = "";
            TextBoxExpiryDate.Text = "";
            TextBoxCreatedBy.Text = "";
            TextBoxCreatedOn.Text = "";
            TextBoxModifiedBy.Text = "";
            TextBoxModifiedOn.Text = "";

            ButtonUpdate.Visible = false;
            ButtonDeactivate.Visible = false;
            ButtonCreate.Visible = true;
        }

        protected void ButtonActivateTeam_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                TeamInformation activatedteaminfo = new TeamInformation();
                if (TextBoxTeamId.Text == null)
                {
                    throw new Exception("Please select a Team to Activate");
                }
                else
                {
                    activatedteaminfo.TeamId = int.Parse(TextBoxTeamId.Text);
                    activatedteaminfo.ExpiryDate = DateTime.Parse("December 31, 9999");
                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    activatedteaminfo.ModifiedBy = SecurityController.FindAccount(windowsAccountName).AccountID;

                    TextBoxTeamId.Text = "";
                    TextBoxTeamName.Text = "";
                    TextBoxDescription.Text = "";
                    TextBoxExpiryDate.Text = "";
                    TextBoxStartDate.Text = "";
                }

                var controller = new TeamController();
                controller.ActivateTeam(activatedteaminfo);
                DropDownTeam.Items.Clear();
                DropDownTeam.DataBind();
                DropDownTeam.Items.Insert(0, new ListItem("Choose a Team", ""));
                DropDownTeam.SelectedIndex = -1;

                CheckboxExpired.Checked = false;
                ButtonActivateTeam.Visible = false;

            }, "Success", "Department : " + TextBoxTeamName.Text + " has been activated.");

            ButtonCreate.Visible = true;
            ButtonUpdate.Visible = false;
            ButtonDeactivate.Visible = false;
            TextBoxTeamName.Enabled = true;
            TextBoxDescription.Enabled = true;
            TextBoxExpiryDate.Enabled = true;
            DropDownUnit.Enabled = true;
            TextBoxStartDate.Enabled = true;
        }
    }
}