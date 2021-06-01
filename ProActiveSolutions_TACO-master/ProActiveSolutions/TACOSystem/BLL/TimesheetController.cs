using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TACOData.Entities;
using TACOData.Entities.POCOs.TimeSheet;
using TACOSystem.DAL;

namespace TACOSystem.BLL
{
    public class TimesheetController
    {
        /// <summary>
        /// <para>
        /// This method will calculate the first and last day of the week.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 16,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <returns> StartEndWeekday object</returns>
        public StartEndWeekDay GetStartEndWeekDay()
        {
            DateTime date = DateTime.Now;
            StartEndWeekDay weekDay = new StartEndWeekDay()
            {
                StartOfWeek = date.Date.AddDays(-(int)date.DayOfWeek), // Prev sunday 00:00
                EndOfWeek = date.Date.AddDays(-(int)date.DayOfWeek).AddDays(7) // Next sunday 00:00
            };
            return weekDay;
        }

        #region Query
        /// <summary>
        /// <para>
        /// This method will retrieve from the database all the projects 
        /// an employee assigned to, by employee id.</para>
        /// Created By: Anton Drantiev
        /// Created On: February 18,2019
        /// Modified By: Anton Drantiev
        /// Modified On: March 06,2019
        /// </summary>
        /// <param name="employeeId">Passed employee id</param>
        /// <returns> List of AssignedProject objects</returns>
        public List<AssignedProject> ProjectsAssignedToEmployeeById(int employeeId)
        {
            using (TacoContext context = new TacoContext())
            {
                var results = context.TB_TACO_ProjectTeam
                    .Where(x => x.EmployeeId == employeeId
                    && x.TB_TACO_Project.StartDate < DateTime.Now
                    && x.TB_TACO_Project.EndDate > DateTime.Now 
                    && x.EndDate > DateTime.Now)
                    .Select(y => new AssignedProject
                    {
                        title = y.TB_TACO_Project.ProjectName,
                        ProjectId = y.TB_TACO_Project.ProjectId,
                        color = y.TB_TACO_Project.ProjectColor
                    });
                return results.ToList();

            }
        }

        /// <summary>
        /// <para>
        /// This method will retrieve from the database all the overtimes 
        /// existing that are not deactivated.</para>
        /// Created By: Anton Drantiev
        /// Created On: March 08,2019
        /// Modified By: Anton Drantiev
        /// Modified On: March 26,2019
        /// </summary>
        /// <returns> List of AssignedOvertime objects</returns>
        public List<AssignedOvertime> AllOvertimes()
        {
            using (TacoContext context = new TacoContext())
            {
                var results = context.TB_TACO_Overtime
                    .Where(x => x.OvertimeDeactivated == false)
                    .Select(y => new AssignedOvertime
                    {
                        id = y.OvertimeId,
                        title = y.OvertimeDescription,
                        color = y.Color
                    });

                return results.ToList();

            }
        }

        /// <summary>
        /// <para>
        /// This method will retrieve from the database all the overtimes 
        /// by the employee id for the current week.</para>
        /// Created By: Anton Drantiev
        /// Created On: March 10,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 02,2019
        /// </summary>
        /// <param name="employeeId">Passed employee id</param>
        /// <returns> List of OvertimeDeatils objects</returns>
        public List<OvertimeDeatils> OvertimeByEmployee(int employeeId)
        {
            using (TacoContext context = new TacoContext())
            {
                StartEndWeekDay weekDay = GetStartEndWeekDay();

                var results = context.TB_TACO_TimesheetDetail
                                .Where(x => x.TB_TACO_Timesheet.EmployeeId == employeeId
                                && x.TB_TACO_Overtime.OvertimeId > 0
                                && x.StartDateTime >= weekDay.StartOfWeek
                                && x.EndDateTime < weekDay.EndOfWeek
                                && x.ArchiveDelete == false)
                                .Select(y => new OvertimeDeatils
                                {
                                    OvertimeId = y.TB_TACO_Overtime.OvertimeId,
                                    Description = y.TB_TACO_Overtime.OvertimeDescription,
                                    start = y.StartDateTime,
                                    end = y.EndDateTime
                                });

                return results.ToList();
            }

        }

        /// <summary>
        /// <para>
        /// This method will retrieve from the database all the timesheet details 
        /// by the employee id and role.</para>
        /// <para>
        /// For employee timesheet details will be retrieved only for the current week. 
        /// For all the other roles all timesheet details will be retrieved. </para>
        /// Created By: Anton Drantiev
        /// Created On: February 26,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 02,2019
        /// </summary>
        /// <param name="employeeId">Passed employee id</param>
        /// <param name="role">Passed security role of the employee</param>
        /// <returns> List of ProjectAndOvertimeDetail objects</returns>
        public List<ProjectAndOvertimeDetail> ProjectOvertimeByEmployee(int employeeId, string role)
        {
            using (TacoContext context = new TacoContext())
            {
                var timesheetExists = context.TB_TACO_Timesheet.SingleOrDefault(x => x.EmployeeId.Equals(employeeId));

                if (timesheetExists == null)
                {
                    List<ProjectAndOvertimeDetail> results = new List<ProjectAndOvertimeDetail>();
                    return results.ToList(); // Return empty object
                }
                else
                {
                    if (role.ToLower() == "employee")
                    {
                        StartEndWeekDay weekDay = GetStartEndWeekDay();

                        var results = context.TB_TACO_TimesheetDetail
                                        .Where(x => x.TB_TACO_Timesheet.EmployeeId == employeeId
                                        && x.StartDateTime >= weekDay.StartOfWeek
                                        && x.EndDateTime < weekDay.EndOfWeek
                                        && x.ArchiveDelete == false)
                                        .Select(y => new ProjectAndOvertimeDetail
                                        {

                                            id = y.TimesheetDetailId,
                                            ProjectId = y.TB_TACO_Project.ProjectId == y.TB_TACO_Project.ProjectId ?
                                                        y.TB_TACO_Project.ProjectId : y.TB_TACO_Overtime.OvertimeId,
                                            title = y.TB_TACO_Project.ProjectName ?? y.TB_TACO_Overtime.OvertimeDescription,
                                            start = y.StartDateTime,
                                            end = y.EndDateTime,
                                            color = y.TB_TACO_Project.ProjectColor ?? y.TB_TACO_Overtime.Color
                                        });

                        return results.ToList();
                    }
                    else
                    {
                        var results = context.TB_TACO_TimesheetDetail
                                        .Where(x => x.TB_TACO_Timesheet.EmployeeId == employeeId
                                        && x.ArchiveDelete == false)
                                        .Select(y => new ProjectAndOvertimeDetail
                                        {
                                            id = y.TimesheetDetailId,
                                            ProjectId = y.TB_TACO_Project.ProjectId == y.TB_TACO_Project.ProjectId ?
                                                        y.TB_TACO_Project.ProjectId : y.TB_TACO_Overtime.OvertimeId,
                                            title = y.TB_TACO_Project.ProjectName ?? y.TB_TACO_Overtime.OvertimeDescription,
                                            start = y.StartDateTime,
                                            end = y.EndDateTime,
                                            color = y.TB_TACO_Project.ProjectColor ?? y.TB_TACO_Overtime.Color
                                        });

                        return results.ToList();
                    }
                }
            }
        }

        /// <summary>
        /// <para>
        /// This method will retrieve from the database all the teams</para> 
        /// Created By: Anton Drantiev
        /// Created On: April 03,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 05,2019
        /// </summary>
        /// <returns> List of currently active teams</returns>
        public List<KeyValue> AllTeams()
        {
            using (TacoContext context = new TacoContext())
            {
                var results = context.TB_TACO_Team
                                .Where(x => x.ExpiryDate > DateTime.Now)
                                .Select(y => new KeyValue
                                {
                                    Key = y.TeamId,
                                    Value = y.TeamName
                                });

                return results.ToList();
            }
        }

        /// <summary>
        /// <para>
        /// This method will retrieve from the database all the teams 
        /// an employee assigned to, by employee id.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 03,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 05,2019
        /// </summary>
        /// <param name="employeeId">Passed employee id</param>
        /// <returns> List of teams by the logged in employee</returns>
        public List<KeyValue> TeamsByEmployeeId(int employeeId)
        {
            using (TacoContext context = new TacoContext())
            {
                var results = context.TB_TACO_TeamMember
                                .Where(x => x.EmployeeId == employeeId)
                                .Select(y => new KeyValue
                                {
                                    Key = y.TeamId,
                                    Value = context.TB_TACO_Team
                                                .Where(p => p.TeamId == y.TeamId)
                                                .Select(n => n.TeamName).FirstOrDefault()
                                });

                return results.ToList();
            }
        }

        /// <summary>
        /// <para>
        /// This method will retrieve from the database all the employess 
        /// that exist in the selected team</para>
        /// Created By: Anton Drantiev
        /// Created On: April 03,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 05,2019
        /// </summary>
        /// <param name="teamId">Passed team id</param>
        /// <returns> List of employees by team id</returns>
        public List<KeyValue> EmployeesByTeamId(int teamId)
        {
            using (TacoContext context = new TacoContext())
            {
                var results = context.TB_TACO_TeamMember
                                .Where(x => x.TeamId == teamId)
                                .Select(y => new KeyValue
                                {
                                    Key = y.EmployeeId,
                                    Value = context.TB_TACO_Employee
                                                .Where(p => p.EmployeeId == y.EmployeeId)
                                                .Select(n => n.EmployeeNumber).FirstOrDefault()
                                });

                return results.ToList();
            }
        }


        #endregion



        #region Processing

        /// <summary>
        /// <para>
        /// This method will create a new timesheet detail
        /// if employee is new then a new timesheet will be created before saving the timesheet detail</para>
        /// Created By: Anton Drantiev
        /// Created On: March 07,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 01,2019
        /// </summary>
        /// <param name="employeeId">Passed employee id</param>
        /// <param name="ProjectId">Passed project id</param>
        /// <param name="start">Passed start date and time</param>
        /// <param name="end">Passed end date and time</param>
        public void CreateTimesheetDetail(string role,int employeeId, int ProjectId, DateTime start, DateTime end)
        {
            StartEndWeekDay weekDay = GetStartEndWeekDay();

            using (TacoContext context = new TacoContext())
            {
                var timesheetIdExists = context.TB_TACO_Timesheet
                                            .Where(x => x.EmployeeId == employeeId)
                                            .Select(y => y.TimesheetId).SingleOrDefault();

                if (timesheetIdExists == 0)
                {
                    TB_TACO_Timesheet newTimesheet = new TB_TACO_Timesheet
                    {
                        EmployeeId = employeeId,
                        StartDate = DateTime.Now,
                        EndDate = Convert.ToDateTime("12/31/9999")
                    };
                    TB_TACO_TimesheetDetail firstNewTimesheetDetail = new TB_TACO_TimesheetDetail
                    {
                        ProjectId = ProjectId,
                        StartDateTime = start,
                        EndDateTime = end,
                        CreatedBy = employeeId,
                        CreatedOn = DateTime.Now
                    };

                    //Check if within current week
                    if (firstNewTimesheetDetail.StartDateTime > weekDay.StartOfWeek && firstNewTimesheetDetail.StartDateTime < weekDay.EndOfWeek && role.ToLower() == "employee")
                    {
                        newTimesheet.TB_TACO_TimesheetDetail.Add(firstNewTimesheetDetail);
                        context.TB_TACO_Timesheet.Add(newTimesheet);
                    }
                    else if (role.ToLower() != "employee")
                    {
                        newTimesheet.TB_TACO_TimesheetDetail.Add(firstNewTimesheetDetail);
                        context.TB_TACO_Timesheet.Add(newTimesheet);
                    }
                    
                }
                else
                {
                    TB_TACO_TimesheetDetail newTimesheetDetail = new TB_TACO_TimesheetDetail
                    {
                        TimesheetId = timesheetIdExists,
                        ProjectId = ProjectId,
                        StartDateTime = start,
                        EndDateTime = end,
                        CreatedBy = employeeId,
                        CreatedOn = DateTime.Now
                    };
                    //Check if within current week
                    if (newTimesheetDetail.StartDateTime > weekDay.StartOfWeek && newTimesheetDetail.StartDateTime < weekDay.EndOfWeek && role.ToLower() == "employee")
                    {
                        context.TB_TACO_TimesheetDetail.Add(newTimesheetDetail);
                    }
                    else if (role.ToLower() != "employee")
                    {
                        context.TB_TACO_TimesheetDetail.Add(newTimesheetDetail);
                    }
                    
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// <para>
        /// This method will create a new timesheet overtime detail as well as an overtime request,
        /// if employee is new then a new timesheet will be created before saving the timesheet detail</para>
        /// Created By: Anton Drantiev
        /// Created On: March 10,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 12,2019
        /// </summary>
        /// <param name="employeeId">Passed employee id</param>
        /// <param name="overtimeId">Passed overtime id</param>
        /// <param name="start">Passed start date and time</param>
        /// <param name="end">Passed end date and time</param>
        public void CreateTimeSheetOvertime(string role, int employeeId, int overtimeId, DateTime start, DateTime end)
        {
            StartEndWeekDay weekDay = GetStartEndWeekDay();

            using (TacoContext context = new TacoContext())
            {
                var timesheetIdExists = context.TB_TACO_Timesheet
                                            .Where(x => x.EmployeeId == employeeId)
                                            .Select(y => y.TimesheetId).SingleOrDefault();

                if (timesheetIdExists == 0)
                {
                    TB_TACO_Timesheet newTimesheet = new TB_TACO_Timesheet
                    {
                        EmployeeId = employeeId,
                        StartDate = DateTime.Now,
                        EndDate = Convert.ToDateTime("12/31/9999")
                    };

                    TB_TACO_TimesheetDetail firstOvertime = new TB_TACO_TimesheetDetail
                    {
                        OvertimeId = overtimeId,
                        StartDateTime = start,
                        EndDateTime = end,
                        CreatedBy = employeeId,
                        CreatedOn = DateTime.Now
                    };
                    //Check if within current week
                    if (firstOvertime.StartDateTime > weekDay.StartOfWeek && firstOvertime.StartDateTime < weekDay.EndOfWeek && role.ToLower() == "employee")
                    {
                        newTimesheet.TB_TACO_TimesheetDetail.Add(firstOvertime);
                        context.TB_TACO_Timesheet.Add(newTimesheet);
                    }
                    else if (role.ToLower() != "employee")
                    {
                        newTimesheet.TB_TACO_TimesheetDetail.Add(firstOvertime);
                        context.TB_TACO_Timesheet.Add(newTimesheet);
                    }


                    // Converting time to numbers
                    decimal totalTime = Convert.ToDecimal(TimeSpan.Parse((end - start).ToString()).Ticks);
                    double totalMinutes = TimeSpan.FromTicks(Convert.ToInt64(totalTime)).TotalMinutes;
                    // Create the request at the request table
                    TB_TACO_Request newRequest = new TB_TACO_Request()
                    {
                        EmployeeId = employeeId,
                        OvertimeId = overtimeId,
                        TotalTime = Convert.ToDecimal(totalMinutes) * Convert.ToDecimal(1.5), //times and a half for each request
                        Status = "Pending",
                        Date = start,
                        CreatedBy = employeeId,
                        CreatedOn = DateTime.Now

                    };
                    //Check if within current week
                    if (firstOvertime.StartDateTime > weekDay.StartOfWeek && firstOvertime.StartDateTime < weekDay.EndOfWeek && role.ToLower() == "employee")
                    {
                        context.TB_TACO_Request.Add(newRequest);
                    }
                    else if (role.ToLower() != "employee")
                    {
                        context.TB_TACO_Request.Add(newRequest);
                    }
                }
                else
                {
                    var newOvertime = new TB_TACO_TimesheetDetail
                    {
                        TimesheetId = timesheetIdExists,
                        OvertimeId = overtimeId,
                        StartDateTime = start,
                        EndDateTime = end,
                        CreatedBy = employeeId,
                        CreatedOn = DateTime.Now
                    };
                    //Check if within current week
                    if (newOvertime.StartDateTime > weekDay.StartOfWeek && newOvertime.StartDateTime < weekDay.EndOfWeek && role.ToLower() == "employee")
                    {
                        context.TB_TACO_TimesheetDetail.Add(newOvertime);
                    }
                    else if (role.ToLower() != "employee")
                    {
                        context.TB_TACO_TimesheetDetail.Add(newOvertime);
                    }

                    // Converting time to numbers
                    decimal totalTime = Convert.ToDecimal(TimeSpan.Parse((end - start).ToString()).Ticks);
                    double totalMinutes = TimeSpan.FromTicks(Convert.ToInt64(totalTime)).TotalMinutes;

                    // Create the request at the request table
                    TB_TACO_Request newRequest = new TB_TACO_Request()
                    {
                        EmployeeId = employeeId,
                        OvertimeId = overtimeId,
                        TotalTime = Convert.ToDecimal(totalMinutes) * Convert.ToDecimal(1.5),
                        Status = "Pending",
                        Date = start,
                        CreatedBy = employeeId,
                        CreatedOn = DateTime.Now
                    };
                    //Check if within current week
                    if (newOvertime.StartDateTime > weekDay.StartOfWeek && newOvertime.StartDateTime < weekDay.EndOfWeek && role.ToLower() == "employee")
                    {
                        context.TB_TACO_Request.Add(newRequest);
                    }
                    else if (role.ToLower() != "employee")
                    {
                        context.TB_TACO_Request.Add(newRequest);
                    }

                }

                context.SaveChanges();

            }

        }

        /// <summary>
        /// <para>
        /// This method will update a timesheet detail
        /// if the role updating is not an 'employee' role the modified by and modified on will be updated</para>
        /// Created By: Anton Drantiev
        /// Created On: March 10,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 03,2019
        /// </summary>
        /// <param name="id">Passed timesheet detail id</param>
        /// <param name="employeeId">Passed employee id</param>
        /// <param name="role">Passed role</param>
        /// <param name="start">Passed start date and time</param>
        /// <param name="end">Passed end date and time</param>
        public void UpdateTimesheetDetail(int id, int employeeId, string role, DateTime start, DateTime end)
        {
            using (TacoContext context = new TacoContext())
            {
                var currentItem = context.TB_TACO_TimesheetDetail.Find(id);
                
                //Check if it is overtime request
                if (currentItem.OvertimeId != null)
                {
                    var requestToUpdate = context.TB_TACO_Request
                                            .Where(x => x.EmployeeId == employeeId
                                            && x.Date.Year == currentItem.StartDateTime.Year
                                            && x.Date.Month == currentItem.StartDateTime.Month
                                            && x.Date.Day == currentItem.StartDateTime.Day
                                            && x.OvertimeId == currentItem.OvertimeId)
                                            .Select(y => y).FirstOrDefault();
                    if (requestToUpdate != null)
                    {
                        // Converting time to numbers
                        decimal totalTime = Convert.ToDecimal(TimeSpan.Parse((end - start).ToString()).Ticks);
                        double totalMinutes = TimeSpan.FromTicks(Convert.ToInt64(totalTime)).TotalMinutes;
                        requestToUpdate.TotalTime = Convert.ToDecimal(totalMinutes) * Convert.ToDecimal(1.5);
                        requestToUpdate.Date = start;
                        var existingRequest = context.Entry(requestToUpdate);
                        existingRequest.State = EntityState.Modified;
                    }
                }

                currentItem.StartDateTime = start;
                currentItem.EndDateTime = end;

                if (role.ToLower() != "employee")
                {
                    currentItem.ModifiedBy = employeeId;
                    currentItem.ModifiedOn = DateTime.Now;
                }
                var existingItem = context.Entry(currentItem);
                existingItem.State = EntityState.Modified;

                context.SaveChanges();
            }

        }

        /// <summary>
        /// <para>
        /// This method will delete a timesheet detail
        /// if the role updating is not an 'employee' role the timesheet detail will be archived</para>
        /// Created By: Anton Drantiev
        /// Created On: March 17,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 03,2019
        /// </summary>
        /// <param name="id">Passed timesheet detail id</param>
        /// <param name="employeeId">Passed employee id</param>
        /// <param name="role">Passed role</param>
        public void DeleteTimesheetDetail(int id, int employeeId, string role)
        {
            using (TacoContext context = new TacoContext())
            {
                var currentItem = context.TB_TACO_TimesheetDetail.Find(id);

                if (role.ToLower() == "employee")
                {
                    //Check if it is overtime request
                    if (currentItem.OvertimeId != null)
                    {
                        var requestToRemove = context.TB_TACO_Request
                                                .Where(x => x.EmployeeId == employeeId
                                                && x.Date == currentItem.StartDateTime
                                                && x.OvertimeId == currentItem.OvertimeId)
                                                .Select(y => y).FirstOrDefault();
                        if (requestToRemove != null)
                        {
                            var existingRequest = context.Entry(requestToRemove);
                            existingRequest.State = EntityState.Deleted;
                        }
                    }

                    var existingItem = context.Entry(currentItem);
                    existingItem.State = EntityState.Deleted;
                }
                else
                {
                    if (currentItem.OvertimeId != null)
                    {
                        var requestToRemove = context.TB_TACO_Request
                                                .Where(x => x.Date == currentItem.StartDateTime
                                                && x.OvertimeId == currentItem.OvertimeId)
                                                .Select(y => y).FirstOrDefault();
                        if (requestToRemove != null)
                        {
                            var existingRequest = context.Entry(requestToRemove);
                            existingRequest.State = EntityState.Deleted;
                        }
                    }
                    //Archive if in any admin role 
                    currentItem.ArchiveDelete = true;
                    currentItem.ModifiedBy = employeeId;
                    currentItem.ModifiedOn = DateTime.Now;
                    var existingItem = context.Entry(currentItem);
                    existingItem.State = EntityState.Modified;
                }

                context.SaveChanges();
            }

        }

        #endregion

    }
}
