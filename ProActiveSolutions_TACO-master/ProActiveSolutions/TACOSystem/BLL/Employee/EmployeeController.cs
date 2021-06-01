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

namespace TACOSystem.BLL.Employee
{
    [DataObject]
    public class EmployeeController
    {
        #region Query
        /// <summary>
        /// This method returns the details of an employee's profile.
        /// Created By: Emily Urdaneta
        /// Created On: February 26, 2018
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns>a single instance of an employee</returns>
        public EmployeeProfile GetEmployee(string fullName)
        {
            using (var context = new TacoContext())
            {
                var result = from employee in context.TB_TACO_Employee
                             where (employee.FirstName + " " + employee.LastName).Contains(fullName)
                             && employee.TerminationDate == null
                             select new EmployeeProfile
                             {
                                 EmployeeId = employee.EmployeeId,
                                 EmployeeNumber = employee.EmployeeNumber,
                                 FirstName = employee.FirstName,
                                 LastName = employee.LastName,
                                 HireDate = employee.HireDate,
                                 PositionId = employee.PositionId,
                                 SecurityRole = employee.SecurityRoleId,
                                 TerminationDate = employee.TerminationDate,
                                 Birthdate = employee.Birthdate,
                                 ScheduleType = employee.ScheduleId,
                                 Phone = employee.Phone,
                                 Email = employee.Email,
                                 EmergencyContact = employee.EmergencyContactName,
                                 EmergencyContactPhone = employee.EmergencyContactPhone,
                                 Station = employee.Station,
                                 Computer = employee.Computer
                             };
                if (!result.Any())
                {
                    throw new Exception("Employee has been terminated");
                }
                else if (result.Count() > 1)
                {
                    throw new Exception("There are more than one employees with that name");
                }
                else
                {
                    return result.Single();
                }

            }
        }

        /// <summary>
        /// This is an overload of the GetEmployee method. 
        /// It taked the employee ID from Windows Authentication.
        /// Created By: Emily Urdaneta
        /// Created On: April 1,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>a single instance of an employee</returns>

        public EmployeeProfile GetEmployee(int employeeId)
        {
            using (var context = new TacoContext())
            {
                var result = from employee in context.TB_TACO_Employee
                             where employee.EmployeeId == employeeId
                             select new EmployeeProfile
                             {
                                 EmployeeId = employee.EmployeeId,
                                 EmployeeNumber = employee.EmployeeNumber,
                                 FirstName = employee.FirstName,
                                 LastName = employee.LastName,
                                 HireDate = employee.HireDate,
                                 PositionId = employee.PositionId,
                                 TerminationDate = employee.TerminationDate,
                                 Birthdate = employee.Birthdate,
                                 ScheduleType = employee.ScheduleId,
                                 SecurityRole = employee.SecurityRoleId,
                                 Phone = employee.Phone,
                                 Email = employee.Email,
                                 EmergencyContact = employee.EmergencyContactName,
                                 EmergencyContactPhone = employee.EmergencyContactPhone,
                                 Station = employee.Station,
                                 Computer = employee.Computer
                             };
                if (!result.Any())
                {
                    throw new Exception("Employee has been terminated");
                }
                else if (result.Count() > 1)
                {
                    throw new Exception("There are more than one employees with that name");
                }
                else
                {
                    return result.Single();
                }

            }
        }

        /// <summary>
        /// This method returns the id of an employee by using their current login (to be used for createdBy and modifiedBy).
        /// Created By: Pavel Tsaryov
        /// Created On: March 26, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns>employee id</returns>
        public int GetEmployeeIdByEmployeeNumber()
        {
            using (var context = new TacoContext())
            {
                var result = from employee in context.TB_TACO_Employee
                             where employee.EmployeeNumber.Equals(System.Environment.UserName)
                             select employee.EmployeeId;

                if (!result.Any())
                {
                    throw new Exception("Employee cannot be found or has been terminated");
                }
                else if (result.Count() > 1)
                {
                    throw new Exception("There are more than one employees with this login");
                }
                else
                {
                    return result.Single();
                }
            }
        }

        /// <summary>
        /// This method returns the full name of an employee by using their id (to be used for createdBy and modifiedBy name retreival).
        /// Created By: Pavel Tsaryov
        /// Created On: March 26, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>employee full name</returns>
        public int GetEmployeeNameByEmployeeId(string employeeId)
        {
            using (var context = new TacoContext())
            {
                var result = from employee in context.TB_TACO_Employee
                             where employee.EmployeeId.Equals(employeeId)
                             select employee.FirstName + " " + employee.LastName;

                if (!result.Any())
                {
                    throw new Exception("Employee cannot be found or has been terminated");
                }
                else if (result.Count() > 1)
                {
                    throw new Exception("There are more than one employees with this login");
                }
                else
                {
                    return result.ToString().Single();
                }

            }
        }

        /// <summary>
        /// This method returns the details of an employee's profile.
        /// Created By: Emily Urdaneta
        /// Created On: March 22, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name=""></param>
        /// <returns>a single instance of an employee</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<SecurityRoleInformation> EmployeeSecurityRolesList()
        {
            using (var context = new TacoContext())
            {
                var result = from role in context.TB_TACO_SecurityRole
                             select new SecurityRoleInformation
                             {
                                 SecurityRoleId = role.SecurityRoleId,
                                 RoleDescription = role.RoleDescription

                             };
                return result.ToList();

            }
        }
        /// <summary>
        /// Checks if the employee number is already taken.
        /// Prevents multiple people from having the same number.
        /// Created By: Emily Urdaneta
        /// Created On: April 19, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="employeeNumber"></param>
        /// <returns></returns>
        public bool EmployeeNumberExists(string employeeNumber)
        {
            using(var context = new TacoContext())
            {
                var doesEmployeeNumberExist = from employee in context.TB_TACO_Employee
                                     where employee.EmployeeNumber == employeeNumber
                                     && employee.TerminationDate == null
                                     select employee;
                if (doesEmployeeNumberExist.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// This method returns the list of employees.
        /// This is a DataSource.
        /// Created By: Emily Urdaneta
        /// Created On: March 26, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name=""></param>
        /// <returns>list of employees</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<EmployeeProfile> EmployeeList()
        {
            using (var context = new TacoContext())
            {
                var result = from employee in context.TB_TACO_Employee
                             where employee.TerminationDate == null
                             select new EmployeeProfile
                             {
                                 EmployeeId = employee.EmployeeId,
                                 EmployeeNumber = employee.EmployeeNumber,
                                 FirstName = employee.FirstName,
                                 LastName = employee.LastName
                             };
                return result.ToList();
            }
        }
        #endregion

        #region Processing
        /// <summary>
        /// Adds the employee data into the database
        /// Created By: Emily Urdaneta
        /// Created On: March 10, 2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="newEmployeeProfile"></param>
        public void CreateEmployeeProfile(EmployeeProfile newEmployeeProfile, int employeeId, int teamId)
        {
            using (var context = new TacoContext())
            {
                var newEmployee = new TB_TACO_Employee();


                newEmployee.EmployeeNumber = newEmployeeProfile.EmployeeNumber;
                newEmployee.FirstName = newEmployeeProfile.FirstName;
                newEmployee.LastName = newEmployeeProfile.LastName;
                newEmployee.PositionId = newEmployeeProfile.PositionId;
                newEmployee.ScheduleId = newEmployeeProfile.ScheduleType;
                newEmployee.SecurityRoleId = newEmployeeProfile.SecurityRoleId;
                newEmployee.TerminationDate = null;
                newEmployee.Birthdate = newEmployeeProfile.Birthdate;
                newEmployee.HireDate = newEmployeeProfile.HireDate;
                newEmployee.EmergencyContactName = newEmployeeProfile.EmergencyContact;
                newEmployee.EmergencyContactPhone = newEmployeeProfile.EmergencyContactPhone;
                newEmployee.Station = newEmployeeProfile.Station;
                newEmployee.Computer = newEmployeeProfile.Computer;
                newEmployee.Phone = newEmployeeProfile.Phone;
                newEmployee.Email = newEmployeeProfile.Email;
                newEmployee.CreatedBy = employeeId;
                newEmployee.CreatedOn = DateTime.Now;



                TB_TACO_TeamMember newTeamMember = new TB_TACO_TeamMember
                {
                    TeamId = teamId,
                    EmployeeId = context.TB_TACO_Employee.Add(newEmployee).EmployeeId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Parse("12-31-9999"),
                    CreatedBy = employeeId,
                    CreatedOn = DateTime.Now
                };

                context.TB_TACO_TeamMember.Add(newTeamMember);
                context.SaveChanges();

            }
        }
        /// <summary>
        /// Updates an employee's profile.
        /// Takes an instance of the POCO EmployeeProfile.
        /// It does not return anything.
        /// Created By: Emily Urdaneta
        /// Created On: February 28, 2019
        /// Modified By: Emily Urdaneta 
        /// Modified On: March 01,2019
        /// </summary>
        /// <param name="updatedProfile"></param>
        public void UpdateEmployeeProfile(EmployeeProfile updatedProfile, int employeeId, int teamId)
        {
            using (var context = new TacoContext())
            {
                var employeeRecord = context.TB_TACO_Employee.Find(updatedProfile.EmployeeId);
                if (employeeRecord == null)
                {
                    throw new Exception("Employee does not exist");
                }
                else
                {
                    employeeRecord.EmployeeNumber = updatedProfile.EmployeeNumber;
                    employeeRecord.FirstName = updatedProfile.FirstName;
                    employeeRecord.LastName = updatedProfile.LastName;
                    employeeRecord.HireDate = updatedProfile.HireDate;
                    employeeRecord.Birthdate = updatedProfile.Birthdate;
                    employeeRecord.TerminationDate = null;
                    employeeRecord.PositionId = updatedProfile.PositionId;
                    employeeRecord.ScheduleId = updatedProfile.ScheduleType;
                    employeeRecord.EmergencyContactName = updatedProfile.EmergencyContact;
                    employeeRecord.EmergencyContactPhone = updatedProfile.EmergencyContactPhone;
                    employeeRecord.Computer = updatedProfile.Computer;
                    employeeRecord.Station = updatedProfile.Station;
                    employeeRecord.Phone = updatedProfile.Phone;
                    employeeRecord.ScheduleId = updatedProfile.ScheduleType;
                    employeeRecord.Email = updatedProfile.Email;
                    employeeRecord.ModifiedBy = employeeId;
                    employeeRecord.ModifiedOn = DateTime.Now;


                    int oldTeamId = context.TB_TACO_TeamMember.Where(x => x.EmployeeId == updatedProfile.EmployeeId).Select(y => y.TeamId).FirstOrDefault();
                    var teamExists = context.TB_TACO_TeamMember.Where(x => x.TeamId == oldTeamId && x.EmployeeId == updatedProfile.EmployeeId).Select(y => y.TeamMemberId).SingleOrDefault();
                    if (teamExists != 0)
                    {
                        int oldTeamMemberId = context.TB_TACO_TeamMember.Where(x => x.EmployeeId == updatedProfile.EmployeeId && x.TeamId == oldTeamId).Select(y => y.TeamMemberId).FirstOrDefault();
                        TB_TACO_TeamMember oldTeamMember = context.TB_TACO_TeamMember.Find(oldTeamMemberId);
                        oldTeamMember.EndDate = DateTime.Now;
                        oldTeamMember.ModifiedBy = employeeId;
                        oldTeamMember.ModifiedOn = DateTime.Now;
                        context.Entry(oldTeamMember).State = EntityState.Modified;
                            
                        TB_TACO_TeamMember newTeamMember = new TB_TACO_TeamMember
                        {
                            TeamId = teamId,
                            EmployeeId = updatedProfile.EmployeeId,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Parse("12-31-9999"),
                            CreatedBy = employeeId,
                            CreatedOn = DateTime.Now
                        };

                        context.TB_TACO_TeamMember.Add(newTeamMember);
                    }

                    context.Entry(employeeRecord).State = EntityState.Modified;
                    context.SaveChanges();


                }
            }
        }
        /// <summary>
        /// Delete's an employees profile.
        /// Created By: Emily Urdaneta
        /// Created On: February 15, 2018
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="employeeId"></param>
        public void DeleteEmployeeProfile(int employeeId)
        {
            using (var context = new TacoContext())
            {
                var employeeRecord = context.TB_TACO_Employee.Find(employeeId);
                employeeRecord.TerminationDate = DateTime.Today;

                context.Entry(employeeRecord).State = EntityState.Modified;
                context.SaveChanges();

            }

        }
        #endregion
    }
}
