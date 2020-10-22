using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Role : ModelPart
    {
        [Key]
        [Required]
        public string Role_Name { get; set; }
        public ICollection<User> Users { get; set; }
        public Role() : base()
        {
            Users = new List<User>();
        }
    }
}
