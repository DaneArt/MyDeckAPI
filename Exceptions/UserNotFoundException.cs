using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Exceptions
{
    public class UserNotFoundException : MDException
    {
        public override int ErrorCode { get { return 106; } }

        public override string Message { get { return "106: User not found"; } }
    }
}
