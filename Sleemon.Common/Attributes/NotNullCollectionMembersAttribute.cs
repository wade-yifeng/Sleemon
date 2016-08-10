namespace Sleemon.Common
{
    using System;

    /// <summary>
    /// Attribute to check that members of a collection parameter are not null
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class NotNullCollectionMembersAttribute : Attribute
    {
    }
}
