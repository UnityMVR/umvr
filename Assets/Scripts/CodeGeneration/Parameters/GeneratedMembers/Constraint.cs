using System;

namespace GenerationParams
{
	public class Constraint : Token
	{
		public TypeParametersCollection Constraints { get; }

		public Constraint(string name, TypeParametersCollection constraints) : base(name)
		{
			Constraints = constraints;
		}
		
		public string ToSingleConstraintString()
		{
			return $"where {Name} : {Constraints.ToTypeParametersString()}";
		}
	}
}