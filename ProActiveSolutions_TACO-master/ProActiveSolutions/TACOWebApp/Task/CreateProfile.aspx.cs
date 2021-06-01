using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOSystem.BLL;
using TACOSystem.BLL.Employee;
using TACOSystem.BLL.Security;

namespace TACOWebApp.Task
{
    public partial class CreateProfile : System.Web.UI.Page
    {   /// <summary>
        /// Executes the methods that are supposed to fire on PageLoad
        /// Checks for security happens in this method
        /// Created By: Emily Urdaneta
        /// Created On: February 5,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();
            int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
            var role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName)
            {
            }
            else
            {// not authorized will hide content and diplay unauthorized page message
                all_content.Visible = false;
            }
            if (!IsPostBack)
            {
                TimesheetController timesheetController = new TimesheetController();
                DropdownTeamMember.DataSource = timesheetController.AllTeams();
                DropdownTeamMember.DataBind();
            }


        }
        /// <summary>
        /// Method to check for exceptions thrown. 
        /// Displays it in the message user control in the web page
        /// Created By: Emily Urdaneta
        /// Created On: February 5,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckForExceptions(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }
        #region Button Events
        /// <summary>
        /// Listens for the click event on the search button when looking up for an employee
        /// Returns an employee object
        /// Created By: Emily Urdaneta
        /// Created On: February 5,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonLookup_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {

                var fullName = TextBoxLookup.Text;

                if (fullName != string.Empty)
                {

                    var controller = new EmployeeController();
                    var employeeInfo = controller.GetEmployee(fullName);

                    var employee = employeeInfo.EmployeeId;
                    PopulateEmployeeProfile(employeeInfo);
                    ButtonTerminate.Visible = true;
                    ButtonUpdate.Visible = true;
                    ButtonCreateProfile.Visible = false;
                    MessageUserControl.Visible = true;
                }
                else
                {
                    throw new Exception("Please fill out the textbox to look for the employee");
                }



            }, "Success", "Employee profile retrieved");
        }

        /// <summary>
        /// Listens for the click event on the create button
        /// Validation is done in this method before the object gets sent to the controller
        /// Returns an employee object
        /// Created By: Emily Urdaneta
        /// Created On: February 5,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonCreateProfile_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
                MessageUserControl.Visible = true;
                EmployeeController controller = new EmployeeController();

                EmployeeProfile newEmployeeProfile = new EmployeeProfile();
                if (TextBoxEmployeeNumber.Text == null)
                {
                    throw new Exception("Employee number cannot be empty.");
                }
                else
                {
                    DateTime convertedDate;
                    var today = DateTime.Today;


                    //newEmployeeProfile.EmployeeId = int.Parse(TextBoxEmployeeId.Text);
                    if (TextBoxEmployeeNumber.Text == "")
                    {
                        throw new Exception("Employee number cannot be empty");
                    }
                    else
                    {
                        var doesEmployeeNumberAlreadyExist = controller.EmployeeNumberExists(TextBoxEmployeeNumber.Text);
                        if (!doesEmployeeNumberAlreadyExist)
                        {
                            newEmployeeProfile.EmployeeNumber = TextBoxEmployeeNumber.Text;

                        }
                        else
                        {
                            throw new Exception("Employee number is already in use.");
                        }

                    }
                    if (TextBoxFirstName.Text == "")
                    {
                        throw new Exception("Employee name cannot be empty");
                    }
                    else
                    {
                        newEmployeeProfile.FirstName = TextBoxFirstName.Text;
                    }
                    if (TextBoxLastName.Text == "")
                    {
                        throw new Exception("Employee last cannot be empty");
                    }
                    else
                    {
                        newEmployeeProfile.LastName = TextBoxLastName.Text;
                    }

                    if (DateTime.TryParse(TextBoxHireDate.Text, out convertedDate))
                    {
                        newEmployeeProfile.HireDate = Convert.ToDateTime(TextBoxHireDate.Text);
                    }
                    else
                    {
                        throw new Exception("Please enter a valid date.");
                    }

                    if (DateTime.TryParse(TextBoxBirthDate.Text, out convertedDate))
                    {
                        var age = today.Year - convertedDate.Year;
                        if (age > 18)
                        {
                            newEmployeeProfile.Birthdate = Convert.ToDateTime(TextBoxBirthDate.Text);
                        }
                        else
                        {
                            throw new Exception("Employee cannot be under 18");

                        }


                    }
                    else
                    {
                        throw new Exception("Please input Birth Date");
                    }



                    if (DropdownPosition.SelectedValue == "0")
                    {
                        throw new Exception("Please choose a postion for the new employee");
                    }
                    else
                    {

                        newEmployeeProfile.PositionId = int.Parse(DropdownPosition.SelectedValue);
                    }
                    if (DropdownScheduleType.SelectedValue == "0")
                    {
                        throw new Exception("Please choose a schedule type for the new employee.");
                    }
                    else
                    {
                        newEmployeeProfile.ScheduleType = int.Parse(DropdownScheduleType.SelectedValue);
                    }
                    if (DropdownSecurityRole.SelectedValue == "0")
                    {
                        throw new Exception("Please choose a security role.");
                    }
                    else
                    {
                        newEmployeeProfile.SecurityRoleId = int.Parse(DropdownSecurityRole.SelectedValue);

                    }

                    newEmployeeProfile.Phone = TextBoxPhone.Text;
                    newEmployeeProfile.EmergencyContact = TextBoxEmergencyContact.Text;
                    newEmployeeProfile.EmergencyContactPhone = TextBoxEmergencyContact.Text;
                    newEmployeeProfile.Computer = TextBoxComputerName.Text;
                    newEmployeeProfile.Station = TextBoxStation.Text;
                    newEmployeeProfile.Email = TextBoxEmail.Text;

                }
                int teamId = int.Parse(DropdownTeamMember.SelectedValue);

                controller.CreateEmployeeProfile(newEmployeeProfile, employeeId, teamId);

                Clear();
                MessageUserControl.Visible = true;
            }, "Success", "Employee profile created");
        }
        ///<summary>
        /// Clear the form in the webpage
        /// Created By: Emily Urdaneta
        /// Created On: February 5,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
        /// <summary>
        /// Listens for the click event on the update button
        /// Validation is done in this method before the object gets sent to the controller
        /// Created By: Emily Urdaneta
        /// Created On: February 5,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
                var controller = new EmployeeController();
                MessageUserControl.Visible = true;
                EmployeeProfile updatedProfile = new EmployeeProfile();

                DateTime convertedDate;
                var today = DateTime.Today;

                updatedProfile.EmployeeId = int.Parse(TextBoxEmployeeId.Text);
                TextBoxEmployeeNumber.Enabled = false;



                updatedProfile.EmployeeNumber = TextBoxEmployeeNumber.Text;

                if (TextBoxFirstName.Text == "")
                {
                    throw new Exception("Employee name cannot be empty");
                }
                else
                {
                    updatedProfile.FirstName = TextBoxFirstName.Text;
                }
                if (TextBoxLastName.Text == "")
                {
                    throw new Exception("Employee last cannot be empty");
                }
                else
                {
                    updatedProfile.LastName = TextBoxLastName.Text;
                }

                if (DateTime.TryParse(TextBoxHireDate.Text, out convertedDate))
                {
                    updatedProfile.HireDate = Convert.ToDateTime(TextBoxHireDate.Text);
                }
                else
                {
                    throw new Exception("Please enter a valid date.");
                }

                if (DateTime.TryParse(TextBoxBirthDate.Text, out convertedDate))
                {
                    var age = today.Year - convertedDate.Year;
                    if (age > 18)
                    {
                        updatedProfile.Birthdate = Convert.ToDateTime(TextBoxBirthDate.Text);
                    }
                    else
                    {
                        throw new Exception("Employee cannot be under 18");

                    }


                }
                else
                {
                    throw new Exception("Please input Expiry Date");
                }



                if (DropdownPosition.SelectedValue == "0")
                {
                    throw new Exception("Please choose a postion for the new employee");
                }
                else
                {

                    updatedProfile.PositionId = int.Parse(DropdownPosition.SelectedValue);
                }
                if (DropdownScheduleType.SelectedValue == "0")
                {
                    throw new Exception("Please choose a schedule type for the new employee.");
                }
                else
                {
                    updatedProfile.ScheduleType = int.Parse(DropdownScheduleType.SelectedValue);
                }

                if (DropdownSecurityRole.SelectedValue == "0")
                {
                    throw new Exception("Please choose a security role.");
                }
                else
                {
                    updatedProfile.SecurityRole = int.Parse(DropdownSecurityRole.SelectedValue);
                }


                updatedProfile.Phone = TextBoxPhone.Text;
                updatedProfile.EmergencyContact = TextBoxEmergencyContact.Text;
                updatedProfile.EmergencyContactPhone = TextBoxEmergencyContact.Text;
                updatedProfile.Computer = TextBoxComputerName.Text;
                updatedProfile.Station = TextBoxStation.Text;
                updatedProfile.Email = TextBoxEmail.Text;



                //}
                //check if team id been changed
                int teamId = int.Parse(DropdownTeamMember.SelectedValue);


                controller.UpdateEmployeeProfile(updatedProfile, employeeId, teamId);
            }, "Success", TextBoxFirstName.Text + " " + TextBoxLastName.Text + "'s profile has been updated.");

        }

        ///<summary>
        /// Listens for the click event on the delete button
        /// Created By: Emily Urdaneta
        /// Created On: February 5,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonTerminate_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                MessageUserControl.Visible = false;
                var employeeId = int.Parse(TextBoxEmployeeId.Text);

                var controller = new EmployeeController();
                controller.DeleteEmployeeProfile(employeeId);
                ButtonTerminate.Visible = false;
                Clear();
            }, "Success", TextBoxFirstName.Text + " " + TextBoxLastName.Text + " has been terminated");

        }
        #endregion

        #region HelperMethods
        /// <summary>
        /// Populates the employee profile form with data
        /// Created By: Emily Urdaneta
        /// Created On: February 5,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="employeeInfo"></param>
        public void PopulateEmployeeProfile(EmployeeProfile employeeInfo)
        {

            TextBoxEmployeeId.Text = employeeInfo.EmployeeId.ToString();
            TextBoxEmployeeNumber.Text = employeeInfo.EmployeeNumber;
            TextBoxFirstName.Text = employeeInfo.FirstName;
            TextBoxLastName.Text = employeeInfo.LastName;
            DropdownPosition.SelectedValue = employeeInfo.PositionId.ToString();
            TextBoxHireDate.Text = employeeInfo.HireDate.ToLongDateString();
            TextBoxBirthDate.Text = employeeInfo.Birthdate.ToLongDateString();
            DropdownScheduleType.SelectedValue = employeeInfo.ScheduleType.ToString();
            DropdownSecurityRole.SelectedValue = employeeInfo.SecurityRole.ToString();

            TextBoxEmergencyContact.Text = employeeInfo.EmergencyContact;
            TextBoxComputerName.Text = employeeInfo.Computer;
            TextBoxStation.Text = employeeInfo.Station;
            TextBoxPhone.Text = employeeInfo.Phone;
            TextBoxEmail.Text = employeeInfo.Email;

        }
        /// <summary>
        /// Clears the form in the web page
        /// Created By: Emily Urdaneta
        /// Created On: February 5,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        public void Clear()
        {
            TextBoxLookup.Text = "";
            TextBoxEmployeeId.Text = "";
            TextBoxEmployeeNumber.Text = "";
            TextBoxFirstName.Text = "";
            TextBoxLastName.Text = "";
            DropdownPosition.SelectedIndex = 0;
            TextBoxHireDate.Text = "";
            TextBoxBirthDate.Text = "";

            DropdownScheduleType.SelectedIndex = 0;
            DropdownSecurityRole.SelectedIndex = 0;
            TextBoxPhone.Text = "";
            TextBoxEmail.Text = "";
            TextBoxComputerName.Text = "";
            TextBoxStation.Text = "";
            TextBoxEmergencyContact.Text = "";
            TextBoxEmergencyContactNumber.Text = "";

            MessageUserControl.Visible = false;
            ButtonUpdate.Visible = false;
            ButtonCreateProfile.Visible = true;
        }
        #endregion






    }
}