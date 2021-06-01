using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs.TimeSheet;
using TACOSystem.BLL;
using TACOSystem.BLL.Employee;
using TACOSystem.BLL.Security;


namespace TACOWebApp
{
    public partial class _Default : Page
    {
        #region Global variables
        // Get controller instance
        private static TimesheetController controller = new TimesheetController();
        #endregion

        /// <summary>
        /// <para>
        /// This method will check the role and load the page appropriately.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: February 16,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 02,2019
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get security information
            var windowsAccountName = User.Identity.Name.Split('\\').Last();
            int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
            string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName)
            {
            }
            else
            {// not authorized will hide content and diplay unauthorized page message
                all_content.Visible = false;
            }

            if (!IsPostBack)
            {
                switch (role.ToLower())
                {
                    case "globaladmin":
                        manage_calendars.Visible = true;

                        DropDownTeamList.DataSource = controller.AllTeams();
                        DropDownTeamList.DataTextField = "Value";
                        DropDownTeamList.DataValueField = "Key";
                        DropDownTeamList.DataBind();
                        DropdonEmployeeLIst.SelectedValue = employeeId.ToString();
                        break;
                    case "teamlead":
                    case "teamadmin":
                        manage_calendars.Visible = true;

                        DropDownTeamList.DataSource = controller.TeamsByEmployeeId(employeeId);
                        DropDownTeamList.DataTextField = "Value";
                        DropDownTeamList.DataValueField = "Key";
                        DropDownTeamList.DataBind();
                        DropdonEmployeeLIst.SelectedValue = employeeId.ToString();
                        break;
                    case "employee":
                        // Will hide administrative features
                        manage_calendars.Visible = false;
                        break;
                }
            }
           

        }

        /// <summary>
        /// <para>
        /// This method will calculate the overtime totals by overtime type and by employee.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: February 16,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 10,2019
        /// </summary>
        /// <returns> JSON objects array string of total overtime Requested by type of overtime</returns>
        [WebMethod]
        public static string CalculateOvertimeTotals(string id)
        {
            string windowsAccountName = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            
            int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
            //check if employee id passed from drop down
            if (int.Parse(id) > 0)
            {
                employeeId = int.Parse(id);
            }

            // Get data of booked overtimes 
            List<OvertimeDeatils> employeeRequestedOvertimes = controller.OvertimeByEmployee(employeeId);
            List<AssignedOvertime> overtimes = controller.AllOvertimes();
            // Temp list for all totals per overtime
            List<OvertimeForTotals> totalOvertimeRequested = new List<OvertimeForTotals>();
            // Loop sort and calculate
            foreach (var overtime in overtimes)
            {
                TimeSpan tempTimeSpan = new TimeSpan();
                OvertimeForTotals newOvertimeToatl = new OvertimeForTotals(overtime.id, overtime.title, tempTimeSpan);

                foreach (var requestedOvertime in employeeRequestedOvertimes)
                {
                    TimeSpan totalPerOvertime = (requestedOvertime.end - requestedOvertime.start);
                    if (overtime.id == requestedOvertime.OvertimeId)
                    {
                        tempTimeSpan += totalPerOvertime;
                    }
                    newOvertimeToatl = new OvertimeForTotals(overtime.id, overtime.title, tempTimeSpan);
                }
                totalOvertimeRequested.Add(newOvertimeToatl);
            }
            var jsonObjArray = JsonConvert.SerializeObject(totalOvertimeRequested);
            return jsonObjArray;
        }

        /// <summary>
        /// <para>
        /// This method retrieves a list of current projects and overtimes by employee id.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: February 16,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 02,2019
        /// </summary>
        /// <returns> JSON objects array string</returns>
        [WebMethod]
        public static string GetProjectsAndOvertimes(string id)
        {
            string windowsAccountName = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
            int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
            //check if employee id passed from drop down
            if (int.Parse(id) > 0)
            {
                employeeId = int.Parse(id);
            }
            //Get pojects and overtimes  
            List<ProjectAndOvertimeDetail> projectsAndOvertimes = controller.ProjectOvertimeByEmployee(employeeId, role);
            // Serialize the list to JSON objects array string
            var jsonObjArray = JsonConvert.SerializeObject(projectsAndOvertimes);
            return jsonObjArray;
        }

        /// <summary>
        /// <para>
        /// This method retrieves a list of currently assigned projects by employee id.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: February 18,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 06,2019
        /// </summary>
        /// <returns> JSON objects array string</returns>
        [WebMethod]
        public static string GetAssignedProjects(string id)
        {
            string windowsAccountName = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
            // Check if employee id passed from drop down
            if (int.Parse(id) > 0)
            {
                employeeId = int.Parse(id);
            }
            // Get assigned projects by employee id
            List<AssignedProject> assignedProjects = controller.ProjectsAssignedToEmployeeById(employeeId);
            // Serialize the list to JSON objects array string
            var jsonObjArray = JsonConvert.SerializeObject(assignedProjects);
            return jsonObjArray;
        }

        /// <summary>
        /// <para>
        /// This method retrieves a list of currently assigned overtimes.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: February 18,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 06,2019
        /// </summary>
        /// <returns> JSON objects array string</returns>
        [WebMethod]
        public static string GetAssignedOvertimes()
        {
            // Access controller 
            //TimesheetController controller = new TimesheetController();
            List<AssignedOvertime> assignedOvertimeList = controller.AllOvertimes();
            // Serialize the list to JSON objects array string
            var jsonObjArray = JsonConvert.SerializeObject(assignedOvertimeList);
            return jsonObjArray;
        }

        /// <summary>
        /// <para>
        /// This method will send the events(overtimes and projects) passed in the JSON objects array
        /// to be updated using a method in the controller.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: March 6,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 06,2019
        /// </summary>
        [WebMethod]
        public static void UpdateEvents(string eventsJson)
        {
            string windowsAccountName = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
            int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
            // Initialize date time for converting
            var stratDateTime = DateTime.Now; 
            var endDateTime = DateTime.Now;
            
            // Creating JSON serializer
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            // Deserialize the JSON list to a c# objects list
            List<JsonUpdatableEvent> test_Time = json_serializer.Deserialize<List<JsonUpdatableEvent>>(eventsJson);
            // Loop through the list and update each event
            foreach (var item in test_Time)
            {
                stratDateTime = DateTime.ParseExact(item.start, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                endDateTime = DateTime.ParseExact(item.end, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                controller.UpdateTimesheetDetail(item.id, employeeId, role, stratDateTime, endDateTime);
            }
        }

        /// <summary>
        /// <para>
        /// This method will send the project events passed in the JSON objects array 
        /// to be created  using a method in the controller.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: March 7,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 06,2019
        /// </summary>
        [WebMethod]
        public static void CreateNewEvents(string eventsJson, string id)
        {
            string windowsAccountName = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
            int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
            // Initialize date time for converting
            var stratDateTime = DateTime.Now; 
            var endDateTime = DateTime.Now;

            // Check if employee id passed from dropdown
            if (id == null)
            {
                id = "0";
            }
            if (int.Parse(id) > 0)
            {
                employeeId = int.Parse(id);
            }
            // Creating JSON serializer
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            // Deserialize the JSON list to a c# objects list
            List<TimesheetDetail> timesheetDetails = json_serializer.Deserialize<List<TimesheetDetail>>(eventsJson);
            // Loop through the list and create each event
            foreach (var timesheetDetail in timesheetDetails)
            {

                stratDateTime = DateTime.ParseExact(timesheetDetail.start, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                endDateTime = DateTime.ParseExact(timesheetDetail.end, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                controller.CreateTimesheetDetail(role, employeeId, timesheetDetail.ProjectId, stratDateTime, endDateTime);
            }

        }

        /// <summary>
        /// <para>
        /// This method will send the overtime request events passed in the JSON objects array 
        /// to be created using a method in the controller.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: March 7,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 06,2019
        /// </summary>
        [WebMethod]
        public static void CreateNewRequest(string eventsJson, string id)
        {
            string windowsAccountName = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
            int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
            // Initialize date time for converting
            var stratDateTime = DateTime.Now;
            var endDateTime = DateTime.Now;

            // Check if employee id passed from dropdown
            if (id == null)
            {
                id = "0";
            }
            if (int.Parse(id) > 0)
            {
                employeeId = int.Parse(id);
            }
            // Creating JSON serializer
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            // Deserialize the JSON list to a c# objects list
            List<TimesheetDetail> overtimeRequests = json_serializer.Deserialize<List<TimesheetDetail>>(eventsJson);
            // Loop through the list and create each event
            foreach (var request in overtimeRequests)
            {
                stratDateTime = DateTime.ParseExact(request.start, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                endDateTime = DateTime.ParseExact(request.end, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                controller.CreateTimeSheetOvertime(role, employeeId, request.ProjectId, stratDateTime, endDateTime);
            }
        }

        /// <summary>
        /// <para>
        /// This method will send the the selected event(JSON object)
        /// to be deleted using a method in the controller.
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: March 17,2019
        /// Modified By: Anton Drantiev
        /// Modified On: March 25,2019
        /// </summary>
        [WebMethod]
        public static void DeleteEvent(string eventsJson)
        {
            string windowsAccountName = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            string role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
            int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
            // Creating JSON serializer
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            // Deserialize the JSON object to a c# object
            JsonUpdatableEvent timesheetDetail = json_serializer.Deserialize<JsonUpdatableEvent>(eventsJson);
            // Send id for delete/deactivate
            controller.DeleteTimesheetDetail(timesheetDetail.id, employeeId, role);
        }

        /// <summary>
        /// <para>
        /// This method acts as a listner to any change in the drop down
        /// and populates the employees drop down by the selected team
        /// </para>
        /// Created By: Anton Drantiev
        /// Created On: March 29,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 02,2019
        /// </summary>
        protected void DropDownTeamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropdonEmployeeLIst.ClearSelection();
            DropdonEmployeeLIst.Items.Clear();

            DropdonEmployeeLIst.DataSource = controller.EmployeesByTeamId(int.Parse(DropDownTeamList.SelectedValue));
            DropdonEmployeeLIst.DataTextField = "Value";
            DropdonEmployeeLIst.DataValueField = "Key";
            DropdonEmployeeLIst.DataBind();
        }
    }
}