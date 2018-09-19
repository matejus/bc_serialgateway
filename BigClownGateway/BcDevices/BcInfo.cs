using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adastra.BigClownGateway
{
    public class BcInfo
    {
        public string ID { get; private set; }

        public string Firmware { get; private set; }

        public string Version { get; private set; }

        public void SyncFromMessage(MqttMessage message)
        {
            JObject data = JsonConvert.DeserializeObject(message.Payload) as JObject;
            SyncFromParsedMessage(data);
        }

        private void SyncFromParsedMessage(JObject data)
        {
            if (data == null)
                return;

            if (data.ContainsKey("firmware"))
                Firmware = data["firmware"].ToString();
            if (data.ContainsKey("version"))
                Version = data["version"].ToString();

        }

        internal static BcInfo CreateFromMessage(MqttMessage msg)
        {
            // ["/info", {"id": "836d19830d33", "firmware": "bcf-gateway-usb-dongle", "version": "vdev"}]

            if (msg == null && string.IsNullOrEmpty(msg.Payload))
                throw new ArgumentNullException(nameof(msg), "Response message missing or empty message payload");

            JObject data = JsonConvert.DeserializeObject(msg.Payload) as JObject;

            if (!data.ContainsKey("id"))
                throw new BcException("Info response message does not contain ID");

            BcInfo info = BcInfo.CreateFromID(data["id"].ToString());
            info.SyncFromParsedMessage(data);

            return info;
        }

        internal static BcInfo CreateFromID(string id)
        {
            return new BcInfo { ID = id };
        }
    }
}
