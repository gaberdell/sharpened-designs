using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Hierarchy {
    [UxmlElement]
    public partial class HierarchyComponentButton : VisualElement {

        public Button button;

        public HierarchyComponentButton() { 
            var template = Resources.Load<VisualTreeAsset>("HierarchyComponentButton");
            template.CloneTree(this);
            button = this.Q<Button>();
        }
    }
}
