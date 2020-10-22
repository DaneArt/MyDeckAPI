using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class AuthUserModel
    {
       
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public Guid Avatar { get; set; }
    }
}
