using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Sharp.Core.UXML.TypedElements
{
    public class EnumTypedElement : AbstractTypedElement
    {
        public const int UnknownIndex = 0;
        public new DropdownField visual_element => base.visual_element as DropdownField;

        private List<string> _enum_options;
        private List<int> _enum_values;

        public EnumTypedElement(DropdownField visual_element)
            : base(visual_element)
        {}

        public override bool Supports(Type value_type)
        {
            return value_type.IsEnum;
        }

        protected override void SetValue(object value, Type value_type)
        {
            var index = _enum_values.IndexOf(Convert.ToInt32(value));

            if (index == -1)
            {
                // Fallback to the 'unknown' value we inserted as option zero
                visual_element.index = UnknownIndex;
            }
            else
            {
                // Offset our found index by one to account for the unknown value at zero
                visual_element.index = index + 1;
            }
        }

        protected override void UpdateType(Type value_type)
        {
            _enum_options = Enum.GetNames(value_type).ToList();
            _enum_options.Insert(UnknownIndex, "<Unknown>");

            _enum_values = Enumerable.ToList(
                from e in Enum.GetValues(value_type) as object[] 
                select Convert.ToInt32(e));

            visual_element.choices = _enum_options;
            visual_element.index = UnknownIndex;
        }
    }
}
