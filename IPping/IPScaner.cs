using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IPping
{
    class IPScaner
    {
        public static List<string> IPAdresses = new List<string>();

        private readonly string who;

        static private Semaphore semaphore = new Semaphore(55, 55);       

        public IPScaner(string ip)
        {
            who = ip;
            var thread = new Thread(ShowIpAdresess);
            thread.Name = ip;
            thread.Start();
        }            

        public void ShowIpAdresess()
        {            
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 100;

            PingReply reply = pingSender.Send(who, timeout, buffer, options);

            if (reply.Status == IPStatus.Success)
                IPAdresses.Add(reply.Address.ToString());
        }

        public static string NextIP(string ip)
        {
            string binIP = "";

            foreach (var item in ip.Split('.'))
                binIP += Convert.ToString(Byte.Parse(item), 2).PadLeft(8, '0');

            int intIP = Convert.ToInt32(binIP, 2);
            intIP++;

            binIP = Convert.ToString(intIP, 2).PadLeft(32, '0');

            string nextIP = "";

            for (int i = 0; i < 32; i += 8)
                nextIP += Convert.ToByte(binIP.Substring(i, 8), 2) + ".";

            nextIP = nextIP.Remove(nextIP.Length - 1);

            return nextIP;
        }

        public static string GetMacAddress(string ipAddress)
        {
            var macAddress = string.Empty;
            var pProcess = new System.Diagnostics.Process();

            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a " + ipAddress;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();

            string strOutput = pProcess.StandardOutput.ReadToEnd();
            string[] substrings = strOutput.Split('-');

            if (substrings.Length >= 8)
            {
                macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                         + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                         + "-" + substrings[7] + "-"
                         + substrings[8].Substring(0, 2);
                return macAddress;
            }
            else
            {
                return "not found";
            }
        }
    }
}
