using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNPOSTWebUILibrary.Model;

namespace VNPOSTWebUI.Models
{
    public class AllGroups
    {
        public IEnumerable<MajorNewsGroup> majorNewsGroup { get; set; }
        public IEnumerable<NewsGroup> newsGroup { get; set; }
    }
}
