using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class FullCardModel
    {
        [Key]
        public Guid Card_Id { get; set; }
        [Required]
        public FullCardPart Answer { get; set; }
        [Required]
        public FullCardPart Question { get; set; }
    }

    public class FullCardPart
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public String Type { get; set; }
    }
}
