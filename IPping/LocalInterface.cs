using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace IPping
{
    class LocalInterface
    {
        public string Name { get; set; }

        public string IPv4 { get; set; }

        public string SubnetMask { get; set; }

        public int Amount { get; private set; }
        
        public string StartPosIP { get; private set; }

        public LocalInterface(string name, string ipv4, string subnetMask)
        {
            Name = name;
            IPv4 = ipv4;
            SubnetMask = subnetMask;
            Initiate();
        }        

        private void Initiate()
        {
            var byteMask = new List<byte>();
            var byteIP = new List<byte>();
            var byteStart = new List<byte>();

            foreach (var item in SubnetMask.Split('.'))
                byteMask.Add(Byte.Parse(item));

            foreach (var item in IPv4.Split('.'))
                byteIP.Add(Byte.Parse(item));

            for (int i = 0; i < byteIP.Count; i++)
                byteStart.Add((byte)(byteIP[i] & byteMask[i]));

            foreach (var item in byteStart)
                StartPosIP += item.ToString() + ".";

            StartPosIP = StartPosIP.Remove(StartPosIP.Length - 1);

            string temp = "";

            foreach (var item in byteMask)
                temp += Convert.ToString(item, 2).PadLeft(8, '0');

            temp = temp.Replace('1', ' ').Trim();

            Amount = (int)Math.Pow(2, (double)(temp.Length)) - 2;
        }        
    }
}
