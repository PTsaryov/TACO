using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using TACOData.Entities;
using TACOData.Entities.POCOs;
using TACOSystem.DAL;

namespace TACOSystem.BLL.Attendance
{
    [DataObject]
    public class AttendanceController
    {
        #region Query
        /// <summary>
        /// This method returns a list of Attendance Codes that have not been deactivated.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Aaron Carlson
        /// Created On: March 30,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns>List of active attendance codes</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AttendanceInformation> AttendanceList()
        {
            using (var context = new TacoContext())
            {
                var result = from code in context.TB_TACO_Attendance
                             where code.AttendanceDeactivated == false
                             orderby code.AttendanceCode
                             select new AttendanceInformation
                             {
                                 AttendanceId = code.AttendanceId,
                                 AttendanceCode = code.AttendanceCode
                             };
                return result.ToList();
            }
        }


        /// <summary>
        /// This method returns a list of Attendance Codes that have been deactivated.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Aaron Carlson
        /// Created On: March 30,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns>List of deactivated attendance codes</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AttendanceInformation> DeactivatedAttendanceList()
        {
            using (var context = new TacoContext())
            {
                var result = from code in context.TB_TACO_Attendance
                             where code.AttendanceDeactivated == true
                             orderby code.AttendanceCode
                             select new AttendanceInformation
                             {
                                 AttendanceId = code.AttendanceId,
                                 AttendanceCode = code.AttendanceCode
                             };
                return result.ToList();
            }
        }

        /// <summary>
        /// This method returns a list of the attendance code details for the selected attendance code.
        /// Created By: Aaron Carlson
        /// Created On: March 30,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="attendanceId"></param>
        /// <returns>Attendance Code details</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public AttendanceInformation GetAttendanceInformation(int attendanceId)
        {
            using (var context = new TacoContext())
            {
                var result = from code in context.TB_TACO_Attendance
                             where code.AttendanceId == attendanceId
                             select new AttendanceInformation
                             {
                                 AttendanceId = code.AttendanceId,
                                 AttendanceCode = code.AttendanceCode,
                                 AttendanceDescription = code.AttendanceDescription,
                                 Units = code.Units,
                                 AttendanceDeactivated = code.AttendanceDeactivated
                             };
                return result.Single();
            }
        }
        #endregion
        #region Processing

        /// <summary>
        /// <para>
        /// This method creates a new Attendance Code.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 30,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 21, 2019
        /// </summary>
        /// <param name="newAttendanceCode"></param>
        /// <param name="userId"></param>
        public void CreateAttendanceCode(AttendanceInformation newAttendanceCode, int userId)
        {
            using (var context = new TacoContext())
            {
                var result = context.TB_TACO_Attendance.Where(x => x.AttendanceCode.ToLower() == newAttendanceCode.AttendanceCode.ToLower()).Select(z => z).SingleOrDefault();
                if (result == null)
                {
                    var newAttendanceCodeInfo = new TB_TACO_Attendance();

                    newAttendanceCodeInfo.AttendanceCode = newAttendanceCode.AttendanceCode;
                    newAttendanceCodeInfo.AttendanceDescription = newAttendanceCode.AttendanceDescription;
                    newAttendanceCodeInfo.Units = newAttendanceCode.Units;
                    newAttendanceCodeInfo.CreatedBy = userId;
                    newAttendanceCodeInfo.CreatedOn = DateTime.Now;

                    context.TB_TACO_Attendance.Add(newAttendanceCodeInfo);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The attendance code already exists.");
                }
            }

        }

        /// <summary>
        /// This method updates an Attendance Code.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 30,2019
        /// Modified By:Aaron Carlson
        /// Modified On: April 21, 2019
        /// </summary>
        /// <param name="updatedAttendanceCodeInfo"></param>
        /// <param name="userId"></param>
        public void UpdateAttendanceCode(AttendanceInformation updatedAttendanceCodeInfo, int userId)
        {
            using (var context = new TacoContext())
            {
                var result = context.TB_TACO_Attendance.Where(x => x.AttendanceCode.ToLower() == updatedAttendanceCodeInfo.AttendanceCode.ToLower()).Select(z => z.AttendanceId).SingleOrDefault();

                var attendanceCodeInfo = context.TB_TACO_Attendance.Find(updatedAttendanceCodeInfo.AttendanceId);
                if (attendanceCodeInfo == null)
                {
                    throw new Exception("Attendance Code does not exist");
                }
                else if (result == updatedAttendanceCodeInfo.AttendanceId || result == 0)
                {
                    attendanceCodeInfo.AttendanceId = updatedAttendanceCodeInfo.AttendanceId;
                    attendanceCodeInfo.AttendanceCode = updatedAttendanceCodeInfo.AttendanceCode;
                    attendanceCodeInfo.AttendanceDescription = updatedAttendanceCodeInfo.AttendanceDescription;
                    attendanceCodeInfo.Units = updatedAttendanceCodeInfo.Units;
                    attendanceCodeInfo.AttendanceDeactivated = updatedAttendanceCodeInfo.AttendanceDeactivated;
                    attendanceCodeInfo.ModifiedBy = userId;
                    attendanceCodeInfo.ModifiedOn = DateTime.Now;

                    context.Entry(attendanceCodeInfo).State = EntityState.Modified;
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Attendance code already exists");
                }
            }
        }

        /// <summary>
        /// This method deactivates an Attendance Code.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 30,2019
        /// Modified By:Aaron Carlson
        /// Modified On: April 20, 2019
        /// </summary>
        /// <param name="attendanceId"></param>
        /// <param name="userId"></param>
        public void DeleteAttendanceCode(int attendanceId, int userId)
        {
            using (var context = new TacoContext())
            {
                var deleteAttendanceCode = context.TB_TACO_Attendance.Find(attendanceId);
                if (deleteAttendanceCode == null)
                {
                    throw new Exception("Attendance code does not exist");
                }
                else
                {
                    deleteAttendanceCode.AttendanceDeactivated = true;
                    deleteAttendanceCode.ModifiedBy = userId;
                    deleteAttendanceCode.ModifiedOn = DateTime.Now;

                    context.Entry(deleteAttendanceCode).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// <para>
        /// This method activates an Attendance Code.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 19,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 20, 2019
        /// </summary>
        /// <param name="role">The role of the user</param>
        /// <param name="attendnceCodeId">Passed attendance code to activate</param>
        public void ActivateAttendanceCode(int userId, string role, int attendnceCodeId)
        {
            using (TacoContext context = new TacoContext())
            {
                if (role.ToLower() == "globaladmin")
                {
                    var attendanceCode = context.TB_TACO_Attendance.Find(attendnceCodeId);
                    attendanceCode.AttendanceDeactivated = false;
                    attendanceCode.ModifiedBy = userId;
                    attendanceCode.ModifiedOn = DateTime.Now;

                    context.Entry(attendanceCode).State = EntityState.Modified;
                    context.SaveChanges();
                }

            }

        }
    }
    #endregion
}
