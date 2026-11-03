using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Sharp
{
    /// <summary>
    /// Testing script for the movement and rotation aspects of the Free Camera.
    /// Can also set Speed and Sensitivity.
    /// </summary>

    public class FreecamBehavior : MonoBehaviour
    {
        [SerializeField] private Camera MainCamera;
        private Camera FreecamCamera;

        private bool CameraEnabled = false;
        private Vector3 InputVector;
        private float CurrentSpeed;

        public float CameraSensitivity = 1f;
        public float SlowSpeed = 2f, NormalSpeed = 5f, SprintSpeed = 8f;

        private void Awake()
        {
            if (Camera.main == null)
                return;

            if (MainCamera == null)
                MainCamera = Camera.main;

            GameObject FreecameraObject = new GameObject("DynamicCamera");
            FreecamCamera = FreecameraObject.AddComponent<Camera>();

            // In the future, these are going to be parameters changable in the settings menu.
            FreecamCamera.fieldOfView = MainCamera.fieldOfView;
            FreecamCamera.nearClipPlane = MainCamera.nearClipPlane;
            FreecamCamera.farClipPlane = MainCamera.farClipPlane;
            FreecamCamera.enabled = false;
        }

        private void Update()
        {
            if (MainCamera == null || FreecamCamera == null)
                return;

            if (!CameraEnabled)
                return;

            if (Keyboard.current.leftShiftKey.isPressed)
            {
                CurrentSpeed = SprintSpeed;
            }
            else if (Keyboard.current.leftAltKey.isPressed)
            {
                CurrentSpeed = SlowSpeed;
            }
            else
            {
                CurrentSpeed = NormalSpeed;
            }

            FreecamCamera.transform.Translate(InputVector * CurrentSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Enables/Disbales the Free Camera based on the key provided in the InputAction map.
        /// </summary>
        /// <param name="callback_context">The context provided to the callback function.</param>
        public void OnEnableFreecam(InputAction.CallbackContext callback_context)
        {
            if (callback_context.started)
            {
                //bool MainCameraEnabled = MainCamera.enabled;

                CameraEnabled = !CameraEnabled;

                Debug.Log("Freecamera " + CameraEnabled);

                if (CameraEnabled)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                    FreecamCamera.enabled = true;
                    MainCamera.enabled = false;

                    FreecamCamera.transform.SetPositionAndRotation(
                        MainCamera.transform.position, MainCamera.transform.rotation
                    );
                }
                else
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    FreecamCamera.enabled = false;
                    MainCamera.enabled = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback_context">The context provided to the callback function.</param>
        void OnRotation()
        {}

        /// <summary>
        /// Invalidate the cache for the provided key.
        /// </summary>
        /// <param name="callback_context">The context provided to the callback function.</param>
        public void OnMovement(InputAction.CallbackContext callback_context)
        {
            if (!CameraEnabled) return;
            InputVector = callback_context.ReadValue<Vector3>();
        }

    }
}
