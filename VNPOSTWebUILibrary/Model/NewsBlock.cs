using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNPOSTWebUILibrary.Model
{
    public class NewsBlock
    {
        public int MajorGroupId { get; set; }
        public string MajorGroupName { get; set; }
        public IEnumerable<News> newsBlock { get; set; }
    }
}
