using System.Collections.Generic;
using WindowsFirewallHelper.FirewallAPIv2;

namespace WindowsFirewallHelper.Helpers
{
    // ReSharper disable once HollowTypeName
    internal static class ICMPHelper
    {
        public static string ICMToString(InternetControlMessage[] internetControlMessages)
        {
            var strList = new string[internetControlMessages.Length];

            for (var i = 0; i < internetControlMessages.Length; i++)
            {
                strList[i] = internetControlMessages[i].ToString();
            }

            return strList.Length == 0 ? null : string.Join(",", strList);
        }

        public static InternetControlMessage[] StringToICM(string str)
        {
            if (string.IsNullOrEmpty(str?.Trim()))
            {
                return new InternetControlMessage[0];
            }

            var internetControlMessages = new List<InternetControlMessage>();

            foreach (var icm in str.Split(','))
            {
                if (InternetControlMessage.TryParse(icm, out var message))
                {
                    internetControlMessages.Add(message);
                }
            }

            return internetControlMessages.ToArray();
        }
    }
}