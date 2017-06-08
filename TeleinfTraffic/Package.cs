using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleinfTraffic
{
    /// <summary>
    /// Paczka, czyli w zasadzie tylko kolekcja pakietów.
    /// </summary>
    class Package
    {
        private List<Packet> packets;

        public Package()
        {
            packets = new List<Packet>();
        }

        public Package(List<Packet> list)
        {
            packets = list;
        }

        public void addPacket(Packet pkt)
        {
            packets.Add(pkt);
        }

        public int getLength()
        {
            return packets.Count;
        }
    }
}
