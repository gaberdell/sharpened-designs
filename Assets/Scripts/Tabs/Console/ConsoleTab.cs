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

            _entries.Rebuild();
        }

        /// <summary>
        /// Callback used to set up the state of the ConsoleEntryElement before it is
        /// rendered by the entry ListView. 'index' is an index into '_log_events', and
        /// 'element' is guaranteed to be a ConsoleEntryElement by the initialization code.
        /// </summary>
        private void BindItem(VisualElement element, int index)
        {
            var console_element = element as ConsoleEntryElement;
            var log_event = _log_events[index];

            // TODO: handle finding an appropriate icon //

            console_element.message.text = log_event.message;
            console_element.backtrace.text = log_event.backtrace;
        }

        protected override void OnInstantiateVisualElement()
        {
            _entries = root_ui_element.Q<ListView>();

            // Setup the list view to have an empty 'none' element, use ConsoleEntryElement,
            // bind to our class, and use the list of log events we've collected.
            _entries.makeNoneElement = () => new VisualElement();
            _entries.makeItem = () => new ConsoleEntryElement();
            _entries.bindItem = BindItem;
            _entries.itemsSource = _log_events;
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
