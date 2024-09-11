using System;

namespace pindwin.umvr.Attributes
{
    /// <summary>
    /// Marks property as generic property. Any field marked as generic property will be packed into
    /// SingleProperty/SingleModelProperty, regardless of its type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class GenericPropertyAttribute : Attribute
    {
        public GenericPropertyAttribute()
        { }
    }
}