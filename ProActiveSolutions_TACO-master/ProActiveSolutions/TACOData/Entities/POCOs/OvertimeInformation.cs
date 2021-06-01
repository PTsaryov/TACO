using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs
{
    public class OvertimeInformation
    {
        public int OvertimeId { get; set; }

        public string OvertimeCode { get; set; }

        public string OvertimeDescription { get; set; }

        public bool OvertimeDeactivated { get; set; }

        public string Units { get; set; }

        public string Color { get; set; }
    }
}
