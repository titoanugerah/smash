using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Castle.MicroKernel.SubSystems.Conversion;

namespace Suscribe_Management_System_Hehe.Models
{
    public class Config
    {
        public int Id { set; get; }
        [Required]
        public int ApplicationId { set; get; }
        public Application Application { set; get; }
        [Required]
        [MaxLength(16)]
        public string Name { set; get; } 
        [Column(TypeName = "bit")]
        [DefaultValue(true)]
        public bool IsExist { set; get; }

    }
}
