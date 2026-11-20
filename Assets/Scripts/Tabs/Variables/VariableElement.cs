using Sharp.Core.UXML;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Variables
{
    [UxmlElement]
    public partial class VariableElement : VisualElement
    {
        /// <summary>
        /// The variable label. Will be bound to <see cref="entry"/>.GetVariableName()
        /// </summary>
        private Label label;
        /// <summary>
        /// The variable value. Will be bound to <see cref="entry"/>.GetValue()
        /// </summary>
        private GenericValueVisualElement value;

        /// <summary>
        /// The variable entry that this element corresponds to. Updates to the underlying variable
        /// will automatically update the value of this widget.
        /// </summary>
        public VariableEntry entry { get; private set; }

        /// <summary>
        /// Set the entry that this element is bound to.
        /// </summary>
        public void BindEntry(VariableEntry entry)
        {
            this.entry = entry;

            label.text = entry.GetVariableName();
        }

        /// <summary>
        /// Called each frame in order to make sure that the variable's value is up-to-date.
        /// </summary>
        private void UpdateValue()
        {
            if(entry.target != null)
            {
                value.SetValue(entry.GetValue(), entry.GetMemberType());
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
