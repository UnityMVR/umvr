using System.Collections.Generic;
using pindwin.development;
using pindwin.umvr.Model;
using pindwin.umvr.View.Binding;
using pindwin.umvr.View.Widgets;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace pindwin.umvr.View
{
	[RequireComponent(typeof(UIDocument))]
	public class DebuggerView : MonoBehaviour, IView
	{
		private UIDocument _document;
		private ModelDropdownLabelsProvider _labelsDropdownProvider;
		private GenericModelFactory _modelFactory;

		private readonly Dictionary<string, IBindingHandler> _properties = new();
		private readonly Dictionary<string, DebuggerEntryWidget> _models = new();
		private readonly Dictionary<string, DebuggerEntryWidget> _types = new();
		private VisualElement Root => _document.rootVisualElement.Q("unity-content-container");

		[Inject]
		private void Inject(UIDocument uiDocument, ModelDropdownLabelsProvider labelsDropdownProvider, GenericModelFactory modelFactory)
		{
			_document = uiDocument.AssertNotNull();
			_labelsDropdownProvider = labelsDropdownProvider.AssertNotNull();
			_modelFactory = modelFactory.AssertNotNull();
		}

		public IBindingHandler Bind(IModel model, string label, BindingHandlerDescriptionFlags descriptionFlags = BindingHandlerDescriptionFlags.None)
		{
			string id = $"{model.Id.ToString()}_{label.ToLower()}";
			
			if (_properties.TryGetValue(id, out IBindingHandler value))
			{
				return value;
			}

			return AddWidget(model, id, label, descriptionFlags);
		}

		private IBindingHandler AddWidget(IModel model, string propertyId, string propertyName, BindingHandlerDescriptionFlags descriptionFlags)
		{
			DebuggerEntryWidget root = EnsureModelRootFor(model);
			IBindingHandler handler;
			VisualElement uiLabel;
			if (descriptionFlags.HasFlag(BindingHandlerDescriptionFlags.Collection))
			{
				uiLabel = AddCollectionWidget(model, propertyName, descriptionFlags.HasFlag(BindingHandlerDescriptionFlags.Model), out handler);
			}
			else if (descriptionFlags.HasFlag(BindingHandlerDescriptionFlags.Model))
			{
				uiLabel = AddDropdownWidget(model, propertyId, propertyName, out handler);
			}
			else
			{
				uiLabel = AddTextFieldWidget(propertyId, propertyName, out handler);
			}
			root.Add(uiLabel);
			root.MarkDirtyRepaint();
			_properties[propertyId] = handler;
			return handler;
		}

		private static TextFieldWidget AddTextFieldWidget(string propertyId, string propertyName, out IBindingHandler result)
		{
			TextFieldWidget singleWidget = new TextFieldWidget()
			{
				name = propertyId,
				Label = propertyName
			};
			result = new SinglePropertyBindingHandler(singleWidget);
			singleWidget.ValueChanged += result.FeedBackFromView;
			return singleWidget;
		}

		private DropdownWidget AddDropdownWidget(
			IModel model,
			string propertyId,
			string propertyName,
			out IBindingHandler result)
		{
			DropdownWidget modelWidget = new DropdownWidget(_labelsDropdownProvider.GetLabelsForType(model.GetType()))
			{
				name = propertyId,
				Label = propertyName
			};
			result = new SinglePropertyBindingHandler(modelWidget);
			modelWidget.ValueChanged += result.FeedBackFromView;
			return modelWidget;
		}

		private CollectionWidget AddCollectionWidget(
			IModel model,
			string propertyName,
			bool isModel,
			out IBindingHandler handler)
		{
			CollectionWidget collectionWidget = isModel
				? new DropdownsCollectionWidget(_labelsDropdownProvider.GetLabelsForType(model.GetType()))
				: new TextFieldsCollectionWidget();
			collectionWidget.Label = propertyName;

			handler = new CollectionPropertyBindingHandler(collectionWidget);
			collectionWidget.ValueChanged += handler.FeedBackFromView;
			collectionWidget.ValueProposed += handler.Validate;
			return collectionWidget;
		}

		private DebuggerEntryWidget EnsureModelRootFor(IModel model)
		{
			if (_models.TryGetValue(model.ToString(), out DebuggerEntryWidget element))
			{
				return element;
			}
			
			string modelId = model.ToString();
			var modelWidget = new DebuggerEntryWidget(modelId, false, DebuggerElement.DELETE_BUTTON, model.Dispose);
			_models[modelId] = modelWidget;
			
			DebuggerEntryWidget root = EnsureTypeRootFor(model);
			root.Add(modelWidget);
			root.MarkDirtyRepaint();
			model.Disposing += _ => root.Remove(modelWidget);
			
			return modelWidget;
		}

		private DebuggerEntryWidget EnsureTypeRootFor(IModel model)
		{
			if (_types.TryGetValue(model.GetType().ToString(), out DebuggerEntryWidget element))
			{
				return element;
			}

			string typeId = model.GetType().ToString();
			var widget = new DebuggerEntryWidget(
				typeId, false, 
				DebuggerElement.ADD_BUTTON,	 
				() => _modelFactory.CreateModel(model.GetType()));
			_types[typeId] = widget;
			
			Root.Add(widget);
			Root.MarkDirtyRepaint();
			return widget;
		}
	}
}