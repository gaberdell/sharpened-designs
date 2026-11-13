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
        public Label tabName;
        public TreeView hierarchyTreeView;
        public Toggle autoUpdate;
        public Button refreshButton;

        public List<TreeViewItemData<Object>> treeViewData = new List<TreeViewItemData<Object>>(10);

        public Object selectedObject { get; private set; }

        public bool IsAutoUpdating { get; private set; }

        private int id = 0;

        private List<TreeViewItemData<Object>> RecursiveBuild(GameObject gO) {
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
                returnList.Add(new TreeViewItemData<Object>(++id, trans.GetChild(i).gameObject, RecursiveBuild(trans.GetChild(i).gameObject)));
            }

            return returnList;
        }

        private void BuildSceneTree() {
            id = 0;
            List<TreeViewItemData<Object>> listOfStuff = new List<TreeViewItemData<Object>>();
            treeViewData.Clear();

            Scene getCurrentScene = SceneManager.GetActiveScene();
            foreach (GameObject gO in getCurrentScene.GetRootGameObjects()) {
                treeViewData.Add(new TreeViewItemData<Object>());
                listOfStuff.Add(new TreeViewItemData<Object>(++id, gO, RecursiveBuild(gO)));
            }

            hierarchyTreeView.SetRootItems(listOfStuff);
            hierarchyTreeView.Rebuild();
        }

        protected override void OnInstantiateVisualElement() {
            tabName = root_ui_element.Q<Label>("ExampleNumber");
            hierarchyTreeView = root_ui_element.Q<TreeView>("HierarchyTreeView");
            autoUpdate = root_ui_element.Q<Toggle>("AutoUpdate");
            refreshButton = root_ui_element.Q<Button>("RefreshSceneHierarchy");

            autoUpdate.RegisterCallback<ChangeEvent<bool>>((evt) => { IsAutoUpdating = evt.newValue; });
            refreshButton.RegisterCallback<ClickEvent>((evt) => { BuildSceneTree(); });

            hierarchyTreeView.makeItem = () => new HierarchyComponent();

            Action<VisualElement, int> bindItem = (e, i) =>
            {
                var item = hierarchyTreeView.GetItemDataForIndex<Object>(i);
                ((HierarchyComponent)e).label.text = (item is GameObject) ? item.name : item.GetType().Name;
            };

            hierarchyTreeView.bindItem = bindItem;

            hierarchyTreeView.selectedIndicesChanged += (selectedType) =>
            {
                int firstSelected = selectedType.First();
                selectedObject = hierarchyTreeView.GetItemDataForIndex<Object>(firstSelected);
            };

            BuildSceneTree();
        }

        private void Update() {
            //Terribly laggy change to something more performenent
            if (IsAutoUpdating) {
                BuildSceneTree();
            }
        }
    }
}
