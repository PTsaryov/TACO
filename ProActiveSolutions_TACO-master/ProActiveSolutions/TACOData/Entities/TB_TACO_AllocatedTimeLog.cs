namespace TACOData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TB_TACO_AllocatedTimeLog
    {
        [Key]
        public int AllocatedTimeLogId { get; set; }

        public int AllocatedTimeId { get; set; }

        public int AllocatedTimeLogged { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual TB_TACO_AllocatedTimeDetail TB_TACO_AllocatedTimeDetail { get; set; }

        public virtual TB_TACO_Employee TB_TACO_Employee { get; set; }
    }
}
