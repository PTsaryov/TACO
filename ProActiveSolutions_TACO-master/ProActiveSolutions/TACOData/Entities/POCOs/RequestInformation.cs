using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs
{
    public class RequestInformation
    {
        public int RequestId { get; set; }

        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public int OvertimeId { get; set; }

        public string OvertimeCode { get; set; }

        public string OvertimeDescription { get; set; }

        public decimal TotalTime { get; set; }

        public string Status { get; set; }

        public DateTime Date { get; set; }

        public string Comment { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
