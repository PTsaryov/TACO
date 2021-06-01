using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs
{
    public class AttendenceEntitlementInfo
    {
        public int AttendanceEntitlementId { get; set; }

        public int AttendanceId { get; set; }

        public string AttendanceCode { get; set; }

        public string Units { get; set; }

        public int EmployeeId { get; set; }

        public string FullName { get; set; }

        public decimal TotalTime { get; set; }
    }
}
