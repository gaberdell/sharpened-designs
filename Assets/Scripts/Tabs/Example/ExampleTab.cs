using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Example
{
    public class ExampleTab : AbstractTab
    {
        public int number;
        public Label number_label;

        protected override void OnInstantiateVisualElement()
        {
            number_label = root_ui_element.Q<Label>("ExampleNumber");
            root_ui_element.Q<Button>("Increment").RegisterCallback<ClickEvent>(ClickIncrement);
        }

        private void ClickIncrement(ClickEvent evt)
        {
            number++;
        }

        private void Update()
        {
            number_label.text = $"number = {number}";
        }
    }
}
