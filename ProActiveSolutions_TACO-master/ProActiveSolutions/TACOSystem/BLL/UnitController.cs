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
    public class UnitController
    {
        /// <summary>
        /// <para>
        /// This method returns a list of Units.
        /// Can be used to populate an ObjectDataSource.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 1,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <returns>List of units</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UnitInformation> UnitList()
        {
            using (var context = new TacoContext())
            {
                var result = from unit in context.TB_TACO_Unit
                             where unit.ExpiryDate >= DateTime.Now
                             orderby unit.UnitName
                             select new UnitInformation
                             {
                                 UnitId = unit.UnitId,
                                 UnitName = unit.UnitName
                             };
                return result.ToList();
            }
        }

        /// <summary>
        /// <para>
        /// This method returns a list of Units.
        /// Can be used to populate an ObjectDataSource.</para>
        /// Created By: Anton Drantiev
        /// Created On: March 22,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <returns>List of units</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UnitInformation> UnitListExpired()
        {
            using (var context = new TacoContext())
            {
                var result = from unit in context.TB_TACO_Unit
                             where unit.ExpiryDate < DateTime.Now
                             orderby unit.UnitName
                             select new UnitInformation
                             {
                                 UnitId = unit.UnitId,
                                 UnitName = unit.UnitName
                             };
                return result.ToList();
            }
        }

        /// <summary>
        /// <para>
        /// This method returns a list of Areas.
        /// Can be used to populate an ObjectDataSource.</para>
        /// Created By: Aaron Carlson
        /// Created On: February 28,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns>List of Areas</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AreaDetails> AreaList()
        {
            using (var context = new TacoContext())
            {
                var result = from area in context.TB_TACO_Area
                             orderby area.AreaName
                             select new AreaDetails
                             {
                                 AreaId = area.AreaId,
                                 AreaName = area.AreaName
                             };
                return result.ToList();
            }
        }

        /// <summary>
        /// <para>
        /// This method returns a list of the unit details.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 1,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="unitSearch"></param>
        /// <returns>Unit details</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public UnitInformation GetUnitInformation(int UnitId)
        {
            using (var context = new TacoContext())
            {
                var result = from unit in context.TB_TACO_Unit
                             where unit.UnitId == UnitId
                             select new UnitInformation
                             {
                                 UnitId = unit.UnitId,
                                 AreaId = unit.AreaId,
                                 UnitName = unit.UnitName,
                                 UnitDescription = unit.UnitDescription,
                                 StartDate = unit.StartDate,
                                 ExpiryDate = unit.ExpiryDate
                             };
                return result.Single();
            }
        }
        #endregion

        #region Processing

        /// <summary>
        /// <para>
        /// Creates a new unit.
        /// Takes an instance of the POCO UnitInformation.
        /// It does not return anything.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 1, 2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <param name="newUnitInformation"></param>
        /// <param name="userId"></param>
        public void CreateUnit(UnitInformation newUnitInformation, int userId)
        {
            using (var context = new TacoContext())
            {
                var newUnit = new TB_TACO_Unit();

                newUnit.AreaId = newUnitInformation.AreaId;
                newUnit.UnitName = newUnitInformation.UnitName;
                newUnit.UnitDescription = newUnitInformation.UnitDescription;
                newUnit.StartDate = newUnitInformation.StartDate;
                newUnit.ExpiryDate = newUnitInformation.ExpiryDate;
                newUnit.CreatedBy = userId;
                newUnit.CreatedOn = DateTime.Now;

                context.TB_TACO_Unit.Add(newUnit);
                context.SaveChanges();

            }
        }

        /// <summary>
        /// <para>
        /// Updates an existing unit.
        /// Takes an instance of the POCO UnitInformation.
        /// It does not return anything.</para>
        /// Created By: Aaron Carlson
        /// Created On: March 1, 2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <param name="updatedUnit"></param>
        /// <param name="userId"></param>
        public void UpdateUnit(UnitInformation updatedUnit, int userId)
        {
            using (var context = new TacoContext())
            {
                var unitInfo = context.TB_TACO_Unit.Find(updatedUnit.UnitId);

                if (unitInfo == null)
                {
                    throw new Exception("Unit does not exist");
                }

                else
                {
                    unitInfo.UnitId = updatedUnit.UnitId;
                    unitInfo.AreaId = updatedUnit.AreaId;
                    unitInfo.UnitName = updatedUnit.UnitName;
                    unitInfo.UnitDescription = updatedUnit.UnitDescription;
                    unitInfo.StartDate = updatedUnit.StartDate;
                    unitInfo.ExpiryDate = updatedUnit.ExpiryDate;
                    unitInfo.ModifiedBy = userId;
                    unitInfo.ModifiedOn = DateTime.Now;

                    context.Entry(unitInfo).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// <para>
        /// Deactivates an existing unit.
        /// Takes an instance of the POCO UnitInformation.
        /// It does not return anything.</para>
        /// Created By: Anton Drantiev
        /// Created On: March 22, 2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="userId"></param>
        public void DeleteUnit(int unitId, int userId)
        {
            using (var context = new TacoContext())
            {
                var unitInfo = context.TB_TACO_Unit.Find(unitId);

                if (unitInfo == null)
                {
                    throw new Exception("Unit does not exist");
                }

                else
                {
                    unitInfo.ExpiryDate = DateTime.Now;
                    
                    unitInfo.ModifiedBy = userId;
                    unitInfo.ModifiedOn = DateTime.Now;


                    context.Entry(unitInfo).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// <para>
        /// This method activates a unit.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 19,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 20, 2019
        /// </summary>
        /// <param name="role">The role of the user</param>
        /// <param name="unitId">Passed unit to activate</param>
        public void ActivateUnit(int userId, string role, int unitId)
        {
            using (TacoContext context = new TacoContext())
            {
                if (role.ToLower() == "globaladmin")
                {
                    var unit = context.TB_TACO_Unit.Find(unitId);
                    unit.ExpiryDate = DateTime.Parse("12/31/9999");
                    unit.ModifiedBy = userId;
                    unit.ModifiedOn = DateTime.Now;

                    context.Entry(unit).State = EntityState.Modified;
                    context.SaveChanges();
                }

            }

        }
        #endregion
    }
}
