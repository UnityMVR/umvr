using System;
using UnityEngine.UIElements;

namespace pindwin.umvr.View.Widgets
{
	public class DebuggerEntryWidget : DebuggerElement
	{
		private readonly Foldout _foldout;
		
		public DebuggerEntryWidget(string columnId, bool initialValue, string buttonLabel, Action buttonAction)
		{
			AddToClassList(COLUMN);
			name = columnId;
			
			_foldout = MakeFoldout(columnId, initialValue, this);
			_foldout.AddToClassList(COLUMN);

			MakeButton(buttonAction, buttonLabel, _foldout.contentContainer);
		}

		public new void Add(VisualElement visualElement)
		{
			_foldout.contentContainer.Add(visualElement);
		}

		public new void Remove(VisualElement visualElement)
		{
			_foldout.contentContainer.Remove(visualElement);
		}
	}
}