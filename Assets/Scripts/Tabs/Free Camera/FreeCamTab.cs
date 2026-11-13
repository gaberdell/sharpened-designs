using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Tabs.FreeCamTab
{
    public class FreeCamTab:AbstractTab
    {
        [SerializeField] private Sharp.FreeCamera _freecamera;

        private Button toggle_cam;
        private Slider look_sensitivity;
        private Slider move_speed;

        protected override void OnInstantiateVisualElement()
        {
            toggle_cam = root_ui_element.Q<Button>("toggle-freecam");
            look_sensitivity = root_ui_element.Q<Slider>("sensitivity");
            move_speed = root_ui_element.Q<Slider>("move-speed");

            look_sensitivity.value = _freecamera.CameraSensitivity;
            move_speed.value = _freecamera.CurrentSpeed;

            toggle_cam.clicked += CamToggle;

            look_sensitivity.RegisterValueChangedCallback
            (evt =>{
                _freecamera.CameraSensitivity = evt.newValue;
            });

            move_speed.RegisterValueChangedCallback
            (evt =>{
                _freecamera.CurrentSpeed = evt.newValue;
            });
        }

        private void CamToggle()
        {
            _freecamera.ToggleFreecam();

            if (_freecamera.CamEnableState)
            {
                toggle_cam.text = "Disable Free Camera Mode";
            } 
            else
            {
                toggle_cam.text = "Enable Free Camera Mode";
            }
        }
    }
}