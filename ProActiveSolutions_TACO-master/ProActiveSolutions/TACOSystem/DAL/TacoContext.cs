namespace TACOSystem.DAL
{
    using System;

    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;
    using TACOData.Entities;

    public partial class TacoContext : DbContext
    {
        public TacoContext()
            : base("name=TacoContext")
        {
        }

        public virtual DbSet<TB_TACO_AllocatedTimeDetail> TB_TACO_AllocatedTimeDetail { get; set; }
        public virtual DbSet<TB_TACO_AllocatedTimeLog> TB_TACO_AllocatedTimeLog { get; set; }
        public virtual DbSet<TB_TACO_Area> TB_TACO_Area { get; set; }
        public virtual DbSet<TB_TACO_Attendance> TB_TACO_Attendance { get; set; }
        public virtual DbSet<TB_TACO_AttendanceEntitlement> TB_TACO_AttendanceEntitlement { get; set; }
        public virtual DbSet<TB_TACO_Category> TB_TACO_Category { get; set; }
        public virtual DbSet<TB_TACO_Department> TB_TACO_Department { get; set; }
        public virtual DbSet<TB_TACO_Employee> TB_TACO_Employee { get; set; }
        public virtual DbSet<TB_TACO_Holiday> TB_TACO_Holiday { get; set; }
        public virtual DbSet<TB_TACO_Overtime> TB_TACO_Overtime { get; set; }
        public virtual DbSet<TB_TACO_OvertimeBalance> TB_TACO_OvertimeBalance { get; set; }
        public virtual DbSet<TB_TACO_Position> TB_TACO_Position { get; set; }
        public virtual DbSet<TB_TACO_Project> TB_TACO_Project { get; set; }
        public virtual DbSet<TB_TACO_ProjectTeam> TB_TACO_ProjectTeam { get; set; }
        public virtual DbSet<TB_TACO_Request> TB_TACO_Request { get; set; }
        public virtual DbSet<TB_TACO_Schedule> TB_TACO_Schedule { get; set; }
        public virtual DbSet<TB_TACO_SecurityRole> TB_TACO_SecurityRole { get; set; }
        public virtual DbSet<TB_TACO_Team> TB_TACO_Team { get; set; }
        public virtual DbSet<TB_TACO_TeamMember> TB_TACO_TeamMember { get; set; }
        public virtual DbSet<TB_TACO_Timesheet> TB_TACO_Timesheet { get; set; }
        public virtual DbSet<TB_TACO_TimesheetDetail> TB_TACO_TimesheetDetail { get; set; }
        public virtual DbSet<TB_TACO_Unit> TB_TACO_Unit { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TB_TACO_AllocatedTimeDetail>()
                .HasMany(e => e.TB_TACO_AllocatedTimeLog)
                .WithRequired(e => e.TB_TACO_AllocatedTimeDetail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Area>()
                .Property(e => e.AreaName)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Area>()
                .Property(e => e.AreaDescription)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Area>()
                .HasMany(e => e.TB_TACO_Unit)
                .WithRequired(e => e.TB_TACO_Area)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Attendance>()
                .Property(e => e.AttendanceCode)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Attendance>()
                .Property(e => e.AttendanceDescription)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Attendance>()
                .Property(e => e.Units)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Attendance>()
                .HasMany(e => e.TB_TACO_AttendanceEntitlement)
                .WithRequired(e => e.TB_TACO_Attendance)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_AttendanceEntitlement>()
                .Property(e => e.TotalTime)
                .HasPrecision(7, 2);

            modelBuilder.Entity<TB_TACO_Category>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Category>()
                .Property(e => e.CategoryDescription)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Category>()
                .HasMany(e => e.TB_TACO_Project)
                .WithRequired(e => e.TB_TACO_Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Department>()
                .Property(e => e.DepartmentName)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Department>()
                .Property(e => e.DepartmentDescription)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Department>()
                .HasMany(e => e.TB_TACO_Area)
                .WithRequired(e => e.TB_TACO_Department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .Property(e => e.EmployeeNumber)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .Property(e => e.EmergencyContactName)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .Property(e => e.EmergencyContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .Property(e => e.Station)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .Property(e => e.Computer)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_AllocatedTimeDetail)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_AllocatedTimeDetail1)
                .WithOptional(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_AllocatedTimeLog)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Area)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Area1)
                .WithOptional(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Attendance)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Attendance1)
                .WithOptional(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_AttendanceEntitlement)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.EmployeeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_AttendanceEntitlement1)
                .WithRequired(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_AttendanceEntitlement2)
                .WithOptional(e => e.TB_TACO_Employee2)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Category)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Category1)
                .WithOptional(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Department)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Department1)
                .WithOptional(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Holiday)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Overtime)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_OvertimeBalance)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.EmployeeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Position1)
                .WithRequired(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_ProjectTeam)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.EmployeeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Request)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.EmployeeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Schedule)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_SecurityRole)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Timesheet)
                .WithRequired(e => e.TB_TACO_Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Holiday1)
                .WithOptional(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Overtime1)
                .WithOptional(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Position2)
                .WithOptional(e => e.TB_TACO_Employee2)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Project)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Schedule2)
                .WithOptional(e => e.TB_TACO_Employee2)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_SecurityRole1)
                .WithOptional(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Team)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_TeamMember)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.EmployeeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Unit)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Employee1)
                .WithRequired(e => e.TB_TACO_Employee2)
                .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_OvertimeBalance1)
                .WithRequired(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Project1)
                .WithOptional(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_ProjectTeam1)
                .WithRequired(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Request1)
                .WithRequired(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Team1)
                .WithOptional(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_TeamMember1)
                .WithRequired(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Unit1)
                .WithOptional(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Employee11)
                .WithOptional(e => e.TB_TACO_Employee3)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_OvertimeBalance2)
                .WithOptional(e => e.TB_TACO_Employee2)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_ProjectTeam2)
                .WithOptional(e => e.TB_TACO_Employee2)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_Request2)
                .WithOptional(e => e.TB_TACO_Employee2)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_TeamMember2)
                .WithOptional(e => e.TB_TACO_Employee2)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_TimesheetDetail)
                .WithRequired(e => e.TB_TACO_Employee)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Employee>()
                .HasMany(e => e.TB_TACO_TimesheetDetail1)
                .WithOptional(e => e.TB_TACO_Employee1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<TB_TACO_Holiday>()
                .Property(e => e.HolidayName)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Overtime>()
                .Property(e => e.OvertimeCode)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Overtime>()
                .Property(e => e.OvertimeDescription)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Overtime>()
                .Property(e => e.Units)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Overtime>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Overtime>()
                .HasMany(e => e.TB_TACO_OvertimeBalance)
                .WithRequired(e => e.TB_TACO_Overtime)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Overtime>()
                .HasMany(e => e.TB_TACO_Request)
                .WithRequired(e => e.TB_TACO_Overtime)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_OvertimeBalance>()
                .Property(e => e.TotalTime)
                .HasPrecision(7, 2);

            modelBuilder.Entity<TB_TACO_Position>()
                .Property(e => e.PositionName)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Position>()
                .Property(e => e.PositionDescription)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Position>()
                .HasMany(e => e.TB_TACO_Employee)
                .WithRequired(e => e.TB_TACO_Position)
                .HasForeignKey(e => e.PositionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Project>()
                .Property(e => e.ProjectName)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Project>()
                .Property(e => e.ProjectDescription)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Project>()
                .Property(e => e.Priority)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Project>()
                .Property(e => e.ProjectColor)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Project>()
                .HasMany(e => e.TB_TACO_ProjectTeam)
                .WithRequired(e => e.TB_TACO_Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_ProjectTeam>()
                .HasMany(e => e.TB_TACO_AllocatedTimeDetail)
                .WithRequired(e => e.TB_TACO_ProjectTeam)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Request>()
                .Property(e => e.TotalTime)
                .HasPrecision(7, 2);

            modelBuilder.Entity<TB_TACO_Request>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Request>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Schedule>()
                .Property(e => e.ScheduleName)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Schedule>()
                .Property(e => e.ScheduleDescription)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Schedule>()
                .HasMany(e => e.TB_TACO_Employee1)
                .WithRequired(e => e.TB_TACO_Schedule1)
                .HasForeignKey(e => e.ScheduleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_SecurityRole>()
                .Property(e => e.SecurityRole)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_SecurityRole>()
                .Property(e => e.RoleDescription)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_SecurityRole>()
                .HasMany(e => e.TB_TACO_Employee2)
                .WithRequired(e => e.TB_TACO_SecurityRole2)
                .HasForeignKey(e => e.SecurityRoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Team>()
                .Property(e => e.TeamName)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Team>()
                .Property(e => e.TeamDescription)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Team>()
                .HasMany(e => e.TB_TACO_TeamMember)
                .WithRequired(e => e.TB_TACO_Team)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Timesheet>()
                .HasMany(e => e.TB_TACO_TimesheetDetail)
                .WithRequired(e => e.TB_TACO_Timesheet)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TB_TACO_Unit>()
                .Property(e => e.UnitName)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Unit>()
                .Property(e => e.UnitDescription)
                .IsUnicode(false);

            modelBuilder.Entity<TB_TACO_Unit>()
                .HasMany(e => e.TB_TACO_Team)
                .WithRequired(e => e.TB_TACO_Unit)
                .WillCascadeOnDelete(false);
        }
    }
}
