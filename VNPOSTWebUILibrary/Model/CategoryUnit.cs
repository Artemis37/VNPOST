using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VNPOSTWebUILibrary.Model
{
    public class CategoryUnit
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public bool status { get; set; }
        [Required]
        public string userid { get; set; }
    }
}
