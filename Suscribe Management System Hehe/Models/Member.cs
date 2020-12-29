using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Suscribe_Management_System_Hehe.Models
{
    public class Member
    {
        public int Id { set; get; }
        public int GroupId { set; get; }
        public Group Group { set; get; }
        public int UserId { set; get; }
        public User User { set; get; }
        [Column(TypeName="bit")]
        [DefaultValue(true)]
        public bool IsExist { set; get; }
    }
}
