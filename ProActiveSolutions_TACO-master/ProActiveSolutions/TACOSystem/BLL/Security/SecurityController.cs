using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TACOData.Entities.POCOs;
using TACOData.Security;
using TACOSystem.DAL;

namespace TACOSystem.BLL.Security
{
    [DataObject]
    public static class SecurityController
    {
        #region Fixed security roles
        public const string SUser = nameof(SUser);

        private static LoggedInUser result;

        #endregion

        /// <summary>
        /// <para>
        /// This method (overloaded) will check if the windows user account is in the database
        /// if yes it will return a user object which consist of: name, role and id</para>
        /// Created By: Anton Drantiev
        /// Created On: March 28,2019
        /// Modified By: Anton Drantiev
        /// Modified On: April 07,2019
        /// </summary>
        /// <param name="windowsAccountName">Found windows account name</param>
        public static LoggedInUser FindAccount()
        {
            return FindAccount(Thread.CurrentPrincipal?.Identity?.Name);
        }
        public static LoggedInUser FindAccount(string windowsAccountName)
        {
            if (string.IsNullOrWhiteSpace(windowsAccountName))
                throw new ArgumentNullException(nameof(windowsAccountName), "You must supply some windows account name");
            using (var context = new TacoContext())
            {

                var found = context.TB_TACO_Employee
                        .Where(x => x.EmployeeNumber.ToLower() == windowsAccountName.ToLower()
                        && x.TerminationDate == null).FirstOrDefault();

                if (found != null)
                {
                    result = new LoggedInUser
                    {
                        AccountID = found.EmployeeId,
                        DisplayName = found.EmployeeNumber,
                        SecurityRoleName = context.TB_TACO_SecurityRole.Where(x => x.SecurityRoleId == found.SecurityRoleId).Select(x => x.SecurityRole).SingleOrDefault()
                    };
                }
                else
                {
                    result = new LoggedInUser
                    {
                        AccountID = 0,
                        DisplayName = "",
                        SecurityRoleName = ""
                    };
                }

                return result;
            }
        }

        public static object FindAccount(object windowsAccountName)
        {
            throw new NotImplementedException();
        }
    }


}

