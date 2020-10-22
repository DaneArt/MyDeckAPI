using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class UserModel 
    {
        [Key]
        public Guid User_Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public Guid Avatar { get; set; }
    }
}
