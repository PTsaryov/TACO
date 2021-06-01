namespace TACOData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_TACO_OvertimeBalance
    {
        [Key]
        public int OvertimeBalanceId { get; set; }

        public int EmployeeId { get; set; }

        public int OvertimeId { get; set; }

        public decimal TotalTime { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee1 { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee2 { get; set; }

        public virtual TB_TACO_Overtime TB_TACO_Overtime { get; set; }
    }
}
