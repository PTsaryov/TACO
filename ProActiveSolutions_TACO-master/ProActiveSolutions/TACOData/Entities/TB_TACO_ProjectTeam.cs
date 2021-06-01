namespace TACOData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_TACO_ProjectTeam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_TACO_ProjectTeam()
        {
            TB_TACO_AllocatedTimeDetail = new HashSet<TB_TACO_AllocatedTimeDetail>();
        }

        [Key]
        public int ProjectTeamId { get; set; }

        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_AllocatedTimeDetail> TB_TACO_AllocatedTimeDetail { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee1 { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee2 { get; set; }

        public virtual TB_TACO_Project TB_TACO_Project { get; set; }
    }
}
