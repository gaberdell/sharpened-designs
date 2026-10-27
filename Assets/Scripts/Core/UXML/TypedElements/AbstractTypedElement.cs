using System;
using UnityEngine.UIElements;

namespace Sharp.Core.UXML.TypedElements
{
    public abstract class AbstractTypedElement
    {
        public VisualElement visual_element { get; }

        public AbstractTypedElement(VisualElement visual_element)
        {
            this.visual_element = visual_element;
        }

        public abstract void SetValue(object value);
        public abstract bool Supports(Type value_type);
        internal abstract void SetType(Type value_type);

        public bool InitializeForType(Type value_type)
        {
            var shown = Supports(value_type);

            if (shown)
            {
                visual_element.style.display = DisplayStyle.Flex;
                SetType(value_type);
            }
            else
            {
                visual_element.style.display = DisplayStyle.None;
            }

            return shown;
        }
    }
}
