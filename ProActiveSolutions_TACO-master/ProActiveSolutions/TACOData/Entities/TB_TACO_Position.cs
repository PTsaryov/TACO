namespace TACOData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_TACO_Position
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_TACO_Position()
        {
            TB_TACO_Employee = new HashSet<TB_TACO_Employee>();
        }

        [Key]
        public int PositionId { get; set; }

        [Required]
        [StringLength(100)]
        public string PositionName { get; set; }

        [Required]
        [StringLength(250)]
        public string PositionDescription { get; set; }

        public bool PositionDeactivated { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_Employee> TB_TACO_Employee { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee1 { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee2 { get; set; }
    }
}
