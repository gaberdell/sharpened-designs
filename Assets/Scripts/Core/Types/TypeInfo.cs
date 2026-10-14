using Sharp.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Sharp.Core.Types
{
    /// <summary>
    /// A wrapper type around a Type instance which stores pre-allocated fields and methods, as well as their attributes.
    /// </summary>
    /// <param name="type">The type that is wrapped.</param>
    /// <param name="instance_fields">The fields defined by the type non-statically.</param>
    /// <param name="static_fields">The fields defined by the type statically.</param>
    /// <param name="instance_methods">The methods defined by the type non-statically.</param>
    /// <param name="static_methods">The methods defined by the type statically.</param>
    public record TypeInfo(
        Type type, 
        TypeInfo base_info,
        IReadOnlyList<AttributedMember<FieldInfo>> instance_fields,
        IReadOnlyList<AttributedMember<FieldInfo>> static_fields,
        IReadOnlyList<AttributedMember<MethodInfo>> instance_methods,
        IReadOnlyList<AttributedMember<MethodInfo>> static_methods);
}