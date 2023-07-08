using System.Collections.Generic;
using Model.TestModel;
using pindwin.umvr.Model;

namespace Model.TestCollection
{
	public interface ITestCollection : IModel
	{
		IList<ITestModel> Collection { get; set; }
	}
}