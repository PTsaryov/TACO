using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs
{
    public class TeamInformation
    {
        public int TeamId { get; set; }

        public int UnitId { get; set; }

        public string TeamName { get; set; }

        public string TeamDescription { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        //public List<TB_TACO_TeamMember> TeamMembers { get; set; }  >>> Not needed??? can get this list via query of members...
        //  or even employees as it should have what team an employee is on
    }
}
