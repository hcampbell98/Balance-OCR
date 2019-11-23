using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace tessOCR
{
    class SteamIDUtils
    {
        static string apiURL = "https://api.steamid.uk/convert.php?api=V83M2JRT6VK54JS44RH7";

        private static string[] GetSteamIDs(string input)
        {
            Console.WriteLine("Contacting API");
            WebClient wc = new WebClient();
            dynamic j = JsonConvert.DeserializeObject(wc.DownloadString(apiURL + $"&input={input}&format=json"));
            string[] ids = { j.converted.steamid, j.converted.steamid64 };
            return ids;
        }

        public static string RetrieveID(string input)
        {
            Console.WriteLine("Retrieving SteamID64");
            if (input.StartsWith("http://steamcommunity.com/profiles/") || input.StartsWith("https://steamcommunity.com/profiles/"))
            {
                if (input.StartsWith("http:")) { return GetSteamIDs(input.Replace("http://steamcommunity.com/profiles/", ""))[1]; }
                else { return GetSteamIDs(input.Replace("https://steamcommunity.com/profiles/", ""))[1]; }
            }
            else if (input.StartsWith("STEAM_"))
            {
                return GetSteamIDs(input)[1];
            }
            return null;
        }
    }
}
