using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suscribe_Management_System_Hehe.Models
{
    public class Role
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public List<Models.User> Users { set; get; }
    }
}
