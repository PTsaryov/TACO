using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs.TimeSheet
{
    public class OvertimeForTotals
    {
        public int? OvertimeId { get; set; }
        public string OvertimeDescription { get; set; }
        public TimeSpan Total { get; set; }

        public OvertimeForTotals(int? OvertimeId,string OvertimeDescription, TimeSpan Total)
        {
            this.OvertimeId = OvertimeId;
            this.OvertimeDescription = OvertimeDescription;
            this.Total = Total;
        }
    }
}
