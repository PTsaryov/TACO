namespace TACOData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_TACO_Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_TACO_Project()
        {
            TB_TACO_ProjectTeam = new HashSet<TB_TACO_ProjectTeam>();
            TB_TACO_TimesheetDetail = new HashSet<TB_TACO_TimesheetDetail>();
        }

        [Key]
        public int ProjectId { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProjectName { get; set; }

        [Required]
        [StringLength(250)]
        public string ProjectDescription { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(15)]
        public string Priority { get; set; }

        [Required]
        [StringLength(10)]
        public string ProjectColor { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual TB_TACO_Category TB_TACO_Category { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_ProjectTeam> TB_TACO_ProjectTeam { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_TimesheetDetail> TB_TACO_TimesheetDetail { get; set; }
    }
}
