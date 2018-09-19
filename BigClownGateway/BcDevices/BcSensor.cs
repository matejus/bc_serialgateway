using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adastra.BigClownGateway
{
    public class BcSensor
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public override string ToString()
        {
            return $"BcSensor -> Name:{Name}; Address:{Address}";
        }
    }

    internal class BcSensorCollection : List<BcSensor>
    {
        internal void SyncFromMessage(string topic)
        {
            if (string.IsNullOrEmpty(topic) || !topic.StartsWith("/") || topic.Length<2)
                return;

            topic = topic.Substring(1);
            string name;
            string addr = null;

            int pos = topic.IndexOf("/");
            if (pos <= 0)
                pos = topic.Length;
            name = topic.Substring(0, pos);

            if (pos+1 < topic.Length)
            {
                topic = topic.Substring(pos + 1);
                pos = topic.IndexOf("/");
                if (pos < 0)
                    pos = topic.Length;
                addr = topic.Substring(0, pos);
            }

            var sens = this.FirstOrDefault(s => s.Name == name && (addr == s.Address));
            if (sens == null)
                this.Add(new BcSensor { Name = name, Address = addr });
        }
    }


}
