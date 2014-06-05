using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
	public static class CryptoExtensions
	{
		/// <summary>
		/// Generates new encryption key.
		/// </summary>
		[NotNull]
		public static string GenerateKey (int length = 16)
		{
			using (var provider = new RNGCryptoServiceProvider ())
			{
				var data = new byte[length];
				provider.GetBytes (data);

				return Convert.ToBase64String (data);
			}
		}


		private static readonly byte[] salt = { 0x19, 0x81, 0x34, 0xA0, 0x05, 0x6D, 0x95, 0xF6, 0x26, 0x75, 0x64, 0x4E, 0x26 };


		/// <summary>
		/// Encodes <paramref name="value"/> using Rijndael algorithm and <paramref name="password"/>.
		/// </summary>
		/// <param name="value">Value. Can be null.</param>
		/// <param name="password">Encryption key (as hex string).</param>
		/// <returns>Encoded value.</returns>
		[ContractAnnotation ("value:null => null")]
		[SuppressMessage ("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		public static string Encode (this string value, [NotNull] string password)
		{
			if (string.IsNullOrEmpty (value))
				return value;

			var data = Encoding.UTF8.GetBytes (value);
			var length_data = BitConverter.GetBytes (data.Length);

			using (var pdb = new Rfc2898DeriveBytes (password, salt))
			{
				using (var algorithm = new RijndaelManaged ())
				{
					algorithm.Padding = PaddingMode.Zeros;
					algorithm.Key = pdb.GetBytes (32);
					algorithm.IV = pdb.GetBytes (16);

					using (var memory_stream = new MemoryStream ())
					{
						memory_stream.Write (length_data, 0, length_data.Length);

						using (var crypto_stream = new CryptoStream (memory_stream, algorithm.CreateEncryptor (), CryptoStreamMode.Write))
						{
							crypto_stream.Write (data, 0, data.Length);
							crypto_stream.FlushFinalBlock ();

							return Convert.ToBase64String (memory_stream.GetBuffer (), 0, (int) memory_stream.Length);
						}
					}
				}
			}
		}


		/// <summary>
		/// Decodes <paramref name="value"/> using Rijndael algorithm and <paramref name="password"/>.
		/// </summary>
		/// <param name="value">Value. Can be null.</param>
		/// <param name="password">Decryption password.</param>
		/// <returns>Decoded value.</returns>
		[ContractAnnotation ("value:null => null")]
		[SuppressMessage ("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		public static string Decode (this string value, [NotNull] string password)
		{
			if (string.IsNullOrEmpty (value))
				return value;

			var data = Convert.FromBase64String (value);
			var length_data = BitConverter.ToInt32 (data, 0);

			using (var pdb = new Rfc2898DeriveBytes (password, salt))
			{
				using (var algorithm = new RijndaelManaged ())
				{
					algorithm.Padding = PaddingMode.Zeros;
					algorithm.Key = pdb.GetBytes (32);
					algorithm.IV = pdb.GetBytes (16);

					using (var memory_stream = new MemoryStream ())
					{
						using (var crypto_stream = new CryptoStream (memory_stream, algorithm.CreateDecryptor (), CryptoStreamMode.Write))
						{
							crypto_stream.Write (data, 4, data.Length - 4);
							crypto_stream.Flush ();

							return Encoding.UTF8.GetString (memory_stream.GetBuffer (), 0, length_data);
						}
					}
				}
			}
		}
	}
}