using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs
{
    public class ProjectTeamInformation
    {
        public int ProjectTeamId { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string ProjectName { get; set; }

        public int ProjectId { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int AllocatedYear { get; set; }

        public int AllocatedDays { get; set; }

        public int January { get; set; }

        public int February { get; set; }

        public int March { get; set; }

        public int April { get; set; }

        public int May { get; set; }

        public int June { get; set; }

        public int July { get; set; }

        public int August { get; set; }

        public int September { get; set; }

        public int October { get; set; }

        public int November { get; set; }

        public int December { get; set; }

        public int DateYears { get; set; }

        public int AllocatedTimeId { get; set; }

        public int AllocatedTimeLogged { get; set; }
    }
}
