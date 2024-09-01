using System.Collections;
using pindwin.umvr.Attributes;
using pindwin.umvr.Command;

namespace GenerationParams
{
	public class Property : Member
	{
		public bool IsReadonly { get; set; }
		public bool IsCollection { get; set; }
		public bool IsCommand { get; set; }
		public bool IsModel { get; set; }
		public bool CustomImplementation { get; set; }
		public bool InitializeExplicitly => InitializationLevel == InitializationLevel.Explicit;
		public bool DoNotInitialize => InitializationLevel == InitializationLevel.Skip;
		public bool CascadeDisposeUpstream => (CascadeDirection & CascadeDirection.Upstream) == CascadeDirection.Upstream;
		public bool CascadeDisposeDownstream => (CascadeDirection & CascadeDirection.Downstream) == CascadeDirection.Downstream;
		public InitializationLevel InitializationLevel { get; set; } 
		public CascadeDirection CascadeDirection { get; set; }
		public string GenericType { get; set; } 
		
		public string Signature
		{
			get
			{
				if (IsCollection)
				{
					return $"public {nameof(IList)}<{Type}> {Name}";
				}

				if (IsCommand)
				{
					return $"public {typeof(ICommand)}<{Type}> {Name}";
				}
				
				return $"public {Type} {Name}";
			}
		}

		public string FieldName => $"_{char.ToLower(Name[0])}{Name.Substring(1)}";
	}
}