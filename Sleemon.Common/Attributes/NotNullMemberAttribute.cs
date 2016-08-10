namespace Sleemon.Common
{
    using System;

    /// <summary>
    /// Attribute to check for a member of a parameter not null
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class NotNullMemberAttribute : Attribute
    {
        /// <summary>
        /// Holds the name of the child member to check
        /// </summary>
        private readonly string memberName;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotNullMemberAttribute"/> class. 
        /// </summary>
        /// <param name="memberName">name of the child member of the parameter to check</param>
        public NotNullMemberAttribute(string memberName)
        {
            this.memberName = memberName;
        }

        /// <summary>
        /// Gets the member name to check
        /// </summary>
        public string MemberName
        {
            get
            {
                return this.memberName;
            }
        }
    }
}
