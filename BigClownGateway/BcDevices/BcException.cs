using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adastra.BigClownGateway
{
    public sealed class BcException : Exception
    {
        internal BcException() : base() { }
        internal BcException(string message) : base(message) { }
        internal BcException(string message, Exception innerException) : base(message, innerException) { }
    }
}
