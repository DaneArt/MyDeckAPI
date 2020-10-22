using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class UserModelDeck
    {
        public UserModel Author { get; set; }
        public FullDeckModel Deck { get; set; }
    }
}
