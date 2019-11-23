using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace tessOCR
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string baseURL = "http://fastdl.friendlyplayers.com/siggen/darkrpbase/";
                Console.Write("Input SteamID: ");
                string inputID = Console.ReadLine();
                Console.Clear();
                string balance = GrabBalance.GetBalance(baseURL + SteamIDUtils.RetrieveID(inputID) + ".png");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"${balance}");
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n----Error----\nMake sure input was valid and try again\n-------------");
            }
            Console.ReadKey();
        }
    }
}
