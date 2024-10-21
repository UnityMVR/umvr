using System.Collections.Generic;
using pindwin.umvr.Editor.CodeGeneration.View.Parameters;
using pindwin.umvr.Editor.CodeGeneration.Designer;
using pindwin.umvr.Editor.CodeGeneration.View.Methods;
using pindwin.umvr.Editor.CodeGeneration.View.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.CodeGeneration.Window
{
	public class DesignerWindow : EditorWindow
	{
		[SerializeField] private VisualTreeAsset _designerWindowUXML;
		[SerializeField] private VisualTreeAsset _parameterUXML;
		[SerializeField] private VisualTreeAsset _propertyUXML;
		[SerializeField] private VisualTreeAsset _methodUXML;
		
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

			VisualElement labelFromUXML = new VisualElement();
			_designerWindowUXML.CloneTree(labelFromUXML);
			labelFromUXML.style.flexGrow = new StyleFloat(1);
			_root.Add(labelFromUXML);

			InitializeListView<DesignerParameter, ParamWidget>(_parameterUXML, "ParamsList");
			InitializeListView<DesignerProperty, PropertyWidget>(_propertyUXML, "PropertiesList", 100);
			InitializeListView<DesignerMethod, MethodWidget>(_methodUXML, "MethodsList", 100);
		}

		private ListView InitializeListView<TElementType, TWidgetType>(VisualTreeAsset elementUXML, string listElementName, float height = 20.0f) 
			where TWidgetType : VisualElement, INotifyValueChanged<TElementType>
		{
			var itemsSource = new List<TElementType>();
			var elementsRoot = _root.Q<ListView>(listElementName);
			elementsRoot.showAddRemoveFooter = true;
			elementsRoot.itemsSource = itemsSource;
			elementsRoot.makeItem = () =>
			{
				VisualElement element = new VisualElement();
				elementUXML.CloneTree(element);
				element.style.flexGrow = new StyleFloat(1);
				return element;
			};

			elementsRoot.bindItem = (e, i) =>
			{
				var w = e.Q<TWidgetType>();
				w.SetValueWithoutNotify(itemsSource[i]);
			};
			
			//elementsRoot.fixedItemHeight = height;
			elementsRoot.style.flexDirection = FlexDirection.Column;
			elementsRoot.style.flexGrow = 1.0f;

			elementsRoot.selectionType = SelectionType.Multiple;

			elementsRoot.onItemsChosen += objects => Debug.Log(objects);
			elementsRoot.onSelectionChange += objects => Debug.Log(objects);
			return elementsRoot;
		}
	}
}