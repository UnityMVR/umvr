using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.Utilities
{
	public static class DisplayExtensions
	{
		public static DisplayStyle ToDisplayStyle(this bool isDisplayed) =>
			isDisplayed ? DisplayStyle.Flex : DisplayStyle.None;
	}
}