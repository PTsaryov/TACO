using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TACOData.Entities.POCOs.DaysOff
{
    public class BookedDaysOff
    {
        public int TimesheetDetailId { get; set; }
        public string AbsenceCode { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int? Duration { get; set; }
     
    }

 
}