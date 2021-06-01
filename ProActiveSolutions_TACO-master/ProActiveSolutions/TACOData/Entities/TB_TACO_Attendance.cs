namespace TACOData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_TACO_Attendance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_TACO_Attendance()
        {
            TB_TACO_AttendanceEntitlement = new HashSet<TB_TACO_AttendanceEntitlement>();
            TB_TACO_TimesheetDetail = new HashSet<TB_TACO_TimesheetDetail>();
        }

        [Key]
        public int AttendanceId { get; set; }

        [Required]
        [StringLength(10)]
        public string AttendanceCode { get; set; }

        [Required]
        [StringLength(250)]
        public string AttendanceDescription { get; set; }

        public bool AttendanceDeactivated { get; set; }

        [Required]
        [StringLength(5)]
        public string Units { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_AttendanceEntitlement> TB_TACO_AttendanceEntitlement { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TACO_TimesheetDetail> TB_TACO_TimesheetDetail { get; set; }
    }
}
