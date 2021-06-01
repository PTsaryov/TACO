using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Security
{
    public class LoggedInUser
    {
        public int AccountID { get; set; }
        public string DisplayName { get; set; }
        public string SecurityRoleName { get; set; }
    }
}
