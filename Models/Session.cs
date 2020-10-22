using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Session : ModelPart
    {
        public Guid Session_Id { get; set; }
        public Guid User_Id {get; set;}
        [Required]
        public string RefreshToken { get; set; }
        public User User { get; set; }
        public Session() : base()
        {

        }
    }
}
