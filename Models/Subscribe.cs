using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Subscribe : ModelPart
    {
        [Key]
        public Guid Subscribe_Id { get; set; } 
        public Guid Follower_Id { get; set; }
        public User Follower { get; set; }
        public Guid Publisher_Id { get; set; }
        public User Publisher {get; set; }

        public Subscribe() : base()
        {

        }

    }
}
