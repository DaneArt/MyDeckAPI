using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Exceptions
{
    public class AlreadyUsedNameException:MDException
    {
        public override int ErrorCode { get { return 102; } }
        public override string Message { get { return "102: Name already used"; } }
    }
}
