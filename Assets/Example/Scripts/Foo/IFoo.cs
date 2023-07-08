using pindwin.umvr.Attributes;
using pindwin.umvr.Model;

namespace pindwin.umvr.example
{
    [Singleton]
    public interface IFoo : IModel
    {
        [CustomImplementation] string Text { get; set; }
        [CustomImplementation][Initialization(InitializationLevel.Skip)] string Text2 { get; }
        [CascadeDispose(CascadeDirection.Both), Initialization(InitializationLevel.Explicit)] IFoo OtherFoo { get; set; }
    }
}
