using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs
{
   public  class MonthDayYear
    {
        public int DayNumber { get; set; }
        public string DayOfWeek { get; set; }
        public int MonthNumber { get; set; }
        public string MonthName { get; set; }
        public int YearNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
