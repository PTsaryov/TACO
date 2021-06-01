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

namespace TACOSystem.BLL.Security
{
    #region Query
    [DataObject]
    public class RoleController
    {
        /// <summary>
        /// This method returns a list of Security Roles.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Aaron Carlson
        /// Created On: April 4,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns>List of Security Roles</returns>
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<RoleInformation> RoleList()
        {
            using (var context = new TacoContext())
            {
                var result = from role in context.TB_TACO_SecurityRole
                             orderby role.SecurityRole
                             select new RoleInformation
                             {
                                 SecurityRoleId = role.SecurityRoleId,
                                 SecurityRole = role.SecurityRole
                             };
                return result.ToList();
            }
        }

        /// <summary>
        /// This method returns a list of the security role details for the selected security role.
        /// Created By: Aaron Carlson
        /// Created On: April 4,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public RoleInformation GetRoleInformation(int roleId)
        {
            using (var context = new TacoContext())
            {
                var result = from role in context.TB_TACO_SecurityRole
                             where role.SecurityRoleId == roleId
                             select new RoleInformation
                             {
                                 SecurityRoleId = role.SecurityRoleId,
                                 SecurityRole = role.SecurityRole,
                                 RoleDescription = role.RoleDescription
                             };
                return result.Single();
            }
        }

        #endregion

        #region Processing

        /// <summary>
        /// <para>
        /// This method creates a new security role.</para>
        /// Created By: Aaron Carlson
        /// Created On: April 4,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <param name="newSecurityRole"></param>
        /// <param name="userId"></param>
        public void CreateSecurityRole(RoleInformation newSecurityRole, int userId)
        {
            using (var context = new TacoContext())
            {
                var newSecurityRoleInfo = new TB_TACO_SecurityRole();

                newSecurityRoleInfo.SecurityRole = newSecurityRole.SecurityRole;
                newSecurityRoleInfo.RoleDescription = newSecurityRole.RoleDescription;
                newSecurityRoleInfo.CreatedBy = userId;
                newSecurityRoleInfo.CreatedOn = DateTime.Now;

                context.TB_TACO_SecurityRole.Add(newSecurityRoleInfo);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// <para>
        /// This method updates a security role.</para>
        /// Created By: Aaron Carlson
        /// Created On: April 4,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <param name="updatedSecurityRole"></param>
        /// <param name=""></param>
        public void UpdateSecurityRole(RoleInformation updatedSecurityRole, int userId)
        {
            using (var context = new TacoContext())
            {
                var securityRoleInfo = context.TB_TACO_SecurityRole.Find(updatedSecurityRole.SecurityRoleId);
                if (securityRoleInfo == null)
                {
                    throw new Exception("Security role does not exist");
                }
                else
                {
                    securityRoleInfo.SecurityRoleId = updatedSecurityRole.SecurityRoleId;
                    securityRoleInfo.SecurityRole = updatedSecurityRole.SecurityRole;
                    securityRoleInfo.RoleDescription = updatedSecurityRole.RoleDescription;
                    securityRoleInfo.ModifiedBy = userId;
                    securityRoleInfo.ModifiedOn = DateTime.Now;
                    

                    context.Entry(securityRoleInfo).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }
        #endregion
    }
}
