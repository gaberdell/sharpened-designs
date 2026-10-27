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

            while(game_object)
            {
                result.Insert(0, $"/{game_object.name}");
                game_object = game_object.transform.parent.gameObject;
            }

            result.Insert(0, $"Scene {game_object.scene.name}");

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
