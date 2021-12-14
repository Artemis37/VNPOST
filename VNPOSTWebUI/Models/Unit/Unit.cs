using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNPOSTWebUI.Models.Unit
{
    public class Unit
    {
        public string CateogryName { get; set; }
        public string Name { get; set; }
        public string CategoryUnitId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string UnitRankId { get; set; }
        public string LicenseNumber { get; set; }
        public string AppointedDate { get; set; }
        public bool Status { get; set; }
        public string Note { get; set; }
        public string UserId { get; set; }
    }
}
