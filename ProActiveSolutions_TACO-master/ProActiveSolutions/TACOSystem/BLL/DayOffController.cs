using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TACOData.Entities;
using TACOData.Entities.POCOs;
using TACOData.Entities.POCOs.DaysOff;
using TACOSystem.BLL.Security;
using TACOSystem.DAL;

namespace TACOSystem.BLL
{
    [DataObject]
    public class DayOffController
    {
        #region Query
        /// <summary>
        /// Queries all the employees and their booked days off.
        /// The initial list from this method gets passed to GetDaysOffPerMonth(List<DayOffInformation> bookedDays, int numberOfDaysPerMonth, int employeeid)
        /// to build a range of days for the selected month.
        /// 
        /// Created By: Emily Urdaneta
        /// Created On: April 5,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="month">selected month from the dropdown</param>
        /// <param name="year">selected year from the dropdown</param>
        /// <param name="employeeid">logged in employee id</param>
        /// <returns>This method returns a List<DayOffInformation></returns>
        public List<DayOffInformation> ListAllEmployeeDaysOff(int month, int year, int employeeid, string role)
        {
            using (var context = new TacoContext())
            {

                var bookedDaysOff = (from employee in context.TB_TACO_Employee
                                     where employee.TerminationDate == null
                                     select new DayOffInformation
                                     {
                                         EmployeeId = employee.EmployeeId,
                                         TimesheetId = (from timesheet in context.TB_TACO_Timesheet
                                                        where timesheet.EmployeeId == employee.EmployeeId
                                                        select timesheet.TimesheetId).FirstOrDefault(),
                                         FirstName = employee.FirstName,
                                         LastName = employee.LastName,
                                         BookedDays = (from person in context.TB_TACO_TimesheetDetail
                                                       where employee.EmployeeId == person.TB_TACO_Timesheet.EmployeeId
                                                       && person.ProjectId == null
                                                       && person.HolidayId == null
                                                       && person.StartDateTime.Month == month
                                                       && person.StartDateTime.Year == year
                                                       orderby person.StartDateTime ascending
                                                       select new BookedDaysOff
                                                       {
                                                           TimesheetDetailId = person.TimesheetDetailId,
                                                           AbsenceCode = person.TB_TACO_Attendance.AttendanceCode ??
                                                           person.TB_TACO_Overtime.OvertimeCode,
                                                           StartDateTime = person.StartDateTime,
                                                           EndDateTime = person.EndDateTime,
                                                           Duration = DbFunctions.DiffDays(person.StartDateTime, person.EndDateTime).Value
                                                       }).ToList()


                                     }).ToList();


                var numberOfDaysPerMonth = GetDaysOfMonthPerYear(month, year);
                var off = GetDaysOffPerMonth(bookedDaysOff, numberOfDaysPerMonth, employeeid, role);
                var employeeswithDaysOff = new List<DayOffInformation>();
                var foreachCounter = 1;
                int i = 0;
                foreach (var item in off)
                {
                    for (int k = i; k < foreachCounter; k++)
                    {
                        employeeswithDaysOff.Add(new DayOffInformation
                        {
                            EmployeeId = bookedDaysOff[k].EmployeeId,
                            FirstName = bookedDaysOff[k].FirstName,
                            LastName = bookedDaysOff[k].LastName,
                            BookedDaysPerMonth = item
                        });

                    }
                    i++;
                    foreachCounter++;
                }

                return employeeswithDaysOff;

            }

        }

        #endregion
        #region Methods

        /// <summary>
        /// Creates a list with the all the absence and overtime codes currently in the database.
        /// Created By: Emily Urdaneta
        /// Created On: February 15, 2018
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns>This method returns a list of absence codes.</returns>
        public List<AbsenceCodes> GetAllAbsenceCodes()
        {
            using (var context = new TacoContext())
            {

                var attendance = (from attendanceCode in context.TB_TACO_Attendance
                                  select new AbsenceCodes
                                  {
                                      Code = attendanceCode.AttendanceCode
                                  }).ToList();

                var overtimes = (from overtime in context.TB_TACO_Overtime
                                 where overtime.OvertimeCode != "OT"
                                 select new AbsenceCodes
                                 {
                                     Code = overtime.OvertimeCode
                                 }).ToList();
                var listOfAbsenceCodes = (attendance.Concat(overtimes)).ToList();

                return listOfAbsenceCodes;
            }

        }

        /// <summary>
        /// This is a helper method to create a range of days for each employee.
        /// A list is created within this method that has the length of the selected month.
        /// Instances of a day off are then inserted into the proper index in the list.
        /// Created By: Emily Urdaneta
        /// Created On: April 8,2019
        /// Modified By: Emily Urdaneta
        /// Modified On: April 14,2019
        /// </summary>
        /// <param name="bookedDays">The initial list from the method ListAllEmployeeDaysOff</param>
        /// <param name="numberOfDaysPerMonth">Calculated depending on the year and month selected on the web page</param>
        /// <param name="employeeid">logged in employee id</param>
        /// <returns></returns>
        public IEnumerable<DayOffIndicator> GetDaysOffPerMonth(List<DayOffInformation> bookedDays, int numberOfDaysPerMonth, int employeeid, string role)
        {
            var numberOfDays = Enumerable.Range(1, numberOfDaysPerMonth);
            var bookedDaysOff = new List<DayOffIndicator>();

            foreach (var employee in bookedDays)
            {
                if (employee.EmployeeId == employeeid || role != "Employee")
                {
                    if (employee.BookedDays.Count() == 0)
                    {
                        var isEmployeeOffwithValues = new List<KeyValuePair<int, string>>();
                        foreach (var item in numberOfDays)
                        {

                            isEmployeeOffwithValues.Add(new KeyValuePair<int, string>(0, " "));

                        }
                        bookedDaysOff.Add(new DayOffIndicator { Flag = isEmployeeOffwithValues });
                    }
                    else if (employee.BookedDays.Count() > 1)
                    {
                        var isEmployeeOffwithValues = new List<KeyValuePair<int, string>>();

                        foreach (var day in numberOfDays)
                        {

                            isEmployeeOffwithValues.Add(new KeyValuePair<int, string>(0, " "));

                        }

                        foreach (var item in employee.BookedDays)
                        {
                            if (item.StartDateTime.Day < numberOfDays.Count())
                            {
                                for (int j = 0; j <= item.Duration; j++)
                                {
                                    isEmployeeOffwithValues.Insert(item.StartDateTime.Day - 1, new KeyValuePair<int, string>(item.TimesheetDetailId, item.AbsenceCode));


                                }

                            }

                        }
                        isEmployeeOffwithValues.RemoveRange(numberOfDays.Count(), isEmployeeOffwithValues.Count() - numberOfDays.Count());
                        bookedDaysOff.Add(new DayOffIndicator { Flag = isEmployeeOffwithValues });
                    }
                    else
                    {
                        var isEmployeeOffwithValues = new List<KeyValuePair<int, string>>();

                        foreach (var item in employee.BookedDays)
                        {

                            foreach (var day in numberOfDays)
                            {

                                if (isEmployeeOffwithValues.Count() < numberOfDays.Count())
                                {
                                    if (item.StartDateTime.Day == day)
                                    {
                                        for (int i = 0; i <= item.Duration; i++)
                                        {
                                            isEmployeeOffwithValues.Add(new KeyValuePair<int, string>(item.TimesheetDetailId, item.AbsenceCode));
                                        }

                                    }
                                    else
                                    {
                                        isEmployeeOffwithValues.Add(new KeyValuePair<int, string>(0, " "));
                                    }

                                }


                            }

                        }
                        bookedDaysOff.Add(new DayOffIndicator { Flag = isEmployeeOffwithValues });

                    }
                }
                else
                {
                    if (employee.BookedDays.Count() == 0)
                    {
                        var isEmployeeOffwithValues = new List<KeyValuePair<int, string>>();
                        foreach (var item in numberOfDays)
                        {
                            //adding the empty space in between the quotes is essential
                            isEmployeeOffwithValues.Add(new KeyValuePair<int, string>(0, " "));

                        }
                        bookedDaysOff.Add(new DayOffIndicator { Flag = isEmployeeOffwithValues });
                    }
                    else if (employee.BookedDays.Count() > 1)
                    {
                        var isEmployeeOffwithValues = new List<KeyValuePair<int, string>>();

                        foreach (var day in numberOfDays)
                        {

                            isEmployeeOffwithValues.Add(new KeyValuePair<int, string>(0, " "));

                        }

                        foreach (var item in employee.BookedDays)
                        {
                            if (item.StartDateTime.Day < numberOfDays.Count())
                            {
                                for (int j = 0; j <= item.Duration; j++)
                                {
                                    isEmployeeOffwithValues.Insert(item.StartDateTime.Day - 1, new KeyValuePair<int, string>(item.TimesheetDetailId, "OFF"));


                                }


                            }

                        }
                        if (isEmployeeOffwithValues.Count() > numberOfDays.Count())
                        {
                            isEmployeeOffwithValues.RemoveRange(numberOfDays.Count(), isEmployeeOffwithValues.Count() - numberOfDays.Count());
                        }

                        bookedDaysOff.Add(new DayOffIndicator { Flag = isEmployeeOffwithValues });
                    }
                    else
                    {
                        var isEmployeeOffwithValues = new List<KeyValuePair<int, string>>();

                        foreach (var item in employee.BookedDays)
                        {

                            foreach (var day in numberOfDays)
                            {

                                if (isEmployeeOffwithValues.Count() < numberOfDays.Count())
                                {
                                    if (item.StartDateTime.Day == day)
                                    {
                                        for (int i = 0; i <= item.Duration; i++)
                                        {
                                            isEmployeeOffwithValues.Add(new KeyValuePair<int, string>(item.TimesheetDetailId, "OFF"));
                                        }

                                    }
                                    else
                                    {
                                        isEmployeeOffwithValues.Add(new KeyValuePair<int, string>(0, " "));
                                    }

                                }



                            }

                        }
                        bookedDaysOff.Add(new DayOffIndicator { Flag = isEmployeeOffwithValues });

                    }
                }


            }
            return bookedDaysOff;
        }
        /// <summary>
        /// Gets the banked time of the employee.
        /// Created By: Emily Urdaneta
        /// Created On: April 19,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public double GetBankTime(int employeeId, string code)
        {
            using (var context = new TacoContext())
            {
                var codeId = (from item in context.TB_TACO_Overtime
                              where item.OvertimeCode == code
                              select item.OvertimeId).FirstOrDefault();

                var entitlement = (from entry in context.TB_TACO_OvertimeBalance
                                   where entry.EmployeeId == employeeId
                                   && entry.OvertimeId == codeId
                                   select entry.TotalTime).FirstOrDefault();

                //maybe look at OT here too
                return Convert.ToDouble(entitlement);
            }
        }

        /// <summary>
        /// A helper method used to calculate the number of days in a month.
        /// Created By: Emily Urdaneta
        /// Created On: April 5,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="month">selected month from the dropdown</param>
        /// <param name="year">selected year from the dropdown</param>
        /// <returns></returns>
        private int GetDaysOfMonthPerYear(int month, int year)
        {
            int daysInMonthPerYear = DateTime.DaysInMonth(year, month);
            return daysInMonthPerYear;

        }
        /// <summary>
        /// Gets entitlement for each employee
        /// Created By: Emily Urdaneta
        /// Created On: April 19,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="code"></param>
        /// <returns>decimal</returns>
        public double GetAbsenceEntitlement(int employeeId, string code)
        {
            using (var context = new TacoContext())
            {

                var codeId = (from item in context.TB_TACO_Attendance
                              where item.AttendanceCode == code
                              select item.AttendanceId).FirstOrDefault();
                var entitlement = (from entry in context.TB_TACO_AttendanceEntitlement
                                   where entry.EmployeeId == employeeId
                                   && entry.AttendanceId == codeId
                                   select entry.TotalTime).FirstOrDefault();

                //maybe look at OT here too
                return Convert.ToDouble(entitlement);
            }

        }
        /// <summary>
        /// Gets the overtime entitlement of an employee.
        /// Created By: Emily Urdaneta
        /// Created On: April 18,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public double GetOvertimeEntitlement(int employeeId, string code)
        {
            using (var context = new TacoContext())
            {

                var codeId = (from item in context.TB_TACO_Overtime
                              where item.OvertimeCode == code
                              select item.OvertimeId).FirstOrDefault();
                var entitlement = (from entry in context.TB_TACO_OvertimeBalance
                                   where entry.EmployeeId == employeeId
                                   && entry.OvertimeId == codeId
                                   select entry.TotalTime).FirstOrDefault();

                //maybe look at OT here too
                return Convert.ToDouble(entitlement);
            }

        }

        /// <summary>
        /// Gets the day off duration
        /// Created By: Emily Urdaneta
        /// Created On: April 18,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="timesheetDetailId"></param>
        /// <returns></returns>

        public BookedDaysOff GetDayOffDuration(int timesheetDetailId)
        {
            using (var context = new TacoContext())
            {
                var duration = from entry in context.TB_TACO_TimesheetDetail
                               where entry.TimesheetDetailId == timesheetDetailId
                               select new BookedDaysOff
                               {
                                   EndDateTime = entry.EndDateTime,
                                   StartDateTime = entry.StartDateTime

                               };

                return duration.FirstOrDefault();
            }
        }

        #endregion

        #region Processing
        /// <summary>
        /// Deletes the appropriate timesheet detail from the database
        /// Created By: Anton Drantiev
        /// Created On: April 16,2019
        /// Modified By: Emily Urdaneta
        /// Modified On: April 20 ,2019
        /// </summary>
        /// <param name="Id">Id of the timesheet detail</param>
        public void DeleteTimesheetDetail(int Id)
        {
            using (var context = new TacoContext())
            {
                var timesheetDetailRecord = context.TB_TACO_TimesheetDetail.Find(Id);
                var duration = (timesheetDetailRecord.EndDateTime - timesheetDetailRecord.StartDateTime);
                var employeeId = (from employee in context.TB_TACO_Timesheet
                                  where employee.TimesheetId == timesheetDetailRecord.TimesheetId
                                  select employee.EmployeeId).FirstOrDefault();
                //find entitlement
                //add the duration
                if (timesheetDetailRecord.AttendanceId == null)
                {
                    //overtime
                    var entitlement = (from item in context.TB_TACO_OvertimeBalance
                                       where item.OvertimeId == timesheetDetailRecord.OvertimeId
                                       && item.EmployeeId == employeeId
                                       select item).FirstOrDefault();

                    entitlement.TotalTime = entitlement.TotalTime + Convert.ToDecimal(duration.TotalHours);
                    context.Entry(entitlement).State = EntityState.Modified;

                }
                else
                {
                    var entitlement = (from item in context.TB_TACO_AttendanceEntitlement
                                       where item.AttendanceId == timesheetDetailRecord.AttendanceId
                                       && item.EmployeeId == employeeId
                                       select item).FirstOrDefault();
                    var units = (from item in context.TB_TACO_Attendance
                                 where item.AttendanceId == timesheetDetailRecord.AttendanceId
                                 select item.Units).FirstOrDefault();

                    if (units == "days")
                    {
                        if (duration.TotalDays < 1)
                        {
                            entitlement.TotalTime = entitlement.TotalTime + Convert.ToDecimal(1);
                        }
                        else
                        {
                            entitlement.TotalTime = entitlement.TotalTime + Convert.ToDecimal(duration.TotalDays);
                        }
                    }
                    else
                    {
                        entitlement.TotalTime = entitlement.TotalTime + Convert.ToDecimal(duration.TotalHours);
                        
                    }
                    context.Entry(entitlement).State = EntityState.Modified;
                }
               

                
                context.Entry(timesheetDetailRecord).State = EntityState.Deleted;
                context.SaveChanges();
            }

        }
        /// <summary>
        /// Updates the appropriate timesheet detail in the database
        /// Created By: Anton Drantiev
        /// Created On: April 16,2019
        /// Modified By: Emily Urdaneta
        /// Modified On: April 20,2019 
        /// </summary>
        /// <param name="Id">Id of the timesheet detail</param>
        /// <param name="code">New code to replace the old reason for the day off</param>
        public void UpdateTimesheetDetail(int Id, string code, int employeeId)
        {
            using (var context = new TacoContext())
            {

                var timesheetDetailRecord = context.TB_TACO_TimesheetDetail.Find(Id);
                var oldOvertimeDuration = (timesheetDetailRecord.EndDateTime - timesheetDetailRecord.StartDateTime);
                var oldAttendanceCode = timesheetDetailRecord.AttendanceId;

                if (oldAttendanceCode == null)
                {
                    //get Banked Entitlement and return the duration 
                    var oldOvertimeCode = timesheetDetailRecord.OvertimeId;
                    var oldEntitlement = (from oldrecord in context.TB_TACO_OvertimeBalance
                                          where oldrecord.EmployeeId == employeeId &&
                                          oldrecord.OvertimeId == oldOvertimeCode
                                          select oldrecord).FirstOrDefault();

                    if (oldEntitlement == null)
                    {
                        throw new Exception("You do not have this entitlement.");
                    }
                    else
                    {
                        oldEntitlement.TotalTime = oldEntitlement.TotalTime + Convert.ToDecimal(oldOvertimeDuration.TotalHours);
                    }
                    context.Entry(oldEntitlement).State = EntityState.Modified;
                    //find new reason code and subtract

                    var newAbsenceCode = (from item in context.TB_TACO_Attendance
                                          where item.AttendanceCode == code
                                          select item).FirstOrDefault();
                    var newAbsenceEntitlement = (from item in context.TB_TACO_AttendanceEntitlement
                                                 where item.EmployeeId == employeeId 
                                                 && item.AttendanceId == newAbsenceCode.AttendanceId
                                                 select item).FirstOrDefault();
                    if (newAbsenceCode.Units == "days")
                    {
                        var durationGoingBack = 0.00;
                        var oldAttendanceDuration = (timesheetDetailRecord.EndDateTime - timesheetDetailRecord.StartDateTime).TotalDays;
                        if (oldAttendanceDuration < 1)
                        {
                            durationGoingBack = 1;
                        }
                        else
                        {
                            durationGoingBack = oldAttendanceDuration;
                        }

                        newAbsenceEntitlement.TotalTime = newAbsenceEntitlement.TotalTime + Convert.ToDecimal(durationGoingBack);
                        context.Entry(newAbsenceEntitlement).State = EntityState.Modified;
                    }
                    else
                    {

                        var oldAttendanceDuration = (timesheetDetailRecord.EndDateTime - timesheetDetailRecord.StartDateTime).TotalHours;
                        newAbsenceEntitlement.TotalTime = newAbsenceEntitlement.TotalTime + Convert.ToDecimal(oldAttendanceDuration);
                        context.Entry(newAbsenceEntitlement).State = EntityState.Modified;

                    }

                    timesheetDetailRecord.AttendanceId = newAbsenceCode.AttendanceId;
                    timesheetDetailRecord.OvertimeId = null;



                }
                else
                {
                    //find entitlement of old attendance
                    var oldAttendaceEntitlement = (from item in context.TB_TACO_AttendanceEntitlement
                                                   where item.AttendanceId == oldAttendanceCode
                                                   && item.EmployeeId == employeeId
                                                   select item).FirstOrDefault();
                    var oldAttendanceDetails = (from entry in context.TB_TACO_Attendance
                                                where entry.AttendanceId == oldAttendanceCode
                                                select entry).FirstOrDefault();

                    if (oldAttendanceDetails.Units == "days")
                    {
                        var durationGoingBack = 0.00;
                        var oldAttendanceDuration = (timesheetDetailRecord.EndDateTime - timesheetDetailRecord.StartDateTime).TotalDays;
                        if (oldAttendanceDuration < 1)
                        {
                            durationGoingBack = 1;
                        }
                        else
                        {
                            durationGoingBack = oldAttendanceDuration;
                        }

                        oldAttendaceEntitlement.TotalTime = oldAttendaceEntitlement.TotalTime + Convert.ToDecimal(durationGoingBack);
                        context.Entry(oldAttendaceEntitlement).State = EntityState.Modified;
                    }
                    else
                    {

                        var oldAttendanceDuration = (timesheetDetailRecord.EndDateTime - timesheetDetailRecord.StartDateTime).TotalHours;
                        oldAttendaceEntitlement.TotalTime = oldAttendaceEntitlement.TotalTime + Convert.ToDecimal(oldAttendanceDuration);
                        context.Entry(oldAttendaceEntitlement).State = EntityState.Modified;

                    }

                    //find entitlement of new attendance and subtract it from there
                    if (code == "BNK")
                    {
                        //get ID of code
                        var newCodeId = (from overtime in context.TB_TACO_Overtime
                                         where overtime.OvertimeCode == code
                                         select overtime.OvertimeId).FirstOrDefault();
                        var newOvertimeEntitlement = (from overtime in context.TB_TACO_OvertimeBalance
                                                      where overtime.OvertimeId == newCodeId
                                                      && overtime.EmployeeId == employeeId
                                                      select overtime).FirstOrDefault();
                        newOvertimeEntitlement.TotalTime = newOvertimeEntitlement.TotalTime - Convert.ToDecimal(oldOvertimeDuration.TotalHours);
                        context.Entry(newOvertimeEntitlement).State = EntityState.Modified;

                        timesheetDetailRecord.AttendanceId = null ;
                        timesheetDetailRecord.OvertimeId = newOvertimeEntitlement.OvertimeId;
                    }
                    else
                    {
                        var newAttendance = (from entry in context.TB_TACO_Attendance
                                         where entry.AttendanceCode == code
                                         select entry).FirstOrDefault();
                        var newAttendanceEntitlement = (from item in context.TB_TACO_AttendanceEntitlement
                                         where item.AttendanceId == newAttendance.AttendanceId
                                         && item.EmployeeId == employeeId
                                         select item).FirstOrDefault();
                        if (newAttendance.Units == "days")
                        {
                            var durationGoingBack = 0.00;
                            var oldAttendanceDuration = (timesheetDetailRecord.EndDateTime - timesheetDetailRecord.StartDateTime).TotalDays;
                            if (oldAttendanceDuration < 1)
                            {
                                durationGoingBack = 1;
                            }
                            else
                            {
                                durationGoingBack = oldAttendanceDuration;
                            }

                            oldAttendaceEntitlement.TotalTime = oldAttendaceEntitlement.TotalTime - Convert.ToDecimal(durationGoingBack);
                            context.Entry(oldAttendaceEntitlement).State = EntityState.Modified;
                        }
                        else
                        {

                            var oldAttendanceDuration = (timesheetDetailRecord.EndDateTime - timesheetDetailRecord.StartDateTime).TotalHours;
                            oldAttendaceEntitlement.TotalTime = oldAttendaceEntitlement.TotalTime - Convert.ToDecimal(oldAttendanceDuration);
                            context.Entry(oldAttendaceEntitlement).State = EntityState.Modified;

                        }
                        timesheetDetailRecord.AttendanceId = newAttendance.AttendanceId;
                        timesheetDetailRecord.OvertimeId = null;

                    }


                    
                }
               
                timesheetDetailRecord.ModifiedBy = employeeId;
                timesheetDetailRecord.ModifiedOn = DateTime.Now;
                context.Entry(timesheetDetailRecord).State = EntityState.Modified;
                context.SaveChanges();

            }

        }
        /// <summary>
        /// Creates an instance of a timesheet detail in the database
        /// Created By: Emily Urdaneta
        /// Created On: April 16,2019
        /// Modified By: Emily Urdaneta
        /// Modified On:
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Code"></param>
        public void BookDayOff(string code, DateTime startTime, DateTime endTime, int employeeId)
        {
            using (var context = new TacoContext())
            {

                TB_TACO_TimesheetDetail newDayOff = new TB_TACO_TimesheetDetail();
                newDayOff.TimesheetId = (from employee in context.TB_TACO_Timesheet
                                         where employee.EmployeeId == employeeId
                                         select employee.TimesheetId).FirstOrDefault();

                if (code == "BNK")
                {
                    var hours = (endTime - startTime).TotalMinutes;
                    var overtimeCodeId = (from item in context.TB_TACO_Overtime
                                          where item.OvertimeCode == code
                                          select item.OvertimeId).FirstOrDefault();
                    newDayOff.OvertimeId = overtimeCodeId;

                    

                    var newEntitlement = (from newrecord in context.TB_TACO_OvertimeBalance
                                          where newrecord.EmployeeId == employeeId &&
                                          newrecord.OvertimeId == overtimeCodeId
                                          select newrecord).FirstOrDefault();

                    newEntitlement.TotalTime = newEntitlement.TotalTime - Convert.ToDecimal(hours);
                   
                    context.Entry(newEntitlement).State = EntityState.Modified;
                    

                }
                else
                {
                    var absenceCode = (from item in context.TB_TACO_Attendance
                                       where item.AttendanceCode == code
                                       select item).FirstOrDefault();
                    newDayOff.AttendanceId = absenceCode.AttendanceId;
                    var newEntitlement = (from newrecord in context.TB_TACO_AttendanceEntitlement
                                          where newrecord.EmployeeId == employeeId &&
                                          newrecord.AttendanceId == absenceCode.AttendanceId
                                          select newrecord).FirstOrDefault();

                    //Since other entitlements are stored as hours/days
                    if (absenceCode.Units == "days")
                    {
                        var days = (startTime - endTime).TotalDays;

                        if (days < 0)
                        {
                            var duration = 1;
                            newEntitlement.TotalTime = newEntitlement.TotalTime - Convert.ToDecimal(duration);
                        }
                        else
                        {
                            newEntitlement.TotalTime = newEntitlement.TotalTime - Convert.ToDecimal(days);
                        }
                    }
                    else
                    {
                        var hours = (startTime - endTime).TotalHours;

                        if (hours < 0)
                        {
                            var duration = 8;
                            newEntitlement.TotalTime = newEntitlement.TotalTime - Convert.ToDecimal(duration);
                        }
                        else
                        {
                            newEntitlement.TotalTime = newEntitlement.TotalTime - Convert.ToDecimal(hours);
                        }

                    }


                    context.Entry(newEntitlement).State = EntityState.Modified;

                }
                newDayOff.StartDateTime = startTime;
                newDayOff.EndDateTime = endTime;
                newDayOff.ArchiveDelete = false;
                newDayOff.CreatedBy = employeeId;
                newDayOff.CreatedOn = DateTime.Now;

                context.TB_TACO_TimesheetDetail.Add(newDayOff);
                context.SaveChanges();


            }
        }
        #endregion
    }
}
