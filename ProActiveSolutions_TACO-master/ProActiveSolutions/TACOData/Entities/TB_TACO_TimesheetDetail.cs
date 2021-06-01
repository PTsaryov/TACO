namespace TACOData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_TACO_TimesheetDetail
    {
        [Key]
        public int TimesheetDetailId { get; set; }

        public int TimesheetId { get; set; }

        public int? ProjectId { get; set; }

        public int? AttendanceId { get; set; }

        public int? HolidayId { get; set; }

        public int? OvertimeId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public bool ArchiveDelete { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual TB_TACO_Attendance TB_TACO_Attendance { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee1 { get; set; }

        public virtual TB_TACO_Holiday TB_TACO_Holiday { get; set; }

        public virtual TB_TACO_Overtime TB_TACO_Overtime { get; set; }

        public virtual TB_TACO_Project TB_TACO_Project { get; set; }

        public virtual TB_TACO_Timesheet TB_TACO_Timesheet { get; set; }
    }
}
