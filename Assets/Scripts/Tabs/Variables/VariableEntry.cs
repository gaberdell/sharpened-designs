using System;
using System.Reflection;

namespace Sharp.Tabs.Variables
{
    /// <summary>
    /// The representation used to refer to a single 'variable' within the variable tab.
    /// Wraps both the target object a variable belongs to, as well as the member info that 
    /// defines it.
    /// </summary>
    public struct VariableEntry
    {
        /// <summary>
        /// The target object this variable belongs to.
        /// </summary>
        public UnityEngine.Object target;
        /// <summary>
        /// The member info this variable describes. May be an instance of either
        /// <see cref="FieldInfo"/> or <see cref="MethodInfo"/>.
        /// </summary>
        public MemberInfo member;

        /// <summary>
        /// Extract the value of this variable from the associated target.
        /// </summary>
        /// <returns>The value of this variable entry</returns>
        public readonly object GetValue()
        {
            if (member is FieldInfo field)
            {
                return field.GetValue(target);
            }
            else if(member is PropertyInfo property)
            {
                return property.GetValue(target);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Get the type that this variable is expected to be. Always equivalent
        /// to the declared type of the property or field that underlies this entry.
        /// </summary>
        /// <returns>The type this variable is expected to be</returns>
        public readonly Type GetMemberType()
        {
            if (member is FieldInfo field)
            {
                return field.FieldType;
            }
            else if (member is PropertyInfo property)
            {
                return property.PropertyType;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Get the name of this variable. Always exactly matches the name as declared in code,
        /// including casing and underscores.
        /// </summary>
        public readonly string GetVariableName()
        {
            return member.Name;
        }
    }
}
