using Sharp.Core.Types;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Variables
{
    public class ObjectVariablesTab : ObjectSpecificTab
    {
        private ListView _variable_list;
        private Label _select_component_label;

        private List<VariableEntry> _variable_entries = new();

        public override void OnSelectedObjectChanged()
        {
            _variable_entries.Clear();

            if (selected_object is Component component && component)
            {
                var type = selected_object.GetType();
                var type_info = TypeCache.Instance.Get(type);

                foreach (var field in type_info.instance_fields)
                {
                    // Skip past backing fields for properties and other compiler generated fields //
                    var should_skip = false;
                    foreach(var attribute in field.attributes)
                    {
                        if(attribute is CompilerGeneratedAttribute)
                        {
                            should_skip = true;
                            break;
                        }
                    }

                    if(should_skip)
                    {
                        continue;
                    }

                    _variable_entries.Add(new VariableEntry
                    {
                        member = field.member,
                        target = selected_object
                    });
                }

                foreach (var property in type_info.instance_properties)
                {
                    // Skip setter only fields //
                    if(!property.member.CanRead)
                    {
                        continue;
                    }

                    // Skip properties defined on the base component type (often stuff like 'camera' or 'transform', which are undesirable) //
                    if(property.member.DeclaringType == typeof(Component) 
                        || property.member.DeclaringType == typeof(UnityEngine.Object)
                        || property.member.DeclaringType == typeof(Behaviour)
                        || property.member.DeclaringType == typeof(MonoBehaviour))
                    {
                        continue;
                    }

                    _variable_entries.Add(new VariableEntry
                    {
                        member = property.member,
                        target = selected_object
                    });
                }

                _select_component_label.style.display = DisplayStyle.None;
                _variable_list.style.display = DisplayStyle.Flex;
            }
            else
            {
                _select_component_label.style.display = DisplayStyle.Flex;
                _variable_list.style.display = DisplayStyle.None;
            }

            _variable_list.Rebuild();
        }

        protected override void OnInstantiateVisualElement()
        {
            _variable_list = root_ui_element.Query<ListView>();
            _select_component_label = root_ui_element.Query<Label>();

            _variable_list.itemsSource = _variable_entries;
            _variable_list.makeItem = () => new VariableElement();
            _variable_list.bindItem = (e, index) =>
            {
                var element = e as VariableElement;
                element.BindEntry(_variable_entries[index]);
            };
        }
    }
}
