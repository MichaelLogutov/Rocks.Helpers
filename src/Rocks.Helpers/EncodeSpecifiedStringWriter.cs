using System;
using System.IO;
using System.Text;

namespace Rocks.Helpers
{
	public class EncodeSpecifiedStringWriter : StringWriter
	{
		private readonly Encoding encoding;


		public override Encoding Encoding
		{
			get { return this.encoding; }
		}


		public EncodeSpecifiedStringWriter (StringBuilder sb, Encoding encoding)
			: base (sb)
		{
			if (encoding == null)
				throw new ArgumentNullException ("encoding");

			this.encoding = encoding;
		}


		public EncodeSpecifiedStringWriter (Encoding encoding)
		{
			if (encoding == null)
				throw new ArgumentNullException ("encoding");

			this.encoding = encoding;
		}
	}
}