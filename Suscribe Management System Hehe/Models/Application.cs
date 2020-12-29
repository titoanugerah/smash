using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Suscribe_Management_System_Hehe.Models
{
    public class Application
    {
        public int Id { set; get; }
        [Required]
        [MaxLength(16)]
        public int Name { set; get; }
        [Column(TypeName="bit")]
        [DefaultValue(true)]
        public bool IsExist { set; get; }

        public List<Models.Group> Groups { set; get; }
        public List<Models.Config> Configs { set; get; }

    }
}
