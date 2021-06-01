using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TACOData.Entities.POCOs.DaysOff
{
    public class DayOffInformation
    {
        public int EmployeeId { get; set; }
        public int TimesheetId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<BookedDaysOff> BookedDays { get; set; }
        public DayOffIndicator BookedDaysPerMonth { get; set; }
      
    }
}
