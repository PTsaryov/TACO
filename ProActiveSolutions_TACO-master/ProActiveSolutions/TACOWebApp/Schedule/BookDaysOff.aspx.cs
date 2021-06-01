using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOData.Entities.POCOs.DaysOff;
using TACOSystem.BLL;
using TACOSystem.BLL.Security;


namespace TACOWebApp.Schedule
{
    public partial class BookDaysOff : System.Web.UI.Page
    {
        List<MonthDayYear> calendarHeaderDays = new List<MonthDayYear>();

        /// <summary>
        /// Loads necessary methods on Page Load
        /// Created By: Emily Urdaneta
        /// Created On: March 26, 2019
        /// Modified By: Emily Urdaneta
        /// Modified On: April 14,2019
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();


            if (Request.IsAuthenticated && User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName)
            {

            }
            else
            {
                all_content.Visible = false;
            }
            if (!IsPostBack)
            {
                PopulateMonthsDropDown();
                PopulateYearsDropDown();
            }

        }
        /// <summary>
        /// Method to get employeeId
        /// Created By: Emily Urdaneta
        /// Created On: April 16,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <returns></returns>
        public int GetEmployeeId()
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();
            int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
            return employeeId;
        }
        /// <summary>
        /// Method to get the security role.
        /// Created By: Emily Urdaneta
        /// Created On: April 15,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <returns>string</returns>
        public string GetSecurityRole()
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();
            int employeeId = SecurityController.FindAccount(windowsAccountName).AccountID;
            var role = SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
            return role;
        }
        /// <summary>
        /// Listens for the submit click after selecting the month and year from the dropdown
        /// Created By: Emily Urdaneta
        /// Created On:April 1, 2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {

            var employeeId = GetEmployeeId();
            var year = GetYear();
            var month = GetMonth();
            var role = GetSecurityRole();
            var controller = new DayOffController();
            var employeeNames = controller.ListAllEmployeeDaysOff(month, year, employeeId, role);
            RepeaterEmployeeDaysOff.DataSource = employeeNames;
            RepeaterEmployeeDaysOff.DataBind();
            DivCalendar.Visible = true;

        }
        /// <summary>
        /// Dynamically builds the calendar days
        /// Created By: Emily Urdaneta
        /// Created On: April 05,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="numberOfDays">IEnumerable days in a month</param>
        /// <param name="year">selected year from dropdown</param>
        /// <param name="month">selected month from dropdown</param>
        /// <returns></returns>
        private List<MonthDayYear> BuildCalendarHeaderDays(IEnumerable<int> numberOfDays, int year, int month)
        {
            var headerDays = new List<MonthDayYear>();
            foreach (var item in numberOfDays)
            {
                headerDays.Add(new MonthDayYear { DayNumber = item, MonthNumber = month, YearNumber = year });

            }
            return headerDays;

        }
        /// <summary>
        /// Calculates the number of days
        /// Created By: Emily Urdaneta
        /// Created On: April 5,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="month">selected month from dropdown</param>
        /// <param name="year">selected year from the dropdown</param>
        /// <returns></returns>
        private int GetDaysOfMonthPerYear(int month, int year)
        {
            int daysInMonthPerYear = DateTime.DaysInMonth(year, month);
            return daysInMonthPerYear;

        }
        /// <summary>
        /// Binds the data when RepeaterEmployeeDaysOff gets created
        /// Mainly used for binding the calendarheader days to the header
        /// Created By: Emily Urdaneta
        /// Created On: April 5,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RepeaterEmployeeDaysOff_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            var year = GetYear();
            var month = GetMonth();

            var daysPerMonthPerYear = GetDaysOfMonthPerYear(month, year);
            var numberOfDays = Enumerable.Range(1, daysPerMonthPerYear);
            calendarHeaderDays = BuildCalendarHeaderDays(numberOfDays, year, month);


            if (e.Item.ItemType == ListItemType.Header)
            {

                var daysRepeater = e.Item.FindControl("RepeaterDaysOfMonth") as Repeater;

                daysRepeater.DataSource = calendarHeaderDays;
                daysRepeater.DataBind();


            }


        }
        /// <summary>
        /// Dynamically populates the dropdown list of months
        /// Created By: Emily Urdaneta
        /// Created On: April 05,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        public void PopulateMonthsDropDown()
        {
            var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            for (int i = 0; i < months.Length; i++)
            {
                DropdownMonths.Items.Add(new ListItem(months[i], i.ToString()));
            }

        }
        /// <summary>
        /// Dynamically populates the dropdown list of years
        /// Calculates the years starting from the current year
        /// Created By: Emily Urdaneta
        /// Created On: April 05,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        public void PopulateYearsDropDown()
        {
            int year = DateTime.Now.Year;
            for (int i = year; i <= year + 5; i++)
            {
                ListItem li = new ListItem(i.ToString());
                DropdownYears.Items.Add(li);
            }
            //DropdownYears.Items.FindByText(year.ToString()).Selected = true;
        }
        /// <summary>
        /// Method to get the selected month from the dropdown list
        /// Created By: Emily Urdaneta
        /// Created On: April 05,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <returns></returns>
        public int GetMonth()
        {
            var month = DropdownMonths.SelectedIndex + 1;
            return month;
        }
        /// <summary>
        /// Method to get the selected year from the dropdown list
        /// Created By: Emily Urdaneta
        /// Created On: April 05,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <returns></returns>
        public int GetYear()
        {
            var year = int.Parse(DropdownYears.SelectedItem.Text);
            return year;
        }

        /// <summary>
        /// Finds all controls in the repeater on databound.
        /// Primarily used to disable fields of other employees.
        /// Created By:Emily Urdaneta
        /// Created On: April 13,2019
        /// Modified By: 
        /// Modified On:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RepeaterEmployeeDaysOff_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var employeeId = GetEmployeeId();
            var securityRole = GetSecurityRole();
            if(securityRole == "Employee")
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    var dayOffRepeater = e.Item.FindControl("RepeaterDaysOff") as Repeater;
                    var hiddenEmployeeId = e.Item.FindControl("HiddenFieldEmployeeId") as HiddenField;
                    int employeeIdFromRepeater = int.Parse(hiddenEmployeeId.Value);
                    if (employeeId != employeeIdFromRepeater)
                    {
                        foreach (RepeaterItem item in dayOffRepeater.Items)
                        {

                            ((Button)item.FindControl("ButtonLaunchModal")).Enabled = false;
                        }
                    }


                }
            }
        
         
        }

        /// <summary>
        /// WebMethod used to accept the jSon data containing the timesheet detail id to be deleted.
        /// Id is passed on to the controller.
        /// Created By: Emily Urdaneta
        /// Created On: April 15,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 16,2019
        /// </summary>
        /// <param name="eventsJson">Json Data</param>
        [WebMethod]
        public static void DeleteDayOff(string Id)
        {
            DayOffController controller = new DayOffController();

            controller.DeleteTimesheetDetail(int.Parse(Id));
        }
        /// <summary>
        /// WebMethod used to accept the jSon data containing the timesheet detail id and new code
        /// to update the timesheet.
        /// Both parameters are passed on to the controller.
        /// Created By: Anton Drantiev
        /// Created On: April 16,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="Id">timesheet detail id</param>
        /// <param name="Code">new code</param>
        [WebMethod]
        public static void UpdateTimesheetDetails(string Id, string Code, string EmployeeId)
        {

            DayOffController controller = new DayOffController();

            var entitlement = controller.GetAbsenceEntitlement(int.Parse(EmployeeId), Code);
            var dayOff = controller.GetDayOffDuration(int.Parse(Id));

            if (Code == "BNK")
            {

                var bankedEntitlement = controller.GetOvertimeEntitlement(int.Parse(EmployeeId), Code);
                var dayOffDuration = (dayOff.EndDateTime - dayOff.StartDateTime).TotalHours;

                if (dayOffDuration > bankedEntitlement)
                {
                    throw new Exception("You do not have enough to book this day off as " + Code + ".");

                }
                else
                {
                    controller.UpdateTimesheetDetail(int.Parse(Id), Code, int.Parse(EmployeeId));
                }

            }
            else
            {
                if (Code == "FWT")
                {
                    var dayOffDuration = (dayOff.EndDateTime - dayOff.StartDateTime).TotalHours;
                    if (dayOffDuration > entitlement)
                    {
                        throw new Exception("You do not have enough to book this day off as " + Code + ".");
                    }
                    else
                    {
                        controller.UpdateTimesheetDetail(int.Parse(Id), Code, int.Parse(EmployeeId));
                    }
                }
                else
                {
                    var duration = (dayOff.EndDateTime - dayOff.StartDateTime).TotalDays;
                    var dayOffDuration = 0;
                    if (duration < 0)
                    {
                        dayOffDuration = 1;
                    }

                    if (dayOffDuration > entitlement)
                    {
                        throw new Exception("You do not have enough to book this day off as " + Code + ".");

                    }
                    else
                    {
                        controller.UpdateTimesheetDetail(int.Parse(Id), Code, int.Parse(EmployeeId));
                    }
                }

            }

        }



        /// <summary>
        /// WebMethod used to create a new day off in the database.
        /// </summary>
        /// <param name="Code">Code chosen from the radio button list</param>
        /// <param name="StartTime">Date inside the start time textbox</param>
        /// <param name="EndTime">Date inside the end time textbox</param>
        /// <param name="EmployeeId">The curren employee logged in</param>
        [WebMethod]
        public static void CreateNewDayOff(string Code, string StartTime, string EndTime, string EmployeeId)
        {
            var startTime = Convert.ToDateTime(StartTime);
            var endTime = Convert.ToDateTime(EndTime);
            DayOffController controller = new DayOffController();


            if (Code == "BNK")
            {
                var daysOffinHours = (endTime - startTime).TotalMinutes;
                var bankedTimeEntitlement = controller.GetBankTime(int.Parse(EmployeeId), Code);
                if (bankedTimeEntitlement - daysOffinHours < 0)
                {
                    throw new Exception("You do not have " + Code + " to book this day off.");
                }
                else
                {
                    controller.BookDayOff(Code, startTime, endTime, int.Parse(EmployeeId));
                }
            }

            else
            {
                var entitlement = controller.GetAbsenceEntitlement(int.Parse(EmployeeId), Code);
                var dayOffinDays = (endTime - startTime);
                if (Code == "FWT")
                {
                    if (entitlement - dayOffinDays.TotalHours < 0)
                    {
                        throw new Exception("You do not have enough to book this day off as " + Code + ".");
                    }
                    else
                    {
                        controller.BookDayOff(Code, startTime, endTime, int.Parse(EmployeeId));
                    }
                }
                else
                {
                    if (entitlement - dayOffinDays.TotalDays < 0)
                    {
                        throw new Exception("You do not have enough to book this day off as " + Code + ".");
                    }
                    else
                    {
                        controller.BookDayOff(Code, startTime, endTime, int.Parse(EmployeeId));
                    }

                }


            }


        }
        /// <summary>
        /// WebMethod to transform the data from the database to a Json object.
        /// Created By: Emily Urdaneta
        /// Created On: April 16,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetAbsenceCodes()
        {
            DayOffController controller = new DayOffController();
            List<AbsenceCodes> listOfCodes = controller.GetAllAbsenceCodes();
            // Serialize the list to JSON objects array string
            var jsonObjArray = JsonConvert.SerializeObject(listOfCodes);
            return jsonObjArray;
        }
    }
}