using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sharp.Core.Types
{
    /// <summary>
    /// A wrapper type around a MemberInfo type which has custom attribute data already loaded,
    /// preventing the need to allocate via GetCustomAttributes().
    /// </summary>
    /// <typeparam name="T">The type of member to wrap; must be a subtype of MemberInfo.</typeparam>
    /// <param name="member">The wrapped member.</param>
    /// <param name="attributes">A list of attribute instances that are attached to this member.</param>
    public record AttributedMember<T>(T member, IReadOnlyList<Attribute> attributes) where T : MemberInfo;

    public static class AttributedMember
    {
        /// <summary>
        /// Factory function to create an AttributedMember from the member instance.
        /// </summary>
        /// <typeparam name="T">The type of the member. Should likely be inferred from usage.</typeparam>
        /// <param name="member">The member.</param>
        /// <returns>The attributed member wrapper.</returns>
        public static AttributedMember<T> From<T>(T member)
            where T : MemberInfo
        {
            var attributes = member.GetCustomAttributes(true).Cast<Attribute>().ToArray();
            return new(member, attributes);
        }
    }
}