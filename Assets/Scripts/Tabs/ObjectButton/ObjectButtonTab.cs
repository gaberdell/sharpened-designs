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
    public class ObjectButtonTab : AbstractTab 
    {
        private HierarchyTab _hierarchy_tab;
        private Label _selected_object_label;
        private VisualElement _scroll_view_content;


        private List<ObjectButtonComponent> _call_method_buttons = new();



        private void BindNewMethod() {
            foreach (var cMButton in _call_method_buttons) {
                _scroll_view_content.Remove(cMButton);
            }
            _call_method_buttons.Clear();

            var selectedObject = _hierarchy_tab.selected_object;
            _selected_object_label.text = selectedObject.name;
            MethodInfo[] mInfoArray = selectedObject.GetType().GetMethods();

            foreach (var mi in mInfoArray) {
                var newAddButton = new ObjectButtonComponent();
                newAddButton.label.text = mi.Name;
                newAddButton.button.RegisterCallback<ClickEvent>((evt) => { mi.Invoke(selectedObject, null); });
                _call_method_buttons.Add(newAddButton);
            }


            
        }

        protected override void OnInstantiateVisualElement() 
        {
            _selected_object_label = root_ui_element.Q<Label>("SelectedObjectName");
            _scroll_view_content = root_ui_element.Q<VisualElement>("CallMethodButton");

            _hierarchy_tab = FindFirstObjectByType<HierarchyTab>();

        }

    }
}
