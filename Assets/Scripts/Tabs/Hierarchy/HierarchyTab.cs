using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Hierarchy
{
    public class HierarchyTab : AbstractTab 
    {
        private Label _tab_name_label;
        private TreeView _hierarchy_tree;
        private Toggle _auto_update_toggle;
        private Button _refresh_button;

        /// <summary>
        /// A tree-view-friendly representation of the current scene hierarchy.
        /// <para>
        /// For the purposes of this data structure, we assume that UnityEngine.Object.GetHashCode() is a 
        /// perfect hash function, i.e. it will never return the same integer value for two distinct instances.
        /// Because of this assumption, we can safely use the hash code of the object as its id within the tree view.
        /// </para>
        /// </summary>
        private readonly List<TreeViewItemData<UnityEngine.Object>> _tree_data = new();

        /// <summary>
        /// Get the (first) currently selected object. Will return null if there is no currently selected object.
        /// </summary>
        public UnityEngine.Object selected_object { get; private set; }
        /// <summary>
        /// If 'true', the hierarchy will attempt to update every frame.
        /// </summary>
        public bool should_auto_update { get; private set; }

        /// <summary>
        /// Called whenever <see cref="selected_object"/> is changed.
        /// </summary>
        public event Action selection_changed;

        /// <summary>
        /// Build a new tree representation of the provided game object
        /// </summary>
        /// <param name="root">The root object for to build the tree from</param>
        /// <returns></returns>
        private List<TreeViewItemData<UnityEngine.Object>> BuildTreeForObject(GameObject root) 
        {
            var result = new List<TreeViewItemData<UnityEngine.Object>>();

            // Iterate the components and transform children of this object //
            for (var i = 0; i < root.GetComponentCount(); i++) 
            {
                var component = root.GetComponentAtIndex(i);
                result.Add(new(component.GetHashCode(), component));
            }

            var trans = root.transform;
            for (int i = 0; i < trans.childCount; i++) 
            {
                var child_object = trans.GetChild(i).gameObject;
                var child_tree = BuildTreeForObject(child_object);
                result.Add(new(child_object.GetHashCode(), child_object, child_tree));
            }

            return result;
        }

        /// <summary>
        /// Build the tree representation of the entire hierarchy
        /// </summary>
        private void BuildSceneTree() 
        {
            _tree_data.Clear();

            var current_scene = SceneManager.GetActiveScene();
            foreach (var root in current_scene.GetRootGameObjects()) 
            {
                var root_tree = BuildTreeForObject(root);
                _tree_data.Add(new(root.GetHashCode(), root, root_tree));
            }

            _hierarchy_tree.SetRootItems(_tree_data);
            _hierarchy_tree.RefreshItems();
        }

        protected override void OnInstantiateVisualElement() 
        {
            _tab_name_label = root_ui_element.Q<Label>("ExampleNumber");
            _hierarchy_tree = root_ui_element.Q<TreeView>("HierarchyTreeView");
            _auto_update_toggle = root_ui_element.Q<Toggle>("AutoUpdate");
            _refresh_button = root_ui_element.Q<Button>("RefreshSceneHierarchy");

            _auto_update_toggle.RegisterCallback<ChangeEvent<bool>>((evt) => { should_auto_update = evt.newValue; });
            _refresh_button.RegisterCallback<ClickEvent>((evt) => { BuildSceneTree(); });

            _hierarchy_tree.makeItem = () => new HierarchyComponent();
            _hierarchy_tree.bindItem = (e, i) =>
            {
                var item = _hierarchy_tree.GetItemDataForIndex<UnityEngine.Object>(i);
                var element = e as HierarchyComponent;

                element.label.text = item switch
                {
                    GameObject => item.name,
                    _ => item.GetType().Name
                };
            };

            _hierarchy_tree.selectedIndicesChanged += (new_selection) =>
            {
                var original_selection = selected_object;

                if (new_selection.Any())
                {
                    var first_selected = new_selection.First();
                    selected_object = _hierarchy_tree.GetItemDataForIndex<UnityEngine.Object>(first_selected);
                }
                else
                {
                    selected_object = null;
                }

                if (original_selection != selected_object)
                {
                    selection_changed?.Invoke();
                }
            };

            BuildSceneTree();
        }

        private void Update() 
        {
            // Since this is quite laggy, only do it if the user wants it //
            if (should_auto_update) 
            {
                BuildSceneTree();
            }
        }
    }
}
