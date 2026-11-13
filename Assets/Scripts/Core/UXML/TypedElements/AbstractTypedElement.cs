using System;
using UnityEngine.UIElements;

namespace Sharp.Core.UXML.TypedElements
{
    public abstract class AbstractTypedElement
    {
        public VisualElement visual_element { get; }
        private Type last_set_value_type;

        public AbstractTypedElement(VisualElement visual_element)
        {
            this.visual_element = visual_element;
        }

        public bool TrySetValue(object value, Type value_type = null)
        {
            value_type ??= value?.GetType();
            var shown = Supports(value_type);

            if (shown)
            {
                visual_element.style.display = DisplayStyle.Flex;

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
                visual_element.style.display = DisplayStyle.None;
            }

            return shown;
        }

        public abstract bool Supports(Type value_type);
        protected abstract void SetValue(object value, Type value_type);
        protected abstract void UpdateType(Type new_value_type);
    }
}
