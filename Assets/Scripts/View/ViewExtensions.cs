using System;
using System.Collections;
using pindwin.umvr.Command;
using pindwin.umvr.Model;
using pindwin.umvr.View.Binding;
using UniRx;

namespace pindwin.umvr.View
{
    public static class ViewExtensions
    {
        public static IDisposable BindProperty<TPropertyType, TModel>(this IView view, TModel model, string label)
            where TModel : class, IModel
        {
            BindingHandlerDescriptionFlags flags = model.GetProperty(label).GetDescriptor();
            return model.GetProperty<TPropertyType>(label).Subscribe(view.Bind(model, label, flags).FeedToView);
        }
        
        public static IDisposable AutoBindProperty<TProperty>(
            this IView view, 
            IModel model, 
            TProperty property, 
            IConditionalCommand<string> feedbackCommand)
            where TProperty : Property
        {
            return new AutoBinding(
                property, 
                view.Bind(model, property.Label, property.GetDescriptor()), 
                feedbackCommand);
        }

        public static IDisposable BindCollection<TItem>(
            this IView view,
            IModel model,
            CollectionProperty<TItem> collection)
        {
            CompositeDisposable disposable = new CompositeDisposable();
            BindingHandlerDescriptionFlags flags = collection.GetDescriptor();
            
            disposable.Add(collection.Collection.ObserveAdd().Subscribe(add =>
            {
                CollectionEvent ev = CollectionEvent.Add(add.Index, add.Value.ToString());
                view.Bind(model, collection.Label, flags).FeedToView(ev);
            }));
            disposable.Add(collection.Collection.ObserveRemove().Subscribe(remove =>
            {
                CollectionEvent ev = CollectionEvent.Remove(remove.Index);
                view.Bind(model, collection.Label, flags).FeedToView(ev);
            }));
            disposable.Add(collection.Collection.ObserveReplace().Subscribe(replace =>
            {
                CollectionEvent ev = CollectionEvent.Replace(replace.Index, replace.NewValue.ToString());
                view.Bind(model, collection.Label, flags).FeedToView(ev);
            }));
            disposable.Add(collection.Collection.ObserveReset().Subscribe(_ =>
            {
                view.Bind(model, collection.Label, flags).FeedToView(CollectionEvent.Clear);
            }));
            return disposable;
        }
        
        public static IDisposable AutoBindCollection<TEnumerableProperty>(
            this IView view, 
            IModel model, 
            TEnumerableProperty collection, 
            IConditionalCommand<string> feedbackCommand)
            where TEnumerableProperty : Property, IEnumerable
        {
            return new CollectionAutoBinding(
                collection, 
                view.Bind(model, collection.Label, collection.GetDescriptor()), 
                feedbackCommand);
        }
    }
}