namespace TACOData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_TACO_Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_TACO_Employee()
        {
            TB_TACO_AllocatedTimeDetail = new HashSet<TB_TACO_AllocatedTimeDetail>();
            TB_TACO_AllocatedTimeDetail1 = new HashSet<TB_TACO_AllocatedTimeDetail>();
            TB_TACO_AllocatedTimeLog = new HashSet<TB_TACO_AllocatedTimeLog>();
            TB_TACO_Area = new HashSet<TB_TACO_Area>();
            TB_TACO_Area1 = new HashSet<TB_TACO_Area>();
            TB_TACO_Attendance = new HashSet<TB_TACO_Attendance>();
            TB_TACO_Attendance1 = new HashSet<TB_TACO_Attendance>();
            TB_TACO_AttendanceEntitlement = new HashSet<TB_TACO_AttendanceEntitlement>();
            TB_TACO_AttendanceEntitlement1 = new HashSet<TB_TACO_AttendanceEntitlement>();
            TB_TACO_AttendanceEntitlement2 = new HashSet<TB_TACO_AttendanceEntitlement>();
            TB_TACO_Category = new HashSet<TB_TACO_Category>();
            TB_TACO_Category1 = new HashSet<TB_TACO_Category>();
            TB_TACO_Department = new HashSet<TB_TACO_Department>();
            TB_TACO_Department1 = new HashSet<TB_TACO_Department>();
            TB_TACO_Holiday = new HashSet<TB_TACO_Holiday>();
            TB_TACO_Overtime = new HashSet<TB_TACO_Overtime>();
            TB_TACO_OvertimeBalance = new HashSet<TB_TACO_OvertimeBalance>();
            TB_TACO_Position1 = new HashSet<TB_TACO_Position>();
            TB_TACO_ProjectTeam = new HashSet<TB_TACO_ProjectTeam>();
            TB_TACO_Request = new HashSet<TB_TACO_Request>();
            TB_TACO_Schedule = new HashSet<TB_TACO_Schedule>();
            TB_TACO_SecurityRole = new HashSet<TB_TACO_SecurityRole>();
            TB_TACO_Timesheet = new HashSet<TB_TACO_Timesheet>();
            TB_TACO_Holiday1 = new HashSet<TB_TACO_Holiday>();
            TB_TACO_Overtime1 = new HashSet<TB_TACO_Overtime>();
            TB_TACO_Position2 = new HashSet<TB_TACO_Position>();
            TB_TACO_Project = new HashSet<TB_TACO_Project>();
            TB_TACO_Schedule2 = new HashSet<TB_TACO_Schedule>();
            TB_TACO_SecurityRole1 = new HashSet<TB_TACO_SecurityRole>();
            TB_TACO_Team = new HashSet<TB_TACO_Team>();
            TB_TACO_TeamMember = new HashSet<TB_TACO_TeamMember>();
            TB_TACO_Unit = new HashSet<TB_TACO_Unit>();
            TB_TACO_Employee1 = new HashSet<TB_TACO_Employee>();
            TB_TACO_OvertimeBalance1 = new HashSet<TB_TACO_OvertimeBalance>();
            TB_TACO_Project1 = new HashSet<TB_TACO_Project>();
            TB_TACO_ProjectTeam1 = new HashSet<TB_TACO_ProjectTeam>();
            TB_TACO_Request1 = new HashSet<TB_TACO_Request>();
            TB_TACO_Team1 = new HashSet<TB_TACO_Team>();
            TB_TACO_TeamMember1 = new HashSet<TB_TACO_TeamMember>();
            TB_TACO_Unit1 = new HashSet<TB_TACO_Unit>();
            TB_TACO_Employee11 = new HashSet<TB_TACO_Employee>();
            TB_TACO_OvertimeBalance2 = new HashSet<TB_TACO_OvertimeBalance>();
            TB_TACO_ProjectTeam2 = new HashSet<TB_TACO_ProjectTeam>();
            TB_TACO_Request2 = new HashSet<TB_TACO_Request>();
            TB_TACO_TeamMember2 = new HashSet<TB_TACO_TeamMember>();
            TB_TACO_TimesheetDetail = new HashSet<TB_TACO_TimesheetDetail>();
            TB_TACO_TimesheetDetail1 = new HashSet<TB_TACO_TimesheetDetail>();
        }

        [Key]
        public int EmployeeId { get; set; }

        public int PositionId { get; set; }

        public int ScheduleId { get; set; }

        public int SecurityRoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        public DateTime Birthdate { get; set; }

        [StringLength(100)]
        public string EmergencyContactName { get; set; }

        [StringLength(15)]
        public string EmergencyContactPhone { get; set; }

        [Required]
        [StringLength(50)]
        public string Station { get; set; }

        [Required]
        [StringLength(50)]
        public string Computer { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }

        [Required]
        [StringLength(250)]
        public string Email { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_AllocatedTimeDetail> TB_TACO_AllocatedTimeDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_AllocatedTimeDetail> TB_TACO_AllocatedTimeDetail1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_AllocatedTimeLog> TB_TACO_AllocatedTimeLog { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Area> TB_TACO_Area { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Area> TB_TACO_Area1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Attendance> TB_TACO_Attendance { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Attendance> TB_TACO_Attendance1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_AttendanceEntitlement> TB_TACO_AttendanceEntitlement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_AttendanceEntitlement> TB_TACO_AttendanceEntitlement1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_AttendanceEntitlement> TB_TACO_AttendanceEntitlement2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Category> TB_TACO_Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Category> TB_TACO_Category1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Department> TB_TACO_Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Department> TB_TACO_Department1 { get; set; }

        public virtual TB_TACO_Position TB_TACO_Position { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Holiday> TB_TACO_Holiday { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Overtime> TB_TACO_Overtime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_OvertimeBalance> TB_TACO_OvertimeBalance { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Position> TB_TACO_Position1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_ProjectTeam> TB_TACO_ProjectTeam { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Request> TB_TACO_Request { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Schedule> TB_TACO_Schedule { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_SecurityRole> TB_TACO_SecurityRole { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Timesheet> TB_TACO_Timesheet { get; set; }

        public virtual TB_TACO_Schedule TB_TACO_Schedule1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Holiday> TB_TACO_Holiday1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Overtime> TB_TACO_Overtime1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Position> TB_TACO_Position2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Project> TB_TACO_Project { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Schedule> TB_TACO_Schedule2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_SecurityRole> TB_TACO_SecurityRole1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Team> TB_TACO_Team { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_TeamMember> TB_TACO_TeamMember { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Unit> TB_TACO_Unit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Employee> TB_TACO_Employee1 { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_OvertimeBalance> TB_TACO_OvertimeBalance1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Project> TB_TACO_Project1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_ProjectTeam> TB_TACO_ProjectTeam1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Request> TB_TACO_Request1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Team> TB_TACO_Team1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_TeamMember> TB_TACO_TeamMember1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Unit> TB_TACO_Unit1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Employee> TB_TACO_Employee11 { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_OvertimeBalance> TB_TACO_OvertimeBalance2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_ProjectTeam> TB_TACO_ProjectTeam2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Request> TB_TACO_Request2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_TeamMember> TB_TACO_TeamMember2 { get; set; }

        public virtual TB_TACO_SecurityRole TB_TACO_SecurityRole2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_TimesheetDetail> TB_TACO_TimesheetDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_TimesheetDetail> TB_TACO_TimesheetDetail1 { get; set; }
    }
}
