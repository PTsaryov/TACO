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

namespace TACOSystem.BLL
{
    [DataObject]
    public class ScheduleTypeController
    {
        #region Query
        /// <summary>
        /// <para></para>
        /// This method returns a list of Schedules.
        /// Can be used to populate an ObjectDataSource.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 22,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns>List of schedules</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ScheduleInformation> ScheduleList()
        {
            using (var context = new TacoContext())
            {
                var result = from schedule in context.TB_TACO_Schedule
                             where schedule.ScheduleDeactivated != true
                             orderby schedule.ScheduleName
                             select new ScheduleInformation
                             {
                                 ScheduleId = schedule.ScheduleId,
                                 ScheduleName = schedule.ScheduleName
                             };
                return result.ToList();
            }
        }
        /// <summary>
        /// <para>
        /// This method returns a list of Schedules.
        /// Can be used to populate an ObjectDataSource.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 29,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns>List of expired Schedules</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ScheduleInformation> ExpiredScheduleList()
        {
            using (var context = new TacoContext())
            {
                var result = from schedule in context.TB_TACO_Schedule
                             where schedule.ScheduleDeactivated == true
                             orderby schedule.ScheduleName
                             select new ScheduleInformation
                             {
                                 ScheduleId = schedule.ScheduleId,
                                 ScheduleName = schedule.ScheduleName
                             };
                return result.ToList();
            }
        }

        /// <summary>
        /// <para>
        /// This method returns a list of the schedule details for the selected schedule.
        /// </para>
        /// Created By: Aaron Carlson
        /// Created On: March 22,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns>Schedule details</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public ScheduleInformation GetScheduleInformation(int scheduleId)
        {
            using (var context = new TacoContext())
            {
                var result = from schedule in context.TB_TACO_Schedule
                             where schedule.ScheduleId == scheduleId
                             select new ScheduleInformation
                             {
                                 ScheduleId = schedule.ScheduleId,
                                 ScheduleName = schedule.ScheduleName,
                                 ScheduleDescription = schedule.ScheduleDescription,
                                 ScheduleTime = schedule.ScheduleTime,
                                 ScheduleDeactivated = schedule.ScheduleDeactivated
                             };
                return result.Single();
            }
        }

        #endregion

        #region Processing

        /// <summary>
        /// <para>
        /// This method deactivates a schedule.
        /// </para>
        /// Created By: Aaron Carlson
        /// Created On: March 22,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <param name="deletedScheduleId"></param>
        /// <param name="userId"></param>
        public void DeleteSchedule(int deletedScheduleId, int userId)
        {
            using (var context = new TacoContext())
            {
                var deleteSchedule = context.TB_TACO_Schedule.Find(deletedScheduleId);
                if (deleteSchedule == null)
                {
                    throw new Exception("Schedule does not exist");
                }
                else
                {
                    deleteSchedule.ScheduleDeactivated = true;
                    deleteSchedule.ModifiedBy = userId;
                    deleteSchedule.ModifiedOn = DateTime.Today;

                    context.Entry(deleteSchedule).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// <para>
        /// This method creates a new schedule.
        /// </para>
        /// Created By: Aaron Carlson
        /// Created On: March 22,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <param name="newSchedule"></param>
        /// <param name="userId"></param>
        public void CreateSchedule(ScheduleInformation newSchedule, int userId)
        {
            using (var context = new TacoContext())
            {
                var newScheduleInfo = new TB_TACO_Schedule();

                newScheduleInfo.ScheduleName = newSchedule.ScheduleName;
                newScheduleInfo.ScheduleDescription = newSchedule.ScheduleDescription;
                newScheduleInfo.ScheduleTime = newSchedule.ScheduleTime;
                newScheduleInfo.CreatedBy = userId;
                newScheduleInfo.CreatedOn = DateTime.Now;

                context.TB_TACO_Schedule.Add(newScheduleInfo);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// <para>
        /// This method updates a schedule.
        /// </para>
        /// Created By: Aaron Carlson
        /// Created On: March 22,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="updatedScheduleInfo"></param>
        /// <param name="userId"></param>
        public void UpdateSchedule(ScheduleInformation updatedScheduleInfo, int userId)
        {
            using (var context = new TacoContext())
            {
                var scheduleInfo = context.TB_TACO_Schedule.Find(updatedScheduleInfo.ScheduleId);
                if (scheduleInfo == null)
                {
                    throw new Exception("Schedule does not exist");
                }
                else
                {
                    scheduleInfo.ScheduleId = updatedScheduleInfo.ScheduleId;
                    scheduleInfo.ScheduleName = updatedScheduleInfo.ScheduleName;
                    scheduleInfo.ScheduleDescription = updatedScheduleInfo.ScheduleDescription;
                    scheduleInfo.ScheduleTime = updatedScheduleInfo.ScheduleTime;
                    scheduleInfo.ScheduleDeactivated = updatedScheduleInfo.ScheduleDeactivated;
                    scheduleInfo.ModifiedBy = userId;
                    scheduleInfo.ModifiedOn = DateTime.Now;

                    context.Entry(scheduleInfo).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// <para>
        /// This method activates an Schedule Type.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 19,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 20, 2019
        /// </summary>
        /// <param name="role">The role of the user</param>
        /// <param name="scheduleTypeId">Passed Schedule Type to activate</param>
        public void ActivateScheduleType (int userId, string role, int scheduleTypeId)
        {
            using (TacoContext context = new TacoContext())
            {
                if (role.ToLower() == "globaladmin")
                {
                    var scheduleType = context.TB_TACO_Schedule.Find(scheduleTypeId);
                    scheduleType.ScheduleDeactivated= false;
                    scheduleType.ModifiedBy = userId;
                    scheduleType.ModifiedOn = DateTime.Now;

                    context.Entry(scheduleType).State = EntityState.Modified;
                    context.SaveChanges();
                }

            }

        }




        #endregion
    }
}
