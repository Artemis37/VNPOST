using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNPOSTWebUILibrary.Model.DataSendToClient
{
    public class RolesForList
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
