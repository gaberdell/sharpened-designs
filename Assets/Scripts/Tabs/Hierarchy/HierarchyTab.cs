using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Object = UnityEngine.Object; //thx vs code

namespace Sharp.Tabs.Hierarchy
{
    public class HierarchyTab : AbstractTab 
    {
        private List<TreeViewItemData<Object>> tree_view_data = new List<TreeViewItemData<Object>>(10);
        private int id = 0;

        public Label tab_name;
        public TreeView hierarchy_tree_view;
        public Toggle auto_update;
        public Button refresh_button;


        public Object selected_object { get; private set; }
        public bool is_auto_updating { get; private set; }


        private List<TreeViewItemData<Object>> DoRecursiveBuild(GameObject gO) {
            //HierarchyFoldout ourNewFoldeout = new HierarchyFoldout();
            List <TreeViewItemData<Object>> returnList = new List<TreeViewItemData<Object>>();

            //Various ways to do this gonna go with going thru transforms for now
            for (int i = 0; i < gO.GetComponentCount(); i++) {
                Component comp = gO.GetComponentAtIndex(i);
                Type compType = comp.GetType();
                string typeName = compType.Name;

                returnList.Add(new TreeViewItemData<Object>(++id, comp));
            }
            Transform trans = gO.transform;
            for (int i = 0; i < trans.childCount; i++) {
                returnList.Add(new TreeViewItemData<Object>(++id, trans.GetChild(i).gameObject, DoRecursiveBuild(trans.GetChild(i).gameObject)));
            }

            return returnList;
        }

        private void BuildSceneTree() {
            id = 0;
            List<TreeViewItemData<Object>> listOfStuff = new List<TreeViewItemData<Object>>();
            tree_view_data.Clear();

            Scene getCurrentScene = SceneManager.GetActiveScene();
            foreach (GameObject gO in getCurrentScene.GetRootGameObjects()) {
                tree_view_data.Add(new TreeViewItemData<Object>());
                listOfStuff.Add(new TreeViewItemData<Object>(++id, gO, DoRecursiveBuild(gO)));
            }

            hierarchy_tree_view.SetRootItems(listOfStuff);
            hierarchy_tree_view.RefreshItems();
        }

        protected override void OnInstantiateVisualElement() {
            tab_name = root_ui_element.Q<Label>("ExampleNumber");
            hierarchy_tree_view = root_ui_element.Q<TreeView>("HierarchyTreeView");
            auto_update = root_ui_element.Q<Toggle>("AutoUpdate");
            refresh_button = root_ui_element.Q<Button>("RefreshSceneHierarchy");

            auto_update.RegisterCallback<ChangeEvent<bool>>((evt) => { is_auto_updating = evt.newValue; });
            refresh_button.RegisterCallback<ClickEvent>((evt) => { BuildSceneTree(); });

            hierarchy_tree_view.makeItem = () => new HierarchyComponent();

            Action<VisualElement, int> bindItem = (e, i) =>
            {
                var item = hierarchy_tree_view.GetItemDataForIndex<Object>(i);
                ((HierarchyComponent)e).label.text = (item is GameObject) ? item.name : item.GetType().Name;
            };

            hierarchy_tree_view.bindItem = bindItem;

            hierarchy_tree_view.selectedIndicesChanged += (selectedType) =>
            {
                int firstSelected = selectedType.First();
                selected_object = hierarchy_tree_view.GetItemDataForIndex<Object>(firstSelected);
            };

            BuildSceneTree();
        }

        private void Update() {
            //Terribly laggy change to something more performenent
            if (is_auto_updating) {
                BuildSceneTree();
            }
        }
    }
}
