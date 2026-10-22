using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Console
{
    /// <summary>
    /// The UXML element that is used to render information about a single entry
    /// in the console view. Structure is elemented by 'UI Toolkit/Tabs/Console/Resources/ConsoleEntry.uxml'.
    /// </summary>
    [UxmlElement]
    public partial class ConsoleEntryElement : VisualElement
    {
        public Label message;
        public Label backtrace;
        public Image icon;

        public ConsoleEntryElement()
        {
            var template = Resources.Load<VisualTreeAsset>("ConsoleEntry");
            template.CloneTree(this);

            // TODO: helper extension method to assert non-null here to catch errors in the uxml early //
            message = this.Q<Label>("message");
            backtrace = this.Q<Label>("backtrace");
            icon = this.Q<Image>("icon");
        }
    }
}
