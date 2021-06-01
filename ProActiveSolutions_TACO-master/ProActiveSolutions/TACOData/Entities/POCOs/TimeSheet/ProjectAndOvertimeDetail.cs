using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs.TimeSheet
{
    public class ProjectAndOvertimeDetail
    {
        public int id { get; set; }
        //can accept null due to the 3 options of schedule type(project, attendance, holiday)
        public int? ProjectId { get; set; }
        public int? OvertimeId { get; set; }
        public string title{ get; set; }
        public string overtimeTitle { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string color { get; set; }
    }
}
