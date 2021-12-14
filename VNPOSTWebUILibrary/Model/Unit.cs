using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNPOSTWebUILibrary.Model
{
    public class Unit
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CategoryUnitId { get; set; }
        [Required]
        public string UnitRankId { get; set; }
        [Required]
        public string LicenseNumber { get; set; }
        public bool Status { get; set; }
#nullable enable
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public DateTime? AppointedDate { get; set; }
        public string? Note { get; set; }
        public string? UserId { get; set; }
#nullable disable
    }
}
