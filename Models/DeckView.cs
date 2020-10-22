using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class DeckView : ModelPart
    {
       
        public Guid Deck_Id { get; set; }
     
        public string Title { get; set; }
      
        public string Description { get; set; }
      
        public bool IsPrivate { get; set; }
        
        public bool AvailableQuickTrain { get; set; }
     
        public Guid Icon { get; set; }
        public string Category_Name { get; set; }
        public string Author { get; set; }  
        public int Cards_Count { get; set; }
        public int Subscribers_Count { get; set; }
    }
}
