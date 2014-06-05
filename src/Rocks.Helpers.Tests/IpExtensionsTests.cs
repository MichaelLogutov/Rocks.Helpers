using System.Net;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests
{
	[TestClass]
	public class IpExtensionsTests
	{
		[TestMethod]
		public void ToLong_CorrectlyConverts ()
		{
			// arrange
			var ip = IPAddress.Parse ("143.24.20.36");


			// act
			var result = ip.ToLong ();


			// assert
			result.Should ().Be (2400719908);
		}


		[TestMethod]
		public void ToIPAddress_CorrectlyConverts ()
		{
			// arrange
			var ip = 2400719908L;


			// act
			var result = ip.ToIPAddress ();


			// assert
			result.Should ().Be (IPAddress.Parse ("143.24.20.36"));
		}
	}
}