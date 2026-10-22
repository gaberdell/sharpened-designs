using Sharp.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp
{
    /// <summary>
    /// The base class to be associated with all windows.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class BaseWindow : BaseSharpBehaviour
    {
        [Header("UXML References")]
        [SerializeField] private VisualTreeAsset _window_uxml;
        [SerializeField] private VisualTreeAsset _tab_uxml;

        /// <summary>
        /// The UIDocument component attached to this instance.
        /// </summary>
        public UIDocument ui_document { get; private set; }
        /// <summary>
        /// The UXML asset which defines the layout of some arbitrary window.
        /// </summary>
        public VisualTreeAsset window_uxml => _window_uxml;
        /// <summary>
        /// The UXML asset which defines the layour of some arbitrary tab.
        /// </summary>
        public VisualTreeAsset tab_uxml => _tab_uxml;

        protected override void Awake()
        {
            base.Awake();

            ui_document = GetComponent<UIDocument>();

            var window_ui_element = window_uxml.Instantiate();
            ui_document.rootVisualElement.Add(window_ui_element);

            window_ui_element.AddManipulator(new WindowDragManipulator());

            var window_tab_view = window_ui_element.Q<TabView>();

            foreach (var tab in GetComponentsInChildren<AbstractTab>())
            {
                var tab_ui_element = tab_uxml.Instantiate();
                var tab_tab = tab_ui_element.Q<Tab>();
                var tab_content_container = tab_ui_element.Q(className: "sharp-tab-contents");

                var tab_content = tab.InstantiateVisualElement();
                tab_content_container.contentContainer.Add(tab_content);

                var tab_header_label = tab_tab.tabHeader.Q<Label>();
                tab_header_label.text = tab.title;

                window_tab_view.Add(tab_tab);
            }

            window_tab_view.selectedTabIndex = 0;
        }
    }
}
