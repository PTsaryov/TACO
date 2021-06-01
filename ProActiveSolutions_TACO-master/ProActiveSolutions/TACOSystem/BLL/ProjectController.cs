using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TACOData.Entities;
using TACOData.Entities.POCOs;
using TACOData.Entities.POCOs.Projects;
using TACOSystem.DAL;

namespace TACOSystem.BLL
{
    [DataObject]
    public class ProjectController
    {
        /// This method CREATE Project. parameters = Project ID.
        /// Created By: Prince Selhi
        /// Created On: March 12,2019
        /// Modified By: Prince Selhi
        /// Modified On: April 05,2019
        /// </summary> = The list also sends the selected employees list that is created in pop up. It sends It in two different times. First it only sends Selected employees list when save is pressed thn when create button is pressed it saves each employee with each project Id.
        /// <returns> Create Project</returns>
        public void Createproject(ProjectInformation newProject = null, List<int> ids = null)
        {
            using (var context = new TacoContext())
            {
                // recieve list of selected employee
                if(ids != null)
                {
                    List<projectIdsToCreate> list = new List<projectIdsToCreate>();
                    
                    foreach(var item in ids)
                    {
                        projectIdsToCreate dto = new projectIdsToCreate(item);
                        dto.id = item;
                        list.Add(dto);
                    }
                    HttpContext.Current.Session.Add("employeeList", list);
                } 

                // It wont go here until the create butoon is pressed. It saves each employee with each project Id and create a project team ID.

                if (newProject.CategoryId != 0)
                {
                    if (newProject.StartDate > newProject.EndDate)
                    {
                        throw new Exception("Expiry date can not be before than start date");
                    }

                    else if (newProject.StartDate < DateTime.Today)
                    {
                        throw new Exception("Start date can not be before today.");
                    }

                    else
                    {
                        var newProjectinfo = new TB_TACO_Project();                       
                        
                        List<projectIdsToCreate> list = HttpContext.Current.Session["employeeList"] as List<projectIdsToCreate>;

                        newProjectinfo.ProjectName = newProject.ProjectName;
                        newProjectinfo.StartDate = newProject.StartDate;
                        newProjectinfo.EndDate = newProject.EndDate;

                        newProjectinfo.CategoryId = newProject.CategoryId;
                        newProjectinfo.Priority = newProject.Priority;

                        newProjectinfo.ProjectDescription = newProject.ProjectDescription;
                        newProjectinfo.CreatedBy = 1;
                        newProjectinfo.CreatedOn = DateTime.Today;
                        newProjectinfo.ProjectColor = newProject.ProjectColor;

                        context.TB_TACO_Project.Add(newProjectinfo);
                        if (list != null)
                        {
                            foreach (var employee in list)
                            {
                              var newProjectTeam = new TB_TACO_ProjectTeam();
                              newProjectTeam.EmployeeId = employee.id;
                              newProjectTeam.ProjectId = newProjectinfo.ProjectId;
                              newProjectTeam.CreatedBy = 1;
                              newProjectTeam.CreatedOn = DateTime.Today;
                              newProjectTeam.StartDate = newProjectinfo.StartDate;
                              newProjectTeam.EndDate = newProjectinfo.EndDate;
                              context.TB_TACO_ProjectTeam.Add(newProjectTeam);
                            }
                        }
                        context.SaveChanges();
                        HttpContext.Current.Session.Remove("employeeList");
                    }
                }
            }
        }

        //public void Createproject(List<int> ids)
        //{
        //    var selectedIds = ids;        
        //}

        /// This method returls a LIST of Projects. parameters = Project ID.
        /// Created By: Prince Selhi
        /// Created On: March 12,2019
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns> List Project</returns>

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ProjectInformation> ProjectLists()
        {
            using (var context = new TacoContext())
            {
                var projects = (from project in context.TB_TACO_Project
                               where project.EndDate > DateTime.Now
                               select new ProjectInformation
                               {
                                 ProjectId = project.ProjectId,
                                 ProjectName = project.ProjectName.ToString(),
                                 StartDate = project.StartDate,
                                 EndDate = project.EndDate,
                                 CategoryName = project.TB_TACO_Category.CategoryName
                                  
                               }).ToList();
                return projects;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ProjectInformation> ProjectListsExpired()
        {
            using (var context = new TacoContext())
            {
                var projects = (from project in context.TB_TACO_Project
                                where project.EndDate < DateTime.Now
                                select new ProjectInformation
                                {
                                    ProjectId = project.ProjectId,
                                    ProjectName = project.ProjectName.ToString(),
                                    StartDate = project.StartDate,
                                    EndDate = project.EndDate,
                                    CategoryName = project.TB_TACO_Category.CategoryName

                                }).ToList();
                return projects;
            }
        }


        /// This method returls a LIST of Employees. 
        /// Created By: Prince Selhi
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns> Popup List For Add employee</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ProjectInformation> EmployeeLists()
        {
            using (var context = new TacoContext())
            {
                var employees = (from employee in context.TB_TACO_TeamMember
                                 where employee.TB_TACO_Employee.TerminationDate == null
                                 orderby employee.EmployeeId ascending
                                 select new ProjectInformation
                                {
                                  EmployeeId = employee.TB_TACO_Employee.EmployeeId,
                                  FullName = employee.TB_TACO_Employee.FirstName + " " + employee.TB_TACO_Employee.LastName,
                                  Position = employee.TB_TACO_Employee.TB_TACO_Position.PositionName,
                                  TeamName = employee.TB_TACO_Team.TeamName,
                                  DepartmentName = employee.TB_TACO_Team.TB_TACO_Unit.TB_TACO_Area.TB_TACO_Department.DepartmentName
                                }).ToList();
                return employees;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ProjectInformation> NotInProjectEmployeeLists(int projectID)
        {
            using (var context = new TacoContext())
            {
                var employees = (from employee in context.TB_TACO_Employee
                                 where employee.TerminationDate == null
                                 orderby employee.EmployeeId ascending
                                 select new ProjectInformation
                                 {
                                     EmployeeId = employee.EmployeeId,
                                     FullName = employee.FirstName + " " + employee.LastName,                                   
                                 }).ToList();


                var results = (from project in context.TB_TACO_ProjectTeam
                               where project.ProjectId == projectID
                               select new ProjectInformation
                               {
                                   EmployeeId = project.EmployeeId,
                                   FullName = context.TB_TACO_Employee.Where(x => x.EmployeeId == project.EmployeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault()
                               }).ToList();
                
                //var results = context.TB_TACO_ProjectTeam.Where(x => x.ProjectId == 5).Select(y => new
                //{
                //    id = y.EmployeeId,
                //    name = context.TB_TACO_Employee.Where(x => x.EmployeeId == y.EmployeeId).Select(name => name.FirstName).SingleOrDefault()
                //}).Distinct();

                var result2 = employees.Where(p => !results.Any(p2 => p2.EmployeeId == p.EmployeeId)).ToList();

                return result2;
            }
        }


        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ProjectInformation> ExistingEmployeeLists(int ProjectId)
        {
            using (var context = new TacoContext())
            {
              var ExistingEmployee = (from project in context.TB_TACO_ProjectTeam
                              where project.ProjectId == ProjectId && project.EndDate > DateTime.Now
                              select new ProjectInformation
                              {
                                  ProjectId = project.ProjectId,
                                  ProjectName = project.TB_TACO_Project.ProjectName,
                                  ProjectTeamId = project.ProjectTeamId,
                                  EmployeeId = project.EmployeeId,
                                  FullName = context.TB_TACO_Employee.Where(x => x.EmployeeId == project.EmployeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault()
                              }).ToList();
                return ExistingEmployee;
            }
        }


        /// This method returns details about the  Projects. parameters = Project ID.
        /// Created By: Prince Selhi
        /// Created On: 
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns> Project Details</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public ProjectInformation GetProjectInformation(int ProjectId)
        {
            using (var context = new TacoContext())
            {
                var results = from project in context.TB_TACO_Project
                              where project.ProjectId == ProjectId
                              select new ProjectInformation
                              {
                                ProjectId = project.ProjectId,
                                ProjectName = project.ProjectName,
                                ProjectDescription = project.ProjectDescription,
                                StartDate = project.StartDate,
                                EndDate = project.EndDate,
                                CategoryId = project.CategoryId,
                                Priority = project.Priority,
                                Color = project.ProjectColor
                              };
                return results.Single();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ProjectTeamInformation> assignedEmployeesList(int ProjectId,int year)
        {
            using (var context = new TacoContext())
            {
                var results = from project in context.TB_TACO_ProjectTeam
                              where project.ProjectId == ProjectId && project.EndDate >DateTime.Now
                              select new ProjectTeamInformation
                              {
                                ProjectId = project.ProjectId,
                                ProjectName = project.TB_TACO_Project.ProjectName,
                                EmployeeId = project.EmployeeId,                                                                
                                EmployeeName = context.TB_TACO_Employee.Where(x => x.EmployeeId == project.EmployeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),
                                ProjectTeamId = project.ProjectTeamId,

                                January = project.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == project.ProjectTeamId).Where(x => x.AllocatedYear == year).Where(x => x.AllocatedMonth == 1).Select(x => x.AllocatedDays).FirstOrDefault(),

                                February = project.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == project.ProjectTeamId).Where(x => x.AllocatedYear == year).Where(x => x.AllocatedMonth == 2).Select(x => x.AllocatedDays).FirstOrDefault(),

                                March = project.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == project.ProjectTeamId).Where(x => x.AllocatedYear == year).Where(x => x.AllocatedMonth == 3).Select(x => x.AllocatedDays).FirstOrDefault(),

                                April = project.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == project.ProjectTeamId).Where(x => x.AllocatedYear == year).Where(x => x.AllocatedMonth == 4).Select(x => x.AllocatedDays).FirstOrDefault(),

                                May = project.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == project.ProjectTeamId).Where(x => x.AllocatedYear == year).Where(x => x.AllocatedMonth == 5).Select(x => x.AllocatedDays).FirstOrDefault(),

                                June = project.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == project.ProjectTeamId).Where(x => x.AllocatedYear == year).Where(x => x.AllocatedMonth == 6).Select(x => x.AllocatedDays).FirstOrDefault(),

                                July = project.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == project.ProjectTeamId).Where(x => x.AllocatedYear == year).Where(x => x.AllocatedMonth == 7).Select(x => x.AllocatedDays).FirstOrDefault(),

                                August = project.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == project.ProjectTeamId).Where(x => x.AllocatedYear == year).Where(x => x.AllocatedMonth == 8).Select(x => x.AllocatedDays).FirstOrDefault(),

                                September = project.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == project.ProjectTeamId).Where(x => x.AllocatedYear == year).Where(x => x.AllocatedMonth == 9).Select(x => x.AllocatedDays).FirstOrDefault(),

                                October = project.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == project.ProjectTeamId).Where(x => x.AllocatedYear == year).Where(x => x.AllocatedMonth == 10).Select(x => x.AllocatedDays).FirstOrDefault(),

                                November = project.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == project.ProjectTeamId).Where(x => x.AllocatedYear == year).Where(x => x.AllocatedMonth == 11).Select(x => x.AllocatedDays).FirstOrDefault(),

                                December = project.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == project.ProjectTeamId).Where(x => x.AllocatedYear == year).Where(x => x.AllocatedMonth == 12).Select(x => x.AllocatedDays).FirstOrDefault()
                              };

                return results.ToList();
            }
        }

        public void UpdateAssignedEmployeeList(List<ProjectTeamInformation> data)
        {
            using (var context = new TacoContext())
            {
                
                foreach (var item in data)
                {
                    var Employees = context.TB_TACO_ProjectTeam.Where(x => x.ProjectTeamId == item.ProjectTeamId).FirstOrDefault();

                    Employees.EndDate = DateTime.Today;
                    Employees.ModifiedBy = item.ModifiedBy;
                    Employees.ModifiedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }


        public void SaveAllocation(List <ProjectTeamInformation> data)
        {
            using (var context = new TacoContext())
            {
                foreach (var item in data)
                {
                    var model = context.TB_TACO_AllocatedTimeDetail.Where(x => x.ProjectTeamId == item.ProjectTeamId).Where(x => x.AllocatedYear == item.AllocatedYear).ToList();
                    if (model == null || model.Count == 0)
                    {
                        
                        var createAllocatedTimeLog = new TB_TACO_AllocatedTimeLog();

                        var createAllocation1 = new TB_TACO_AllocatedTimeDetail();
                        createAllocation1.ProjectTeamId = item.ProjectTeamId;
                        createAllocation1.AllocatedDays = item.January;
                        createAllocation1.AllocatedMonth = 1;
                        createAllocation1.CreatedBy = item.CreatedBy;
                        createAllocation1.CreatedOn = DateTime.Now;
                        createAllocation1.AllocatedYear = item.AllocatedYear;
                        context.TB_TACO_AllocatedTimeDetail.Add(createAllocation1);
                        context.SaveChanges();
                        
                        createAllocatedTimeLog.AllocatedTimeId = createAllocation1.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = createAllocation1.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = createAllocation1.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();


                        var createAllocation2 = new TB_TACO_AllocatedTimeDetail();
                        createAllocation2.ProjectTeamId = item.ProjectTeamId;
                        createAllocation2.AllocatedDays = item.February;
                        createAllocation2.AllocatedMonth = 2;
                        createAllocation2.CreatedBy = item.CreatedBy;
                        createAllocation2.AllocatedYear = item.AllocatedYear;
                        createAllocation2.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeDetail.Add(createAllocation2);
                        context.SaveChanges();

                        createAllocatedTimeLog.AllocatedTimeId = createAllocation2.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = createAllocation2.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = createAllocation2.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();


                        var createAllocation3 = new TB_TACO_AllocatedTimeDetail();
                        createAllocation3.ProjectTeamId = item.ProjectTeamId;
                        createAllocation3.AllocatedDays = item.March;
                        createAllocation3.AllocatedMonth = 3;
                        createAllocation3.CreatedBy = item.CreatedBy;
                        createAllocation3.AllocatedYear = item.AllocatedYear;
                        createAllocation3.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeDetail.Add(createAllocation3);
                        context.SaveChanges();

                        createAllocatedTimeLog.AllocatedTimeId = createAllocation3.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = createAllocation3.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = createAllocation3.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();


                        var createAllocation4= new TB_TACO_AllocatedTimeDetail();
                        createAllocation4.ProjectTeamId = item.ProjectTeamId;
                        createAllocation4.AllocatedDays = item.April;
                        createAllocation4.AllocatedMonth = 4;
                        createAllocation4.CreatedBy = item.CreatedBy;
                        createAllocation4.AllocatedYear = item.AllocatedYear;
                        createAllocation4.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeDetail.Add(createAllocation4);
                        context.SaveChanges();
                        
                        createAllocatedTimeLog.AllocatedTimeId = createAllocation4.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = createAllocation4.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = createAllocation4.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();

                        var createAllocation5 = new TB_TACO_AllocatedTimeDetail();
                        createAllocation5.ProjectTeamId = item.ProjectTeamId;
                        createAllocation5.AllocatedDays = item.May;
                        createAllocation5.AllocatedMonth = 5;
                        createAllocation5.CreatedBy = item.CreatedBy;
                        createAllocation5.AllocatedYear = item.AllocatedYear;
                        createAllocation5.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeDetail.Add(createAllocation5);
                        context.SaveChanges();
                                                
                        createAllocatedTimeLog.AllocatedTimeId = createAllocation5.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = createAllocation5.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = createAllocation5.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();


                        var createAllocation6 = new TB_TACO_AllocatedTimeDetail();
                        createAllocation6.ProjectTeamId = item.ProjectTeamId;
                        createAllocation6.AllocatedDays = item.June;
                        createAllocation6.AllocatedMonth = 6;
                        createAllocation6.CreatedBy = item.CreatedBy;
                        createAllocation6.AllocatedYear = item.AllocatedYear;
                        createAllocation6.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeDetail.Add(createAllocation6);
                        context.SaveChanges();

                        createAllocatedTimeLog.AllocatedTimeId = createAllocation6.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = createAllocation6.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = createAllocation6.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();


                        var createAllocation7 = new TB_TACO_AllocatedTimeDetail();

                        createAllocation7.ProjectTeamId = item.ProjectTeamId;
                        createAllocation7.AllocatedDays = item.July;
                        createAllocation7.AllocatedMonth = 7;
                        createAllocation7.CreatedBy = item.CreatedBy;
                        createAllocation7.AllocatedYear = item.AllocatedYear;
                        createAllocation7.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeDetail.Add(createAllocation7);
                        context.SaveChanges();

                        createAllocatedTimeLog.AllocatedTimeId = createAllocation7.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = createAllocation7.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = createAllocation7.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();


                        var createAllocation8 = new TB_TACO_AllocatedTimeDetail();
                        createAllocation8.ProjectTeamId = item.ProjectTeamId;
                        createAllocation8.AllocatedDays = item.August;
                        createAllocation8.AllocatedMonth = 8;
                        createAllocation8.CreatedBy = item.CreatedBy;
                        createAllocation8.AllocatedYear = item.AllocatedYear;
                        createAllocation8.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeDetail.Add(createAllocation8);
                        context.SaveChanges();

                        createAllocatedTimeLog.AllocatedTimeId = createAllocation8.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = createAllocation8.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = createAllocation8.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();


                        var createAllocation9 = new TB_TACO_AllocatedTimeDetail();
                        createAllocation9.ProjectTeamId = item.ProjectTeamId;
                        createAllocation9.AllocatedDays = item.September;
                        createAllocation9.AllocatedMonth = 9;
                        createAllocation9.CreatedBy = item.CreatedBy;
                        createAllocation9.AllocatedYear = item.AllocatedYear;
                        createAllocation9.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeDetail.Add(createAllocation9);
                        context.SaveChanges();

                        createAllocatedTimeLog.AllocatedTimeId = createAllocation9.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = createAllocation9.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = createAllocation9.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();


                        var createAllocation10 = new TB_TACO_AllocatedTimeDetail();

                        createAllocation10.ProjectTeamId = item.ProjectTeamId;
                        createAllocation10.AllocatedDays = item.October;
                        createAllocation10.AllocatedMonth = 10;
                        createAllocation10.CreatedBy = item.CreatedBy;
                        createAllocation10.AllocatedYear = item.AllocatedYear;
                        createAllocation10.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeDetail.Add(createAllocation10);
                        context.SaveChanges();

                        createAllocatedTimeLog.AllocatedTimeId = createAllocation10.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = createAllocation10.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = createAllocation10.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();


                        var createAllocation11 = new TB_TACO_AllocatedTimeDetail();

                        createAllocation11.ProjectTeamId = item.ProjectTeamId;
                        createAllocation11.AllocatedDays = item.November;
                        createAllocation11.AllocatedMonth = 11;
                        createAllocation11.CreatedBy = item.CreatedBy;
                        createAllocation11.AllocatedYear = item.AllocatedYear;
                        createAllocation11.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeDetail.Add(createAllocation11);
                        context.SaveChanges();

                        createAllocatedTimeLog.AllocatedTimeId = createAllocation11.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = createAllocation11.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = createAllocation11.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();


                        var createAllocation12 = new TB_TACO_AllocatedTimeDetail();

                        createAllocation12.ProjectTeamId = item.ProjectTeamId;
                        createAllocation12.AllocatedDays = item.December;
                        createAllocation12.AllocatedMonth = 12;
                        createAllocation12.CreatedBy = item.CreatedBy;
                        createAllocation12.AllocatedYear = item.AllocatedYear;
                        createAllocation12.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeDetail.Add(createAllocation12);
                        context.SaveChanges();

                        createAllocatedTimeLog.AllocatedTimeId = createAllocation12.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = createAllocation12.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = createAllocation12.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();


                    }
                    else if ( model.Count != 0)
                    {
                        var createAllocatedTimeLog = new TB_TACO_AllocatedTimeLog();
                        var jan = model.Where(x => x.AllocatedMonth == 1).FirstOrDefault();
                        jan.AllocatedDays = item.January;
                        jan.ModifiedBy = item.CreatedBy;
                        jan.ModifiedOn = DateTime.Now;
                        context.Entry(jan).State = EntityState.Modified;
                        context.SaveChanges();
                        createAllocatedTimeLog.AllocatedTimeId = jan.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = jan.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = item.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();

                        var feb = model.Where(x => x.AllocatedMonth == 2).FirstOrDefault();
                        feb.AllocatedDays = item.February;
                        feb.ModifiedBy = item.CreatedBy;
                        feb.ModifiedOn = DateTime.Now;
                        context.SaveChanges();
                        var createAllocatedTimeLog1 = new TB_TACO_AllocatedTimeLog();
                        createAllocatedTimeLog1.AllocatedTimeId = feb.AllocatedTimeId;
                        createAllocatedTimeLog1.AllocatedTimeLogged = feb.AllocatedDays;
                        createAllocatedTimeLog1.CreatedBy = item.CreatedBy;
                        createAllocatedTimeLog1.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog1);
                        context.SaveChanges();

                        var mar = model.Where(x => x.AllocatedMonth == 3).FirstOrDefault();
                        mar.AllocatedDays = item.March;
                        mar.ModifiedBy = item.CreatedBy;
                        mar.ModifiedOn = DateTime.Now;
                        context.SaveChanges();
                        createAllocatedTimeLog.AllocatedTimeId = mar.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = mar.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = item.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();

                        var apr = model.Where(x => x.AllocatedMonth == 4).FirstOrDefault();
                        apr.AllocatedDays = item.April;
                        apr.ModifiedBy = item.CreatedBy;
                        apr.ModifiedOn = DateTime.Now;
                        context.SaveChanges();
                        createAllocatedTimeLog.AllocatedTimeId = apr.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = apr.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = item.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();

                        var may = model.Where(x => x.AllocatedMonth == 5).FirstOrDefault();
                        may.AllocatedDays = item.May;
                        may.ModifiedBy = item.CreatedBy;
                        may.ModifiedOn = DateTime.Now;
                        context.SaveChanges();
                        createAllocatedTimeLog.AllocatedTimeId = may.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = may.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = item.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();

                        var jun = model.Where(x => x.AllocatedMonth == 6).FirstOrDefault();
                        jun.AllocatedDays = item.June;
                        jun.ModifiedBy = item.CreatedBy;
                        jun.ModifiedOn = DateTime.Now;
                        context.SaveChanges();
                        createAllocatedTimeLog.AllocatedTimeId = jun.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = jun.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = item.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();

                        var jul = model.Where(x => x.AllocatedMonth == 7).FirstOrDefault();
                        jul.AllocatedDays = item.July;
                        jul.ModifiedBy = item.CreatedBy;
                        jul.ModifiedOn = DateTime.Now;
                        context.SaveChanges();
                        createAllocatedTimeLog.AllocatedTimeId = jul.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = jul.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = item.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();

                        var aug = model.Where(x => x.AllocatedMonth == 8).FirstOrDefault();
                        aug.AllocatedDays = item.August;
                        aug.ModifiedBy = item.CreatedBy;
                        aug.ModifiedOn = DateTime.Now;
                        context.SaveChanges();
                        createAllocatedTimeLog.AllocatedTimeId = aug.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = aug.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = item.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();

                        var sept = model.Where(x => x.AllocatedMonth == 9).FirstOrDefault();
                        sept.AllocatedDays = item.September;
                        sept.ModifiedBy = item.CreatedBy;
                        sept.ModifiedOn = DateTime.Now;
                        context.SaveChanges();
                        createAllocatedTimeLog.AllocatedTimeId = sept.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = sept.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = item.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();

                        var oct = model.Where(x => x.AllocatedMonth == 10).FirstOrDefault();
                        oct.AllocatedDays = item.October;
                        oct.ModifiedBy = item.CreatedBy;
                        oct.ModifiedOn = DateTime.Now;
                        context.SaveChanges();
                        createAllocatedTimeLog.AllocatedTimeId = oct.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = oct.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = item.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();

                        var nov = model.Where(x => x.AllocatedMonth == 11).FirstOrDefault();
                        nov.AllocatedDays = item.November;
                        nov.ModifiedBy = item.CreatedBy;
                        nov.ModifiedOn = DateTime.Now;
                        context.SaveChanges();
                        createAllocatedTimeLog.AllocatedTimeId = nov.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = nov.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = item.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();

                        var dec = model.Where(x => x.AllocatedMonth == 12).FirstOrDefault();
                        dec.AllocatedDays = item.December;
                        dec.ModifiedBy = item.CreatedBy;
                        dec.ModifiedOn = DateTime.Now;
                        context.SaveChanges();
                        createAllocatedTimeLog.AllocatedTimeId = dec.AllocatedTimeId;
                        createAllocatedTimeLog.AllocatedTimeLogged = dec.AllocatedDays;
                        createAllocatedTimeLog.CreatedBy = item.CreatedBy;
                        createAllocatedTimeLog.CreatedOn = DateTime.Now;
                        context.TB_TACO_AllocatedTimeLog.Add(createAllocatedTimeLog);
                        context.SaveChanges();
                    }
                }
            }
        }



        /// <summary>
        /// This method updates project DETAILS. parameters = Project ID.
        /// Created By: Prince Selhi
        /// Created On: April 04,2019
        /// Modified By: Prince Selhi
        /// Modified On: April 05,2019
        /// </summary> This is an update method for 
        /// <returns> Updated Project details</returns>

        public void UpdateProject(ProjectInformation updatedProjectinfo = null, List<int> ids = null)
        {
            using (var context = new TacoContext())
            {

                if (ids != null)
                {
                    List<projectIdsToCreate> list = new List<projectIdsToCreate>();

                    foreach (var item in ids)
                    {
                        projectIdsToCreate dto = new projectIdsToCreate(item);
                        dto.id = item;
                        list.Add(dto);
                    }
                    HttpContext.Current.Session.Add("employeeList", list);
                }
                
                if (updatedProjectinfo.ProjectName != null && updatedProjectinfo.ProjectName != "")
                {
                   var projectInfo = context.TB_TACO_Project.Find(updatedProjectinfo.ProjectId);
                    
                    if (projectInfo == null)
                    {
                        throw new Exception("Department does not exist.");
                    }

                    else if (updatedProjectinfo.EndDate < DateTime.Today)
                    {
                        throw new Exception("Expiry date can not be before today.");
                    }

                    else if (updatedProjectinfo.StartDate > updatedProjectinfo.EndDate)
                    {
                        throw new Exception("Expiry date can not be before than start date.");
                    }

                    else
                    {
                        List<projectIdsToCreate> list = HttpContext.Current.Session["employeeList"] as List<projectIdsToCreate>;

                        projectInfo.ProjectId = updatedProjectinfo.ProjectId;
                        projectInfo.ProjectName = updatedProjectinfo.ProjectName;
                        projectInfo.ProjectDescription = updatedProjectinfo.ProjectDescription;
                        projectInfo.StartDate = updatedProjectinfo.StartDate;
                        projectInfo.EndDate = updatedProjectinfo.EndDate;
                        projectInfo.Priority = updatedProjectinfo.Priority;
                        //projectInfo.CategoryId = updatedProjectinfo.CategoryId;
                        projectInfo.ProjectColor = updatedProjectinfo.ProjectColor;

                        context.Entry(projectInfo).State = EntityState.Modified;

                        if (list != null)
                        {
                            foreach (var employee in list)
                            {
                              var newProjectTeam = new TB_TACO_ProjectTeam();
                              newProjectTeam.EmployeeId = employee.id;
                              newProjectTeam.ProjectId = updatedProjectinfo.ProjectId;
                              newProjectTeam.CreatedBy = 1;
                              newProjectTeam.CreatedOn = DateTime.Today;
                              newProjectTeam.StartDate = updatedProjectinfo.StartDate;
                              newProjectTeam.EndDate = updatedProjectinfo.EndDate;
                              context.TB_TACO_ProjectTeam.Add(newProjectTeam);
                            }
                        }

                    context.SaveChanges();
                    HttpContext.Current.Session.Remove("employeeList");
                }
                }
            }
        }


        public void TerminateProject(ProjectInformation DeletedProjectInfo)
        {
            using (var context = new TacoContext())
            {
                var delProject = context.TB_TACO_Project.Find(DeletedProjectInfo.ProjectId);

                if (delProject == null)
                {
                    throw new Exception("Project does not exist");
                }

                else
                {
                    delProject.ProjectId = DeletedProjectInfo.ProjectId;
                    delProject.EndDate = DateTime.Today;
                    delProject.ModifiedBy = DeletedProjectInfo.ModifiedBy;
                    delProject.ModifiedOn = DateTime.Now;

                    context.Entry(delProject).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }


        public void ActivateProject(ProjectInformation ActivatedProjectInfo)
        {
            using (var context = new TacoContext())
            {
                var activateProject = context.TB_TACO_Project.Find(ActivatedProjectInfo.ProjectId);

                if (activateProject == null)
                {
                    throw new Exception("Project does not exist");
                }

                else if (ActivatedProjectInfo.EndDate < DateTime.Now)
                {
                    throw new Exception("Please select a End date after today to activate project.");
                }

                else
                {
                    activateProject.ProjectId = ActivatedProjectInfo.ProjectId;
                    activateProject.EndDate = ActivatedProjectInfo.EndDate;
                    activateProject.ModifiedBy = ActivatedProjectInfo.ModifiedBy;
                    activateProject.ModifiedOn = DateTime.Now;

                    context.Entry(activateProject).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }
    }
}
