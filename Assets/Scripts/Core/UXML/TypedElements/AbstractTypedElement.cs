using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Core.UXML.TypedElements
{
    public abstract class AbstractTypedElement
    {
        public VisualElement visual_element { get; }
        private Type last_set_value_type;
        protected object last_set_value { get; private set; }

        public AbstractTypedElement(VisualElement visual_element)
        {
            this.visual_element = visual_element;
        }

        public bool TrySetValue(object value, Type value_type = null)
        {
            value_type ??= value?.GetType();
            var shown = Supports(value_type);

            // Optimization; only update value if it has changed //
            if (object.Equals(last_set_value, value))
            {
                return shown;
            }

            if (shown)
            {
                // Optimization; only update style if needed //
                if (visual_element.style.display != DisplayStyle.Flex)
                {
                    visual_element.style.display = DisplayStyle.Flex;
                }

                if(last_set_value_type != value_type)
                {
                    last_set_value_type = value_type;
                    UpdateType(value_type);
                }

                SetValue(value, value_type);

                last_set_value_type = value_type;
            }
            else
            {
                // Optimization; only update style if needed //
                if (visual_element.style.display != DisplayStyle.None)
                {
                    visual_element.style.display = DisplayStyle.None;
                }
            }

            last_set_value = value;
            return shown;
        }

        public abstract bool Supports(Type value_type);
        protected abstract void SetValue(object value, Type value_type);
        protected abstract void UpdateType(Type new_value_type);
    }
}
