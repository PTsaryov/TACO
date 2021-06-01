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
    public class PositionController
    {
        #region Query
        /// <summary>
        /// <para>
        /// This method returns a list of active positions. It does not take any parameters.
        /// Can be used to populate an ObjectDataSource.</para>
        /// Created By: Emily Urdaneta
        /// Created On: February 27,2019
        /// Modified By: Anton Drantiev 
        /// Modified On: April 22 ,2019
        /// </summary>
        /// <returns> List of active positions</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<PositionInformation> PositionDetails()
        {
            using (TacoContext context = new TacoContext())
            {
                var activePositions = context.TB_TACO_Position
                             .Where(x => x.PositionDeactivated == false)
                             .Select(y => new PositionInformation
                             {
                                 PositionId = y.PositionId,
                                 PositionDescription = y.PositionDescription,
                                 PositionName = y.PositionName
                             });
                return activePositions.ToList();

            }
        }

        /// <summary>
        /// <para>
        /// This method returns a list of deactivated positions. It does not take any parameters.
        /// Can be used to populate an ObjectDataSource.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 22 ,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <returns> List of deactivated positions</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<PositionInformation> DeactivatedPositions()
        {
            using (TacoContext context = new TacoContext())
            {
                var deactivatedPositions = context.TB_TACO_Position
                             .Where(x =>x.PositionDeactivated == true)
                             .Select(y  => new PositionInformation
                             {
                                 PositionId = y.PositionId, 
                                 PositionDescription = y.PositionDescription,
                                 PositionName = y.PositionName
                             });
                return deactivatedPositions.ToList();
            }
        }

        /// <summary>
        /// <para>
        /// This method gets the position information by position id.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 22 ,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="id">Passed position id to lookup for</param>
        /// <returns>PositionInformation object</returns>
        public PositionInformation GetPosition(int id)
        {
            using (TacoContext context = new TacoContext())
            {
                PositionInformation results = context.TB_TACO_Position
                    .Where(x => x.PositionId == id)
                    .Select(y => new PositionInformation
                    {
                        PositionId = y.PositionId,
                        PositionName = y.PositionName,
                        PositionDescription = y.PositionDescription
                    }).SingleOrDefault();
                return results;
            }
        }

        #endregion
        #region Processing


        /// <summary>
        /// <para>
        /// This method creates a position.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 22 ,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="position">Passed PositionInformation object</param>
        /// <param name="employeeId">Passed employee id</param>
        public void CreatePosition(PositionInformation position, int employeeId)
        {
            using (TacoContext context = new TacoContext())
            {
                var positionNameExists = context.TB_TACO_Position
                    .Where(x => x.PositionName.ToLower() == position.PositionName.ToLower())
                    .SingleOrDefault();
                if (positionNameExists == null)
                {
                    TB_TACO_Position newPosition = new TB_TACO_Position()
                    {
                        PositionName = position.PositionName,
                        PositionDescription = position.PositionDescription,
                        PositionDeactivated = false,
                        CreatedBy = employeeId,
                        CreatedOn = DateTime.Now
                    };
                    context.TB_TACO_Position.Add(newPosition);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The position name already exists.");
                }

            }
        }

        /// <summary>
        /// <para>
        /// This method updates an existing position.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 22 ,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="position">Passed PositionInformation object</param>
        /// <param name="employeeId">Passed employee id</param>
        /// <param name="deactivate">Passed position activation status</param>
        public void UpdatePosition(PositionInformation position, int employeeId, bool deactivate)
        {
            using (TacoContext context = new TacoContext())
            {
                var existingPosition = context.TB_TACO_Position.Find(position.PositionId);

                existingPosition.PositionName = position.PositionName;
                existingPosition.PositionDescription = position.PositionDescription;
                existingPosition.PositionDeactivated = deactivate;
                existingPosition.ModifiedBy = employeeId;
                existingPosition.ModifiedOn = DateTime.Now;

                context.Entry(existingPosition).State = EntityState.Modified;
                context.SaveChanges();

            }
        }


        /// <summary>
        /// <para>
        /// This method activates a position.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 22,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="employeeId">Passed employee id</param>
        /// <param name="role">The role of the user</param>
        /// <param name="positionId">Passed position id to activate</param>
        public void ActivatePosition(int employeeId, string role, int positionId)
        {
            using (TacoContext context = new TacoContext())
            {
                if (role.ToLower() == "globaladmin")
                {
                    var position = context.TB_TACO_Position.Find(positionId);
                    position.PositionDeactivated = false;
                    position.ModifiedBy = employeeId;
                    position.ModifiedOn = DateTime.Now;

                    context.Entry(position).State = EntityState.Modified;
                    context.SaveChanges();
                }

            }

        }

        /// <summary>
        /// <para>
        /// This method deactivates a position.</para>
        /// Created By: Anton Drantiev
        /// Created On: April 22,2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <param name="employeeId">Passed employee id</param>
        /// <param name="role">The role of the user</param>
        /// <param name="positionId">Passed position id to activate</param>
        public void DeactivatePosition(int employeeId, string role, int positionId)
        {
            using (TacoContext context = new TacoContext())
            {
                if (role.ToLower() == "globaladmin")
                {
                    var position = context.TB_TACO_Position.Find(positionId);
                    position.PositionDeactivated = true;
                    position.ModifiedBy = employeeId;
                    position.ModifiedOn = DateTime.Now;

                    context.Entry(position).State = EntityState.Modified;
                    context.SaveChanges();
                }

            }

        }
        #endregion
    }
}
