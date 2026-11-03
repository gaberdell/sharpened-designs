using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Console
{
    [CreateAssetMenu(menuName = "Sharp Debugger/Console Entry Settings")]
    public class ConsoleEntrySettings : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private Texture _icon_note;
        [SerializeField] private Texture _icon_warning;
        [SerializeField] private Texture _icon_error;

        public Texture icon_note => _icon_note;
        public Texture icon_warning => _icon_warning;
        public Texture icon_error => _icon_error;
    }
}
