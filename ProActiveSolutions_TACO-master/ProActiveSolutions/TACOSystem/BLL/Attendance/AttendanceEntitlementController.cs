using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TACOData.Entities;
using TACOData.Entities.POCOs;
using TACOSystem.DAL;

namespace TACOSystem.BLL.Attendance
{
    #region Query
    [DataObject]
    public class AttendanceEntitlementController
    {
        /// <summary>
        /// <para>
        /// This method returns a list of employees and attendance entitlements for the selected attendanceId.
        /// Used to populate update entitlements grid.
        /// </para>
        /// Created By: Aaron Carlson
        /// Created On: April 4, 2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 11, 2019
        /// </summary>
        /// <returns>List of active employees and associated attendance entitlement values</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AttendenceEntitlementInfo> AttendanceEntitlementList(int attendanceId)
        {
            using (var context = new TacoContext())
            {

                var result = from item in context.TB_TACO_AttendanceEntitlement
                             join name in context.TB_TACO_Employee
                             on item.EmployeeId equals name.EmployeeId
                             join code in context.TB_TACO_Attendance
                             on item.AttendanceId equals code.AttendanceId
                             where item.AttendanceId == attendanceId && name.TerminationDate == null || name.TerminationDate > DateTime.Now
                             orderby name.LastName
                             select new AttendenceEntitlementInfo
                             {
                                 AttendanceEntitlementId = item.AttendanceEntitlementId,
                                 EmployeeId = item.EmployeeId,
                                 FullName = name.FirstName + " " + name.LastName,
                                 AttendanceCode = code.AttendanceCode,
                                 TotalTime = item.TotalTime,
                                 Units = code.Units
                             };

                return result.ToList();
            }
        }

        /// <summary>
        /// This method returns a list of employees and attendance entitlements.
        /// Used to populate update entitlements grid.
        /// Created By: Aaron Carlson
        /// Created On: April 11, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns>List of active employees and associated attendance entitlement values</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AttendenceEntitlementInfo> AttendanceEntitlementList()
        {
            using (var context = new TacoContext())
            {
                var result = from item in context.TB_TACO_AttendanceEntitlement
                             join code in context.TB_TACO_Attendance
                             on item.AttendanceId equals code.AttendanceId
                             join name in context.TB_TACO_Employee
                             on item.EmployeeId equals name.EmployeeId
                             where name.TerminationDate == null || name.TerminationDate > DateTime.Now
                             orderby name.LastName
                             select new AttendenceEntitlementInfo
                             {
                                 AttendanceEntitlementId = item.AttendanceEntitlementId,
                                 EmployeeId = item.EmployeeId,
                                 FullName = name.FirstName + " " + name.LastName,
                                 AttendanceCode = code.AttendanceCode,
                                 TotalTime = item.TotalTime,
                                 Units = code.Units
                             };

                return result.ToList();
            }
        }

        /// <summary>
        /// This method returns a list of active employees.
        /// Used to populate an object data source.
        /// Created By: Aaron Carlson
        /// Created On: April 15, 2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 16, 2019
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AttendenceEntitlementInfo> ActiveEmployeeList()
        {
            using (var context = new TacoContext())
            {
                var result = from name in context.TB_TACO_Employee
                             where name.TerminationDate == null || name.TerminationDate > DateTime.Now
                             orderby name.LastName
                             select new AttendenceEntitlementInfo
                             {
                                 EmployeeId = name.EmployeeId,
                                 FullName = name.FirstName + " " + name.LastName
                             };
                return result.ToList();
            }
        }

        #endregion

        #region Processing
        /// <summary>
        /// <para>
        /// This method will update an attendance entitlement record
        /// </para>
        /// Created By: Aaron Carlson
        /// Created On: April 11, 2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="updatedAttendanceEntitlementInfo"></param>
        /// <param name="userId">Passed employee id of user</param>
        public void UpdateAttendanceEntitlement(AttendenceEntitlementInfo updatedAttendanceEntitlementInfo, int userId)
        {
            using (var context = new TacoContext())
            {
                var entitlementInfo = context.TB_TACO_AttendanceEntitlement.Find(updatedAttendanceEntitlementInfo.AttendanceEntitlementId);

                entitlementInfo.TotalTime = updatedAttendanceEntitlementInfo.TotalTime + entitlementInfo.TotalTime;

                entitlementInfo.ModifiedBy = userId;
                entitlementInfo.ModifiedOn = DateTime.Now;

                context.Entry(entitlementInfo).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// <para>
        /// This method will create an attendance entitlement record
        /// </para>
        /// Created By: Aaron Carlson
        /// Created On: April 15, 2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="newAttendanceEntitlementInfo"></param>
        /// <param name="userId"></param>
        public void CreateAttendanceEntitlement(AttendenceEntitlementInfo newAttendanceEntitlementInfo, int userId)
        {
            using (var context = new TacoContext())
            {
                //check if attendancecode/employeeid combination exists
                var result = context.TB_TACO_AttendanceEntitlement.Where(x => x.EmployeeId == newAttendanceEntitlementInfo.EmployeeId).Select(z => z);
                var specificCombo = result.Where(x => x.AttendanceId == newAttendanceEntitlementInfo.AttendanceId).Select(z => z).SingleOrDefault();
                if (specificCombo == null)
                {
                    var newEntitlementInfo = new TB_TACO_AttendanceEntitlement();
                    newEntitlementInfo.AttendanceId = newAttendanceEntitlementInfo.AttendanceId;
                    newEntitlementInfo.EmployeeId = newAttendanceEntitlementInfo.EmployeeId;
                    newEntitlementInfo.TotalTime = newAttendanceEntitlementInfo.TotalTime;
                    newEntitlementInfo.CreatedBy = userId;
                    newEntitlementInfo.CreatedOn = DateTime.Now;

                    context.TB_TACO_AttendanceEntitlement.Add(newEntitlementInfo);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The employee you selected already has that attendance code");
                }
                
            }
        }
        #endregion
    }
}

