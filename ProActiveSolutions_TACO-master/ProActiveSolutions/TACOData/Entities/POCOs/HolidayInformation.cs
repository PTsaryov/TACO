using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs
{
    public class HolidayInformation
    {
        public int HolidayId { get; set; }

        public string HolidayName { get; set; }

        public DateTime Date { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool HolidayDeactivated { get; set; }
    }
}
