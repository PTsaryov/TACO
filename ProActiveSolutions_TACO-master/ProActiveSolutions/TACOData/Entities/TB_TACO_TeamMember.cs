namespace TACOData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_TACO_TeamMember
    {
        [Key]
        public int TeamMemberId { get; set; }

        public int TeamId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee1 { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee2 { get; set; }

        public virtual TB_TACO_Team TB_TACO_Team { get; set; }
    }
}
