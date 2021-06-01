using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs.TimeSheet
{
    public class OvertimeDeatils
    {
        public int OvertimeId { get; set; }
        public string Description { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }
}
