using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adastra.BigClownGateway
{
    public class BcNode
    {
        public BcInfo Info { get; private set; } = null;

        private BcSensorCollection _sensors = new BcSensorCollection();
        public BcSensor[] Sensors => _sensors.ToArray();

        internal void SyncSensorsFromMessage(string topic)
        {
            _sensors.SyncFromMessage(topic);
        }

        internal static BcNode CreateFormID(string id)
        {
            return new BcNode { Info = BcInfo.CreateFromID(id) };
        }


        internal static string GetIdFromTopic(string topic)
        {
            if (string.IsNullOrEmpty(topic))
                return null;
            topic = topic.Trim();
            if (topic.Length < 12)
                return null;
            topic = topic.Substring(0, 12);
            long id;
            if (Int64.TryParse(topic, System.Globalization.NumberStyles.HexNumber, null, out id))
                return topic;
            return null;
        }
    }

    public class BcNodeCollection : List<BcNode>
    {
        internal void SyncFromList(string getNodesPayload)
        {
            if (string.IsNullOrEmpty(getNodesPayload))
                return;

            var pl = getNodesPayload.Trim();
            if (!pl.StartsWith("[") || !pl.EndsWith("]"))
                return;

            pl = pl.Substring(1, pl.Length - 2);
            var nodeIdList = pl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.Replace("\"", ""));

            foreach (string id in nodeIdList)
            {
                if (!this.Any(n=>n.Info.ID == id))
                    this.Add(BcNode.CreateFormID(id));
            }

            this.RemoveAll(n => !nodeIdList.Contains(n.Info.ID));
        }

        internal void SyncFromMessage(MqttMessage message)
        {
            if (message == null)
                return;

            var nodeId = BcNode.GetIdFromTopic(message.Topic);
            if (nodeId == null || message.Topic.Length <= nodeId.Length + 1)
                return;

            string topic = message.Topic.Substring(nodeId.Length);

            var node = this.FirstOrDefault(n => n.Info.ID == nodeId);
            if (node == null)
            {
                node = BcNode.CreateFormID(nodeId);
                this.Add(node);
            }

            if (topic.StartsWith("/info"))
                node.Info.SyncFromMessage(message);
            else
                node.SyncSensorsFromMessage(topic);
        }
    }
}
