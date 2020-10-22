using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Exceptions
{
    public class NonValidatedModelException<T> : MDException
    {
        public override int ErrorCode { get { return 103; } } 
        public ICollection<T> ExceptionList { get; }
        public NonValidatedModelException(ICollection<T> objects)
        {
            ExceptionList = objects;
        }
    }
}
