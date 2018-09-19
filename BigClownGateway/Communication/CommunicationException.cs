using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adastra.BigClownGateway
{
    public sealed class CommunicationException : Exception
    {
        internal CommunicationException() : base() { }
        internal CommunicationException(string message) : base(message) { }
        internal CommunicationException(string message, Exception innerException) : base(message, innerException) { }
        internal CommunicationException(Exception innerException) : base("BcSerialPort.CommunicationException", innerException) { }
    }
}
