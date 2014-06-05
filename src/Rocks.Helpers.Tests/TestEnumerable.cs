using System.Collections.Generic;

namespace Rocks.Helpers.Tests
{
	public class TestEnumerable<T>
	{
		private readonly IEnumerable<T> items;
		private int enumeratedCount;


		public int EnumeratedCount
		{
			get { return this.enumeratedCount; }
		}


		public TestEnumerable (IEnumerable<T> items)
		{
			this.items = items;
		}


		public IEnumerable<T> Enumerate ()
		{
			enumeratedCount = 0;

			foreach (var item in items)
			{
				enumeratedCount++;
				yield return item;
			}
		}
	}
}
