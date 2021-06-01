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
    public class DepartmentController
    {
        /// <summary>
        /// This method returns a list of Departments. It does not take any parameters.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Prince Selhi
        /// Created On: February 28,2019
        /// Modified By: Prince Selhi
        /// Modified On: march 22, 2019
        /// </summary> It shows list of active departments
        /// <returns> list of department </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<DepartmentInformation> DepartmentList()
        {
            using (var context = new TacoContext())
            {
                var result = from department in context.TB_TACO_Department
                             where department.ExpiryDate > DateTime.Now
                             select new DepartmentInformation
                             {
                                 DepartmentId = department.DepartmentId,
                                 DepartmentName = department.DepartmentName
                             };
                return result.ToList();
            }
        }

        /// <summary>
        /// This method returns a list of Departments. It does not take any parameters.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Prince Selhi
        /// Created On: April 04, 2019
        /// Modified By: Prince Selhi
        /// Modified On: 
        /// </summary> It shows list of terminated departments
        /// <returns> list of department </returns>

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DepartmentInformation> DepartmentListExpired()
        {
            using (var context = new TacoContext())
            {
                var result = from department in context.TB_TACO_Department
                             where department.ExpiryDate <= DateTime.Today
                             select new DepartmentInformation
                             {
                                 DepartmentId = department.DepartmentId,
                                 DepartmentName = department.DepartmentName
                             };
                return result.ToList();
            }
        }

        /// <summary>
        /// This method returns a list of Departments DETAILS. parameters = DEPARTMENT ID.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Prince Selhi
        /// Created On: February 28,2019
        /// Modified By: Prince Selhi
        /// Modified On:march 22, 2019
        /// </summary>
        /// <returns> Department details</returns>

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DepartmentInformation GetDepartmentInformation(int DepartmentId)
        {
            using (var context = new TacoContext())
            {
                var results = from department in context.TB_TACO_Department
                              where department.DepartmentId == DepartmentId
                              select new DepartmentInformation
                              {
                                  DepartmentId = department.DepartmentId,
                                  DepartmentName = department.DepartmentName,
                                  DepartmentDescription = department.DepartmentDescription,
                                  StartDate = department.StartDate,
                                  ExpiryDate = department.ExpiryDate
                              };
                return results.Single();
            }
        }

        /// <summary>
        /// This method updates Departments DETAILS. parameters = DEPARTMENT ID.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Prince Selhi
        /// Created On: February 28,2019
        /// Modified By: Prince Selhi
        /// Modified On:march 22, 2019
        /// </summary>
        /// <returns> Updated Department details</returns>

        public void UpdateDepartment(DepartmentInformation updatedDepartmentinfo)
        {
            using (var context = new TacoContext())
            {
                var departmentInfo = context.TB_TACO_Department.Find(updatedDepartmentinfo.DepartmentId);

                if (departmentInfo == null)
                {
                    throw new Exception("Department does not exist.");
                }               

                else if (updatedDepartmentinfo.ExpiryDate < DateTime.Today)
                {
                    throw new Exception("Expiry date can not be before today.");
                }

                else if (updatedDepartmentinfo.StartDate > updatedDepartmentinfo.ExpiryDate)
                {
                    throw new Exception("Expiry date can not be before than start date.");
                }

                else
                {
                    departmentInfo.DepartmentId = updatedDepartmentinfo.DepartmentId;
                    departmentInfo.DepartmentName = updatedDepartmentinfo.DepartmentName;
                    departmentInfo.DepartmentDescription = updatedDepartmentinfo.DepartmentDescription;
                    departmentInfo.StartDate = updatedDepartmentinfo.StartDate;
                    departmentInfo.ExpiryDate = updatedDepartmentinfo.ExpiryDate;
                    departmentInfo.ModifiedBy = updatedDepartmentinfo.ModifiedBy;
                    departmentInfo.ModifiedOn = DateTime.Today;

                    context.Entry(departmentInfo).State = EntityState.Modified;
                    context.SaveChanges();
                }               
            }           
        }

        /// <summary>
        /// This method CREATE Department. parameters = DEPARTMENT ID.
        /// Created By: Prince Selhi
        /// Created On: February 29,2019
        /// Modified By: Prince Selhi
        /// Modified On:march 22, 2019
        /// </summary>
        /// <returns> Create Department</returns>
        /// 
        public void CreateDepartment (DepartmentInformation newDepartment)
        {
            using (var context = new TacoContext())
            {
                var newDepartmentinfo = new TB_TACO_Department();

                if (newDepartment.StartDate > newDepartment.ExpiryDate)
                {
                    throw new Exception("Expiry date can not be before than start date");
                }

                else if (newDepartment.StartDate < DateTime.Today)
                {
                    throw new Exception("Start date can not be before today.");
                }

                else
                {
                    newDepartmentinfo.DepartmentName = newDepartment.DepartmentName;
                    newDepartmentinfo.DepartmentDescription = newDepartment.DepartmentDescription;
                    newDepartmentinfo.StartDate = newDepartment.StartDate;
                    newDepartmentinfo.ExpiryDate = newDepartment.ExpiryDate;
                    newDepartmentinfo.CreatedBy = newDepartment.CreatedBy;
                    newDepartmentinfo.CreatedOn = DateTime.Today;

                    context.TB_TACO_Department.Add(newDepartmentinfo);
                    context.SaveChanges();
                }                
            }
        }


        public void DeleteDepartment (DepartmentInformation deletedepartmentinfo)
        {
            using (var context = new TacoContext())
            {
                var delDepartment = context.TB_TACO_Department.Find(deletedepartmentinfo.DepartmentId);

                if (delDepartment == null)
                {
                    throw new Exception("Department does not exist");
                }
                                
                else
                {
                    delDepartment.DepartmentId = deletedepartmentinfo.DepartmentId;
                    delDepartment.ExpiryDate = DateTime.Today;
                    delDepartment.ModifiedBy = deletedepartmentinfo.ModifiedBy;
                    delDepartment.ModifiedOn = DateTime.Today;                    

                    context.Entry(delDepartment).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }



        public void ActivateDepartment(DepartmentInformation activateddepartmentinfo)
        {
            using (var context = new TacoContext())
            {
                var actDepartment = context.TB_TACO_Department.Find(activateddepartmentinfo.DepartmentId);

                if (actDepartment == null)
                {
                    throw new Exception("Department does not exist");
                }

                else
                {
                    actDepartment.DepartmentId = activateddepartmentinfo.DepartmentId;
                    actDepartment.ExpiryDate = DateTime.Parse("December 31, 9999");
                    actDepartment.ModifiedBy = activateddepartmentinfo.ModifiedBy;
                    actDepartment.ModifiedOn = DateTime.Today;
                    
                    context.Entry(actDepartment).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }


    }
}
