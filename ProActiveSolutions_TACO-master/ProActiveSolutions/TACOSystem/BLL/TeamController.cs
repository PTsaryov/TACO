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
    public class TeamController
    {
        /// <summary>
        /// This method returns a list of teams. It does not take any parameters.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Emily Urdaneta
        /// Created On: February 27,2019
        /// Modified By: Pavel Tsaryov
        /// Modified On: April 11, 2019
        /// </summary>
        /// <returns> list of teams </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<TeamInformation> TeamDetails()
        {
            using (var context = new TacoContext())
            {
                var result = from team in context.TB_TACO_Team
                             where team.ExpiryDate > DateTime.Today
                             select new TeamInformation
                             {
                                 TeamId = team.TeamId,
                                 UnitId = team.UnitId,
                                 TeamDescription = team.TeamDescription,
                                 TeamName = team.TeamName,
                                 StartDate = team.StartDate,
                                 ExpiryDate = team.ExpiryDate,
                                 CreatedBy = team.CreatedBy,
                                 CreatedOn = team.CreatedOn,
                                 ModifiedBy = team.ModifiedBy,
                                 ModifiedOn = team.ModifiedOn
                             };

                return result.ToList();

            }
        }

        /// <summary>
        /// This method returns a list of expired teams. It does not take any parameters.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Pavel Tsaryov
        /// Created On: April 11,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns> list of expired teams </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<TeamInformation> ExpiredTeamDetails()
        {
            using (var context = new TacoContext())
            {
                var result = from team in context.TB_TACO_Team
                             where team.ExpiryDate <= DateTime.Today
                             select new TeamInformation
                             {
                                 TeamId = team.TeamId,
                                 UnitId = team.UnitId,
                                 TeamDescription = team.TeamDescription,
                                 TeamName = team.TeamName,
                                 StartDate = team.StartDate,
                                 ExpiryDate = team.ExpiryDate,
                                 CreatedBy = team.CreatedBy,
                                 CreatedOn = team.CreatedOn,
                                 ModifiedBy = team.ModifiedBy,
                                 ModifiedOn = team.ModifiedOn
                             };

                return result.ToList();

            }
        }

        /// <summary>
        /// This method returns one team via a name parameter
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Pavel Tsaryov
        /// Created On: March 27,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns> instance of one team </returns>
        public TeamInformation TeamById(int teamid)
        {
            using (var context = new TacoContext())
            {
                var result = from team in context.TB_TACO_Team
                             where team.TeamId == teamid
                             select new TeamInformation
                             {
                                 TeamId = team.TeamId,
                                 UnitId = team.UnitId,
                                 TeamDescription = team.TeamDescription,
                                 TeamName = team.TeamName,
                                 StartDate = team.StartDate,
                                 ExpiryDate = team.ExpiryDate,
                                 CreatedBy = team.CreatedBy,
                                 CreatedOn = team.CreatedOn,
                                 ModifiedBy = team.ModifiedBy,
                                 ModifiedOn = team.ModifiedOn

                                 //TODO: Team Member >>> Members can be done via lookup? why is it a list in a poco?

                             };
                return result.Single();

            }
        }

        #region Processing
        /// <summary>
        /// This method creates an instance of the details of a team.
        /// Created By: Pavel Tsaryov
        /// Created On: March 29, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="newTeamInformation"></param>
        public void CreateNewTeam(TeamInformation newTeamInformation)
        {
            using (var context = new TacoContext())
            {
                var newTeam = new TB_TACO_Team()
                {
                    TeamName = newTeamInformation.TeamName,
                    TeamId = newTeamInformation.TeamId,
                    UnitId = newTeamInformation.UnitId,
                    TeamDescription = newTeamInformation.TeamDescription,
                    StartDate = newTeamInformation.StartDate,
                    ExpiryDate = newTeamInformation.ExpiryDate,
                    CreatedBy = newTeamInformation.CreatedBy,
                    CreatedOn = DateTime.Now
                };

                context.TB_TACO_Team.Add(newTeam);
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
        public void UpdateTeam(TeamInformation updatedTeam)
        {
            using (var context = new TacoContext())
            {
                var teamRecord = context.TB_TACO_Team.Find(updatedTeam.TeamId);
                if (teamRecord == null)
                {
                    throw new Exception("Team does not exist.");
                }
                else
                {
                    teamRecord.TeamName = updatedTeam.TeamName;
                    teamRecord.TeamId = updatedTeam.TeamId;
                    teamRecord.UnitId = updatedTeam.UnitId;
                    teamRecord.TeamDescription = updatedTeam.TeamDescription;
                    teamRecord.StartDate = updatedTeam.StartDate;
                    teamRecord.ExpiryDate = updatedTeam.ExpiryDate;
                    teamRecord.ModifiedBy = updatedTeam.ModifiedBy;
                    teamRecord.ModifiedOn = DateTime.Now;

                    context.Entry(teamRecord).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        public void ActivateTeam(TeamInformation activatedteaminfo)
        {
            using (var context = new TacoContext())
            {
                var activateTeam = context.TB_TACO_Team.Find(activatedteaminfo.TeamId);

                if (activateTeam == null)
                {
                    throw new Exception("Department does not exist");
                }

                else
                {
                    activateTeam.TeamId = activatedteaminfo.TeamId;
                    activateTeam.ExpiryDate = DateTime.Parse("December 31, 9999");
                    activateTeam.ModifiedBy = activatedteaminfo.ModifiedBy;
                    activateTeam.ModifiedOn = DateTime.Today;

                    context.Entry(activateTeam).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }


        #endregion
    }
}
