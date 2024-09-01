using System.Collections.Generic;
using pindwin.umvr.Attributes;
using pindwin.umvr.Model;

namespace pindwin.umvr.example
{
    public interface IFoo : IModel
    {
        [CustomImplementation] string Text { get; set; }
        [CustomImplementation][Initialization(InitializationLevel.Skip)] string Text2 { get; }
        [CascadeDispose(CascadeDirection.Both), Initialization(InitializationLevel.Explicit)] IFoo OtherFoo { get; set; }
        IList<int> Collection { get; set; }
        IList<IFoo> FooCollection { get; set; }
    }
}
