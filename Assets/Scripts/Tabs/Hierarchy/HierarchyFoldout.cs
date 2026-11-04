using Sharp.Tabs.Console;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Hierarchy
{
    [UxmlElement]
    public partial class HierarchyFoldout : VisualElement {

        public Foldout hierarchyFoldout;
        public Label gameObjectName;
        public VisualElement foldoutContent;


        public HierarchyFoldout() 
        {
            var template = Resources.Load<VisualTreeAsset>("HierarchyFoldout");
            template.CloneTree(this);
            hierarchyFoldout = this.Q<VisualElement>().Q<Foldout>();
            gameObjectName = hierarchyFoldout.Q<Toggle>().Q<VisualElement>().Q<Label>();
            foldoutContent = hierarchyFoldout.Q<VisualElement>("unity-content");
        }
    }
}
