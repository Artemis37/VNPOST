using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNPOSTWebUILibrary.Model.DataSendToClient
{
    public class CategoryUnitForListUnit
    {
        public string id { get; set; }
        public string name { get; set; }

        public bool status { get; set; }
        public IEnumerable<UnitForListUnit> _children { get; set; }
    }
}
