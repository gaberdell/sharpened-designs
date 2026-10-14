using Sharp.Core;
using System;
using System.Linq;
using System.Reflection;

namespace Sharp.Core.Types
{
    /// <summary>
    /// A cache for TypeInfo objects from Type instances, used to reduce allocations per-frame.
    /// </summary>
    public class TypeCache : AbstractCache<Type, TypeInfo>
    {
        protected override TypeInfo ComputeCacheValue(Type type)
        {
            var base_info = null as TypeInfo;

            if(type.BaseType != null)
            {
                base_info = this.Get(type.BaseType);
            }

            return new(
                type,
                base_info,
                Enumerable.ToList(
                    from field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    select AttributedMember.From(field)),
                Enumerable.ToList(
                    from field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    select AttributedMember.From(field)),
                Enumerable.ToList(
                    from field in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    select AttributedMember.From(field)),
                Enumerable.ToList(
                    from field in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    select AttributedMember.From(field))
            );
        }

        /// <summary>
        /// A singleton instance of the type cache; use to get type information without needing to allocate.
        /// </summary>
        public static TypeCache Instance { get; } = new();
    }
}