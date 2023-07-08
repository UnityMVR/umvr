using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace pindwin.development
{
	public static class Assertions
	{
		public static void Assert(
			bool condition,
			[CallerFilePath] string filePath = "",
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0)
		{
			if (condition == false)
			{
				throw new ArgumentException($"Assertion failed! {caller}:({filePath} ln. {line})");
			}
		}

		public static TObject AssertNotNull<TObject>(
			this TObject o,
			[CallerFilePath] string filePath = "",
			[CallerMemberName] string caller = "",
			[CallerLineNumber] int line = 0)
			where TObject : class
		{
#if UNITY_EDITOR
			if (o == null)
			{
				throw new ArgumentException($"Value cannot be null! {caller}:({filePath} ln. {line})");
			}
#endif
			return o;
		}

		public static bool IsValidIndex(this IList list, int index) => index >= 0 && index < list.Count;
	}
}