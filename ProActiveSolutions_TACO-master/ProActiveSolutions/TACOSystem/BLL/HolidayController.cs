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
    public class HolidayController
    {
        /// <summary>
        /// This method returns a list of holidays. It does not take any parameters.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Pavel Tsaryov
        /// Created On: April 5, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns> list of holidays </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<HolidayInformation> HolidayList()
        {
            using (var context = new TacoContext())
            {
                var result = from holiday in context.TB_TACO_Holiday
                             where holiday.HolidayDeactivated != true
                             select new HolidayInformation
                             {
                                 HolidayId = holiday.HolidayId,
                                 HolidayName = holiday.HolidayName,
                                 Date = holiday.HolidayDate,
                                 HolidayDeactivated = holiday.HolidayDeactivated
                             };
                return result.ToList();

            }
        }

        /// <summary>
        /// This method returns a list of deactivated holidays. It does not take any parameters.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Pavel Tsaryov
        /// Created On: April 11, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns> list of deactivated holidays </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<HolidayInformation> ExpiredHolidayList()
        {
            using (var context = new TacoContext())
            {
                var result = from holiday in context.TB_TACO_Holiday
                             where holiday.HolidayDeactivated == true
                             select new HolidayInformation
                             {
                                 HolidayId = holiday.HolidayId,
                                 HolidayName = holiday.HolidayName,
                                 Date = holiday.HolidayDate,
                                 HolidayDeactivated = holiday.HolidayDeactivated
                             };
                return result.ToList();
            }
        }

        #region Query
        /// <summary>
        /// This method returns the details of a holiday.
        /// Created By: Pavel Tsaryov
        /// Created On: March 12, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="holidayName"></param>
        /// <returns>a single instance of an area</returns>
        public HolidayInformation HolidayById(int holidayid)
        {
            using (var context = new TacoContext())
            {
                var result = from holiday in context.TB_TACO_Holiday
                             where holiday.HolidayId == holidayid
                             select new HolidayInformation
                             {
                                 HolidayId = holiday.HolidayId,
                                 HolidayName = holiday.HolidayName,
                                 Date = holiday.HolidayDate,
                                 HolidayDeactivated = holiday.HolidayDeactivated
                             };
                return result.Single();
            }
        }
        #endregion

        #region Processing
        /// <summary>
        /// This method creates an instance of the details of an area.
        /// Created By: Pavel Tsaryov
        /// Created On: March 12, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="newHolidayInforation"></param>
        public void CreateNewHoliday(HolidayInformation newHolidayInformation)
        {
            using (var context = new TacoContext())
            {
                var newHoliday = new TB_TACO_Holiday();

                newHoliday.HolidayId = newHolidayInformation.HolidayId;
                newHoliday.HolidayName = newHolidayInformation.HolidayName;
                newHoliday.HolidayDate = newHolidayInformation.Date;
                newHoliday.CreatedBy = newHolidayInformation.CreatedBy;
                newHoliday.CreatedOn = newHolidayInformation.CreatedOn;

                context.TB_TACO_Holiday.Add(newHoliday);
                context.SaveChanges();

            }
        }

        /// <summary>
        /// This method updates the details of a holiday.
        /// Created By: Pavel Tsaryov
        /// Created On: March 12, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// </summary>
        /// <param name="updatedHoliday"></param>
        public void UpdateHoliday(HolidayInformation updatedHoliday)
        {
            using (var context = new TacoContext())
            {
                var holidayRecord = context.TB_TACO_Holiday.Find(updatedHoliday.HolidayId);
                if (holidayRecord == null)
                {
                    throw new Exception("Holiday does not exist.");
                }
                else
                {
                    holidayRecord.HolidayId = updatedHoliday.HolidayId;
                    holidayRecord.HolidayName = updatedHoliday.HolidayName;
                    holidayRecord.HolidayDate = updatedHoliday.Date;
                    holidayRecord.HolidayDeactivated = updatedHoliday.HolidayDeactivated;
                    holidayRecord.ModifiedBy = updatedHoliday.ModifiedBy;
                    holidayRecord.ModifiedOn = updatedHoliday.ModifiedOn;

                    context.Entry(holidayRecord).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }


        public void ActivateHoliday(HolidayInformation activatedholidayinfo)
        {
            using (var context = new TacoContext())
            {
                var actHoliday = context.TB_TACO_Holiday.Find(activatedholidayinfo.HolidayId);

                if (actHoliday == null)
                {
                    throw new Exception("Holiday does not exist");
                }

                else
                {
                    actHoliday.HolidayId = activatedholidayinfo.HolidayId;
                    actHoliday.HolidayDeactivated = false;
                    actHoliday.ModifiedBy = activatedholidayinfo.ModifiedBy;
                    actHoliday.ModifiedOn = DateTime.Today;

                    context.Entry(actHoliday).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        #endregion
    }
}
