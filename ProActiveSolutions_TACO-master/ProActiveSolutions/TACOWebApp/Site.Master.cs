using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOSystem.BLL.Security;

namespace TACOWebApp
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            var role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;

            if (windowsAccountName == SecurityController.FindAccount(windowsAccountName).DisplayName)
            {

                LabelUser.Text = "Welcome, " + windowsAccountName + " you are logged in as " + role + " !";
                if (role.ToLower() == "employee")
                {
                    nav_task_admin_crud.Style.Add("display", "none");
                    nav_task_admin_project.Style.Add("display", "none");
                }
                else
                {
                    nav_task_admin_crud.Style.Add("display", "inherit");
                    nav_task_admin_project.Style.Add("display", "inherit");
                }

            }
            else
            {
                main_nav_bar.Visible = false;
                forbidden_content.Visible = true;

            }
        }


    }
}