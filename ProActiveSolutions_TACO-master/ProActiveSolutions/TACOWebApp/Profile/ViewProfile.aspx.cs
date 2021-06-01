using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOSystem.BLL.Employee;
using TACOSystem.BLL.Security;

namespace TACOWebApp.Profile
{
    public partial class createProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();


            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName)
            {
                string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
                int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;

                var controller = new EmployeeController();
                var employeeProfile = controller.GetEmployee(employeeId);
                if (employeeProfile == null)
                {
                    throw new Exception("Please contact your Team Lead or Admin to create a profile.");
                }
                PopulateEmployeeProfile(employeeProfile);
               
            }
            else
            {
                all_content.Visible = false;
               
            }


            if (!IsPostBack)
            {

            }
        }

        public void PopulateEmployeeProfile(EmployeeProfile employeeProfile)
        {
            TextBoxEmployeeId.Text = employeeProfile.EmployeeId.ToString();
            TextBoxEmployeeNumber.Text = employeeProfile.EmployeeNumber;
            TextBoxFirstName.Text = employeeProfile.FirstName;
            TextBoxLastName.Text = employeeProfile.LastName;
            DropdownPosition.SelectedValue = employeeProfile.PositionId.ToString();
            TextBoxHireDate.Text = employeeProfile.HireDate.ToLongDateString();
            TextBoxBirthDate.Text = employeeProfile.Birthdate.ToLongDateString();
            DropdownScheduleType.SelectedValue = employeeProfile.ScheduleType.ToString();
            DropdownSecurityRole.SelectedValue = employeeProfile.SecurityRole.ToString();

            TextBoxEmergencyContact.Text = employeeProfile.EmergencyContact;
            TextBoxComputerName.Text = employeeProfile.Computer;
            TextBoxStation.Text = employeeProfile.Station;
            TextBoxPhone.Text = employeeProfile.Phone;
            TextBoxEmail.Text = employeeProfile.Email;
        }

        
    }
}