using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Exceptions
{
    public class WrongPasswordException : MDException
    {
        public override int ErrorCode { get { return 107; } }

        public override string Message { get { return "107: Wrong password"; } }
    }
}
