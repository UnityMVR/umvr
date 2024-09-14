using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.CodeGeneration.Window
{
	public class DesignerWindow : EditorWindow
	{
		[SerializeField] private VisualTreeAsset _designerWindowUXML;
		[SerializeField] private VisualTreeAsset _parameterUXML;
		
		private VisualElement _root;
		private VisualElement _parameter;

		[MenuItem("Tools/UMVR/Designer")]
		public static void OpenWindow()
		{
			DesignerWindow codeGenWindow = GetWindow<DesignerWindow>();
			codeGenWindow.titleContent = new GUIContent("UMVR Designer");
		}
		
		public void CreateGUI()
		{
			AssetDatabase.Refresh();
			_root = rootVisualElement;
			
			VisualElement paramFromUXML = new VisualElement();
			_parameterUXML.CloneTree(paramFromUXML);
			paramFromUXML.style.flexGrow = new StyleFloat(1);

			VisualElement labelFromUXML = new VisualElement();
			_designerWindowUXML.CloneTree(labelFromUXML);
			labelFromUXML.style.flexGrow = new StyleFloat(1);
			_root.Add(labelFromUXML);

			var itemsSource = new List<string>() {string.Empty, String.Empty, String.Empty, String.Empty, String.Empty};
			var elementsRoot = _root.Q<ListView>("ParamsList");
			elementsRoot.showAddRemoveFooter = true;
			elementsRoot.itemsSource = itemsSource;
			elementsRoot.makeItem = () =>
			{
				VisualElement element = new VisualElement();
				_parameterUXML.CloneTree(element);
				paramFromUXML.style.flexGrow = new StyleFloat(1);
				return element;
			};

			elementsRoot.bindItem = (e, i) => e.Q<TextField>().SetValueWithoutNotify(itemsSource[i]);
			elementsRoot.fixedItemHeight = 20;

			elementsRoot.selectionType = SelectionType.Multiple;

			elementsRoot.onItemsChosen += objects => Debug.Log(objects);
			elementsRoot.onSelectionChange += objects => Debug.Log(objects);
		}
	}
}