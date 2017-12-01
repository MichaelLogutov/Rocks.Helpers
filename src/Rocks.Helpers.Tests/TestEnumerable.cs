using System.Collections.Generic;

namespace Rocks.Helpers.Tests
{
	public class TestEnumerable<T>
	{
		private readonly IEnumerable<T> items;

		
		public int EnumeratedCount { get; private set; }


		public TestEnumerable (IEnumerable<T> items)
		{
			this.items = items;
		}


		public IEnumerable<T> Enumerate ()
		{
			EnumeratedCount = 0;

			foreach (var item in items)
			{
				EnumeratedCount++;
				yield return item;
			}
		}
	}
}
