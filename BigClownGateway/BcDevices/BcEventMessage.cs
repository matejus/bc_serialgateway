using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adastra.BigClownGateway
{
    public class BcEventMessage
    {
        /// <summary>
        /// identification of USB dongle or core module used as a gatweay
        /// </summary>
        public string GatewayId { get; private set; }

        /// <summary>
        /// identification of node hosting sensor
        /// </summary>
        public string NodeId { get; private set; }

        /// <summary>
        /// sensor identification
        /// </summary>
        public string SensorName { get; private set; }

        /// <summary>
        /// sensor physical address on node if is awailable
        /// </summary>
        public string SensorAddress { get; private set; }

        /// <summary>
        /// identification of event in the node
        /// </summary>
        public string EventName { get; private set; }

        /// <summary>
        /// measured value
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// time when message was retrieved over serial port
        /// </summary>
        public DateTime Retrieved { get; } = DateTime.Now;

        public override string ToString()
        {
            return $"{GatewayId}/{NodeId}/{SensorName}/{SensorAddress}/{EventName} : {Value}";
        }

        public static BcEventMessage CreateFromMessage(MqttMessage message, BcSerialPort gateway = null)
        {
            if (string.IsNullOrEmpty(message?.Topic))
                return null;

            if (message.Topic.StartsWith("/"))
                return null;

            var parts = message.Topic.Split('/');

            BcEventMessage bc = new BcEventMessage();
            bc.GatewayId = gateway?.Info?.ID;
            if (parts.Length > 0)
                bc.NodeId = parts[0];
            if (parts.Length > 1)
                bc.SensorName = parts[1];
            if (parts.Length > 2)
                bc.SensorAddress = parts[2];
            if (parts.Length > 3)
                bc.EventName = parts[3];

            bc.Value = message.Payload;

            return bc;
        }
    }
}
