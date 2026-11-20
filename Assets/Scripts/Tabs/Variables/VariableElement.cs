using Sharp.Core.UXML;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Variables
{
    [UxmlElement]
    public partial class VariableElement : VisualElement
    {
        private Label label;
        private GenericValueVisualElement value;

        public VariableEntry entry { get; private set; }

        public void BindEntry(VariableEntry entry)
        {
            this.entry = entry;

            label.text = entry.GetVariableName();
        }

        private void UpdateValue()
        {
            if(entry.target != null)
            {
                value.SetValue(entry.GetValue());
            }
        }

        public VariableElement()
        {
            var template = Resources.Load<VisualTreeAsset>("VariableElement");
            template.CloneTree(this);

            label = this.Q<Label>();
            value = this.Q<GenericValueVisualElement>();

            schedule.Execute(UpdateValue).Every(0);
        }
    }
}
