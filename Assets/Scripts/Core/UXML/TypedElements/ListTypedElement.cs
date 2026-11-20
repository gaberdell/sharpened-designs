using System;
using System.Collections;
using System.Linq;
using UnityEngine.UIElements;

namespace Sharp.Core.UXML.TypedElements
{
    public class ListTypedElement : AbstractTypedElement
    {
        public new ListView visual_element => base.visual_element as ListView;

        /// <summary>
        /// A hash of the contents of the value that was previously assigned into this typed element.
        /// Used to prevent a full rebuild of the list UI every time its value is assigned, in the common
        /// case that the list's contents remained the same frame over frame.
        /// </summary>
        private int _previous_sequence_hash;

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
            // Optimization; do not refresh list items unless the contents of the list has changed //
            var new_sequence_hash = 0;
            var value_list = value as IList;
            if (value_list != null)
            {
                foreach (var item in value_list)
                {
                    new_sequence_hash = HashCode.Combine(new_sequence_hash, item.GetHashCode());
                }
            }

            if (_previous_sequence_hash != new_sequence_hash)
            {
                if (value_list != null)
                {
                    visual_element.itemsSource = value_list;
                }
                else
                {
                    visual_element.itemsSource = Array.Empty<object>();
                }

                visual_element.Rebuild();
            }

            _previous_sequence_hash = new_sequence_hash;
        }

        protected override void UpdateType(Type value_type)
        {
            // Nothing to do here; we only care about the concrete type once
            // we get to binding an actual value.
        }
    }
}
