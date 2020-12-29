using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Suscribe_Management_System_Hehe.Models
{
    public class Group
    {
        public int Id { set; get; }
        [Required]
        public int ApplicationId { set; get; }
        public Application Application { set; get; }
        [Column(TypeName = "bit")]
        [DefaultValue(true)]
        public bool IsExist { set; get; }
        public List<Models.Member> Members { set; get; }
        public List<Models.GroupConfig> GroupConfigs { set; get; }
    }
}
