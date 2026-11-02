using System;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Core.UXML.TypedElements
{
    public class UnityObjectTypedElement : AbstractTypedElement
    {
        private readonly TextField _name_field;

        public UnityObjectTypedElement(VisualElement visual_element)
            : base(visual_element)
        {
            _name_field = visual_element.Q<TextField>("UObject-Name");
        }

        private static string GameObjectToString(GameObject game_object)
        {
            var result = new StringBuilder();

            var object_iter = game_object;
            while(object_iter)
            {
                result.Insert(0, $"/{object_iter.name}");
                var parent_transform = object_iter.transform.parent;

                object_iter = parent_transform ? parent_transform.gameObject : null;
            }

            result.Insert(0, $"Scene {game_object.scene.name}");
            result.Insert(0, $"{game_object.name}: ");

            return result.ToString();
        }

        public override void SetValue(object value)
        {
            var text_value = "";

            if(value is GameObject game_object)
            {
                text_value = GameObjectToString(game_object);
            }
            else if(value is Component component)
            {
                text_value = GameObjectToString(component.gameObject);
            }
            else if(value is UnityEngine.Object uobject)
            {
                text_value = uobject.name;
            }

            var value_type = value.GetType();
            _name_field.value = $"({value_type.Name}) {text_value}";
        }

        public override bool Supports(Type value_type)
        {
            return typeof(UnityEngine.Object).IsAssignableFrom(value_type);
        }

        internal override void SetType(Type value_type)
        {
            // Nothing to do here; we only care about the concrete type once
            // we get to binding an actual value.
        }
    }
}
