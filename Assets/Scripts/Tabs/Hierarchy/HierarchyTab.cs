using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Hierarchy
{
    public class HierarchyTab : AbstractTab 
    {
        public Label tabName;
        public ScrollView hierarchyScrollView;
        public VisualElement scrollViewContent;
        public Toggle autoUpdate;
        public Button refreshButton;

        public List<VisualElement> vsToDelete = new List<VisualElement>();

        public bool IsAutoUpdating { get; private set; }

        private void RecursiveBuild(VisualElement addToParent, GameObject gO) {
            HierarchyFoldout ourNewFoldeout = new HierarchyFoldout();

            addToParent.Add(ourNewFoldeout);
            vsToDelete.Add(ourNewFoldeout);

            ourNewFoldeout.gameObjectName.text = gO.name;

            //Various ways to do this gonna go with going thru transforms for now
            for (int i = 0; i < gO.GetComponentCount(); i++) {
                Component comp = gO.GetComponentAtIndex(i);
                Type compType = comp.GetType();
                string typeName = compType.Name;
                HierarchyComponentButton compButton = new HierarchyComponentButton();
                compButton.button.text = typeName;
                ourNewFoldeout.foldoutContent.Add(compButton);
                vsToDelete.Add(compButton);
            }
            Transform trans = gO.transform;
            for (int i = 0; i < trans.childCount; i++) {
                RecursiveBuild(ourNewFoldeout.foldoutContent, trans.GetChild(i).gameObject);
            }
        }

        private void BuildSceneTree() {

            while (vsToDelete.Count > 0) {
                vsToDelete[0].parent.Remove(vsToDelete[0]);
                vsToDelete.RemoveAt(0);
            }

            Scene getCurrentScene = SceneManager.GetActiveScene();
            foreach (GameObject gO in getCurrentScene.GetRootGameObjects()) {
                RecursiveBuild(scrollViewContent, gO);
            }
        }

        protected override void OnInstantiateVisualElement() {
            tabName = root_ui_element.Q<Label>("ExampleNumber");
            hierarchyScrollView = root_ui_element.Q<ScrollView>("HierarchyScrollView");
            scrollViewContent = hierarchyScrollView
                                .Q<VisualElement>("unity-content-and-vertical-scroll-container")
                                .Q<VisualElement>("unity-content-viewport")
                                .Q<VisualElement>("unity-content-container");
            autoUpdate = root_ui_element.Q<Toggle>("AutoUpdate");
            refreshButton = root_ui_element.Q<Button>("RefreshSceneHierarchy");

            autoUpdate.RegisterCallback<ChangeEvent<bool>>((evt) => { IsAutoUpdating = evt.newValue; });
            refreshButton.RegisterCallback<ClickEvent>((evt) => { BuildSceneTree(); });


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
