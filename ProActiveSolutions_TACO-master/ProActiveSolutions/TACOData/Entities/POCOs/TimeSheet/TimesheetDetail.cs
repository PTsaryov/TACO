using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs.TimeSheet
{
    public class TimesheetDetail
    {
        public int ProjectId { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
}
