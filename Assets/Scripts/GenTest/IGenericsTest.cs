using System.Collections.Generic;
using pindwin.umvr.example;
using pindwin.umvr.Model;

namespace GenTest
{
    public interface IGenericsTest : IModel
    {
        int? NullableInt { get; set; }
        HashSet<int> SingleParam { get; set; }
        Dictionary<int, IFoo> MultipleParams { get; set; }
        IDictionary<int, IFoo> MultipleParamsInterface { get; set; }
    }
}