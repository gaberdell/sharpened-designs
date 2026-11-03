using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Console
{
    /// <summary>
    /// Implementation of the 'Console' tab. Uses a ListView element to
    /// manage rendering of console output, with each output rendered as a 'ConsoleEntryElement'.
    /// </summary>
    public class ConsoleTab : AbstractTab
    {
        [Header("Console Settings")]
        [SerializeField] private ConsoleEntrySettings entry_settings;

        [CreateProperty] private bool show_notes = true;
        [CreateProperty] private int note_count = 0;
        [CreateProperty] private bool show_warnings = true;
        [CreateProperty] private int warning_count = 0;
        [CreateProperty] private bool show_errors = true;
        [CreateProperty] private int error_count = 0;

        /// <summary>
        /// A reference to the list view that will render the entries.
        /// Populated on UI creation.
        /// </summary>
        private ListView _entries;
        /// <summary>
        /// The list of all log events that have occured with this window open.
        /// 
        /// TODO: Set some cap on this list's length, after which we treat it as an LRU cache,
        /// evicting old events so that we don't balloon our memory usage, particularly in large scale games.
        /// </summary>
        private readonly List<LogEvent> _log_events = new();
        private readonly List<LogEvent> _displayed_events = new();

        private bool IsShowingLogType(LogType log_type)
        {
            return log_type switch
            {
                LogType.Log => show_notes,
                LogType.Warning => show_warnings,
                LogType.Exception or LogType.Error or LogType.Assert => show_errors,
                _ => throw new InvalidOperationException()
            };
        }

        private void RecomputeDisplayedEvents()
        {
            _displayed_events.Clear();

            foreach(var log_event in _log_events)
            {
                if(IsShowingLogType(log_event.type))
                {
                    _displayed_events.Add(log_event);
                }
            }

            _entries.Rebuild();
        }

        private void OnNoteToggleClick(ChangeEvent<bool> evt)
        {
            show_notes = evt.newValue;
            RecomputeDisplayedEvents();
        }

        private void OnWarningToggleClick(ChangeEvent<bool> evt)
        {
            show_warnings = evt.newValue;
            RecomputeDisplayedEvents();
        }

        private void OnErrorToggleClick(ChangeEvent<bool> evt)
        {
            show_errors = evt.newValue;
            RecomputeDisplayedEvents();
        }

        /// <summary>
        /// Callback used to add a log event to the list of all recorded log events.
        /// </summary>
        private void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            _log_events.Add(new LogEvent
            {
                message = condition,
                backtrace = stackTrace,
                type = type
            });

            if(type is LogType.Log)
            {
                note_count++;
            }
            else if(type is LogType.Warning)
            {
                warning_count++;
            }
            else if(type is LogType.Error or LogType.Exception or LogType.Assert)
            {
                error_count++;
            }

            RecomputeDisplayedEvents();
        }

        /// <summary>
        /// Callback used to set up the state of the ConsoleEntryElement before it is
        /// rendered by the entry ListView. 'index' is an index into '_log_events', and
        /// 'element' is guaranteed to be a ConsoleEntryElement by the initialization code.
        /// </summary>
        private void BindItem(VisualElement element, int index)
        {
            var console_element = element as ConsoleEntryElement;
            var log_event = _displayed_events[index];

            console_element.message.text = log_event.message;
            console_element.backtrace.text = log_event.backtrace;
            console_element.icon.image = log_event.type switch
            {
                LogType.Log => entry_settings.icon_note,
                LogType.Warning => entry_settings.icon_warning,
                LogType.Assert or LogType.Exception or LogType.Error => entry_settings.icon_error,

                _ => throw new InvalidOperationException()
            };
        }

        protected override void OnInstantiateVisualElement()
        {
            _entries = root_ui_element.Q<ListView>();

            // Setup the list view to have an empty 'none' element, use ConsoleEntryElement,
            // bind to our class, and use the list of log events we've collected.
            _entries.makeNoneElement = () => new VisualElement();
            _entries.makeItem = () => new ConsoleEntryElement();
            _entries.bindItem = BindItem;
            _entries.itemsSource = _displayed_events;

            var note_toggle = root_ui_element.Q<Toggle>("Note-Toggle");
            note_toggle.RegisterValueChangedCallback(OnNoteToggleClick);

            var warning_toggle = root_ui_element.Q<Toggle>("Warning-Toggle");
            warning_toggle.RegisterValueChangedCallback(OnWarningToggleClick);

            var error_toggle = root_ui_element.Q<Toggle>("Error-Toggle");
            error_toggle.RegisterValueChangedCallback(OnErrorToggleClick);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // Connect to the application's logger //

            // TODO. Perform this connection in a static context for the following reasons:
            // 1) This will allow us to track log events that occur when this component is inactive.
            // 2) We can perform this hookup in an InitializeOnLoad method, meaning we don't need to worry
            //    about script initialization order making us 'drop' logs.
            Application.logMessageReceived += LogMessageReceived;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Disconnect from the application's logger //
            Application.logMessageReceived -= LogMessageReceived;
        }
    }
}
