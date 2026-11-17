using Sharp.Tabs.Hierarchy;
using UnityEngine;

namespace Sharp
{
    /// <summary>
    /// Base class for all tabs which operate on a particular object.
    /// Provides a property to read the currently selected object from the hierarchy,
    /// as well as a callback for when the selection changes.
    /// </summary>
    public abstract class ObjectSpecificTab : AbstractTab
    {
        /// <summary>
        /// Private reference to the hierarchy tab so we can read its current selection
        /// </summary>
        private HierarchyTab _hierarchy;

        /// <summary>
        /// Helper property to get the currently selected object. Will return null if there
        /// is no current selection
        /// </summary>
        protected UnityEngine.Object selected_object => _hierarchy.selected_object;

        /// <summary>
        /// Called when the hierarchy selection changes
        /// </summary>
        public abstract void OnSelectedObjectChanged();

        protected override void Awake()
        {
            base.Awake();

            // Note: this introduces a dependency on the hierarchy tab in a kind-of awkward way, but its mostly necessary since
            // we need to know what object we have selected.
            _hierarchy = GameObject.FindAnyObjectByType<HierarchyTab>();
            _hierarchy.selection_changed += OnSelectedObjectChanged;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _hierarchy.selection_changed -= OnSelectedObjectChanged;
        }
    }
}
