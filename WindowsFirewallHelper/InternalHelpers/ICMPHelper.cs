using System.Collections.Generic;

namespace WindowsFirewallHelper.InternalHelpers
{
    // ReSharper disable once HollowTypeName
    internal static class ICMPHelper
    {
        public static string ICMToString(FirewallWASInternetControlMessage[] internetControlMessages)
        {
            var strList = new string[internetControlMessages.Length];

            for (var i = 0; i < internetControlMessages.Length; i++)
            {
                strList[i] = internetControlMessages[i].ToString();
            }

            return strList.Length == 0 ? null : string.Join(",", strList);
        }

        public static FirewallWASInternetControlMessage[] StringToICM(string str)
        {
            if (string.IsNullOrEmpty(str?.Trim()))
            {
                return new FirewallWASInternetControlMessage[0];
            }

            var internetControlMessages = new List<FirewallWASInternetControlMessage>();

            foreach (var icm in str.Split(','))
            {
                if (FirewallWASInternetControlMessage.TryParse(icm, out var message))
                {
                    internetControlMessages.Add(message);
                }
            }

            return internetControlMessages.ToArray();
        }
    }
}