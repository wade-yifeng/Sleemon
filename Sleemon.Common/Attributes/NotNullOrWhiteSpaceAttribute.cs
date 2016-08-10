namespace Sleemon.Common
{
    using System;

    /// <summary>
    /// parameter attribute to check for not null or white space
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class NotNullOrWhiteSpaceAttribute : Attribute
    {
    }
}
