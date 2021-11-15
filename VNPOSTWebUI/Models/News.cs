using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace VNPOSTWebUI.Models
{
    public class News
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        //[Required]
        public string GroupId { get; set; }
        public Dictionary<string, string> newsGroups { get; set; }
        [Required]
        public string MajorGroupId { get; set; }
        public Dictionary<string, string> MajorGroups { get; set; }
        public int Views { get; set; }
        public IFormFile LabelImage { get; set; }
    }
}
