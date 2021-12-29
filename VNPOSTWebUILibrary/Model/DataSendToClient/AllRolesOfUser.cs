using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNPOSTWebUILibrary.Model.DataSendToClient
{
    public class AllRolesOfUser
    {
        public string Id { get; set; }
        public IEnumerable<RolesForList> AssignedRoles { get; set; }
        public IEnumerable<RolesForList> AvailableRoles { get; set; }
    }
}
