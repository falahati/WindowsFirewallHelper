using System.Collections.Generic;
using WindowsFirewallHelper.FirewallAPIv2;

namespace WindowsFirewallHelper.Helpers
{
    internal static class ICMPHelper
    {
        public static InternetControlMessage[] StringToICM(string str)
        {
            if (string.IsNullOrEmpty(str?.Trim()))
            {
                return new InternetControlMessage[0];
            }
            var icms = new List<InternetControlMessage>();
            foreach (var icm in str.Split(','))
            {
                InternetControlMessage message;
                if (InternetControlMessage.TryParse(icm, out message))
                {
                    icms.Add(message);
                }
            }
            return icms.ToArray();
        }

        public static string ICMToString(InternetControlMessage[] icms)
        {
            var strList = new string[icms.Length];
            for (var i = 0; i < icms.Length; i++)
            {
                strList[i] = icms[i].ToString();
            }
            return strList.Length == 0 ? null : string.Join(",", strList);
        }
    }
}