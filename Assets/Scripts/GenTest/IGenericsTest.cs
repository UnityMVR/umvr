using System.Collections.Generic;
using pindwin.umvr.Attributes;
using pindwin.umvr.example;
using pindwin.umvr.Model;

namespace GenTest
{
    public interface IGenericsTest : IModel
    {
        int? NullableInt { get; set; }
        [GenericProperty] int? NullableIntProperty { get; set; }
        
        HashSet<IFoo> SingleParam { get; set;}
        Dictionary<int, IFoo> MultipleParams { get; set;}
        IDictionary<int, IFoo> MultipleParamsInterface { get; set;}
        
        IList<int> Collection { get; set; }
        [GenericProperty] IList<int> NotCollection { get; set; }
    }
}