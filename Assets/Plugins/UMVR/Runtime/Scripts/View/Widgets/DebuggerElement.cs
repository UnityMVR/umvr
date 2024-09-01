using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace pindwin.umvr.View.Widgets
{
	public class DebuggerElement : VisualElement
	{
		protected const string BUTTON_CLASS = "property-widget__button";
		protected const string PROPERTY_LABEL_CLASS = "property-widget__label";
		protected const string PROPERTY_DROPDOWN_CLASS = "property-widget__dropdown";
		protected const string PROPERTY_TEXT_FIELD_CLASS = "property-widget__text-field";
		protected const string COLLECTION_ENTRY_CLASS = "collection-widget__entry";
		protected const string COLLECTION_CLASS = "collection-widget";
		protected const string PROPERTY_CLASS = "property-widget";
		protected const string COLUMN = "debugger-column";
		protected const string FOLDOUT_CHECKMARK = "foldout__checkmark";
		protected const string FOLDOUT_CONTENT = "foldout__content";
		protected const string FOLDOUT = "debugger-column__foldout";

		protected const string COMMIT_BUTTON = "\u2713";
		protected const string REVERT_BUTTON = "\u2717";
		public const string DELETE_BUTTON = "\u24CD";
		public const string ADD_BUTTON = "+";
		protected const string CLEAR_BUTTON = "\u24CD";
        
		protected static Button MakeButton(Action callback, string text, VisualElement root)
		{
			var button = new Button(callback)
			{
				text = text,
				style = { unityFontStyleAndWeight = FontStyle.Bold}
			};
			button.AddToClassList(BUTTON_CLASS);
			root.Add(button);
			return button;
		}

		protected static DropdownField MakeDropdown(string label, List<string> choices, VisualElement root)
		{
			DropdownField dropdown = new DropdownField(label)
			{
				choices = choices
			};
			dropdown.AddToClassList(PROPERTY_DROPDOWN_CLASS);
			root.Add(dropdown);
			dropdown.labelElement.AddToClassList(PROPERTY_LABEL_CLASS);
			return dropdown;
		}

		protected static TextField MakeTextField()
		{
			var textField = new TextField();
			textField.AddToClassList(PROPERTY_TEXT_FIELD_CLASS);
			textField.labelElement.AddToClassList(PROPERTY_LABEL_CLASS);
			return textField;
		}

		protected static Label MakeLabel(string text, VisualElement root)
		{
			var label = new Label(text);
			root.Add(label);
			return label;
		}

		protected static VisualElement MakeHorizontalBar(VisualElement root)
		{
			var bar = new VisualElement
			{
				style = { flexDirection = FlexDirection.Row, alignSelf = Align.FlexStart}
			};
			root.Add(bar);
			return bar;
		}

		protected static Foldout MakeFoldout(string label, bool initialValue, VisualElement root)
		{
			Foldout foldout = new Foldout()
			{
				value = initialValue,
				text = label
			};
			foldout.AddToClassList(FOLDOUT);
			foldout.Q("unity-checkmark").AddToClassList(FOLDOUT_CHECKMARK);
			foldout.contentContainer.AddToClassList(FOLDOUT_CONTENT);
			root.Add(foldout);
			return foldout;
		}
	}
}