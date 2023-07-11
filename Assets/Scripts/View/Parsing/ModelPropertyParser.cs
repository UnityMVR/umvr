using System;
using pindwin.development;
using pindwin.umvr.Exceptions;
using pindwin.umvr.Model;
using pindwin.umvr.Repository;
using UnityEngine;

namespace pindwin.umvr.View.Parsing
{
	public sealed class ModelPropertyParser<TModel> : PropertyParser
		where TModel : class, IModel
	{
		private readonly IRepository<TModel> _modelRepository;
		public override Type Type => typeof(TModel);

		public ModelPropertyParser(IRepository<TModel> modelRepository)
		{
			_modelRepository = modelRepository.AssertNotNull();
		}

		public override bool IsValid(string payload)
		{
			if (IsNullPayload(payload))
			{
				return true;
			}
			
			try
			{
				Id id = ExtractId(payload);
				return _modelRepository.Get(id) != null;
			}
			catch (UMVRParsingException)
			{
				return false;
			}
		}

		protected override bool TryParse<TProperty>(string value, TProperty property)
		{
			if (property is IValueContainer<TModel> container)
			{
				if (IsNullPayload(value))
				{
					container.Value = null;
					return true;
				}
				
				TModel model = _modelRepository.Get(ExtractId(value));
				if (model == null)
				{
					return false;
				}

				container.Value = model;
				return true;
			}

			return false;
		}

		private static Id ExtractId(string modelString)
		{
			try
			{
				string[] parts = modelString.Split('#');
				return Id.Parse(parts[1]);
			}
			catch
			{
				Debug.LogError($"Failed to parse {modelString} to {nameof(Id)}");
				return Id.UNKNOWN;
			}
		}

		private static bool IsNullPayload(string payload) => string.IsNullOrEmpty(payload) || payload == "null";
	}
}