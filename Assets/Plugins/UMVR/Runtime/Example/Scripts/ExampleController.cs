using System.Collections;
using System.Collections.Generic;
using pindwin.development;
using pindwin.umvr.example;
using UnityEngine;
using Zenject;

namespace pindwin.umvr.Example.Example.Scripts
{
	public class ExampleController : IInitializable
	{
		private readonly FooFactory _factory;
		private IFoo _foo;
		private List<IFoo> _fooList = new List<IFoo>();

		public ExampleController(FooFactory factory)
		{
			_factory = factory.AssertNotNull();
		}
		
		public void Initialize()
		{
			_foo = _factory.Create(string.Empty, null);
			_foo.Text = "Hello World!";
			_foo.Collection.Add(1);
			_foo.Collection.Add(2);
			_foo.Collection.Add(3);
			_foo.Collection.Add(4);
			
			var foo2 = _factory.Create(string.Empty, null);
			foo2.Text = "Hello there!";
			
			var foo3 = _factory.Create(string.Empty, null);
			foo3.Text = "General Kenobi!";

			for (int i = 0; i < 20; i++)
			{
				var foo = _factory.Create(string.Empty, null);
				foo.Text = $"{i + 3} more Foo!";
				_fooList.Add(foo);
			}

			GameObject go = new GameObject("Coroutine");
			var cr = go.AddComponent<CoroutineRunner>();
			cr.StartCoroutine(UpdateFoo());
		}

		private IEnumerator UpdateFoo()
		{
			for (int i = 0; i < 10; i++)
			{
				yield return new WaitForSeconds(1);
				_foo.Text = i.ToString();
				_foo.Collection[1] = i;
				_foo.OtherFoo = _fooList[i + 1];
			}
		}

		private class CoroutineRunner : MonoBehaviour
		{ }
	}
}