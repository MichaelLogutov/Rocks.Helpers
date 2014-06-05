using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests
{
	[TestClass]
	public class CryptoExtensionsTests
	{
		[TestMethod]
		public void Decrypt_Null_ReturnsNull ()
		{
			((string) null).Decode ("abc").Should ().BeNull ();
		}


		[TestMethod]
		public void Decrypt_Empty_ReturnsEmpty ()
		{
			string.Empty.Decode ("abc").Should ().BeEmpty ();
		}


		[TestMethod]
		public void ShouldEncryptAndDecrypt ()
		{
			var original = "ZWNBAgL5cLH6UaDs";
			var key = CryptoExtensions.GenerateKey ();

			var encoded = original.Encode (key);
			var decoded = encoded.Decode (key);

			encoded.Should ().NotBe (decoded).And.Should ().NotBe (original);
			decoded.Should ().Be (original);
		}
	}
}