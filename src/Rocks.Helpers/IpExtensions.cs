using System;
using System.Net;
using System.Net.Sockets;

namespace Rocks.Helpers
{
	public static class IpExtensions
	{
		public static long ToLong (this IPAddress ip)
		{
			if (ip.AddressFamily == AddressFamily.InterNetworkV6)
				ip = ip.MapToIPv4 ();

			var address_bytes = ip.GetAddressBytes ();

			if (BitConverter.IsLittleEndian)
				Array.Reverse (address_bytes);

			var result = BitConverter.ToUInt32 (address_bytes, 0);

			return result;
		}


		public static IPAddress ToIPAddress (this long ip)
		{
			var address_bytes = BitConverter.GetBytes ((uint) ip);

			if (BitConverter.IsLittleEndian)
				Array.Reverse (address_bytes);

			return new IPAddress (address_bytes);
		}
	}
}