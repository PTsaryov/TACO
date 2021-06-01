using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOSystem.BLL.Employee;
using TACOSystem.BLL.Security;

namespace TACOWebApp.Profile
{
    public partial class Requests : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();

            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName)
            {                
                BindRequestGrid();
            }
            else
            {
                all_content.Visible = false;

            }
            
        }

        protected void closeRequest_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void RequestGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // add colour to the row based on status of the request
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[5].Text == "Denied")
                {
                    e.Row.BackColor = System.Drawing.Color.IndianRed;

                }
                if (e.Row.Cells[5].Text == "Approved")
                {
                    e.Row.BackColor = System.Drawing.Color.LightGreen;

                }
            }

            // convert minutes to hours and minutes and display text
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label content = (Label)e.Row.FindControl("LabelTotalTime");
                decimal value = decimal.Parse(content.Text);
                String output;
                if (value / 60 < 1)
                {
                    output = Math.Floor(value % 60) + " min";
                }
                else if (value / 60 < 2)
                {
                    output = Math.Floor(value / 60) + " hour " + Math.Floor(value % 60) + " min";
                }
                else
                {
                    output = Math.Floor(value / 60) + " hours " + Math.Floor(value % 60) + " min";
                }
                content.Text = output;
            }

        }

        public void BindRequestGrid()
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();
            string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
            int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;

            var controller = new RequestController();
            var employeeRequests = controller.RequestList(employeeId);

            GridViewRequest.DataSource = employeeRequests;
            GridViewRequest.DataBind();

        }
        protected void RequestGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRequest.PageIndex = e.NewPageIndex;
            BindRequestGrid();
        }
    }
}