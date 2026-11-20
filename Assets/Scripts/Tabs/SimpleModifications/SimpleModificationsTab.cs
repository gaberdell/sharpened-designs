using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp
{
    public class SimpleModificationsTab : ObjectSpecificTab

        /// <summary>
        /// Implementation of the Object Specific Tab.
        /// </summary>
    {
        private Button toggle_object;

        protected override void OnInstantiateVisualElement()
        {
            toggle_object = root_ui_element.Q<Button>("toggle-object");

            toggle_object.clicked += ToggleObjectFunctionality;


        }

        private void ToggleObjectFunctionality()
        {
            throw new System.NotImplementedException();
        }

        public override void OnSelectedObjectChanged()
        {
            
        }

        private void ToggleObjectFunctionality()
        {

        }



    }
}
