using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests
{
	[TestClass]
	public class CryptoExtensionsTests
	{
		[TestMethod]
		public void Decode_Null_ReturnsNull ()
		{
			((string) null).Decode ("abc").Should ().BeNull ();
		}


		[TestMethod]
		public void Decode_Empty_ReturnsEmpty ()
		{
			string.Empty.Decode ("abc").Should ().BeEmpty ();
		}


		[TestMethod]
		public void Decode_AfterEncode_ReturnsOriginalValue ()
		{
			var original = "ZWNBAgL5cLH6UaDs";
			var key = CryptoExtensions.GenerateKey ();

			var encoded = original.Encode (key);
			var decoded = encoded.Decode (key);

			encoded.Should ().NotBe (decoded).And.Should ().NotBe (original);
			decoded.Should ().Be (original);
		}


		[TestMethod]
		public void GetHash_Default_ReturnsCorrectSHA256Hash ()
		{
			// arrange
			var data = "abc";


			// act
			var result = data.GetHash ();


			// assert
			result.Should ().Equal ("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad".HexStringToByteArray ());
		}


		[TestMethod]
		public void GetHash_MD5_ReturnsCorrectHash ()
		{
			// arrange
			var data = "abc";


			// act
			var result = data.GetHash ("MD5");


			// assert
			result.Should ().Equal ("900150983cd24fb0d6963f7d28e17f72".HexStringToByteArray ());
		}


		[TestMethod]
		public void GetHash_SHA512_ReturnsCorrectHash ()
		{
			// arrange
			var data = "abc";


			// act
			var result = data.GetHash ("SHA512");


			// assert
			result.Should ().Equal ("ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f".HexStringToByteArray ());
		}
	}
}