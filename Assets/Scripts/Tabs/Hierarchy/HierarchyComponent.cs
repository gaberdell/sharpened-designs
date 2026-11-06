using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Hierarchy {
    [UxmlElement]
    public partial class HierarchyComponent : VisualElement {

        public Label label;

        public HierarchyComponent() { 
            var template = Resources.Load<VisualTreeAsset>("HierarchyComponent");
            template.CloneTree(this);
            label = this.Q<Label>();
        }
    }
}
