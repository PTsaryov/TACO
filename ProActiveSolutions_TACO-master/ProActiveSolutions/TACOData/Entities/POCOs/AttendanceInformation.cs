using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs
{
    public class AttendanceInformation
    {
        public int AttendanceId { get; set; }

        public string AttendanceCode { get; set; }

        public string AttendanceDescription { get; set; }

        public bool AttendanceDeactivated { get; set; }

        public string Units { get; set; }
    }
}
