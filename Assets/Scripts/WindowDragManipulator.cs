using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp
{
    /// <summary>
    /// A manipulator for BaseWindow instances which allow them to be clicked, dragged, and dropped
    /// by the mouse cursor.
    /// </summary>
    public class WindowDragManipulator : PointerManipulator
    {
        private VisualElement _header;
        private bool _is_dragging;
        private Vector2 _position_at_start;
        private Vector2 _pointer_at_start;

        protected override void RegisterCallbacksOnTarget()
        {
            _header = GetTargetHeader();
            _header.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _header.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            _header.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            _header = GetTargetHeader();
            _header.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            _header.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            _header.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }

        private VisualElement GetTargetHeader()
        {
            return target.Q(className: "unity-tab-view__header-container");
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if(evt.target != _header)
            {
                return;
            }

            if(evt.button != 0)
            {
                return;
            }

            _is_dragging = true;
            _position_at_start = target.resolvedStyle.translate;
            _pointer_at_start = evt.position;

            _header.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if(_is_dragging && _header.HasPointerCapture(evt.pointerId))
            {
                target.style.translate = _position_at_start - _pointer_at_start + new Vector2(evt.position.x, evt.position.y);
            }
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if(_is_dragging && _header.HasPointerCapture(evt.pointerId))
            {
                _is_dragging = false;
                _header.ReleasePointer(evt.pointerId);
            }
        }
    }
}
