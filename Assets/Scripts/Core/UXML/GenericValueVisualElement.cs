using Sharp.Core.UXML.TypedElements;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Core.UXML
{
    /// <summary>
    /// The UXML element that is used to render information about a single entry
    /// in the console view. Structure is elemented by 'UI Toolkit/Tabs/Console/Resources/ConsoleEntry.uxml'.
    /// </summary>
    [UxmlElement]
    public partial class GenericValueVisualElement : VisualElement
    {
        private readonly VisualElement _unsupported;
        private readonly AbstractTypedElement[] _contained_typed_elements;

        public TypedElement typed_element { get; }

        public GenericValueVisualElement()
        {
            var template = Resources.Load<VisualTreeAsset>("GenericValue");
            template.CloneTree(this);

            _unsupported = this.Q("Unsupported");
            _contained_typed_elements = new AbstractTypedElement[]
            {
                new ReflectiveTypedElement(this.Q<TextField>("String")),
                new UnityObjectTypedElement(this.Q("UObject")),
                new EnumTypedElement(this.Q<DropdownField>("Dropdown")),
                new ReflectiveTypedElement(this.Q<Toggle>("Bool")),
                new ReflectiveTypedElement(this.Q<IntegerField>("Int")),
                new ReflectiveTypedElement(this.Q<LongField>("Long")),
                new ReflectiveTypedElement(this.Q<UnsignedIntegerField>("UInt")),
                new ReflectiveTypedElement(this.Q<UnsignedLongField>("ULong")),
                new ReflectiveTypedElement(this.Q<FloatField>("Float")),
                new ReflectiveTypedElement(this.Q<DoubleField>("Double")),
                new ReflectiveTypedElement(this.Q<Vector2Field>("Vector2")),
                new ReflectiveTypedElement(this.Q<Vector3Field>("Vector3")),
                new ReflectiveTypedElement(this.Q<Vector4Field>("Vector4")),
                new ReflectiveTypedElement(this.Q<Vector2IntField>("Vector2Int")),
                new ReflectiveTypedElement(this.Q<Vector3IntField>("Vector3Int")),
                new ListTypedElement(this.Q<ListView>("List")),
            };

            typed_element = new TypedElement(this);
        }

        public void SetValue(object value)
        {
            typed_element.TrySetValue(value);
        }

        public class TypedElement : AbstractTypedElement
        {
            private AbstractTypedElement _active_element;

            public new GenericValueVisualElement visual_element => base.visual_element as GenericValueVisualElement;

            public TypedElement(GenericValueVisualElement visual_element)
                : base(visual_element)
            { }

            public override bool Supports(Type value_type)
            {
                return true;
            }

            protected override void SetValue(object value, Type value_type)
            {
                _active_element.TrySetValue(value, value_type);
            }

            protected override void UpdateType(Type value_type)
            {
                // Find the first widget that supports the provided value type
                _active_element = null;

                foreach (var element in visual_element._contained_typed_elements)
                {
                    if (element.Supports(value_type))
                    {
                        _active_element = element;
                        break;
                    }
                }

                foreach (var element in visual_element._contained_typed_elements)
                {
                    if(element != _active_element)
                    {
                        element.visual_element.style.display = DisplayStyle.None;
                    }
                }

                // Show the 'unsupported' widget if no other widgets can display the type
                if (_active_element == null)
                {
                    visual_element._unsupported.style.display = DisplayStyle.Flex;
                }
                else
                {
                    visual_element._unsupported.style.display = DisplayStyle.None;
                }
            }
        }
    }
}
