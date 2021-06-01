using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOSystem.BLL.Employee;
using TACOSystem.BLL.Security;

namespace TACOWebApp.Profile
{
    public partial class approveOvertime : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();

            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName
                && SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin" || SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "TeamLead")
            {
                string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
                int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;

                if (!IsPostBack)
                {
                    BindOvertimeGrid();
                }
            }
            else
            {
                all_content.Visible = false;

            }

        }
        public void BindOvertimeGrid()
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();

            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName)
            {
                string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
                int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;

                if (role.ToLower() == "teamlead")
                {
                    RequestController controller = new RequestController();
                    var employeeRequests = controller.ApproveRequestList(employeeId);

                    GridViewOvertime.DataSource = employeeRequests;
                    GridViewOvertime.DataBind();
                }
                else if (role.ToLower() == "globaladmin")
                {

                    RequestController controller = new RequestController();
                    var employeeRequests = controller.ApproveRequestList();

                    GridViewOvertime.DataSource = employeeRequests;
                    GridViewOvertime.DataBind();
                }
            }
        }

        protected void ButtonSubmitApproval_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                RequestInformation updatedRequest = new RequestInformation();
                RequestController controller = new RequestController();

                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;


                if (GridViewOvertime.Rows.Count > 0)
                {
                    foreach (GridViewRow gvRow in GridViewOvertime.Rows)
                    {
                        string rId = gvRow.Cells[6].Text;
                        RadioButtonList rblStatus = (RadioButtonList)gvRow.FindControl("RadioButtonStatusItem");
                        string status = rblStatus.SelectedValue;
                        TextBox approval = (TextBox)gvRow.FindControl("TextBox1");
                        string comment = approval.Text;
                        string eid = gvRow.Cells[7].Text;
                        int employeeid = int.Parse(eid);
                        string oid = gvRow.Cells[8].Text;
                        int overtimeid = int.Parse(oid);

                        if (status != "Pending" || comment != "")
                        {
                            if (comment.Length > 250)
                            {
                                throw new Exception("Comment must be 250 characters or less");
                            }
                            else
                            {
                                updatedRequest.EmployeeId = employeeid;
                                updatedRequest.OvertimeId = overtimeid;
                                updatedRequest.RequestId = int.Parse(rId);
                                updatedRequest.Status = status;
                                updatedRequest.Comment = comment;
                                controller.RequestApproval(updatedRequest, employeeId);
                            }
                        }

                    }

                }

                BindOvertimeGrid();

            }, "Success", "Changes have been saved");
        }

        protected void ButtonCancelApproval_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;
            Response.Redirect("~/Default.aspx");
        }

        protected void OvertimeGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

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

        protected void OvertimeGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewOvertime.PageIndex = e.NewPageIndex;
            this.BindOvertimeGrid();
        }

    }
}