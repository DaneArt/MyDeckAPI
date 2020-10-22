using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Exceptions
{

    /// <summary>
    /// Error codes: 
    /// 100 - unhandled exception
    /// 101 - email already in use
    /// 102 - username already in use
    /// 103 - email not confirmed
    /// 104 - invalid google token
    /// 105 - invalid user credentialss
    /// 106 - user not found
    /// 107 - wrong password
    /// </summary>
    public class MDException: Exception
    {
        public virtual int ErrorCode { get { return 100; } }
    }
}
