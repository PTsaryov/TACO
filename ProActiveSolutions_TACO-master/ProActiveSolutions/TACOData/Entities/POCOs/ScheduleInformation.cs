using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs
{
   public  class ScheduleInformation
    {
        public int ScheduleId { get; set; }

        public string ScheduleName { get; set; }

        public string ScheduleDescription { get; set; }

        public int ScheduleTime { get; set; }

        public bool ScheduleDeactivated { get; set; }

    }
}
