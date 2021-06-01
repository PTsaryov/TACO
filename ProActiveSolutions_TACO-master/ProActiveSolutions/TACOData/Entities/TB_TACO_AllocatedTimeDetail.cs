namespace TACOData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_TACO_AllocatedTimeDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_TACO_AllocatedTimeDetail()
        {
            TB_TACO_AllocatedTimeLog = new HashSet<TB_TACO_AllocatedTimeLog>();
        }

        [Key]
        public int AllocatedTimeId { get; set; }

        public int ProjectTeamId { get; set; }

        public int AllocatedMonth { get; set; }

        public int AllocatedYear { get; set; }

        public int AllocatedDays { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual TB_TACO_ProjectTeam TB_TACO_ProjectTeam { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_AllocatedTimeLog> TB_TACO_AllocatedTimeLog { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee1 { get; set; }
    }
}
