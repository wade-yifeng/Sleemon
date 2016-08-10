namespace Sleemon.Common
{
    using System;

    /// <summary>
    /// Implements the NotNull Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class NotNullAttribute : Attribute
    {
    }
}
