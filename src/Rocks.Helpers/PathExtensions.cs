using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rocks.Helpers
{
	public static class PathExtensions
	{
		private static readonly Regex RegexInvalidFileNameChars =
			new Regex ("[" +
			           string.Join (string.Empty,
			                        Path.GetInvalidFileNameChars ().Select (x => Regex.Escape (x.ToString (CultureInfo.InvariantCulture)))) +
			           "]",
			           RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);


		private static readonly Regex RegexInvalidPathChars =
			new Regex ("[" +
			           string.Join (string.Empty,
			                        Path.GetInvalidPathChars ().Select (x => Regex.Escape (x.ToString (CultureInfo.InvariantCulture)))) +
			           "]",
			           RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);


		public static string PathReplaceInvalidFileNameChars (this string str, string replacement = null)
		{
			if (string.IsNullOrEmpty (str))
				return str;

			var result = RegexInvalidFileNameChars.Replace (str, replacement ?? string.Empty);
			result = result.Trim ();

			return result;
		}


		public static string PathReplaceInvalidPathChars (this string str, string replacement = null)
		{
			if (string.IsNullOrEmpty (str))
				return str;

			var result = RegexInvalidPathChars.Replace (str, replacement ?? string.Empty);
			result = result.Trim ();

			return result;
		}
	}
}