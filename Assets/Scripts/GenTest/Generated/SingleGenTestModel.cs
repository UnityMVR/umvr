// <auto-generated>
//	 This code was generated by a tool.
//
//	 Changes to this file may cause incorrect behavior and will be lost if
//	 the code is regenerated.
// </auto-generated>

using pindwin.umvr.Model;
using System.Collections.Generic;
using UniRx;

// ReSharper disable once CheckNamespace
namespace GenTest
{
	public partial class SingleGenTestModel : Model<SingleGenTestModel>, ISingleGenTest
	{
		private SingleProperty<System.Int32> _prop0;
		public System.Int32 Prop0
		{
			get => _prop0.Value;
			set
			{
				_prop0.Value = value;
			}
		}

		private SingleProperty<System.Int32> _prop1;
		public System.Int32 Prop1
		{
			get => _prop1.Value;
			set
			{
				_prop1.Value = value;
			}
		}

		private SingleProperty<System.Int32> _prop2;
		public System.Int32 Prop2
		{
			get => _prop2.Value;
			set
			{
				_prop2.Value = value;
			}
		}

		private ModelSingleProperty<pindwin.umvr.example.IFoo> _prop6;
		public pindwin.umvr.example.IFoo Prop6
		{
			get => _prop6.Value;
			set
			{
				_prop6.Value = value;
			}
		}

		private ModelSingleProperty<pindwin.umvr.example.IFoo> _prop7;
		public pindwin.umvr.example.IFoo Prop7
		{
			get => _prop7.Value;
			set
			{
				_prop7.Value = value;
			}
		}

		private ModelSingleProperty<pindwin.umvr.example.IFoo> _prop8;
		public pindwin.umvr.example.IFoo Prop8
		{
			get => _prop8.Value;
			set
			{
				_prop8.Value = value;
			}
		}


		public SingleGenTestModel(pindwin.umvr.Model.Id id, System.Int32 prop1, System.Int32 prop4, pindwin.umvr.example.IFoo prop7, pindwin.umvr.example.IFoo prop10) : base(id)
		{
			_prop0 = new SingleProperty<System.Int32>(nameof(Prop0));
			Prop0 = default;

			_prop1 = new SingleProperty<System.Int32>(nameof(Prop1));
			Prop1 = prop1;

			_prop2 = new SingleProperty<System.Int32>(nameof(Prop2));
			Prop2 = default;

			Prop3 = default;

			Prop4 = prop4;

			// Prop5 not initialized because of custom implementation & do not initialize flag

			_prop6 = new ModelSingleProperty<pindwin.umvr.example.IFoo>(nameof(Prop6));
			Prop6 = default;

			_prop7 = new ModelSingleProperty<pindwin.umvr.example.IFoo>(nameof(Prop7));
			Prop7 = prop7;

			_prop8 = new ModelSingleProperty<pindwin.umvr.example.IFoo>(nameof(Prop8));
			Prop8 = default;

			Prop9 = default;

			Prop10 = prop10;

			// Prop11 not initialized because of custom implementation & do not initialize flag

			RegisterDataStreams(this);
		}
	}
}