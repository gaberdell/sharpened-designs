using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Tabs.ObjectButton 
{
    [UxmlElement]
    public partial class ObjectButtonComponent : VisualElement 
    {
        public Label label;
        public Button button;

        public ObjectButtonComponent() 
        { 
            var template = Resources.Load<VisualTreeAsset>("ObjectButtonComponent");
            template.CloneTree(this);

            label = this.Q<Label>();
            button = this.Q<Button>();
        }
    }
}
