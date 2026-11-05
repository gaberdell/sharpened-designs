using Sharp.Core;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Sharp
{
    /// <summary>
    /// The class responsible for orchestrating behaviour of windows.
    /// </summary>
    public class WindowManager : BaseSharpBehaviour
    {
        private SharpInputs _inputs;
        private BaseWindow[] _windows;

        protected override void Awake()
        {
            base.Awake();

            _inputs = new();
            _inputs.Debug.Toggle.performed += TogglePerformed;

            _windows = GetComponentsInChildren<BaseWindow>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _inputs.Enable();
        }

        protected void OnDisable()
        {
            _inputs.Disable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _inputs.Dispose();
        }

        private void TogglePerformed(InputAction.CallbackContext context)
        {
            foreach (var window in _windows)
            {
                var window_style = window.ui_document.rootVisualElement.style;

                window_style.display = window_style.display.value switch
                {
                    DisplayStyle.Flex => DisplayStyle.None,
                    DisplayStyle.None => DisplayStyle.Flex,
                    _ => throw new InvalidOperationException()
                };
            }
        }
    }
}
