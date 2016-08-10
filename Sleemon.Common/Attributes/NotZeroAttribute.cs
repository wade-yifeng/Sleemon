namespace Sleemon.Common
{
    using System;

    /// <summary>
    /// Attribute to specify that a parameter value cannot be 0
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class NotZeroAttribute : Attribute
    {
    }
}
