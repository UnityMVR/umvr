﻿using System;
using System.Collections.Generic;
using System.Linq;
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
		
		private VisualElement _root;
		private VisualElement _parameter;
		
		private Button _addMethodButton;
		private Button _removeMethodButton;
		private DesignerMethod _selectedMethod;
		private readonly Dictionary<DesignerMethod, VisualElement> _methods = new ();

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
			
			VisualElement methodsRoot = _root.Q<VisualElement>("MethodsGroup");
			InitializeButton("MethodsAddButton", () => AddMethod(methodsRoot, new DesignerMethod($"Method{_methods.Count}", "void")));
			InitializeButton("MethodsRemoveButton", () => RemoveMethod(methodsRoot, _selectedMethod));
		}

		private void InitializeListView<TElementType, TWidgetType>(VisualTreeAsset elementUXML, string listElementName, float height = 20.0f, Action<TWidgetType> initAction = null) 
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
				initAction?.Invoke(w);
			};
			
			elementsRoot.fixedItemHeight = height;
			elementsRoot.style.flexDirection = FlexDirection.Column;
			elementsRoot.style.flexGrow = 1.0f;

			elementsRoot.selectionType = SelectionType.Multiple;

			elementsRoot.onItemsChosen += Debug.Log;
			elementsRoot.onSelectionChange += Debug.Log;
		}
		
		private void InitializeButton(string buttonName, Action onClick)
		{
			var button = _root.Q<Button>(buttonName);
			button.clickable.clicked += onClick;
		}

		private void AddMethod(VisualElement root, DesignerMethod method)
		{
			MethodWidget methodWidget = new MethodWidget(method.Type, method.Name);
			root.Insert(root.childCount - 1, methodWidget);
			
			methodWidget.RegisterValueChangedCallback(_ => { _selectedMethod = method; });
			methodWidget.Parameters = method.Parameters;
			_methods.Add(method, methodWidget);
			_selectedMethod = method;
		}

		private void RemoveMethod(VisualElement root, DesignerMethod method)
		{
			if (_methods.TryGetValue(method, out var widget))
			{
				root.Remove(widget);
				_methods.Remove(method);
				_selectedMethod = _methods.Keys.Count > 0 ? _methods.Keys.Last() : null;
			}
		}
	}
}