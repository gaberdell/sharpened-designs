using System;
using UnityEngine;

namespace Sharp.Tabs.ObjectButton 
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class SharpButtonAttribute : Attribute
    {
        public string name_override { get; }

        public SharpButtonAttribute(string name_override = null)
        {
            this.name_override = name_override;
        }
    }
}
