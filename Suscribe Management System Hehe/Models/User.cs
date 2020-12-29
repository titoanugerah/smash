using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Suscribe_Management_System_Hehe.Models
{
    public class User
    {
        public int Id { set; get; }
        [MaxLength(32)]
        public string Name { set; get; }
        [Required]
        public string Email { set; get; }
        public string Image { set; get; }
        [DefaultValue(2)]
        public int RoleId { set; get; }
        public Role Role { set; get; }
        [Column(TypeName = "bit")]
        [DefaultValue(true)]
        public bool IsExist { set; get; }

    }
}
