using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adastra.BigClownGateway
{
    /// <summary>
    /// simple implementation of MQTT message in the format [topic, payload]
    /// 
    /// </summary>
    public class MqttMessage
    {
        const string NULL_STRING = "null";

        public string Topic { get; set; }

        public string Payload { get; set; }

        public string ToMessageString()
        {
            string pl = Payload;
            if (string.IsNullOrEmpty(pl))
            {
                pl = NULL_STRING;
            }
            else if (pl.Trim().IndexOfAny(new char[] { '[','{'}) != 0)
            {
                pl = "\"" + pl + "\"";
            }
            return $"[\"{Topic ?? string.Empty}\", {pl}]";
        }

        public static bool TryParse(string source, out MqttMessage message)
        {
            message = null;

            if (string.IsNullOrEmpty(source))
                return false;

            // mqtt have to be in form [topic, payload]
            // when topic is string before first comma (,) the text can be qualified by quotes  
            source = source.Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
            if (!source.StartsWith("[") || !source.EndsWith("]"))
                return false;

            try
            {
                source = source.Substring(1, source.Length - 2).Trim();
                if (string.IsNullOrEmpty(source) || source.Length < 2) // one char topic and delimiter is minimum
                    return false;

                int cPos = source.IndexOf(",");
                if (cPos <= 0)                  // delimiter or topic is missing
                    return false;

                int qPos = source.IndexOf("\"");  // check quotas in topic
                if (qPos >= 0 && qPos < cPos)
                {
                    qPos = source.IndexOf("\"", qPos + 1);
                    if (qPos < 0 || qPos >= source.Length)                  // missing end quotation mark or nothing after end of topic
                        return false;
                    cPos = source.IndexOf(",", qPos);
                }

                string topic = source.Substring(0, cPos - 1).Replace("\"", string.Empty).Replace(" ", string.Empty).Trim();
                string payload = source.Substring(cPos + 1).Trim();

                if (payload.ToLower() == NULL_STRING)
                {
                    payload = null;
                }
                else if (payload.StartsWith("\"") && payload.EndsWith("\""))
                {
                    payload = payload.Substring(1, payload.Length - 2).Trim();
                }

                message = new MqttMessage { Topic = topic, Payload = payload };

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
