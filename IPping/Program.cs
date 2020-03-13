using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IPping
{
    class Program
    { 
        public static void Main(string[] args)
        {
            NetworkFinder.FindNetworkInterfaces();

            Task();
        }   
        
        public static void Task()
        {
            int answer = -1;

            for (int i = 0; i < NetworkFinder.localInterfaces.Count; i++)
            {
                Console.WriteLine(i.ToString() + "  -- " + NetworkFinder.localInterfaces[i].Name);
                Console.WriteLine("IPv4............." + NetworkFinder.localInterfaces[i].IPv4);
                Console.WriteLine("Subnet Mask......" + NetworkFinder.localInterfaces[i].SubnetMask);
                Console.WriteLine();
            }

            while (true)
            {
                Console.Write("|> ");
                try
                {
                    answer = Int32.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("  Wrong format");
                    continue;
                }

                if (answer == -1)
                    return;                

                IPScaner.IPAdresses.Clear();
                string tryIP = "";
                try
                {
                    tryIP = NetworkFinder.localInterfaces[answer].StartPosIP;
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong interface selected");
                    continue;
                }

                for (int i = 0; i < NetworkFinder.localInterfaces[answer].Amount; i++)
                {
                    var scan = new IPScaner(tryIP);
                    tryIP = IPScaner.NextIP(tryIP);
                }
                Thread.Sleep(2000);
                foreach (var item in IPScaner.IPAdresses)
                {
                    Console.WriteLine(item + "...." + IPScaner.GetMacAddress(item));
                }
                Console.WriteLine();
            }
        }
    }
}
