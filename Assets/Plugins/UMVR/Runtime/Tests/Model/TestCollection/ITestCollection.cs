using System.Collections.Generic;
using Model.TestModel;
using pindwin.umvr.Attributes;
using pindwin.umvr.Model;

namespace Model.TestCollection
{
	public interface ITestCollection : IModel
	{
		[CascadeDispose] IList<ITestModel> Collection { get; set; }
	}
}