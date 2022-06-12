using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace PureSharp
{
    public class Configure
    {
        public string GetValueConfigByKey(string key)
        {
            var config = GetConfigs();
            return config.ContainsKey(key) ? config[key] : "";
        }

        public void SetKeyAndValueConfig(string key, string value)
        {
            var config = GetConfigs();
            // check if value is exist
            if (config.ContainsKey(key))
            {
                config[key] = value;
            }
            else
            {
                config.Add(key, value);
            }

            CheckConfigFile();
            // clear all lines 
            File.WriteAllText(Constant.configFile, "");
            foreach (var item in config)
            {
                var strTemp = item.Key + "=" + item.Value;
                var path = Constant.configFile;
                File.AppendAllText(path, strTemp + "\n");
            }
        }

        public Dictionary<string, string> GetConfigs()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            var input = GetLineList();
            input.ForEach(x =>
            {
                var temp = x.Split('=');
                if (temp.Length == 2)
                {
                    data.Add(temp[0], temp[1]);
                }
            });
            return data;
        }

        private List<string> GetLineList()
        {
            CheckConfigFile();
            // read content in file
            var content = File.ReadAllText(Constant.configFile);
            // split content by line
            var lines = content.Split('\n').ToList();
            // find key in lines
            return lines;
        }

        private void CheckConfigFile()
        {
            if (!File.Exists(Constant.configFile))
            {
                File.Create(Constant.configFile);
            }

            // File.SetAttributes(Constant.configFile, FileAttributes.Hidden);
            // set file permission all user can read and write
            GrantAccess(Constant.configFile);
            GrantAccess(Constant.hostFile);
        }
        
        private void GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
        }
    }
}