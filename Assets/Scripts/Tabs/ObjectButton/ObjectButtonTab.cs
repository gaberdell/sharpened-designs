using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Sharp.Tabs.Hierarchy;
using Unity.VisualScripting;
using Sharp.Core.Types;

namespace Sharp.Tabs.ObjectButton
{
    public class ObjectButtonTab : ObjectSpecificTab 
    {
        private Label _selected_object_label;
        private VisualElement _scroll_view_content;

        private readonly List<ObjectButtonComponent> _buttons = new();

        public override void OnSelectedObjectChanged() 
        {
            foreach (var button in _buttons) 
            {
                _scroll_view_content.Remove(button);
            }
            _buttons.Clear();

            if(!selected_object)
            {
                return;
            }

            _selected_object_label.text = selected_object.name;

            var selected_type = selected_object.GetType();
            var selected_type_info = TypeCache.Instance.Get(selected_type);

            foreach (var method_info in selected_type_info.instance_methods) 
            {
                var sharp_button_attribute = null as SharpButtonAttribute;
                
                foreach(var attribute in method_info.attributes)
                {
                    if(attribute is SharpButtonAttribute sba)
                    {
                        sharp_button_attribute = sba;
                        break;
                    }
                }

                if (sharp_button_attribute != null) 
                {
                    var new_button = new ObjectButtonComponent();
                    new_button.label.text = sharp_button_attribute.name_override ?? method_info.member.Name;
                    new_button.button.RegisterCallback<ClickEvent>((evt) => 
                    { 
                        method_info.member.Invoke(selected_object, null); 
                    });

                    _scroll_view_content.Add(new_button);
                    _buttons.Add(new_button);
                }
            }
        }

        protected override void OnInstantiateVisualElement() 
        {
            _selected_object_label = root_ui_element.Q<Label>("SelectedObjectName");
            _scroll_view_content = root_ui_element.Q<VisualElement>("unity-content-container");
        }
    }
}
