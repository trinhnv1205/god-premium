using System.Collections.Generic;

namespace PureSharp
{
    public class Constant
    {
        public static string hostFile = "C:\\Windows\\System32\\drivers\\etc\\hosts";
        public static string configFile = $"{System.IO.Path.GetTempPath()}\\config.txt";
        public static Dictionary<string, bool> options = new Dictionary<string, bool>();
        public static string lockTime = "0";
    }
}