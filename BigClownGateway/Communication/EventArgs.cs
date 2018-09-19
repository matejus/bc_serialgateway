using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adastra.BigClownGateway
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }


    [Serializable]
    public class DataEventArgs : EventArgs
    {
        internal DataEventArgs(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }

    [Serializable]
    public class MessageEventArgs : EventArgs
    {
        internal MessageEventArgs(MqttMessage message)
        {
            Message = message;
        }

        public MqttMessage Message { get; private set; }
    }

}
