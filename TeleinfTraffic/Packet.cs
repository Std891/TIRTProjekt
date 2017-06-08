using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TeleinfTraffic
{
    /// <summary>
    /// Struktura pakietu wzorowana na tej używanej przez Pcap
    /// </summary>
    class Packet
    {
        private byte[] _data;
  //      private DateTime _timestamp;

        public Packet()
        {
            _data = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(_data);
  //          _timestamp = DateTime.Now;
        }

        public Packet(byte[] data)
        {
            _data = data;
  //          _timestamp = DateTime.Now;
        }

        public byte[] getData()
        {
            return _data;
        }

    }
}
