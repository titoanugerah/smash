using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suscribe_Management_System_Hehe.Models
{
    public class GroupConfig
    {
        public int Id { set; get; }
        [Required]
        public int GroupId { set; get; }
        public Group Group { set; get; }
        [Required]
        public int ConfigId { set; get; }
        public Config Config { set; get; }
        [Required]
        public string Value { set; get; }
        [Column(TypeName="bit")]
        [DefaultValue(true)]
        public bool IsExist { set; get; }
    }
}
