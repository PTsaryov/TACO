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
    #region Query
    [DataObject]
    public class RequestController
    {
        /// <summary>
        /// This method returns a list of Requests for the current employee.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Aaron Carlson
        /// Created On: March 10,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns>List of requests by employee</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RequestInformation> RequestList(int employeeId)
        {
            using (var context = new TacoContext())
            {
                var result = from request in context.TB_TACO_Request
                             where request.EmployeeId == employeeId
                             orderby request.Date descending
                             select new RequestInformation
                             {
                                 RequestId = request.RequestId,
                                 EmployeeId = request.EmployeeId,
                                 OvertimeId = request.OvertimeId,
                                 OvertimeDescription = request.TB_TACO_Overtime.OvertimeDescription,
                                 Date = request.Date,
                                 TotalTime = request.TotalTime,
                                 Status = request.Status,
                                 Comment = request.Comment
                             };
                return result.ToList();
            }
        }
        /// <summary>
        /// This method returns a list of Requests for all employees.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Aaron Carlson
        /// Created On: March 19,2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 9, 2019
        /// </summary>
        /// <returns>List of pending overtime requests</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RequestInformation> ApproveRequestList()
        {
            using (var context = new TacoContext())
            {
                var result = from request in context.TB_TACO_Request
                             where request.Status == "Pending"
                             orderby request.EmployeeId descending
                             select new RequestInformation
                             {
                                 RequestId = request.RequestId,
                                 EmployeeId = request.EmployeeId,
                                 FullName = request.TB_TACO_Employee.FirstName + " " + request.TB_TACO_Employee.LastName,
                                 FirstName = request.TB_TACO_Employee.FirstName,
                                 LastName = request.TB_TACO_Employee.LastName,
                                 OvertimeId = request.OvertimeId,
                                 OvertimeDescription = request.TB_TACO_Overtime.OvertimeDescription,
                                 Date = request.Date,
                                 TotalTime = request.TotalTime,
                                 Status = request.Status,
                                 Comment = request.Comment
                             };
                return result.ToList();
            }
        }
        /// <summary>
        /// This method returns a list of Requests for all employees.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Aaron Carlson
        /// Created On: April 9, 2019
        /// Modified By: 
        /// Modified On: 
        /// </summary>
        /// <returns>List of pending overtime requests</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RequestInformation> ApproveRequestList(int employeeId)
        {
            using (var context = new TacoContext())
            {
                var teamId = context.TB_TACO_TeamMember.Where(x => x.EmployeeId == employeeId && x.EndDate > DateTime.Now).Select(y => y.TeamId).SingleOrDefault();

                var result = from request in context.TB_TACO_Request
                             join team in context.TB_TACO_TeamMember
                             on request.EmployeeId equals team.EmployeeId
                             where request.Status == "Pending" && team.TeamId == teamId
                             orderby request.EmployeeId descending
                             select new RequestInformation
                             {
                                 RequestId = request.RequestId,
                                 EmployeeId = request.EmployeeId,
                                 FullName = request.TB_TACO_Employee.FirstName + " " + request.TB_TACO_Employee.LastName,
                                 FirstName = request.TB_TACO_Employee.FirstName,
                                 LastName = request.TB_TACO_Employee.LastName,
                                 OvertimeId = request.OvertimeId,
                                 OvertimeDescription = request.TB_TACO_Overtime.OvertimeDescription,
                                 Date = request.Date,
                                 TotalTime = request.TotalTime,
                                 Status = request.Status,
                                 Comment = request.Comment
                             };
                return result.ToList();
            }
        }
        #endregion

        #region Processing

        /// <summary>
        /// <para>
        /// This method updates the status and comment fields of a Request.</para>
        /// Created By: Aaron Carlson
        /// Created On: April 9, 2019
        /// Modified By: Aaron Carlson
        /// Modified On: April 12, 2019
        /// </summary>
        /// <param name="updatedRequest"></param>
        /// <param name="user"></param>
        public void RequestApproval(RequestInformation updatedRequest, int user)
        {
            using (var context = new TacoContext())
            {
                var requestApproval = context.TB_TACO_Request.Find(updatedRequest.RequestId);
                if (requestApproval == null)
                {
                    throw new Exception("Request does not exist");
                }
                else
                {
                    requestApproval.Status = updatedRequest.Status;
                    requestApproval.Comment = updatedRequest.Comment;
                    requestApproval.ModifiedBy = user;
                    requestApproval.ModifiedOn = DateTime.Now;
                    var result = context.TB_TACO_OvertimeBalance.Where(x => x.EmployeeId == requestApproval.EmployeeId).Select(z => z);
                    var specificOvertime = result.Where(x => x.OvertimeId == requestApproval.OvertimeId).Select(z => z).SingleOrDefault();
                    var time = context.TB_TACO_Request.Find(requestApproval.RequestId).TotalTime;
                    if (specificOvertime != null)
                    {
                        specificOvertime.TotalTime += time;
                        specificOvertime.ModifiedBy = user;
                        specificOvertime.ModifiedOn = DateTime.Now;
                        context.Entry(specificOvertime).State = EntityState.Modified;
                    }
                    else
                    {
                        TB_TACO_OvertimeBalance newOvertimeBalance = new TB_TACO_OvertimeBalance()
                        {
                            EmployeeId = updatedRequest.EmployeeId,
                            OvertimeId = updatedRequest.OvertimeId,
                            TotalTime = time,
                            CreatedBy = user,
                            CreatedOn = DateTime.Now
                        };
                        context.TB_TACO_OvertimeBalance.Add(newOvertimeBalance);
                    }                 



                    
                    context.Entry(requestApproval).State = EntityState.Modified;
                    
                }
                context.SaveChanges();
            }
        }
        #endregion
    }
}
