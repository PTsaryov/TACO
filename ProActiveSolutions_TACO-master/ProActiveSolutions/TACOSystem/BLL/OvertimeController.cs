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
    #region Query
    [DataObject]
    public class OvertimeController
    {
        /// <summary>
        /// <para>
        /// This method returns a list of active Overtime Codes.</para>
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Aaron Carlson
        /// Created On: March 31,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns>List of active overtime codes</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<OvertimeInformation> OvertimeList()
        {
            using (var context = new TacoContext())
            {
                var result = from code in context.TB_TACO_Overtime
                             where code.OvertimeDeactivated == false
                             orderby code.OvertimeCode
                             select new OvertimeInformation
                             {
                                 OvertimeId = code.OvertimeId,
                                 OvertimeCode = code.OvertimeCode
                             };
                return result.ToList();
            }
        }


        /// <summary>
        /// <para>
        /// this method returns a list of deactivated Overtime codes.</para>
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Aaron Carlson
        /// Created On: March 31,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns>List of deactivated overtime codes</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<OvertimeInformation> DeactivatedOvertimeList()
        {
            using (var context = new TacoContext())
            {
                var result = from code in context.TB_TACO_Overtime
                             where code.OvertimeDeactivated == true
                             orderby code.OvertimeCode
                             select new OvertimeInformation
                             {
                                 OvertimeId = code.OvertimeId,
                                 OvertimeCode = code.OvertimeCode
                             };
                return result.ToList();
            }
        }

        /// <summary>
        /// <para>
        /// This method returns a list of the overtime code details for the selected overtime code.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 31,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="attendanceId"></param>
        /// <returns>Overtime Code details</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public OvertimeInformation GetOvertimeInformation(int overtimeId)
        {
            using (var context = new TacoContext())
            {
                var result = from code in context.TB_TACO_Overtime
                             where code.OvertimeId == overtimeId
                             select new OvertimeInformation
                             {
                                 OvertimeId = code.OvertimeId,
                                 OvertimeCode = code.OvertimeCode,
                                 OvertimeDescription = code.OvertimeDescription,
                                 Units = code.Units,
                                 Color = code.Color,
                                 OvertimeDeactivated = code.OvertimeDeactivated
                             };
                return result.Single();
            }
        }

        #endregion

        #region Processing

        /// <summary>
        /// <para>
        /// This method creates a new overtime code.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 31,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <param name="newOvertimeCode"></param>
        /// <param name="userId"></param>
        public void CreateOvertimeCode(OvertimeInformation newOvertimeCode, int userId)
        {
            using (var context = new TacoContext())
            {
                var result = context.TB_TACO_Overtime.Where(x => x.OvertimeCode.ToLower() == newOvertimeCode.OvertimeCode.ToLower()).Select(z => z).SingleOrDefault();
                if (result == null)
                {
                    var newOvertimeCodeInfo = new TB_TACO_Overtime();

                    newOvertimeCodeInfo.OvertimeCode = newOvertimeCode.OvertimeCode;
                    newOvertimeCodeInfo.OvertimeDescription = newOvertimeCode.OvertimeDescription;
                    newOvertimeCodeInfo.Units = newOvertimeCode.Units;
                    newOvertimeCodeInfo.Color = newOvertimeCode.Color;
                    newOvertimeCodeInfo.CreatedBy = userId;
                    newOvertimeCodeInfo.CreatedOn = DateTime.Now;

                    context.TB_TACO_Overtime.Add(newOvertimeCodeInfo);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The overtime code already exists.");
                }

            }
        }

        /// <summary>
        /// <para>
        /// This method updates an overtime code.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 31,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <param name="updatedOvertimeCodeInfo"></param>
        /// <param name="userId"></param>
        public void UpdateOvertimeCode(OvertimeInformation updatedOvertimeCodeInfo, int userId)
        {
            using (var context = new TacoContext())
            {
                var result = context.TB_TACO_Overtime.Where(x => x.OvertimeCode.ToLower() == updatedOvertimeCodeInfo.OvertimeCode.ToLower()).Select(z => z.OvertimeId).SingleOrDefault();

                var overtimeCodeInfo = context.TB_TACO_Overtime.Find(updatedOvertimeCodeInfo.OvertimeId);
                if (overtimeCodeInfo == null)
                {
                    throw new Exception("Overtime does not exist");
                }
                else if (result == updatedOvertimeCodeInfo.OvertimeId || result == 0)
                {
                    overtimeCodeInfo.OvertimeId = updatedOvertimeCodeInfo.OvertimeId;
                    overtimeCodeInfo.OvertimeCode = updatedOvertimeCodeInfo.OvertimeCode;
                    overtimeCodeInfo.OvertimeDescription = updatedOvertimeCodeInfo.OvertimeDescription;
                    overtimeCodeInfo.Units = updatedOvertimeCodeInfo.Units;
                    overtimeCodeInfo.Color = updatedOvertimeCodeInfo.Color;
                    overtimeCodeInfo.OvertimeDeactivated = updatedOvertimeCodeInfo.OvertimeDeactivated;
                    overtimeCodeInfo.ModifiedBy = userId;
                    overtimeCodeInfo.ModifiedOn = DateTime.Now;

                    context.Entry(overtimeCodeInfo).State = EntityState.Modified;
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Overtime code already exists");
                }
            }
        }

        /// <summary>
        /// <para>
        /// This method deactivates an overtime code.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 31,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <param name="overtimeId"></param>
        /// <param name="userId"></param>
        public void DeleteOvertimeCode(int overtimeId, int userId)
        {
            using (var context = new TacoContext())
            {
                var deleteOvertimeCode = context.TB_TACO_Overtime.Find(overtimeId);
                if (deleteOvertimeCode == null)
                {
                    throw new Exception("Overtime code does not exist");
                }
                else
                {
                    deleteOvertimeCode.OvertimeDeactivated = true;
                    deleteOvertimeCode.ModifiedBy = userId;
                    deleteOvertimeCode.ModifiedOn = DateTime.Now;

                    context.Entry(deleteOvertimeCode).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }



        /// <summary>
        /// <para>
        /// This method activates an Overtime Code.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 19,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 20, 2019
        /// </summary>
        /// <param name="role">The role of the user</param>
        /// <param name="overtimeCodeId">Passed overtime code to activate</param>
        public void ActivateOvertimeCode(int userId, string role, int overtimeCodeId)
        {
            using (TacoContext context = new TacoContext())
            {
                if (role.ToLower() == "globaladmin")
                {
                    var overtimeCode = context.TB_TACO_Overtime.Find(overtimeCodeId);
                    overtimeCode.OvertimeDeactivated = false;
                    overtimeCode.ModifiedBy = userId;
                    overtimeCode.ModifiedOn = DateTime.Now;

                    context.Entry(overtimeCode).State = EntityState.Modified;
                    context.SaveChanges();
                }

            }

        }
    }
    #endregion
}

