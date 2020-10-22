using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Exceptions
{
    public class NonValidatedGoogleIdTokenException:MDException
    {
        public override int ErrorCode { get { return 104; } }

        public override string Message { get { return "104: IdToken is not valid"; } }
    }
}
