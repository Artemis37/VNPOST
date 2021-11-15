using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNPOSTWebUILibrary.Model
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? GroupId { get; set; }
        public int MajorGroupId { get; set; }
        public int Views { get; set; }
        public string LabelImage { get; set; }
    }
}
