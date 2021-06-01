using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs
{
    public class EmployeeProfile
    {
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }

        public string FirstName { get; set; }
 
        public string LastName { get; set; }

        public int PositionId { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        public DateTime Birthdate { get; set; }

        public int ScheduleType { get; set; }

        public int SecurityRoleId { get; set; }

        public int SecurityRole { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Computer { get; set; }

        public string Station { get; set; }

        public string EmergencyContact { get; set; }

        public string EmergencyContactPhone { get; set; }

    }
}
