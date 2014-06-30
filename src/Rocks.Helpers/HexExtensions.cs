using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
	public static class HexExtensions
	{
		/// <summary>
		/// Converts <paramref name="bytes"/> to string using hex (upper case) representation.
		/// </summary>
		/// <param name="bytes">Source bytes. If null or empty returns empty string.</param>
		[NotNull]
		public static string ToHexString ([CanBeNull] this IList<byte> bytes)
		{
			if (bytes == null || bytes.Count == 0)
				return string.Empty;

			var c = new char[bytes.Count * 2];

			for (var i = 0; i < bytes.Count; i++)
			{
				var b = bytes[i] >> 4;
				c[i * 2] = (char) (55 + b + (((b - 10) >> 31) & -7));
				b = bytes[i] & 0xF;
				c[i * 2 + 1] = (char) (55 + b + (((b - 10) >> 31) & -7));
			}

			return new string (c);
		}


		/// <summary>
		/// Converts source hex string to byte array.
		/// </summary>
		/// <param name="str">Source string. If null or empty returns empty array.</param>
		[NotNull]
		public static byte[] HexStringToByteArray ([CanBeNull] this string str)
		{
			if (string.IsNullOrEmpty (str))
				return new byte[0];

			if (str.Length % 2 == 1)
				str = '0' + str;

			var buffer = new byte[str.Length / 2];

			for (var i = 0; i < buffer.Length; ++i)
				buffer[i] = byte.Parse (str.Substring (i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);

			return buffer;
		}
	}
}