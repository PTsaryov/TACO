using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs
{
    public class ProjectInformation
    {
        public int ProjectId { get; set; }

        public int CategoryId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectDescription { get; set; }

        public string Priority { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string CategoryName { get; set; }

        public string Status { get; set; }

        public string FullName { get; set; }

        public string DepartmentName { get; set; }

        public string TeamName { get; set; }

        public int EmployeeId { get; set; }

        public string Position { get; set; }

        public DateTime? Termination { get; set; }

        public string ProjectColor { get; set; }

        public string Color { get; set; }

        public int ProjectTeamId { get; set; }

    }
}
