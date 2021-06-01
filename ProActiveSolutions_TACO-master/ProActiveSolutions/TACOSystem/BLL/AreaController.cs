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
    public class AreaController
    {
        #region Query
        /// <summary>
        /// This method returns a list of all areas.
        /// Created By: Pavel Tsaryov
        /// Created On: April 9, 2019
        /// Modified By: Pavel Tsaryov
        /// Modified On: April 11, 2019
        /// </summary>
        /// <param>no parameters</param>
        /// <returns>a list of areas</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<AreaDetails> AreaList()
        {
            using (var context = new TacoContext())
            {
                var result = from area in context.TB_TACO_Area
                             where area.ExpiryDate > DateTime.Today
                             select new AreaDetails
                             {
                                 AreaId = area.AreaId,
                                 DepartmentId = area.DepartmentId,
                                 AreaName = area.AreaName,
                                 AreaDescription = area.AreaDescription,
                                 StartDate = area.StartDate,
                                 ExpiryDate = area.ExpiryDate,
                                 CreatedBy = area.CreatedBy,
                                 CreatedOn = area.CreatedOn,
                                 ModifiedBy = area.ModifiedBy,
                                 ModifiedOn = area.ModifiedOn
                             };
                return result.ToList();
            }

        }

        /// <summary>
        /// This method returns a list of all areas.
        /// Created By: Pavel Tsaryov
        /// Created On: April 11, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param>no parameters</param>
        /// <returns>a list of areas</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<AreaDetails> ExpiredAreaList()
        {
            using (var context = new TacoContext())
            {
                var result = from area in context.TB_TACO_Area
                             where area.ExpiryDate <= DateTime.Today
                             select new AreaDetails
                             {
                                 AreaId = area.AreaId,
                                 DepartmentId = area.DepartmentId,
                                 AreaName = area.AreaName,
                                 AreaDescription = area.AreaDescription,
                                 StartDate = area.StartDate,
                                 ExpiryDate = area.ExpiryDate,
                                 CreatedBy = area.CreatedBy,
                                 CreatedOn = area.CreatedOn,
                                 ModifiedBy = area.ModifiedBy,
                                 ModifiedOn = area.ModifiedOn
                             };
                return result.ToList();
            }

        }

        /// <summary>
        /// This method returns the details of an area.
        /// Created By: Pavel Tsaryov
        /// Created On: February 28, 2018
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="areaid"></param>
        /// <returns>a single instance of an area</returns>
        public AreaDetails AreaById(int areaid)
        {
            using (var context = new TacoContext())
            {
                var result = from area in context.TB_TACO_Area
                             where area.AreaId == areaid
                             select new AreaDetails
                             {
                                 AreaId = area.AreaId,
                                 DepartmentId = area.DepartmentId,
                                 AreaName = area.AreaName,
                                 AreaDescription = area.AreaDescription,
                                 StartDate = area.StartDate,
                                 ExpiryDate = area.ExpiryDate,
                                 CreatedBy = area.CreatedBy,
                                 CreatedOn = area.CreatedOn,
                                 ModifiedBy = area.ModifiedBy,
                                 ModifiedOn = area.ModifiedOn
                             };
                return result.FirstOrDefault();
            }

        }
        #endregion

        #region Processing
        /// <summary>
        /// This method creates an instance of the details of an area.
        /// Created By: Pavel Tsaryov
        /// Created On: March 1, 2018
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="newAreaDetails"></param>
        public void CreateNewArea(AreaDetails newAreaDetails)
        {
            using (var context = new TacoContext())
            {
                var newArea = new TB_TACO_Area()
                {
                    DepartmentId = newAreaDetails.DepartmentId,
                    AreaName = newAreaDetails.AreaName,
                    AreaDescription = newAreaDetails.AreaDescription,
                    StartDate = newAreaDetails.StartDate,
                    ExpiryDate = newAreaDetails.ExpiryDate,
                    CreatedBy = newAreaDetails.CreatedBy,
                    CreatedOn = DateTime.Now
                };

                context.TB_TACO_Area.Add(newArea);
                context.SaveChanges();

            }
        }

        /// <summary>
        /// This method updates the details of an area.
        /// Created By: Pavel Tsaryov
        /// Created On: March 1, 2018
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// </summary>
        /// <param name="updatedArea"></param>
        public void UpdateArea(AreaDetails updatedArea)
        {
            using (var context = new TacoContext())
            {
                var areaRecord = context.TB_TACO_Area.Find(updatedArea.AreaId);
                if (areaRecord == null)
                {
                    throw new Exception("Area does not exist.");
                }
                else
                {
                    areaRecord.DepartmentId = updatedArea.DepartmentId;
                    areaRecord.AreaName = updatedArea.AreaName;
                    areaRecord.AreaDescription = updatedArea.AreaDescription;
                    areaRecord.StartDate = updatedArea.StartDate;
                    areaRecord.ExpiryDate = updatedArea.ExpiryDate;
                    areaRecord.ModifiedBy = updatedArea.ModifiedBy;
                    areaRecord.ModifiedOn = DateTime.Now;

                    context.Entry(areaRecord).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }



        /// <summary>
        /// <para>
        /// This method activates an Area.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 20,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="role">The role of the user</param>
        /// <param name="areaId">Passed area id to activate</param>
        public void ActivateArea(int userId, string role, int areaId)
        {
            using (TacoContext context = new TacoContext())
            {
                if (role.ToLower() == "globaladmin")
                {
                    var area = context.TB_TACO_Area.Find(areaId);
                    area.ExpiryDate = DateTime.Parse("12/31/9999");
                    area.ModifiedBy = userId;
                    area.ModifiedOn = DateTime.Now;

                    context.Entry(area).State = EntityState.Modified;
                    context.SaveChanges();
                }

            }

        }



        #endregion
    }
}
