using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Exceptions
{
    public class NonConfirmedEmailException:MDException
    {
        public override int ErrorCode { get { return 103; } }

        public override string Message { get { return "103: Email is not confirmed";} }
    }
}
