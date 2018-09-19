using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adastra.BigClownGateway
{
    public static class BcCommands
    {
        public static MqttMessage Info { get; } = new MqttMessage { Topic = "/info/get" };
        public static MqttMessage PairingModeStart { get; } = new MqttMessage { Topic = "/pairing-mode/start" };
        public static MqttMessage PairingModeStop { get; } = new MqttMessage { Topic = "/pairing-mode/stop" };
        public static MqttMessage AutoPairingModeStart { get; } = new MqttMessage { Topic = "/automatic-pairing/start" };
        public static MqttMessage AutoPairingModeStop { get; } = new MqttMessage { Topic = "/automatic-pairing/stop" };
        public static MqttMessage ScanStart { get; } = new MqttMessage { Topic = "/scan/start" };
        public static MqttMessage ScanStop { get; } = new MqttMessage { Topic = "/scan/stop" };
        public static MqttMessage GetNodes { get; } = new MqttMessage { Topic = "/nodes/get" };
        public static MqttMessage AddNode { get; } = new MqttMessage { Topic = "/nodes/add" };
        public static MqttMessage RemoveNode { get; } = new MqttMessage { Topic = "/nodes/remove" };
        public static MqttMessage PurgeNodes { get; } = new MqttMessage { Topic = "/nodes/purg" };

        /// <summary> does not work over radio </summary>
        public static MqttMessage GetNodeInfo { get; } = new MqttMessage { Topic = "{nodeID}/info" };
        public static MqttMessage GetNodeLed { get; } = new MqttMessage { Topic = "{nodeID}/led/-/state/get" };
        public static MqttMessage SetNodeLed { get; } = new MqttMessage { Topic = "{nodeID}/led/-/state/set" };


        /*
    {"led/-/state/set", led_state_set, 0, NULL},
    {"led/-/state/get", led_state_get, 0, NULL},
    {"relay/-/state/set", relay_state_set, 0, NULL},
    {"relay/-/state/get", relay_state_get, 0, NULL},
    {"relay/0:0/state/set", module_relay_state_set, 0, NULL},
    {"relay/0:0/state/get", module_relay_state_get, 0, NULL},
    {"relay/0:0/pulse/set", module_relay_pulse, 0, NULL},
    {"relay/0:1/state/set", module_relay_state_set, 1, NULL},
    {"relay/0:1/state/get", module_relay_state_get, 1, NULL},
    {"relay/0:1/pulse/set", module_relay_pulse, 1, NULL},
    {"lcd/-/text/set", lcd_text_set, 0, NULL},
    {"lcd/-/screen/clear", lcd_screen_clear, 0, NULL},
    {"led-strip/-/color/set", led_strip_color_set, 0, NULL},
    {"led-strip/-/brightness/set", led_strip_brightness_set, 0, NULL},
    {"led-strip/-/compound/set", led_strip_compound_set, 0, NULL},
    {"led-strip/-/effect/set", led_strip_effect_set, 0, NULL},
    {"led-strip/-/thermometer/set", led_strip_thermometer_set, 0, NULL},
    {"/info/get", info_get, 0, NULL},
    {"/nodes/get", nodes_get, 0, NULL},
    {"/nodes/add", nodes_add, 0, NULL},
    {"/nodes/remove", nodes_remove, 0, NULL},
    {"/nodes/purge", nodes_purge, 0, NULL},
    {"/scan/start", scan_start, 0, NULL},
    {"/scan/stop", scan_stop, 0, NULL},
    {"/pairing-mode/start", pairing_start, 0, NULL},
    {"/pairing-mode/stop", pairing_stop, 0, NULL},
    {"/automatic-pairing/start", automatic_pairing_start, 0, NULL},
    {"/automatic-pairing/stop", automatic_pairing_stop, 0, NULL},
    {"$eeprom/alias/add", alias_add, 0, NULL},
    {"$eeprom/alias/remove", alias_remove, 0, NULL},
    {"$eeprom/alias/list", alias_list, 0, NULL}
         * */
    }
}
