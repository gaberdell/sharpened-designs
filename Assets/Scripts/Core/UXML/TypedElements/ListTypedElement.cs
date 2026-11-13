using System;
using System.Collections;
using UnityEngine.UIElements;

namespace Sharp.Core.UXML.TypedElements
{
    public class ListTypedElement : AbstractTypedElement
    {
        public new ListView visual_element => base.visual_element as ListView;

        public ListTypedElement(ListView visual_element)
            : base(visual_element)
        {
            visual_element.makeItem = () => new GenericValueVisualElement();
            visual_element.bindItem = (element, index) =>
            {
                var generic_element = element as GenericValueVisualElement;
                var typed_element = generic_element.typed_element;

                var value = visual_element.itemsSource[index];

                typed_element.TrySetValue(value);
            };
        }

        public override bool Supports(Type value_type)
        {
            return typeof(IList).IsAssignableFrom(value_type);
        }

        protected override void SetValue(object value, Type value_type)
        {
            visual_element.itemsSource = value as IList;
        }

        protected override void UpdateType(Type value_type)
        {
            // Nothing to do here; we only care about the concrete type once
            // we get to binding an actual value.
        }
    }
}
