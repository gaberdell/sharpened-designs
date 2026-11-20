using System;
using System.Reflection;

namespace Sharp.Tabs.Variables
{
    public struct VariableEntry
    {
        public UnityEngine.Object target;
        public MemberInfo member;

        public object GetValue()
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

        public string GetVariableName()
        {
            return member.Name;
        }
    }
}
