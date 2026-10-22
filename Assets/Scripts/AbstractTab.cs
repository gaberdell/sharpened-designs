using Sharp.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp
{
    /// <summary>
    /// The base class to be associated with all tabs.
    /// </summary>
    public abstract class AbstractTab : BaseSharpBehaviour
    {
        [Header("UXML References")]
        [SerializeField] private VisualTreeAsset _content_uxml;

        [Header("Settings")]
        [SerializeField] private string _title;

        public VisualElement root_ui_element { get; private set; }
        public string title => _title;

        /// <summary>
        /// Callback used to set up the state of the root ui element, or to collect
        /// references to within the root ui element.
        /// </summary>
        protected abstract void OnInstantiateVisualElement();

        /// <summary>
        /// Create the VisualElement associated with this tab.
        /// Most only be called once.
        /// </summary>
        /// <returns>The newly created visual element.</returns>
        public VisualElement InstantiateVisualElement()
        {
            root_ui_element = _content_uxml.Instantiate();
            root_ui_element.dataSource = this;
            OnInstantiateVisualElement();
            return root_ui_element;
        }
    }
}
