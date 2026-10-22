using System;
using UnityEngine;

namespace Sharp.Tabs.Console
{
    /// <summary>
    /// A single event from Unity's console/logging system.
    /// </summary>
    public struct LogEvent
    {
        public LogType type;
        public string message;
        public string backtrace;
    }
}
