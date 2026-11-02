using System;
using System.Reflection;
using UnityEngine.UIElements;

namespace Sharp.Core.UXML.TypedElements
{
    public class ReflectiveTypedElement : AbstractTypedElement
    {
        private readonly PropertyInfo _value_property;
        private readonly Type _value_type;

        public ReflectiveTypedElement(VisualElement inner_element, string property_name = "value")
            : base(inner_element)
        {
            _value_property = inner_element.GetType().GetProperty(property_name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) 
                ?? throw new InvalidOperationException($"Type {inner_element.GetType()} does not contain a property '{property_name}'");
            _value_type = _value_property.PropertyType;
        }

        public override void SetValue(object value)
        {
            // Will throw on type mismatch.
            _value_property.SetValue(visual_element, value);
        }

        public override bool Supports(Type value_type)
        {
            return _value_type == value_type;
        }

        internal override void SetType(Type value_type)
        {
            // Nothing to do; we assume that the visual_element already takes
            // care of initializing data for its associated type
        }
    }
}
