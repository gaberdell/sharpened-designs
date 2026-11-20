using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp
{
    public class SimpleModificationsTab : ObjectSpecificTab

        /// <summary>
        /// Implementation of the Simple Modifications Tab. Handles functions that can be applied to objects
        /// that are selected in the Hierarchy tab.
        /// </summary>
    {
        private Toggle toggle_object;
        private Vector3Field transform_position;
        private Vector3Field transform_rotation;
        private Vector3Field transform_scale;
        private GameObject game_object => selected_object switch 
        { 
            GameObject go => go,
            _ => null
        };

        protected override void OnInstantiateVisualElement()
        {
            toggle_object = root_ui_element.Q<Toggle>("toggle-object");
            transform_position = root_ui_element.Q<Vector3Field>("transform-position");
            transform_rotation = root_ui_element.Q<Vector3Field>("transform-rotation");
            transform_scale = root_ui_element.Q<Vector3Field>("transform-scale");

            toggle_object.RegisterValueChangedCallback(ToggleObjectFunctionality);
            transform_position.RegisterValueChangedCallback(UpdateObjectPosition);
            transform_rotation.RegisterValueChangedCallback(UpdateObjectRotation);
            transform_scale.RegisterValueChangedCallback(UpdateObjectScale);

            ToggleValidFeatures();
        }

        public override void OnSelectedObjectChanged()
        {
            ToggleValidFeatures();
            if (game_object == null)
                return;

            toggle_object.SetValueWithoutNotify(game_object.activeSelf);
            transform_position.value = game_object.transform.position;
            transform_rotation.value = game_object.transform.rotation.eulerAngles;
            transform_scale.value = game_object.transform.localScale;
        }

        /// <summary>
        /// Helper function to check if the object selected is valid for 
        /// the features listed in the Simple Modifications tab.
        /// </summary>
        private void ToggleValidFeatures()
        {
            bool is_valid = game_object != null;

            toggle_object.SetEnabled(is_valid);
            transform_position.SetEnabled(is_valid);
            transform_rotation.SetEnabled(is_valid);
            transform_scale.SetEnabled(is_valid);
        }

        /// <summary>
        /// Function that handles enable/disable functionality of the 
        /// Simple Modifications tab, allowing you to enable/disable GameObjects. (and maybe components soon)
        /// </summary>
        /// <param name="evt">Event for when the Toggle toggle_object changes values.</param>
        private void ToggleObjectFunctionality(ChangeEvent<bool> evt)
        {
            if (game_object == null)
                return;

            game_object.SetActive(evt.newValue);
        }
        
        /// <summary>
        /// Function that handles the transform functionality of
        /// the Simple Modifications tab, allowing you to move an object to where you want.
        /// </summary>
        /// <param name="evt"></param>
        private void UpdateObjectPosition(ChangeEvent<Vector3> evt)
        {
            if (game_object == null)
                return;

            if (game_object is GameObject go)
            {
                go.transform.position = evt.newValue;
            }
        }

        private void UpdateObjectRotation(ChangeEvent<Vector3> evt)
        {
            if (game_object == null)
                return;

            if (game_object is GameObject go)
            {
                go.transform.rotation = Quaternion.Euler(evt.newValue.x, evt.newValue.y, evt.newValue.z);
            }
        }

        private void UpdateObjectScale(ChangeEvent<Vector3> evt)
        {
            if (game_object == null)
                return;

            if (game_object is GameObject go)
            {
                go.transform.localScale = evt.newValue;
            }
        }
    }
}
