using UnityEngine;
using UnityEngine.InputSystem;

namespace Sharp
{
    public class FreecamBehavior : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        [SerializeField]
        private Camera freemodeCamera;

        [SerializeField]
        private Key toggleKey = Key.F12;

        private Camera gameCamera;
        private bool freeMode = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            gameCamera = Camera.main;

            freemodeCamera.enabled = false;
            gameCamera.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Keyboard.current != null &&
             Keyboard.current[toggleKey].wasPressedThisFrame)
            {
                ToggleFreecam();
            }
        }

        private void ToggleFreecam()
        {
            SetFreecam(!freeMode);
        }

        public void SetFreecam(bool enable)
        {
            freeMode = enable;

            if (freeMode)
            {
                freemodeCamera.transform.SetPositionAndRotation(
                    gameCamera.transform.position, gameCamera.transform.rotation
                );

                freemodeCamera.fieldOfView = gameCamera.fieldOfView;

                gameCamera.enabled = false;
                freemodeCamera.enabled = true;

                Debug.Log("Freecam Enabled");
            }
            else
            {
                freemodeCamera.enabled = false;
                gameCamera.enabled = true;

                Debug.Log("Freecam Disabled");
            }
        }
    }
}
