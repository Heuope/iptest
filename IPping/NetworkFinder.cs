using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IPping
{
    static class NetworkFinder
    {
        public static List<LocalInterface> localInterfaces = new List<LocalInterface>();

        public static void FindNetworkInterfaces()
        {
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface Interface in Interfaces)
            {
                if (Interface.OperationalStatus != OperationalStatus.Up) continue;
                //if (Interface.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;

                UnicastIPAddressInformationCollection UnicastIPInfoCol = Interface.GetIPProperties().UnicastAddresses;

                localInterfaces.Add(new LocalInterface(
                    Interface.Description,
                    UnicastIPInfoCol[1].Address.ToString(),
                    UnicastIPInfoCol[1].IPv4Mask.ToString())
                    );                
            }
        }
    }
}
