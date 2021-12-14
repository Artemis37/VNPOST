using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNPOSTWebUILibrary.Model
{
    public class Roles
    {
        public string userid { get; set; }
        public bool newsAdd { get; set; }
        public bool newsEdit { get; set; }
        public bool newsDelete { get; set; }
        public bool unitRead { get; set; }
        public bool unitAdd { get; set; }
        public bool unitEdit { get; set; }
        public bool unitDelete { get; set; }
    }
}
