using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOSystem.BLL.Security;

namespace TACOWebApp.Task
{
    public partial class CreateRole : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();


            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName && SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin") { }
            else { all_content.Visible = false; }
        }

        protected void ButtonCreateSecurityRole_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;

            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;

                RoleInformation newSecurityCode = new RoleInformation();

                if (TextBoxSecurityRole.Text == "" || TextBoxSecurityRole.Text.Length > 50)
                {
                    throw new Exception("Security role cannot be empty and must be less than 50 characters");
                }
                if (TextBoxDescription.Text == "" || TextBoxDescription.Text.Length > 250)
                {
                    throw new Exception("Description cannot be empty and must be less than 250 characters");
                }
                
                else
                {
                    newSecurityCode.SecurityRole = TextBoxSecurityRole.Text;
                    newSecurityCode.RoleDescription = TextBoxDescription.Text;                    
                    
                }
                var controller = new RoleController();
                controller.CreateSecurityRole(newSecurityCode, userId);

                TextBoxSecurityRoleId.Text = "";
                TextBoxSecurityRole.Text = "";
                TextBoxDescription.Text = "";

                DropdownRoleLookup.Items.Clear();
                DropdownRoleLookup.DataBind();
                DropdownRoleLookup.Items.Insert(0, new ListItem("Choose a Security Role", ""));
                DropdownRoleLookup.SelectedIndex = -1;


            }, "Success", "Security role has been created.");
        }

        protected void ButtonUpdateSecurityRole_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;

                RoleInformation updatedSecurityCode = new RoleInformation();
                               
                if (TextBoxDescription.Text == "" || TextBoxDescription.Text.Length > 250)
                {
                    throw new Exception("Description cannot be empty and must be less than 250 characters");
                }
                else
                {
                    updatedSecurityCode.SecurityRoleId = int.Parse(TextBoxSecurityRoleId.Text);;
                    updatedSecurityCode.RoleDescription = TextBoxDescription.Text;

                }
                var controller = new RoleController();
                controller.UpdateSecurityRole(updatedSecurityCode, userId);

                TextBoxSecurityRoleId.Text = "";
                TextBoxSecurityRole.Text = "";
                TextBoxDescription.Text = "";

                DropdownRoleLookup.Items.Clear();
                DropdownRoleLookup.DataBind();
                DropdownRoleLookup.Items.Insert(0, new ListItem("Choose a Security Role", ""));
                DropdownRoleLookup.SelectedIndex = -1;


            }, "Success", "Security role has been updated.");
        }

        protected void ButtonClearSecurityRole_Click(object sender, EventArgs e)
        {
            DropdownRoleLookup.SelectedIndex = 0;
            TextBoxSecurityRoleId.Text = "";
            TextBoxSecurityRole.Text = "";
            TextBoxDescription.Text = "";
            TextBoxSecurityRole.Enabled = true;

            ButtonUpdateSecurityRole.Visible = false;
            ButtonCreateSecurityRole.Visible = true;
            MessageUserControl.Visible = false;
        }

        protected void ButtonSecurityRoleLookup_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;
            if (DropdownRoleLookup.SelectedIndex == 0)
            {
                MessageUserControl.Visible = true;
                MessageUserControl.ShowInfo("Please select a security role.");
            }
            else
            {
                var controller = new RoleController();
                var securityRoleInfo = controller.GetRoleInformation(int.Parse(DropdownRoleLookup.SelectedValue));

                TextBoxSecurityRoleId.Text = securityRoleInfo.SecurityRoleId.ToString();
                TextBoxSecurityRole.Text = securityRoleInfo.SecurityRole;
                TextBoxDescription.Text = securityRoleInfo.RoleDescription;

                TextBoxSecurityRole.Enabled = false;

                ButtonUpdateSecurityRole.Visible = true;
                ButtonCreateSecurityRole.Visible = false;
                
            }
        }
    }
}