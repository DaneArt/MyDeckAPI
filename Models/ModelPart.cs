using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public abstract class ModelPart
    {
        public DateTime LastUpdate { get; set; }
        public string Tag { get; set; }
        public ModelPart()
        {
            Tag = "";
            LastUpdate = DateTime.UtcNow;
        }
    }
}
