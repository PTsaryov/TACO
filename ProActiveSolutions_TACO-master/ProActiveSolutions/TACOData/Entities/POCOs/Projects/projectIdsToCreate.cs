using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TACOData.Entities.POCOs.Projects
{
    public class projectIdsToCreate
    {
        public projectIdsToCreate(int id)
        {
            this.id = id;
        }
        public int id { get; set; }
    }
}
