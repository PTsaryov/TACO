
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOSystem.BLL.Attendance;
using TACOSystem.BLL.Security;

namespace TACOWebApp.Task
{
    public partial class UpdateEntitlement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();

            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName && SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin")
            {
                string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
                int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;


                if (!IsPostBack)
                {
                    BindEntitlementGrid();
                }
            }
            else
            {
                all_content.Visible = false;

            }
        }
        public DataTable Get_EmptyDataTable()
        {
            DataTable dtEmpty = new DataTable();
            //Here ensure that you have added all the column available in your gridview
            dtEmpty.Columns.Add("AttendanceEntitlementId", typeof(string));
            dtEmpty.Columns.Add("EmployeeId", typeof(string));
            dtEmpty.Columns.Add("FullName", typeof(string));
            dtEmpty.Columns.Add("AttendanceCode", typeof(string));
            dtEmpty.Columns.Add("Units", typeof(string));
            dtEmpty.Columns.Add("TotalTime", typeof(string));
            dtEmpty.Columns.Add("TextBoxNewAmount", typeof(string));

            DataRow datatRow = dtEmpty.NewRow();

            //Inserting a new row,datatable .newrow creates a blank row
            dtEmpty.Rows.Add(datatRow);//adding row to the datatable
            return dtEmpty;
        }
        protected void ButtonSubmitUpdates_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;

                int attendanceId = int.Parse(DropdownAttendanceLookup.SelectedValue);

                AttendenceEntitlementInfo updatedEntitlement = new AttendenceEntitlementInfo();
                AttendanceEntitlementController controller = new AttendanceEntitlementController();
                int updated = 0;
                if (GridViewEntitlement.Rows.Count > 0)
                {
                    foreach (GridViewRow gvRow in GridViewEntitlement.Rows)
                    {
                        decimal number;
                        TextBox eId = (TextBox)gvRow.FindControl("TextBoxId");
                        int entitlementId = int.Parse(eId.Text);
                        TextBox amount = (TextBox)gvRow.FindControl("TextBoxUpdateAmount");
                        String total = amount.Text;
                        
                        if (total != "")
                        {
                            if (decimal.TryParse(amount.Text, out number))
                            {
                                decimal updateAmount = Convert.ToDecimal(total);
                                updatedEntitlement.AttendanceEntitlementId = entitlementId;
                                updatedEntitlement.TotalTime = updateAmount;

                                controller.UpdateAttendanceEntitlement(updatedEntitlement, userId);
                                updated += 1;
                            }
                            else
                            {
                                throw new Exception("Amount to update must be a number");
                            }
                        }
                    }

                    var entitlements = controller.AttendanceEntitlementList(attendanceId);
                    if (entitlements.Count > 0)
                    {
                        GridViewEntitlement.DataSource = entitlements;
                        GridViewEntitlement.DataBind();
                    }
                    else
                    {
                        BindEntitlementGrid();
                    }
                    if (updated == 0)
                    {
                        MessageUserControl.Visible = true;
                        throw new Exception("No records to update.");
                    }
                }

                MessageUserControl.Visible = true;
            }, "Success", "Attendance entitlements have been updated.");
        }

        protected void ButtonCancelUpdates_Click(object sender, EventArgs e)
        {
            BindEntitlementGrid();
            DropdownAttendanceLookup.SelectedIndex = 0;
            MessageUserControl.Visible = false;
        }
        protected void BindEntitlementGrid()
        {
            var controller = new AttendanceEntitlementController();
            var entitlements = controller.AttendanceEntitlementList();
            GridViewEntitlement.DataSource = entitlements;
            GridViewEntitlement.DataBind();
            if (entitlements.Count > 0)
            {
                GridViewEntitlement.DataSource = entitlements;
                GridViewEntitlement.DataBind();
            }
            else
            {
                //if the Data is empty then bind the GridView with an Empty Dataset
                GridViewEntitlement.DataSource = Get_EmptyDataTable();
                GridViewEntitlement.DataBind();
                foreach (GridViewRow r in GridViewEntitlement.Rows)
                {
                    if (r.RowType == DataControlRowType.DataRow)
                    {
                        (r.FindControl("TextBoxUpdateAmount") as TextBox).Enabled = false;
                    }
                }
            }
        }
        protected void EntitlementGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewEntitlement.PageIndex = e.NewPageIndex;
            try
            {
                AttendanceEntitlementController controller = new AttendanceEntitlementController();
                int attendanceId = int.Parse(DropdownAttendanceLookup.SelectedValue);
                if (attendanceId != 0)
                {
                    var entitlements = controller.AttendanceEntitlementList(attendanceId);
                    GridViewEntitlement.DataSource = entitlements;
                    GridViewEntitlement.DataBind();
                }
                else
                {
                    var entitlements = controller.AttendanceEntitlementList();
                    GridViewEntitlement.DataSource = entitlements;
                    GridViewEntitlement.DataBind();
                }
            }
            catch (Exception)
            {
                MessageUserControl.ShowInfo("Error", "Unexpected error on page load");
            }
        }

        protected void DropdownAttendanceLookup_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;
            DropDownList dd = (DropDownList)sender;
            int attendanceId = int.Parse(dd.SelectedValue);

            var controller = new AttendanceEntitlementController();
            var entitlements = controller.AttendanceEntitlementList(attendanceId);
            if (entitlements.Count > 0)
            {
                GridViewEntitlement.DataSource = entitlements;
                GridViewEntitlement.DataBind();
            }
            else
            {
                //if the Data is empty then bind the GridView with an Empty Dataset
                GridViewEntitlement.DataSource = Get_EmptyDataTable();
                GridViewEntitlement.DataBind();
                foreach (GridViewRow r in GridViewEntitlement.Rows)
                {
                    if (r.RowType == DataControlRowType.DataRow)
                    {
                        (r.FindControl("TextBoxUpdateAmount") as TextBox).Enabled = false;
                    }
                }
            }

        }

        protected void ButtonAddEntitlement_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var windowsAccountName = User.Identity.Name.Split('\\').Last();
                int userId = SecurityController.FindAccount(windowsAccountName).AccountID;

                AttendenceEntitlementInfo newEntitlement = new AttendenceEntitlementInfo();
                AttendanceEntitlementController controller = new AttendanceEntitlementController();
                GridViewRow row = (GridViewRow)((sender as Button).NamingContainer);
                TextBox amount = (TextBox)row.FindControl("TextBoxNewAmount");
                String total = amount.Text;
                if (total == "")
                {
                    total = "0";
                }
                DropDownList eId = (DropDownList)row.FindControl("DropDownName");
                int employeeId = int.Parse(eId.SelectedValue);
                DropDownList aId = (DropDownList)row.FindControl("DropDownAttendance");
                int attendanceId = int.Parse(aId.SelectedValue);
                if (employeeId < 0 || attendanceId < 0)
                {
                    MessageUserControl.Visible = true;
                    throw new Exception("Please select an employee and an attendance code.");
                }
                else if (eId.SelectedIndex > -1 && aId.SelectedIndex > -1)
                {
                    decimal updateAmount = Convert.ToDecimal(total);
                    newEntitlement.EmployeeId = employeeId;
                    newEntitlement.AttendanceId = attendanceId;
                    newEntitlement.TotalTime = updateAmount;

                    controller.CreateAttendanceEntitlement(newEntitlement, userId);
                }

                var entitlements = controller.AttendanceEntitlementList();
                GridViewEntitlement.DataSource = entitlements;
                GridViewEntitlement.DataBind();

                MessageUserControl.Visible = true;
            }, "Success", "Attendance entitlement has been added.");
        }
    }
}