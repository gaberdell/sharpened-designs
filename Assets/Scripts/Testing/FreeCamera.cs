using UnityEngine;
using UnityEngine.InputSystem;

namespace Sharp
{
    /// <summary>
    /// Testing script for the movement and rotation aspects of the Free Camera.
    /// Can also set Speed and Sensitivity.
    /// </summary>
    public class FreeCamera : MonoBehaviour
    {   
        [SerializeField] private Camera main_camera;
        private Camera _freecamera;
        private float _camera_pitch;
        private float _camera_yaw;

        private bool _camera_enabled = false;
        private bool _freecam_initialized = false;
        private Vector3 _input_vector;

        public bool CamEnableState => _camera_enabled;

        public float CameraSensitivity = 0.1f;

        public float CurrentSpeed = 5;

        private void Awake()
        {
            if (Camera.main == null)
                return;

            if (main_camera == null)
                main_camera = Camera.main;

            _freecamera = GetComponent<Camera>();

            // In the future, these are going to be parameters changable in the settings menu.
            _freecamera.fieldOfView = main_camera.fieldOfView;
            _freecamera.nearClipPlane = main_camera.nearClipPlane;
            _freecamera.farClipPlane = main_camera.farClipPlane;
            _freecamera.enabled = false;
        }

        // Working on moving this to the Input Action Asset
        private void Update()
        {
            if (main_camera == null || _freecamera == null)
                return;

            if (!_camera_enabled)
                return;

            _freecamera.transform.Translate(_input_vector * CurrentSpeed * Time.deltaTime);
        }


        public void ToggleFreecam()
        {
            SetFreeCameraEnabled(!_camera_enabled);
        }

        public void SetFreeCameraEnabled(bool enabled)
        {
            _camera_enabled = enabled;
            
            Debug.Log("Freecamera " + _camera_enabled);

            if (_camera_enabled)
            {
                Cursor.visible = false;
                // Cursor.lockState = CursorLockMode.Locked;

                _freecamera.enabled = true;
                main_camera.enabled = false;

                if (!_freecam_initialized)
                {
                    _freecamera.transform.SetPositionAndRotation(
                        main_camera.transform.position, main_camera.transform.rotation
                    );

                    _freecam_initialized = true;
                }
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                _freecamera.enabled = false;
                main_camera.enabled = true;
            }
        }
        /// <summary>
        ///  Handles mouse input for looking around on the Free Camera.
        /// </summary>
        /// <param name="callback_context">The context provided to the callback function.</param>
        public void OnRotation(InputAction.CallbackContext callback_context)
        {
            if (!_camera_enabled)
                return;

            Vector2 MouseInput = callback_context.ReadValue<Vector2>();

            _camera_yaw += MouseInput.x * CameraSensitivity;
            _camera_pitch -= Mathf.Clamp(MouseInput.y * CameraSensitivity, -90f, 90f);
            
            _freecamera.transform.rotation = Quaternion.Euler(_camera_pitch, _camera_yaw, 0);
        }

        /// <summary>
        /// Handles updating the input vector for WASD movement on the Free Camera.
        /// </summary>
        /// <param name="callback_context">The context provided to the callback function.</param>
        public void OnMovement(InputAction.CallbackContext callback_context)
        {
            if (!_camera_enabled) 
                return;
            _input_vector = callback_context.ReadValue<Vector3>();
        }

    }
}
