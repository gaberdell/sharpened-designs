using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Sharp.Tabs.Hierarchy;
using Unity.VisualScripting;


namespace Sharp.Tabs.ObjectButton
{
    public class ObjectButtonTab : ObjectSpecificTab {
        private Label _selected_object_label;
        private VisualElement _scroll_view_content;


        private List<ObjectButtonComponent> _call_method_buttons = new();

        public override void OnSelectedObjectChanged() {
            foreach (var cMButton in _call_method_buttons) {
                _scroll_view_content.Remove(cMButton);
            }
            _call_method_buttons.Clear();

            _selected_object_label.text = selected_object.name;
            MethodInfo[] mInfoArray = selected_object.GetType().GetMethods();

            foreach (var mi in mInfoArray) {
                var sBA = Attribute.GetCustomAttribute(mi, typeof(SharpButtonAttribute));
                if (sBA != null) {
                    var newAddButton = new ObjectButtonComponent();
                    newAddButton.label.text = mi.Name;
                    newAddButton.button.RegisterCallback<ClickEvent>((evt) => { mi.Invoke(selected_object, null); });
                    _scroll_view_content.Add(newAddButton);
                    _call_method_buttons.Add(newAddButton);
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
